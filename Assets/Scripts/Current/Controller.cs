﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private Player duck;
    private bool isWalking;

    private void Start()
    {
        Tile[] tiles = Object.FindObjectsOfType<Tile>();    // saving all tiles in the stage to an array
        duck = FindObjectOfType<Player>();                  // saving reference to player
        duck.addTiles(tiles);                               // passing the array of tiles to the player so that they can used later
        isWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isWalking)
            {
                duck.setInitialVelocity(5);  // having the duck start moving
                isWalking = true;
            }
            else
            {
                duck.stop();  // having the duck start moving
                isWalking = false;
            }
        }
    }
}
