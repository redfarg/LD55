using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerPainter : MonoBehaviour, IPlayerPainter
{
    [SerializeField] private Tilemap playerTilemap;
    [SerializeField] private List<Tile> tileList;
    [SerializeField] private List<AudioSource> chalkSounds;
    private AudioSource chalkSound;
    private Tile selectedTile;
    private bool isAllowedToPaint = false;
    private int lowerXBound = -20;
    private int upperXBound = 50;
    private int lowerYBound = -7;
    private int upperYBound = 45;
    private float brushSize = 1f;
    private bool isPainting = false;

    private void Start()
    {
        selectedTile = tileList.Find(tile => tile.name == "white_tile");
    }

    void Update()
    {
        if (!isAllowedToPaint)
        {
            StopChalkSound();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (!isPainting)
            {
                isPainting = true;
                PlayChalkSound();
            }
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = playerTilemap.WorldToCell(mouseWorldPos);

            if (PositionIsInBounds(cellPos) && !playerTilemap.HasTile(cellPos))
            {
                paintTilesAccordingToBrushSize(cellPos);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isPainting)
            {
                StopChalkSound();
            }
        }
    }

    private void PlayChalkSound()
    {
        var index = UnityEngine.Random.Range(0, chalkSounds.Count - 1);
        Debug.Log(index);
        chalkSound = chalkSounds[index];
        Debug.Log(chalkSound.name);
        chalkSound.Play();
    }

    private void StopChalkSound()
    {
        if (isPainting)
        {
            isPainting = false;
            chalkSound.Stop();
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
