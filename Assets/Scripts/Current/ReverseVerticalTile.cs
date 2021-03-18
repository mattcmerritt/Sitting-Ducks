using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseVerticalTile : Tile
{
    public override void actOnPlayer(Player duck)
    {
        base.actOnPlayer(duck);
        duck.setVelocity(duck.getCurrentVelocity().x, -duck.getCurrentVelocity().y);
    }
}
