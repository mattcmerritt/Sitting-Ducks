using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{

    private bool tileUsed;

    private void Start()
    {
        tileUsed = false;
    }

    // method with specific code related to how the tile influences the player
    public virtual void actOnPlayer(Player duck)
    {
        setTileUsed(true);
        Debug.Log("Acted on player!");
    }

    // method to check if the duck has already used this tile
    public bool getTileUsed()
    {
        return tileUsed;
    }

    // method to update or reset the tile based on if it has been used
    public void setTileUsed(bool tileUsed)
    {
        this.tileUsed = tileUsed;
    }


}
