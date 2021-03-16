using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private Player duck;

    private void Start()
    {
        Tile[] tiles = Object.FindObjectsOfType<Tile>();    // saving all tiles in the stage to an array
        duck = FindObjectOfType<Player>();           // saving reference to player
        duck.addTiles(tiles);   // passing the array of tiles to the player so that they can used later
    }

    // Update is called once per frame
    void Update()
    {
        duck.setInitialVelocity(5);
    }
}
