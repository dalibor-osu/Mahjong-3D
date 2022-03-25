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
        List<Tile> suitTiles;

        switch (discardedTile)
        {
            case Tile.Bamboo bamboo:
                suitTiles = player.hand.Where(t => t is Tile.Bamboo).ToList();
                break;

            case Tile.Man man:
                suitTiles = player.hand.Where(t => t is Tile.Man).ToList();
                break;

            case Tile.Circle circle:
                suitTiles = player.hand.Where(t => t is Tile.Circle).ToList();
                break;

            default:
                return;
        }

        int chiCount = ContainsChi(suitTiles, ((Tile.SuitedTile) discardedTile).suitRank);
        if (chiCount > 0)
        {
            Debug.Log("Player " + playerIndex + " can call CHI! " + chiCount + " times!");
        }
    }

    private int ContainsChi(List<Tile> tiles, int discardedTileRank)
    {
        List<int> ranks = new List<int>();
        int chiCount = 0;

        foreach (var tile in tiles)
        {
            ranks.Add(((Tile.SuitedTile) tile).suitRank);
        }

        if (ranks.Contains(discardedTileRank - 2) && ranks.Contains(discardedTileRank - 1)) chiCount++;
        if (ranks.Contains(discardedTileRank - 1) && ranks.Contains(discardedTileRank + 1)) chiCount++;
        if (ranks.Contains(discardedTileRank + 1) && ranks.Contains(discardedTileRank + 2)) chiCount++;

        return chiCount;
    }

    public void CallPon(Tile ponTile, Player player)
    {

    }

    public void CallChi()
    {

    }

    public void CallKan()
    {

    }

    public void CallRon()
    {

    }

    public void CallTsumo()
    {

    }

    public void CallDraw()
    {
        
    }

}
