using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToolSelection : MonoBehaviour
{

    
    // loading in all of the game objects
    public GameObject[] tools;
    private int currentIndex = -1;

    // array to be built in start
    private Rigidbody2D[] rbs;
    private Collider2D[] colliders;
    private Vector2 startPosition;

    
    // Start is called before the first frame update
    void Start()
    {
        rbs = new Rigidbody2D[tools.Length];
        colliders = new Collider2D[tools.Length];
        for (int i = 0; i < tools.Length; i++) {
            rbs[i] = tools[i].GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
            colliders[i] = tools[i].GetComponent(typeof(Collider2D)) as Collider2D;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            if (currentIndex != -1) {
                PutDown(tools[currentIndex]);
            }
        }

        // if left clicked
        if (Input.GetMouseButtonDown(0)) {
            UnityEngine.Debug.Log("Clicked");

            Vector2 clickLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            for (int i = 0; i < tools.Length; i++) {
                if (colliders[i].bounds.Contains(clickLoc)) {

                    if (currentIndex == -1) {
                        currentIndex = i;
                        PickUp(tools[currentIndex]);
                    }
                    else {
                        PutDown(tools[currentIndex]);
                    }
                    
                }
            }

            // ClickBehavior(tools[currentIndex]);
        }
    }

    void FixedUpdate() {
        moveObject();
    }

    // 
    void PickUp(GameObject tool) {
        UnityEngine.Debug.Log("pickup index " + currentIndex);
        currentIndex = Array.FindIndex(tools, t => t == tool);
        startPosition = rbs[currentIndex].position;
    }

    void PutDown(GameObject tool) {
        UnityEngine.Debug.Log("putdown " + currentIndex);

        //UnityEngine.Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2 placement = new Vector2(
            (float) (Math.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + 0.5)), 
            (float) (Math.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y) + 0.5)
        );
        //UnityEngine.Debug.Log(placement);

        bool tileInUse = false;
        for (int i = 0; i < tools.Length; i++) {

            // UnityEngine.Debug.Log("placement " + placement);
            // UnityEngine.Debug.Log("rbs" + i + " " + rbs[i].position);

            if (rbs[i].position == placement && i != currentIndex) {
                tileInUse = true;
                UnityEngine.Debug.Log("something here");
            }
        }

        if (!tileInUse) {
            //UnityEngine.Debug.Log("Moving");
            rbs[currentIndex].position = placement;
            currentIndex = -1;
        }
        else {
            //UnityEngine.Debug.Log("Defaulting");
            rbs[currentIndex].position = startPosition;
            currentIndex = -1;
        }
    }

    // ClickBehavior is the method that should be run on click in the editor
    public void ClickBehavior(GameObject tool) {
        // pick it up if it has been selected
        if (currentIndex == -1) {
            PickUp(tool);            
        } 
        // put the object down if it has already been selected
        else if (tool == tools[currentIndex]) {
            PutDown(tool);
        } 
        // UnityEngine.Debug.Log(tool);
        // UnityEngine.Debug.Log(currentIndex);
    }

    void moveObject() {
        if (currentIndex != -1) {
            // move the positition of tools[currentIndex] to the mouse position
            rbs[currentIndex].position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
