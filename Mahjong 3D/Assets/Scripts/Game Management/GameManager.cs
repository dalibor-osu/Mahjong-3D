using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public TileManager tileManager;
    public GameObject tilePrefab;
    public RoundManager roundManager;
    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector3 tileOffset;
    private Vector3 nextPlayerStartPosition;
    private Quaternion nextPlayerStartRotation;

    private int eastPlayer;
    // Start is called before the first frame update
    private void Awake()
    {
        nextPlayerStartRotation = startRotation;
        nextPlayerStartPosition = startPosition;
        DecideSeatWinds();
        DrawPlayersHands();
        SpawnPlayerTiles();
        SetWindImages();
    }

    void Start()
    {
        roundManager.StartGame(eastPlayer);
    }

    private void DrawPlayersHands()
    {
        foreach (var player in playerManager.players)
        {
            player.hand = tileManager.DrawStartingHand();
        }
    }

    private void DecideSeatWinds()
    {
        int eastIndex = Random.Range(0, 4);
        eastPlayer = eastIndex;
        
        char[] winds = {'E', 'S', 'W', 'N'};

        for (int i = 0; i < 4; i++)
        {
            playerManager.players[eastIndex].seatWind = winds[i];
            eastIndex++;

            if (eastIndex > 3) eastIndex = 0;
        }
    }

    private void SpawnPlayerTiles()
    {
        int i = 0;
        int playerIndex = 0;
        foreach (Player player in playerManager.players)
        {
            foreach (var tile in player.hand)
            {
                GameObject newTile = Instantiate(tilePrefab, nextPlayerStartPosition + tileOffset * i, nextPlayerStartRotation);
                TileProperties prop = newTile.GetComponent<TileProperties>();
                player.tilesObjects.Add(newTile);
                prop.transform.Rotate(new Vector3(0, 1, 0), 90 * playerIndex);
                prop.tile = tile;
                prop.playerIndex = playerIndex;
                prop.AssignProperties();
                newTile.name = $"Tile ({tile.tileName})";
                i++;
            }

            i = 0;
            playerIndex++;
            tileOffset = RotatePointAroundPivot(tileOffset, Vector3.zero, new Vector3(0, 90, 0));
            nextPlayerStartPosition = RotatePointAroundPivot(nextPlayerStartPosition, new Vector3(0, nextPlayerStartPosition.y, 0), new Vector3(0, 90, 0));
        }
        
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
    
    public void SortPlayerHand(int playerIndex, GameObject discardedTile)
    {
        Player player = playerManager.players[playerIndex];
        int i = 0;
        
        foreach (GameObject tilesObject in player.tilesObjects)
        {
            if (tilesObject == discardedTile || tilesObject.GetComponent<TileProperties>().playerIndex != playerIndex) continue;
            Destroy(tilesObject);
        }
        
        player.tilesObjects.RemoveRange(0, player.tilesObjects.Count);
        
        foreach (var tile in player.hand)
        {
            GameObject newTile = Instantiate(tilePrefab, nextPlayerStartPosition + tileOffset * i, nextPlayerStartRotation);
            TileProperties prop = newTile.GetComponent<TileProperties>();
            player.tilesObjects.Add(newTile);
            prop.transform.Rotate(new Vector3(0, 1, 0), 90 * playerIndex);
            prop.tile = tile;
            prop.playerIndex = playerIndex;
            prop.AssignProperties();
            newTile.name = $"Tile ({tile.tileName.Split('_')[0]})";
            i++;
        }
    }

    private void SetWindImages()
    {
        char[] winds = {'E', 'S', 'W', 'N'};
        int playerIndex = eastPlayer;
        
        foreach (var wind in winds)
        {
            RawImage image = GameObject.Find($"Player{playerIndex}SeatWind").GetComponentInChildren<RawImage>();
            Texture sprite = Resources.Load<Texture>($"Tiles/Wind{wind}");

            image.texture = sprite;
            if (--playerIndex < 0) playerIndex = 3;
        }
    }

    public void CheckPlayerCall(int playerDiscarded, Tile discardedTile)
    {
        int playerIndex = 0;
        int chiPlayer = playerDiscarded - 1 < 0 ? 3 : playerDiscarded - 1;
        
        foreach (var player in playerManager.players)
        {
            if (playerIndex == playerDiscarded)
            {
                playerIndex++;
                continue;
            }
            playerManager.CheckPlayerPon(player, discardedTile, playerIndex);
            playerIndex++;
        }
        
        if (discardedTile is Tile.SuitedTile) playerManager.CheckPlayerChi(playerManager.players[chiPlayer], discardedTile, chiPlayer);
    }
}
