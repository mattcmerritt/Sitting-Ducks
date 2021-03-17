using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector2 currentVelocity;
    public Rigidbody2D rb;
    //private Tile[] tiles;   // may be unnecessary, included to show how tiles can be better loaded into player
    private Vector3 startingPosition;

    private void Start()
    {
        currentVelocity = Vector2.zero; // start off not moving
        startingPosition = transform.position;
    }

    private void Update()
    {
        rb.velocity = currentVelocity; // constantly update the duck's velocity
    }

    // used to start the duck at first with positive horizontal speed
    public void setInitialVelocity(float speed)
    {
        currentVelocity = new Vector2(speed, 0); // always starts by moving to the right
    }

    // used to modify the duck's velocity once it has already started moving
    public void setVelocity(float x, float y)
    {
        currentVelocity = new Vector2(x, y);
    }

    // used to access duck's velocity
    public Vector2 getCurrentVelocity()
    {
        return currentVelocity;
    }

    public void stop() 
    {
        currentVelocity = Vector2.zero;
        transform.position = startingPosition;
    }

    public bool hasPassedCenter(Collider2D collider)
    {
        // if coming in from the left
        if (currentVelocity.x > 0)
            return transform.position.x >= collider.transform.position.x;
        // if coming in from the right
        else if (currentVelocity.x < 0)
            return transform.position.x <= collider.transform.position.x;

        // if coming from bottom
        if (currentVelocity.y > 0)
            return transform.position.y >= collider.transform.position.y;
        // if coming from top
        else if (currentVelocity.y < 0)
            return transform.position.x <= collider.transform.position.x;

        // not moving, not a case to catch
        return false;
    }

    // unlikely to catch anything with tiles, unless game is running extremely fast
    // also handles collecting the ducklings and bread
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tile currentTile = collision.GetComponent<Tile>();

        if (currentTile != null)
        {
            if (!currentTile.getTileUsed() && this.hasPassedCenter(collision))
            {
                currentTile.actOnPlayer(this);
            }
        }
        else
        {
            if (collision.tag == "Duckling")
            {
                collision.gameObject.SetActive(false);
            }
            else if (collision.tag == "Bread")
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

    // checks to see if the collision is with a tile, if the tile has not been used, and if the player has passed the center of the tile
    // if so, the tile will act on the player
    private void OnTriggerStay2D(Collider2D collision)
    {
        Tile currentTile = collision.GetComponent<Tile>();

        if (currentTile != null)
        {
            if (!currentTile.getTileUsed() && this.hasPassedCenter(collision))
            {
                currentTile.actOnPlayer(this);
            }
        }
    }

    // checks to see if the player is leaving a tile
    // if so, marks the tile as active again
    private void OnTriggerExit2D(Collider2D collision)
    {
        Tile currentTile = collision.GetComponent<Tile>();
        if (currentTile != null)
        {
            currentTile.setTileUsed(false);
        }
    }
}
