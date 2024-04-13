using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintedTile
{
    public TileColors Color { get; private set; }
    public Vector3Int Position { get; private set; }


    public PaintedTile(string name, Vector3Int position)
    {
        SetColor(name);
        Position = position;
    }

    private void SetColor(string name)
    {
        Color = TileColors.NONE;

        Color = name switch
        {
            "blacK_tile" => TileColors.BLACK,
            "white_tile" => TileColors.WHITE,
            "red_tile" => TileColors.RED,
            "purple_tile" => TileColors.PURPLE,
            "green_tile" => TileColors.GREEN,
            _ => TileColors.NONE
        };
    }

    public bool IsCorrectlyPainted(PaintedTile other)
    {
        return Color == other.Color && Position == other.Position;
    }

    public string GetColorName()
    {
        return Color switch
        {
            TileColors.BLACK => "black_tile",
            TileColors.WHITE => "white_tile",
            TileColors.RED => "red_tile",
            TileColors.PURPLE => "purple_tile",
            TileColors.GREEN => "green_tile",
            _ => ""
        };
    }
}


