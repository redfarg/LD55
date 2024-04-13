using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;

public class SummoningProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject summoningProgressBar;
    private ScoreTracker scoreTracker;
    void Start()
    {
        scoreTracker = GameObject.Find("ScoreTracker").GetComponent<ScoreTracker>();
        scoreTracker.OnSummoningSuccess += UpdateSummoningProgressBar;
        scoreTracker.OnScoreChange += UpdateSummoningProgressBarSize;
    }

    private void UpdateSummoningProgressBarSize(int totalScore)
    {
        Debug.Log("Score: " + totalScore);
        summoningProgressBar.transform.localScale = new Vector3(totalScore / 500f, 1, 1);
    }

    private void UpdateSummoningProgressBar()
    {
        Debug.Log("Summoning Success");
        LeanTween.color(summoningProgressBar, Color.green, 1f);
    }
}
