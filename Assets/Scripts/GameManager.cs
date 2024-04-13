using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject tileMapManager;
    [SerializeField] private GameObject playerTileMap;
    [SerializeField] private GameObject playerPainter;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private List<Sprite> backgroundImages;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float playerPaintTime = 20f;
    [SerializeField] private float resultDisplayTime = 10f;
    [SerializeField] private int numberOfSigils = 0;
    private Tilemap currentTilemap;
    private ITileMapManager tileMapmanager;
    private HashSet<PaintedTile> correctlyPaintedTiles = new HashSet<PaintedTile>();
    private List<PaintedTile> playerPaintedTiles = new List<PaintedTile>();
    public delegate void CorrectPercentageEventHandler(float percentage);
    public event CorrectPercentageEventHandler OnDeterminedCorrectPercentage;
    private float correctPercentage;

    public delegate void ScoreChangeEventHandler(int score);
    public event ScoreChangeEventHandler OnScoreChange;

    public delegate void TimerEventHandler(float playerPaintTime);
    public event TimerEventHandler OnTimerStart;

    public delegate void EndOfRitualsEventHandler();
    public event EndOfRitualsEventHandler OnEndOfRituals;

    public delegate void RemoveSigilAccuracyTextEventHandler();
    public event RemoveSigilAccuracyTextEventHandler OnRemoveSigilAccuracyText;

    void Start()
    {
        tileMapmanager = tileMapManager.GetComponent<ITileMapManager>();
        StartCoroutine(StartRituals());
    }

    IEnumerator StartRituals()
    {
        correctlyPaintedTiles.Clear();
        playerPaintedTiles.Clear();

        if (numberOfSigils < 5)
        {
            numberOfSigils++;
            StartCoroutine(DisplayRitual());
        }
        else
        {
            //TODO Display Score Screen, either restart or next level
            OnEndOfRituals?.Invoke();
            Debug.Log("Score Screen");
            yield return null;
        }
    }

    IEnumerator DisplayRitual()
    {
        tileMapmanager.DisplayNewSummoningShape();
        currentTilemap = tileMapmanager.GetCurrentTilemap();
        GetPaintedTilesFromMap(correctlyPaintedTiles, currentTilemap);

        yield return new WaitForSeconds(displayTime);

        tileMapmanager.HideSummoningShape();
        StartCoroutine(PlayerPaintingPhase());
    }

    IEnumerator PlayerPaintingPhase()
    {
        tileMapmanager.DisplayPlayerTileMap();
        OnTimerStart?.Invoke(playerPaintTime);

        var player = playerPainter.GetComponent<IPlayerPainter>();
        player.playerIsAllowedToPaint(true);

        yield return new WaitForSeconds(playerPaintTime);

        player.playerIsAllowedToPaint(false);
        GetPaintedTilesFromMap(playerPaintedTiles, playerTileMap.GetComponent<Tilemap>());

        StartCoroutine(DisplayResult());
    }

    IEnumerator DisplayResult()
    {
        correctPercentage = CompareArrays(correctlyPaintedTiles, playerPaintedTiles);
        OnDeterminedCorrectPercentage?.Invoke(correctPercentage);
        OnScoreChange?.Invoke((int)correctPercentage);
        Debug.Log($"Correctly painted: {correctPercentage:0.00}%");
        backgroundImage.GetComponent<Image>().sprite = backgroundImages[numberOfSigils - 1];

        tileMapmanager.DisplayResultTileMap(playerPaintedTiles, correctlyPaintedTiles);

        yield return new WaitForSeconds(resultDisplayTime);

        tileMapmanager.HideResultTileMap();
        OnRemoveSigilAccuracyText?.Invoke();

        StartCoroutine(StartRituals());
    }


    private void GetPaintedTilesFromMap<TContainer>(TContainer container, Tilemap tilemap) where TContainer : ICollection<PaintedTile>
    {
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(pos))
                {
                    TileBase currentTile = tilemap.GetTile(pos);
                    container.Add(new PaintedTile(currentTile, pos));
                }
            }
        }
    }

    public static float CompareArrays(HashSet<PaintedTile> correctTileContainer, List<PaintedTile> playerTileContainer)
    {
        if (correctTileContainer.Count == 0)
        {
            return 0;
        }

        int totalElements = correctTileContainer.Count;
        Debug.Log($"Total elements: {totalElements}");
        int correctElements = 0;
        int incorrectElements = 0;

        foreach (var item in playerTileContainer)
        {
            if (correctTileContainer.Any(x => x.IsCorrectlyPainted(item)))
            {
                correctElements++;
            }
            else
            {
                bool isPainted = false;
                for (int x = -2; x <= 2; x++)
                {
                    for (int y = -2; y <= 2; y++)
                    {
                        Vector3Int pos = new Vector3Int(item.Position.x + x, item.Position.y + y, item.Position.z);
                        if (correctTileContainer.Any(x => x.IsCorrectlyPainted(new PaintedTile(item.Tile, pos))))
                        {
                            isPainted = true;
                            break;
                        }
                    }
                    if (isPainted)
                    {
                        break;
                    }
                }
                if (!isPainted)
                {
                    incorrectElements++;
                }
            }
        }

        Debug.Log($"Correct: {correctElements}, Incorrect: {incorrectElements}");
        var matchedElements = correctElements - incorrectElements;

        if (matchedElements <= 0)
        {
            return 0f;
        }
        else
        {
            return (float)matchedElements / totalElements * 100f;
        }
    }
}
