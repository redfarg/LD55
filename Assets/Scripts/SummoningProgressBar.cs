using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;
using UnityEngine.UI;

public class SummoningProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject summoningProgressBar;
    [SerializeField] private GameObject summoningProgressBarActive;
    [SerializeField] private GameObject scoreTracker;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Sprite progressBarBackground_50p;
    [SerializeField] private Sprite progressBarBackground_60p;
    [SerializeField] private Sprite progressBarBackground_75p;

    private Image image;
    void Start()
    {
        var scoreTrackerScript = scoreTracker.GetComponent<ScoreTracker>();
        scoreTrackerScript.OnSummoningSuccess += UpdateSummoningProgressBar;
        scoreTrackerScript.OnScoreChange += UpdateSummoningProgressBarSize;
        summoningProgressBar.SetActive(true);
        var gameManagerScript = gameManager.GetComponent<GameManager>();
        gameManagerScript.OnEndOfRitual += ResetSummoningProgressBar;
        gameManagerScript.OnRitualStart += UpdateSummoningBarBackgroundImage;
        image = gameObject.GetComponent<Image>();
    }

    private void ResetSummoningProgressBar(float totalPercentage, int ritualCount)
    {
        summoningProgressBar.SetActive(true);
        summoningProgressBarActive.SetActive(false);

        if(ritualCount == 1) image.sprite = progressBarBackground_60p;
        else if (ritualCount == 2) image.sprite = progressBarBackground_75p;
        else image.sprite= progressBarBackground_50p;
    }

    private void UpdateSummoningBarBackgroundImage(int ritualCount)
    {
        if(ritualCount == 1) image.sprite = progressBarBackground_60p;
        else if (ritualCount == 2) image.sprite = progressBarBackground_75p;
        else image.sprite= progressBarBackground_50p;
    }

    private void UpdateSummoningProgressBarSize(int totalScore)
    {
        Debug.Log("Score: " + totalScore);
        summoningProgressBar.transform.localScale = new Vector3(totalScore / 500f, 1, 1);
        summoningProgressBarActive.transform.localScale = new Vector3(totalScore / 500f, 1, 1);
    }

    private void UpdateSummoningProgressBar()
    {
        Debug.Log("Summoning Success");
        summoningProgressBar.SetActive(false);
        summoningProgressBarActive.SetActive(true);
    }
}
