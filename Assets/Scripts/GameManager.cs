using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject tileMapManager;
    [SerializeField] private GameObject playerTileMap;
    [SerializeField] private GameObject playerPainter;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private GameObject backgroundImageText;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Sprite defaultBackgroundScreen;
    [SerializeField] private List<Sprite> activeBackgroundImages;
    [SerializeField] private List<Sprite> backgroundTexts;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private AudioSource circleUpdateSound;
    [SerializeField] private List<int> summoningSuccessFullThreshhold = new List<int>() { 250, 300, 350 };
    [SerializeField] private float sigilDisplayTime = 2f;
    [SerializeField] private float introDisplayTime = 10f;
    [SerializeField] private float sigilIntroDisplayTime = 5f;
    [SerializeField] private float playerPaintTime = 20f;
    [SerializeField] private float sigilResultDisplayTime = 10f;
    [SerializeField] private float ritualResultDisplayTime = 20f;
    [SerializeField] private int numberOfSigils = 0;
    [SerializeField] private int numberOfRituals = 0;
    private Tilemap currentTilemap;
    private ITileMapManager tileMapmanager;
    private float totalRitualPercentage;
    private HashSet<PaintedTile> correctlyPaintedTiles = new HashSet<PaintedTile>();
    private List<PaintedTile> playerPaintedTiles = new List<PaintedTile>();
    public delegate void CorrectPercentageEventHandler(float percentage);
    public event CorrectPercentageEventHandler OnDeterminedCorrectPercentage;
    private float correctPercentage;

    public delegate void ScoreChangeEventHandler(int score, int ritualCount);
    public event ScoreChangeEventHandler OnScoreChange;

    public delegate void TimerEventHandler(float playerPaintTime);
    public event TimerEventHandler OnTimerStart;

    public delegate void EndOfRitualsEventHandler(float totalPercentage, int ritualCount);
    public event EndOfRitualsEventHandler OnEndOfRitual;

    public delegate void RemoveSigilAccuracyTextEventHandler();
    public event RemoveSigilAccuracyTextEventHandler OnRemoveSigilAccuracyText;
    public delegate void RitualStartEventHandler(int ritualCount);
    public event RitualStartEventHandler OnRitualStart;
    public delegate void IntroEndEventHandler();
    public event IntroEndEventHandler OnIntroEnd;
    public delegate void SigilIntroStartHandler();
    public event SigilIntroStartHandler OnSigilIntroStart;
    public delegate void SigilIntroEndHandler();
    public event SigilIntroEndHandler OnSigilIntroEnd;
    public delegate void EndOfGameHandler();
    public event EndOfGameHandler OnEndOfGame;


    void Start()
    {
        tileMapmanager = tileMapManager.GetComponent<ITileMapManager>();
        totalRitualPercentage = 0f;
        StartCoroutine(StartRitual());
    }

    IEnumerator StartRitual()
    {
        yield return StartCoroutine(DisplayRitualIntro());
        StartCoroutine(StartSigil());

    }

    IEnumerator DisplaySigilIntro()
    {
        OnSigilIntroStart?.Invoke();
        yield return new WaitForSecondsRealtime(sigilIntroDisplayTime);
        OnSigilIntroEnd?.Invoke();
    }

    IEnumerator StartSigil()
    {
        correctlyPaintedTiles.Clear();
        playerPaintedTiles.Clear();

        if (numberOfSigils < 5)
        {
            yield return StartCoroutine(DisplaySigilIntro());
            numberOfSigils++;
            StartCoroutine(DisplaySigil());
        }
        else
        {
            OnEndOfRitual?.Invoke(totalRitualPercentage / 5, numberOfRituals);
            if (numberOfRituals < 2)
            {
                StartCoroutine(RestartRitual());
            }
            else
            {
                StartCoroutine(DisplayEndScreen());
                yield return null;
            }
        }
    }


    IEnumerator DisplayEndScreen()
    {
        yield return new WaitForSeconds(ritualResultDisplayTime);
        endScreen.SetActive(true);
        OnEndOfGame?.Invoke();
        yield return StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput()
    {
        while (true)
        {
            yield return null;
        }
    }

    IEnumerator DisplaySigil()
    {
        tileMapmanager.DisplayNewSummoningShape();
        currentTilemap = tileMapmanager.GetCurrentTilemap();
        GetPaintedTilesFromMap(correctlyPaintedTiles, currentTilemap);

        yield return new WaitForSeconds(sigilDisplayTime);

        tileMapmanager.HideSummoningShape();
        StartCoroutine(PlayerPaintingPhase());
    }

    IEnumerator DisplayRitualIntro()
    {
        OnRitualStart?.Invoke(numberOfRituals);

        yield return new WaitForSecondsRealtime(introDisplayTime);

        OnIntroEnd?.Invoke();
    }

    IEnumerator RestartRitual()
    {
        numberOfSigils = 0;
        totalRitualPercentage = 0f;
        numberOfRituals++;
        yield return new WaitForSecondsRealtime(ritualResultDisplayTime);
        backgroundImage.GetComponent<Image>().sprite = defaultBackgroundScreen;
        backgroundImageText.GetComponent<Image>().sprite = backgroundTexts[numberOfRituals];
        StartCoroutine(StartRitual());
    }

    IEnumerator PlayerPaintingPhase()
    {
        tileMapmanager.DisplayPlayerTileMap();
        OnTimerStart?.Invoke(playerPaintTime);

        var player = playerPainter.GetComponent<IPlayerPainter>();
        player.playerIsAllowedToPaint(true);

        yield return new WaitForSecondsRealtime(playerPaintTime);

        player.playerIsAllowedToPaint(false);
        GetPaintedTilesFromMap(playerPaintedTiles, playerTileMap.GetComponent<Tilemap>());

        StartCoroutine(DisplayResult());
    }

    IEnumerator DisplayResult()
    {
        correctPercentage = CompareArrays(correctlyPaintedTiles, playerPaintedTiles);
        OnDeterminedCorrectPercentage?.Invoke(correctPercentage);
        totalRitualPercentage += correctPercentage;
        OnScoreChange?.Invoke((int)correctPercentage, numberOfRituals);
        Debug.Log($"Correctly painted: {correctPercentage:0.00}%");
        backgroundImage.GetComponent<Image>().sprite = activeBackgroundImages[numberOfSigils - 1];
        circleUpdateSound.Play();

        GetPaintedTilesFromMap(correctlyPaintedTiles, currentTilemap);
        GetPaintedTilesFromMap(playerPaintedTiles, playerTileMap.GetComponent<Tilemap>());

        tileMapmanager.DisplayResultTileMap(playerPaintedTiles, correctlyPaintedTiles);

        yield return new WaitForSecondsRealtime(sigilResultDisplayTime);

        tileMapmanager.HideResultTileMap();
        OnRemoveSigilAccuracyText?.Invoke();

        StartCoroutine(StartSigil());
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
                correctTileContainer.RemoveWhere(x => x.IsCorrectlyPainted(item));
            }
            else if (TileIsInProximity(correctTileContainer, item))
            {
                correctElements++;
            }
            else
            {
                incorrectElements++;
            }
        }

        Debug.Log($"Correct: {correctElements}, Incorrect: {incorrectElements}");
        var matchedElements = correctElements - incorrectElements / 3;
        Debug.Log($"Matched Elements: {matchedElements}");

        if (matchedElements <= 0)
        {
            return 0f;
        }
        else
        {
            return (float)matchedElements / totalElements * 100f;
        }
    }

    private static bool TileIsInProximity(HashSet<PaintedTile> correctTileContainer, PaintedTile item)
    {
        int proximity = 1;
        foreach (var tile in correctTileContainer)
        {
            if (Math.Abs(tile.Position.x - item.Position.x) <= proximity && Math.Abs(tile.Position.y - item.Position.y) <= proximity)
            {
                correctTileContainer.Remove(tile);
                return true;
            }
        }
        return false;
    }

    public List<int> GetSummoningSuccessFullThreshholds()
    {
        return summoningSuccessFullThreshhold;
    }
}
