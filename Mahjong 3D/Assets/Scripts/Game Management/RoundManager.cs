using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public TileManager tileManager;
    public bool discarded;
    public int currentPlayer;

    public bool gameEnd = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tileManager.tiles.Count <= 0 && !gameEnd && discarded)
        {
            Debug.Log("Game ended.");
            gameEnd = true;
        }
        
        if (discarded && !gameEnd)
        {
            NextPlayer();
        }
    }

    public void StartGame(int dealerIndex)
    {
        currentPlayer = dealerIndex;
        GameObject drawnTile = tileManager.DrawTile(currentPlayer);
        
        if (currentPlayer != 0)
        {
            tileManager.DiscardTile(drawnTile.GetComponent<TileProperties>().tile, drawnTile, currentPlayer);
        }
    }

    private void NextPlayer()
    {
        if (--currentPlayer < 0) currentPlayer = 3;

        GameObject drawnTile = tileManager.DrawTile(currentPlayer);
        discarded = false;

        if (currentPlayer != 0)
        {
            StartCoroutine(Wait(0.5f, drawnTile));
        }
    }

    private IEnumerator Wait(float seconds, GameObject drawnTile)
    {
        yield return new WaitForSecondsRealtime(seconds);
        
        tileManager.DiscardTile(drawnTile.GetComponent<TileProperties>().tile, drawnTile, currentPlayer);
    }
}
