using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerPainter : MonoBehaviour, IPlayerPainter
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile blackTile;
    [SerializeField] private Tile whiteTile;
    private bool isAllowedToPaint = false;

    private float brushSize = 1f;

    void Update()
    {
        if (!isAllowedToPaint)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

            if (tilemap.HasTile(cellPos))
            {
                TileBase currentTile = tilemap.GetTile(cellPos);
                if (currentTile == whiteTile)
                {
                    Debug.Log(cellPos.ToString());
                    paintTilesAccordingToBrushSize(cellPos);
                }
            }
        }
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
                if (tilemap.HasTile(pos))
                {
                    TileBase currentTile = tilemap.GetTile(pos);
                    if (currentTile == whiteTile)
                    {
                        tilemap.SetTile(pos, blackTile);
                    }
                }
            }
        }
    }

    private void paintSingleTile(Vector3Int cellPos)
    {
        tilemap.SetTile(cellPos, blackTile);
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
