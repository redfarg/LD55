using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;

public class SummoningProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject summoningProgressBar;
    [SerializeField] private GameObject summoningProgressBarActive;
    [SerializeField] private GameObject scoreTracker;
    [SerializeField] private GameObject gameManager;

    void Start()
    {
        var scoreTrackerScript = scoreTracker.GetComponent<ScoreTracker>();
        scoreTrackerScript.OnSummoningSuccess += UpdateSummoningProgressBar;
        scoreTrackerScript.OnScoreChange += UpdateSummoningProgressBarSize;
        summoningProgressBar.SetActive(true);
        var gameManagerScript = gameManager.GetComponent<GameManager>();
        gameManagerScript.OnEndOfRitual += ResetSummoningProgressBar;
    }

    private void ResetSummoningProgressBar(float totalPercentage, int ritualCount)
    {
        summoningProgressBar.SetActive(true);
        summoningProgressBarActive.SetActive(false);
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
