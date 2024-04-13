using System.Collections.Generic;
using UnityEngine.Tilemaps;

interface ITileMapManager
{
    public Tilemap GetCurrentTilemap();
    public void DisplayNewSummoningShape();
    public void HideSummoningShape();
    public void DisplayPlayerTileMap();
    public void DisplayResultTileMap(List<PaintedTile> paintedPlayerTiles, HashSet<PaintedTile> correctTiles);
    public void HideResultTileMap();
}