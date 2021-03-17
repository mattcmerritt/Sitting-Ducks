using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector2 currentVelocity;
    public Rigidbody2D rb;
    private Tile[] tiles;   // may be unnecessary, included to show how tiles can be better loaded into player
    private Vector3 startingPosition;

    private void Start()
    {
        currentVelocity = Vector2.zero; // start off not moving
        startingPosition = transform.position;
    }

    private void Update()
    {
        rb.velocity = currentVelocity;
    }

    public void setInitialVelocity(float speed)
    {
        currentVelocity = new Vector2(speed, 0); // always starts by moving to the right
    }

    public void stop() 
    {
        currentVelocity = Vector2.zero;
        transform.position = startingPosition;
    }

    public void addTiles(Tile[] tiles)
    {
        this.tiles = tiles;
        printTiles();
    }

    public void printTiles()
    {
        foreach (Tile tile in tiles)
        {
            Debug.Log(tile.gameObject.name);
        }
    }
}
