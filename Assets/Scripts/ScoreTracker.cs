using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private int summoningSuccessFullThreshhold;

    public delegate void SummoningSuccessEventHandler();
    public event SummoningSuccessEventHandler OnSummoningSuccess;
    public delegate void ScoreChangeEventHandler(int totalScore);
    public event ScoreChangeEventHandler OnScoreChange;
    public List<int> scores = new List<int>();
    private int totalScore = 0;

    private void Start()
    {
        gameManager.GetComponent<GameManager>().OnScoreChange += AddScore;
    }

    private void AddScore(int score)
    {
        scores.Add(score);
        totalScore += score;
        Debug.Log("Total Score: " + totalScore);
        if (totalScore >= summoningSuccessFullThreshhold)
        {
            Debug.Log("Summoning Success");
            OnSummoningSuccess?.Invoke();
        }
        OnScoreChange?.Invoke(totalScore);
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}
