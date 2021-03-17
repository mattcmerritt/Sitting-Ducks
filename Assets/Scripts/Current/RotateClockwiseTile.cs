using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateClockwiseTile : Tile
{
    public override void actOnPlayer(Player duck)
    {
        base.actOnPlayer(duck);
        duck.setVelocity(duck.getCurrentVelocity().y, -duck.getCurrentVelocity().x);
    }
}
