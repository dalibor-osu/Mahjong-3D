using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileProperties : MonoBehaviour
{
    public Tile tile;
    public string tileName;
    public int tileNumber;
    public int suitRank;
    public char type;
    public bool isHonor = false;
    public bool isSuited = false;
    public bool isWind = false;
    public bool isDragon = false;
    public bool isRedFive = false;
    public bool isTerminal = false;
    public bool isGreen = false;
    public bool isDiscarded = false;
    public int playerIndex;
    private TileManager tileManager;
    private PlayerManager playerManager;
    private GameManager gameManager;
    private Vector3 originalScale;
    private Vector3 mouseOverScale;
    private RawImage image;
    private float scale = 1.1f;
    private bool isEnemy = true;
    private RoundManager roundManager;

    void Awake()
    {
        GameObject gameManagerObj = GameObject.Find("GameManager");
        tileManager = gameManagerObj.GetComponent<TileManager>();
        playerManager = gameManagerObj.GetComponent<PlayerManager>();
        gameManager = gameManagerObj.GetComponent<GameManager>();
        roundManager = gameManagerObj.GetComponent<RoundManager>();
        
        originalScale = transform.localScale;
        mouseOverScale = originalScale * scale;
        playerManager.players[playerIndex].tilesObjects.Add(gameObject);
    }
    void Start()
    {
        SetImage();
        
        // if (playerIndex == 0) isEnemy = false;
        // if(!isEnemy) SetImage();
    }

    private void OnMouseDown()
    {
        if (isDiscarded || playerIndex != 0 || roundManager.gameEnd || roundManager.currentPlayer != playerIndex) return;
        tileManager.DiscardTile(tile, gameObject, playerIndex);
        MoveDiscarded();
    }

    public void MoveDiscarded()
    {
        isDiscarded = true;
        transform.position = GetDiscardPosition();
        RotateDiscarded();
        
        if (playerIndex != 0) SetImage();
    }

    private void OnMouseEnter()
    {
        if (isDiscarded || playerIndex != 0 || roundManager.gameEnd) return;
        transform.localScale = mouseOverScale;
    }

    private void OnMouseExit()
    {
        transform.localScale = originalScale;
    }

    public void AssignProperties()
    {
        tileName = tile.tileName;
        tileNumber = tile.tileNumber;
        
        switch (tile)
        {
            case Tile.Dragon dragon:
                isHonor = true;
                isDragon = true;
                type = dragon.dragonType;
                break;
            
            case Tile.Wind wind:
                isHonor = true;
                isWind = true;
                type = wind.windType;
                break;
            
            case Tile.SuitedTile suitedTile:
            {
                isSuited = true;
                isRedFive = suitedTile.isRedFive;
                isTerminal = suitedTile.isTerminal;

                if (suitedTile is Tile.Bamboo) isGreen = ((Tile.Bamboo) tile).isGreen;
                break;
            }
        }
    }

    private void SetImage()
    {
        image = gameObject.GetComponentInChildren<RawImage>();
        string dora = "";
        if (tile is Tile.SuitedTile)
        {
            if (((Tile.SuitedTile) tile).isRedFive) dora = "Dora";
        }

        Texture sprite = Resources.Load<Texture>($"Tiles/{tile.tileName}{dora}");
        image.texture = sprite;
    }

    private Vector3 GetDiscardPosition()
    {
        int discardedCount = playerManager.players[playerIndex].discardedTiles.Count - 1;
        int row = discardedCount / 6 + 1;
        int rowNumber = discardedCount - row * 6;

        if (row > 3)
        {
            row = 3;
            rowNumber += 6;
        }

        float positionX = tileManager.discardPileStart.x + tileManager.discardPileOffset.x * rowNumber;
        float positionZ = tileManager.discardPileStart.z + tileManager.discardPileOffset.z * row;
        Vector3 pos = new Vector3(positionX, transform.position.y, positionZ);
        return gameManager.RotatePointAroundPivot(pos, Vector3.zero, new Vector3(0, 90 * playerIndex, 0));
    }

    private void RotateDiscarded()
    {
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0, 0 + 90 * playerIndex, 0);
        transform.rotation = rotation;
    }
}
