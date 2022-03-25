using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player[] players = {new Player(), new Player(), new Player(), new Player()};
    
    // Start is called before the first frame update
    void Awake()
    {
        InitializeLists();
    }

    // Update is called once per frame
    void Start()
    {
        
    }

    private void InitializeLists()
    {
        foreach (var player in players)
        {
            player.discardedTiles = new List<Tile>();
            player.tilesObjects = new List<GameObject>();
        }
    }

    public void CheckPlayerPon(Player player, Tile discardedTile, int playerIndex)
    {
        switch (player.hand.Count(t => t.tileName == discardedTile.tileName))
        {
            case 2:
                Debug.Log("Player " + playerIndex + ": PON!");
                break;
            
            case 3:
                Debug.Log("Player " + playerIndex + ": KAN!");
                break;
        }
    }

    public void CheckPlayerChi(Player player, Tile discardedTile, int playerIndex)
    {
        string tileNameNoDigits;
        List<Tile> suitTiles = new List<Tile>();
        tileNameNoDigits = Regex.Replace(discardedTile.tileName, @"[\d-]", string.Empty);

        foreach (var tile in player.hand)
        {
            if (tile.tileName.Contains(tileNameNoDigits)) suitTiles.Add(tile);
        }

        if (ContainsChi(suitTiles, discardedTile)) Debug.Log("Player " + playerIndex + ": CHI!");
    }

    private bool ContainsChi(List<Tile> tiles, Tile discardedTile)
    {
        return false;
    }
}
