using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private GameObject playerTileMap;
    [SerializeField] private Tile blackTile;
    [SerializeField] private Tile whiteTile;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float playerPaintTime = 20f;
    private Tilemap currentTilemap;
    private HashSet<PaintedTile> correctlyPaintedTiles = new HashSet<PaintedTile>();
    private List<PaintedTile> playerPaintedTiles = new List<PaintedTile>();
    public delegate void CorrectPercentageEventHandler(float percentage);
    public event CorrectPercentageEventHandler OnDeterminedCorrectPercentage;
    private float correctPercentage;

    void Start()
    {
        currentTilemap = DetermineRandomTilemap();
        GetPaintedTilesFromMap(correctlyPaintedTiles, currentTilemap);
        StartCoroutine(ChangeTilemap());
    }

    IEnumerator ChangeTilemap()
    {
        currentTilemap.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        currentTilemap.gameObject.SetActive(false);
        StartCoroutine(PlayerCanPaint());
    }

    IEnumerator PlayerCanPaint()
    {
        playerTileMap.SetActive(true);
        var player = playerTileMap.GetComponent<IPlayerPainter>();
        player.playerIsAllowedToPaint(true);

        yield return new WaitForSeconds(playerPaintTime);

        player.playerIsAllowedToPaint(false);
        GetPaintedTilesFromMap(playerPaintedTiles, playerTileMap.GetComponent<Tilemap>());
        correctPercentage = CompareArrays(correctlyPaintedTiles, playerPaintedTiles);
        OnDeterminedCorrectPercentage?.Invoke(correctPercentage);
        Debug.Log($"Correctly painted: {correctPercentage:0.00}%");
        //StartCoroutine(DisplayResult());
    }

    IEnumerator DisplayResult()
    {
        throw new NotImplementedException();
    }

    private Tilemap DetermineRandomTilemap()
    {
        int randomIndex = UnityEngine.Random.Range(0, tilemaps.Count - 1);
        return tilemaps[randomIndex];
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
            if (correctTileContainer.Contains(item))
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
                        if (correctTileContainer.Contains(new PaintedTile(null, pos)))
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
