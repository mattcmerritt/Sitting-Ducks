using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCounterclockwiseTile : Tile
{
    public override void actOnPlayer(Player duck)
    {
        base.actOnPlayer(duck);
        duck.setVelocity(-duck.getCurrentVelocity().y, duck.getCurrentVelocity().x);
    }
}
