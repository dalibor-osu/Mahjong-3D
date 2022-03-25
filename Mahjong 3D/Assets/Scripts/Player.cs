using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Playables;

public class Player
{
    public List<Tile> hand;
    public List<Tile> discardedTiles;
    public List<GameObject> tilesObjects;
    public char seatWind;
    public int points = 25000;
}
