using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour, ITileMapManager
{
    [SerializeField] private List<Tilemap> easyRitualTileMaps;
    [SerializeField] private Tilemap playerTilemap;
    [SerializeField] private List<Tile> tileList;
    [SerializeField] private Tilemap resultTileMap;
    private Tilemap currentTilemap;

    private void Start()
    {
        playerTilemap.gameObject.SetActive(false);
        resultTileMap.gameObject.SetActive(false);
    }

    public void DisplayNewSummoningShape()
    {
        currentTilemap = DetermineRandomTilemap();
        currentTilemap.gameObject.SetActive(true);
    }

    private Tilemap DetermineRandomTilemap()
    {
        int randomIndex = Random.Range(0, easyRitualTileMaps.Count - 1);
        return easyRitualTileMaps[randomIndex];
    }

    public void HideSummoningShape()
    {
        currentTilemap.gameObject.SetActive(false);
        easyRitualTileMaps.Remove(currentTilemap);
    }

    public void DisplayPlayerTileMap()
    {
        playerTilemap.gameObject.SetActive(true);
    }


    public void HidePlayerTileMap()
    {
        playerTilemap.gameObject.SetActive(false);
        playerTilemap.ClearAllTiles();
    }

    public void DisplayResultTileMap(List<PaintedTile> paintedPlayerTiles, HashSet<PaintedTile> correctTiles)
    {
        foreach (var paintedTile in paintedPlayerTiles)
        {
            if (correctTiles.Contains(paintedTile))
            {
                resultTileMap.SetTile(paintedTile.Position, tileList.Find(tile => tile.name == "green_tile"));
            }
            else
            {
                resultTileMap.SetTile(paintedTile.Position, tileList.Find(tile => tile.name == "red_tile"));
            }
        }
    }


    public void HideResultTileMap()
    {
        resultTileMap.gameObject.SetActive(false);
        resultTileMap.ClearAllTiles();
    }

    public Tilemap GetCurrentTilemap()
    {
        return currentTilemap;
    }
}
