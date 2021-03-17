using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private Player duck;
    private bool isWalking;
    private GameObject duckling;
    private GameObject bread;

    private void Start()
    {
        duck = FindObjectOfType<Player>();  // saving reference to player
        isWalking = false;
        duckling = GameObject.FindGameObjectWithTag("Duckling"); // saving reference to duckling
        bread = GameObject.FindGameObjectWithTag("Bread"); // saving reference to duckling

        if (duckling == null || bread == null)
        {
            Debug.LogError("Critical game objects not loaded.");
        }

        //Tile[] tiles = Object.FindObjectsOfType<Tile>();    // saving all tiles in the stage to an array
        //duck.addTiles(tiles);                               // passing the array of tiles to the player so that they can used later
    }

    // Update is called once per frame
    void Update()
    {
        if (bread.activeSelf == false && duckling.activeSelf == false)
        {
            Debug.Log("Level completed.");
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isWalking)
            {
                duck.setInitialVelocity(5);  // having the duck start moving
                isWalking = true;
            }
            else
            {
                duck.stop();
                isWalking = false;
                duckling.SetActive(true);
                bread.SetActive(true);
            }
        }
    }
}
