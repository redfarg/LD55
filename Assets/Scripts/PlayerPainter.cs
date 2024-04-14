using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerPainter : MonoBehaviour, IPlayerPainter
{
    [SerializeField] private Tilemap playerTilemap;
    [SerializeField] private List<Tile> tileList;
    private Tile selectedTile;
    private bool isAllowedToPaint = false;
    private int lowerXBound = -20;
    private int upperXBound = 50;
    private int lowerYBound = -7;
    private int upperYBound = 45;
    private float brushSize = 1f;

    void Update()
    {
        selectedTile = tileList.Find(tile => tile.name == "white_chalk");
        if (!isAllowedToPaint)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = playerTilemap.WorldToCell(mouseWorldPos);


            if (PositionIsInBounds(cellPos) && !playerTilemap.HasTile(cellPos))
            {
                paintTilesAccordingToBrushSize(cellPos);
            }
        }
    }

    private bool PositionIsInBounds(Vector3Int cellPos)
    {
        return cellPos.x >= lowerXBound && cellPos.x <= upperXBound && cellPos.y >= lowerYBound && cellPos.y <= upperYBound;
    }

    void paintTilesAccordingToBrushSize(Vector3Int cellPos)
    {
        if (brushSize == 0)
        {
            paintSingleTile(cellPos);
        }
        else
        {
            paintMultipleTiles(cellPos);
        }
    }

    private void paintMultipleTiles(Vector3Int cellPos)
    {
        for (int x = (int)-brushSize; x <= brushSize; x++)
        {
            for (int y = (int)-brushSize; y <= brushSize; y++)
            {
                Vector3Int pos = new Vector3Int(cellPos.x + x, cellPos.y + y, cellPos.z);
                if (PositionIsInBounds(pos) && !playerTilemap.HasTile(pos))
                {
                    playerTilemap.SetTile(pos, selectedTile);
                }
            }
        }
    }

    private void paintSingleTile(Vector3Int cellPos)
    {
        playerTilemap.SetTile(cellPos, selectedTile);
    }

    public void changeBrushSize(float size)
    {
        brushSize = size;
    }

    public void playerIsAllowedToPaint(bool isAllowed)
    {
        isAllowedToPaint = isAllowed;
    }
}
