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
    public List<float> aggregatedScores = new List<float>();
    private int totalScore = 0;

    private void Start()
    {
        gameManager.GetComponent<GameManager>().OnScoreChange += AddScore;
        gameManager.GetComponent<GameManager>().OnEndOfRitual += ResetScore;
    }

    private void ResetScore(float totalPercentage, int ritualCount)
    {
        aggregatedScores.Add(totalPercentage);
        totalScore = 0;
        OnScoreChange?.Invoke(totalScore);
    }

    private void AddScore(int score)
    {
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
