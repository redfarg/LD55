using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour, ITileMapManager
{
    [SerializeField] private List<Tilemap> easyRitualTileMaps;
    [SerializeField] private AudioSource newRuneSound;
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
        newRuneSound.Play();

    }

    private Tilemap DetermineRandomTilemap()
    {
        int randomIndex = Random.Range(0, easyRitualTileMaps.Count - 1);
        return easyRitualTileMaps[randomIndex];
    }

    public void HideSummoningShape()
    {
        foreach (var position in currentTilemap.cellBounds.allPositionsWithin)
        {
            resultTileMap.SetTile(position, currentTilemap.GetTile(position));
        }
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
        playerTilemap.gameObject.SetActive(false);
        foreach (var tile in correctTiles)
        {
            resultTileMap.SetTile(tile.Position, tileList.Find(tile => tile.name == "red_tile"));
        }
        resultTileMap.gameObject.SetActive(true);
        playerTilemap.gameObject.SetActive(true);
    }


    public void HideResultTileMap()
    {
        resultTileMap.ClearAllTiles();
        resultTileMap.gameObject.SetActive(false);
        HidePlayerTileMap();
    }

    public Tilemap GetCurrentTilemap()
    {
        return currentTilemap;
    }
}
