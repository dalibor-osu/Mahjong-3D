using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<Tile> tiles;
    public List<Tile> discardedTiles;
    public List<Tile> deadTiles;

    public PlayerManager playerManager;
    public GameManager gameManager;
    public RoundManager roundManager;
    private TileOrder tileOrder;

    public Vector3 discardPileStart;
    public Vector3 discardPileOffset;

    public Vector3 drawnTileOffset;

    private void Awake()
    {
        tiles = new List<Tile>();
        discardedTiles = new List<Tile>();
        deadTiles = new List<Tile>();
        tileOrder = new TileOrder();
        
        LoadDragons();
        LoadWinds();
        LoadSuited();
        DrawDeadWall();
    }
    
    private void Update()
    {
        
    }

    private void LoadDragons()
    {
        char[] dragons = {'R', 'G', 'W'};
        foreach (var dragon in dragons)
        {
            for (int i = 0; i < 4; i++)
            {
                Tile tile = new Tile.Dragon(dragon, i + 1);
                tiles.Add(tile);
            }
        }
    }

    private void LoadWinds()
    {
        char[] winds = {'E', 'S', 'W', 'N'};
        foreach (var wind in winds)
        {
            for (int i = 0; i < 4; i++)
            {
                Tile tile = new Tile.Wind(wind, i + 1);
                tiles.Add(tile);
            }
        }
    }

    private void LoadSuited()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Tile newBamboo = new Tile.Bamboo(i + 1, j + 1);
                Tile newCircle = new Tile.Circle(i + 1, j + 1);
                Tile newMan = new Tile.Man(i + 1, j + 1);
                
                tiles.AddRange(new[] {newBamboo, newCircle, newMan});
            }
        }
    }

    private void DrawDeadWall()
    {
        for (int i = 0; i < 14; i++)
        {
            int index = Random.Range(0, tiles.Count);
            deadTiles.Add(tiles[index]);
            tiles.RemoveAt(index);
        }
    }

    public List<Tile> DrawStartingHand()
    {
        List<Tile> hand = new List<Tile>();

        for (int i = 0; i < 13; i++)
        {
            int index = Random.Range(0, tiles.Count);
            hand.Add(tiles[index]);
            tiles.RemoveAt(index);
        }
        
        return SortHand(hand);
    }
    
    public GameObject DrawTile(int playerIndex)
    {
        int index = Random.Range(0, tiles.Count);
        Tile newTile = tiles[index];
        Vector3 drawnTilePos = gameManager.RotatePointAroundPivot(drawnTileOffset, new Vector3(0, drawnTileOffset.y, 0), new Vector3(0, 90 * playerIndex, 0));
        playerManager.players[playerIndex].hand.Add(newTile);
        GameObject newTileObject = Instantiate(gameManager.tilePrefab, drawnTilePos, gameManager.startRotation);
        TileProperties prop = newTileObject.GetComponent<TileProperties>();
        prop.playerIndex = playerIndex;
        prop.transform.Rotate(new Vector3(0, 1, 0), 90 * (playerIndex + 1));
        prop.tile = newTile;
        prop.AssignProperties();
        newTileObject.name = $"Tile ({newTile.tileName})";
        tiles.RemoveAt(index);
        return newTileObject;
    }

    public void DiscardTile(Tile discardedTile, GameObject tilePrefab, int playerIndex)
    {
        discardedTiles.Add(discardedTile);
        Player player = playerManager.players[playerIndex];
        player.discardedTiles.Add(discardedTile);
        player.hand.RemoveAt(player.hand.IndexOf(discardedTile));
        player.hand = SortHand(player.hand);
        
        if (playerIndex != 0)
        {
            tilePrefab.GetComponent<TileProperties>().MoveDiscarded();
            roundManager.discarded = true;
        }
        else
        {
            gameManager.SortPlayerHand(playerIndex, tilePrefab);
            roundManager.discarded = true;
        }

        gameManager.CheckPlayerCall(playerIndex, discardedTile);
    }

    private List<Tile> SortHand(List<Tile> hand)
    {
        return hand.OrderByDescending(item => Enumerable.Reverse(tileOrder.list).ToList().IndexOf(item.tileName.Split('_')[0])).ToList();
    }
}
