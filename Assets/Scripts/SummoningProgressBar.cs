using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;

public class SummoningProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject summoningProgressBar;
    [SerializeField] private GameObject summoningProgressBarActive;

    private ScoreTracker scoreTracker;
    void Start()
    {
        scoreTracker = GameObject.Find("ScoreTracker").GetComponent<ScoreTracker>();
        scoreTracker.OnSummoningSuccess += UpdateSummoningProgressBar;
        scoreTracker.OnScoreChange += UpdateSummoningProgressBarSize;
        summoningProgressBar.SetActive(true);
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
