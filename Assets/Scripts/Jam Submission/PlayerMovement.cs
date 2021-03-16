using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// potential improvements.
// make a parent class or interface for tools
// create classes of tools with properties and methods instead of using delegates and arrays
// optimize portal pairing so that we dont need individual portal types for each color

public class PlayerMovement : MonoBehaviour
{
    // player data
    public float moveSpeed = 3;
    public Rigidbody2D rb;
    public Vector2 movement = Vector2.zero;
    public Vector2 startDirection;
    public Collider2D playerCollider;
    private Vector2 startPosition;
    private bool isMoving = false;
    private int currentDirection;
    private Vector2[] directions;
    public Animator animator;

    // bread data
    public Collider2D breadCollider;

    // duckling data
    public GameObject ducky;
    private Collider2D duckyCollider;
    private bool foundDucky = false;

    // tool data and behaviors
    public Collider2D[] rotatorCCWs;
    public Collider2D[] rotatorCWs;
    public Collider2D[] horizontalFlips;
    public Collider2D[] verticalFlips;
    public Collider2D[] teleporterSet1;
    public Collider2D[] teleporterSet2;
    public Collider2D[] teleporterSet3;

    // // tool positions
    // public Rigidbody2D[] rbRotatorCCWs;
    // public Rigidbody2D[] rbRotatorCWs;
    // public Rigidbody2D[] rbHorizontalFlips;
    // public Rigidbody2D[] rbVerticalFlips;
    // public Rigidbody2D[] rbTeleporterSet1;
    // public Rigidbody2D[] rbTeleporterSet2;
    // public Rigidbody2D[] rbTeleporterSet3;

    // tool behavior
    private ToolBehavior RotatorCCW;
    private ToolBehavior RotatorCW;
    private ToolBehavior HorizontalFlip;
    private ToolBehavior VerticalFlip;
    private ToolBehavior Teleporter;

    private bool[][] colliderFlags;

    /*
    Useful player locations - IGNORE
    X: -9.5 Y: 4.5      Top-left
    X: 8.5 Y: 4.5      Top-right
    X: -9.5 Y: -4.5      Bottom-right
    X: 8.5 Y: -4.5      Top-left
    */

    // Start should run when frame launches
    void Start() 
    {
        // get the start position of the duck for reloads
        startPosition = rb.position;
        // create array of directions going clockwise from right
        directions = new Vector2[4] {Vector2.right, Vector2.down, Vector2.left, Vector2.up};
        // get index of starting direction from editor
        currentDirection = Array.FindIndex(directions, d => d == startDirection);

        duckyCollider = ducky.GetComponent(typeof(Collider2D)) as Collider2D;

        // adding empty arrays if not configured in editor
        if (rotatorCCWs == null) {
            rotatorCCWs = new Collider2D[0];
        }
        if (rotatorCWs == null) {
            rotatorCWs = new Collider2D[0];
        }
        if (horizontalFlips == null) {
            horizontalFlips = new Collider2D[0];
        }
        if (verticalFlips == null) {
            verticalFlips = new Collider2D[0];
        }
        if (teleporterSet1 == null) {
            teleporterSet1 = new Collider2D[0];
        }
        if (teleporterSet2 == null) {
            teleporterSet2 = new Collider2D[0];
        }
        if (teleporterSet3 == null) {
            teleporterSet3 = new Collider2D[0];
        }

        // if (rbRotatorCCWs == null) {
        //     rbRotatorCCWs = new Rigidbody2D[0];
        // }
        // if (rbRotatorCWs == null) {
        //     rbRotatorCWs = new Rigidbody2D[0];
        // }
        // if (rbHorizontalFlips == null) {
        //     rbHorizontalFlips = new Rigidbody2D[0];
        // }
        // if (rbVerticalFlips == null) {
        //     rbVerticalFlips = new Rigidbody2D[0];
        // }
        // if (rbTeleporterSet1 == null) {
        //     rbTeleporterSet1 = new Rigidbody2D[0];
        // }
        // if (rbTeleporterSet2 == null) {
        //     rbTeleporterSet2 = new Rigidbody2D[0];
        // }
        // if (rbTeleporterSet3 == null) {
        //     rbTeleporterSet3 = new Rigidbody2D[0];
        // }

        // jagged array of tool collider flags
        // subarrays are for each type of tool
        colliderFlags = new bool[7][] {
            new bool[rotatorCCWs.Length],
            new bool[rotatorCWs.Length],
            new bool[horizontalFlips.Length],
            new bool[verticalFlips.Length],
            new bool[teleporterSet1.Length],
            new bool[teleporterSet2.Length],
            new bool[teleporterSet3.Length]
        };

        for (int type = 0; type < colliderFlags.Length; type++) {
            for (int tool = 0; tool < colliderFlags[type].Length; tool++) {
                colliderFlags[type][tool] = false;
            }
        }

        RotatorCCW = new ToolBehavior(RotatorCCWBehavior);
        RotatorCW = new ToolBehavior(RotatorCWBehavior);
        HorizontalFlip = new ToolBehavior(HorizontalFlipBehavior);
        VerticalFlip = new ToolBehavior(VerticalFlipBehavior);
        Teleporter = new ToolBehavior(TeleporterBehavior);
        
    }

    // Update is called once per frame
    // use this to handle inputs
    void Update()
    {
        // this section handles whether the movement should start or stop
        if (Input.GetKeyDown("space") && !isMoving) {
            movement = directions[currentDirection];
            //UnityEngine.Debug.Log("starting");
            isMoving = true;
        }
        else if (Input.GetKeyDown("space") && isMoving) {
            movement = Vector2.zero;
            //UnityEngine.Debug.Log("stopping");
            isMoving = false;

            // tried using 
            //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            // instead but it wouldn't do anything

            rb.position = startPosition;
            //UnityEngine.Debug.Log("moved to start");

            foundDucky = false;
            ducky.SetActive(true);

            currentDirection = Array.FindIndex(directions, d => d == startDirection);
        }

        if (playerCollider.bounds.Intersects(duckyCollider.bounds) && !foundDucky) {
            foundDucky = true;
            ducky.SetActive(false);
            UnityEngine.Debug.Log("Ducky");
        }

        // collision with bread
        if (playerCollider.bounds.Intersects(breadCollider.bounds) && foundDucky) {
            UnityEngine.Debug.Log("Bread");
            // this value should be 11
            SceneManager.LoadScene(6);
        }

        if (isMoving) {
            // checking tool collisions using the collider lists, flag lists, and behavior delegates
            checkToolCollision(rotatorCCWs, colliderFlags[0], RotatorCCW);
            checkToolCollision(rotatorCWs, colliderFlags[1], RotatorCW);
            checkToolCollision(horizontalFlips, colliderFlags[2], HorizontalFlip);
            checkToolCollision(verticalFlips, colliderFlags[3], VerticalFlip);
            checkToolCollision(teleporterSet1, colliderFlags[4], Teleporter);
            checkToolCollision(teleporterSet2, colliderFlags[5], Teleporter);
            checkToolCollision(teleporterSet3, colliderFlags[6], Teleporter);

            if (isMoving && movement != directions[currentDirection]) {
                movement = directions[currentDirection];
            }
        }

    }

    // fixed update function that runs consistently
    // use this for updating sprite positions
    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Animate();
    }

    void Animate() {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }

    // delegate for the behavior of each tool.
    // many of the tools don't need the collider, but teleporter will
    // teleporter may cause more problems with linking
    delegate void ToolBehavior(Collider2D[] cols, bool[] flags, Collider2D col); 

    void RotatorCCWBehavior(Collider2D[] cols, bool[] flags, Collider2D col) {
        currentDirection--;
        if (currentDirection < 0) {
            currentDirection = directions.Length - 1;
        }
        rb.position = col.bounds.center;
        //UnityEngine.Debug.Log("rotated counterclockwise to index " + currentDirection);
    }

    void RotatorCWBehavior(Collider2D[] cols, bool[] flags, Collider2D col) {
        currentDirection++;
        if (currentDirection > directions.Length - 1) {
            currentDirection = 0;
        }
        rb.position = col.bounds.center;
        //UnityEngine.Debug.Log("rotated clockwise to index " + currentDirection);
    }

    void HorizontalFlipBehavior(Collider2D[] cols, bool[] flags, Collider2D col) {
        if (currentDirection == 0) {
            currentDirection = 2;
        }
        else if (currentDirection == 2) {
            currentDirection = 0;
        }
        rb.position = col.bounds.center;
    }

    void VerticalFlipBehavior(Collider2D[] cols, bool[] flags, Collider2D col) {
        if (currentDirection == 1) {
            currentDirection = 3;
        }
        else if (currentDirection == 3) {
            currentDirection = 1;
        }
        rb.position = col.bounds.center;
    }

    void TeleporterBehavior(Collider2D[] cols, bool[] flags, Collider2D col) {
        //  getting the indices of the tp that was activated and the one that the player should be taken to
        int steppedOn = Array.FindIndex(cols, tp => tp == col);
        int notSteppedOn = 1 - steppedOn;

        // flipping the collided flags when the teleport occurs
        flags[steppedOn] = false;
        flags[notSteppedOn] = true; 

        // setting the player position to the center of the collider for the other portal
        rb.position = cols[notSteppedOn].bounds.center;
    }

    void checkToolCollision (Collider2D[] cols, bool[] flags, ToolBehavior behavior) {
        for (int i = 0; i < cols.Length; i++) {
            // if the player is colliding with the tool collider, change the flag and do the behavior
            if (playerCollider.bounds.Intersects(cols[i].bounds) && !flags[i]) {
                //UnityEngine.Debug.Log("collided");
                flags[i] = true;
                behavior(cols, flags, cols[i]);
            }
            else if (playerCollider.bounds.Intersects(cols[i].bounds) && flags[i]) {
                //UnityEngine.Debug.Log("currently colliding");
                // this isn't supposed to do anything, I just wanted the else to be different
            }
            else {
                flags[i] = false;
            }
        }    
    }


    void movePlayer(Vector2 target, bool rbPos) {
        if (rbPos) {
            rb.position = new Vector2(target.x - (float) 0.5, target.y);
        }
        else {
            rb.position = target;
        }
    }
}
