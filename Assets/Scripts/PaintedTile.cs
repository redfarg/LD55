using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintedTile
{
    public TileColors Color { get; private set; }
    public Vector3Int Position { get; private set; }


    public PaintedTile(TileBase tileBase, Vector3Int position)
    {
        SetColor(tileBase);
        Position = position;
    }

    private void SetColor(TileBase tileBase)
    {
        Color = TileColors.NONE;

        if (tileBase == null)
        {
            return;
        }

        Color = tileBase.name switch
        {
            "blacK_tile" => TileColors.BLACK,
            "white_tile" => TileColors.WHITE,
            "red_tile" => TileColors.RED,
            "green_tile" => TileColors.GREEN,
            "purple_tile" => TileColors.PURPLE,
            "background_tile" => TileColors.BACKGROUND,
            _ => TileColors.NONE
        };
    }
}


