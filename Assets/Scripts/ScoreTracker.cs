using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    public delegate void SummoningSuccessEventHandler();
    public event SummoningSuccessEventHandler OnSummoningSuccess;
    public delegate void ScoreChangeEventHandler(int totalScore);
    public event ScoreChangeEventHandler OnScoreChange;
    public delegate void FinalScoreEventHandler(List<float> aggregatedScores);
    public event FinalScoreEventHandler OnFinalScore;
    private List<int> summoningSuccessFullThreshholds;
    public static List<float> aggregatedScores = new List<float>();
    private int totalScore = 0;

    private void Start()
    {
        gameManager.GetComponent<GameManager>().OnScoreChange += AddScore;
        gameManager.GetComponent<GameManager>().OnEndOfRitual += ResetScore;
        gameManager.GetComponent<GameManager>().OnEndOfGame += PresentFinalResults;
        summoningSuccessFullThreshholds = gameManager.GetComponent<GameManager>().GetSummoningSuccessFullThreshholds();
    }

    private void PresentFinalResults()
    {
        OnFinalScore?.Invoke(aggregatedScores);
    }

    private void ResetScore(float totalPercentage, int ritualCount)
    {
        if (aggregatedScores.Count >= 3)
        {
            aggregatedScores.Clear();
        }
        aggregatedScores.Add(totalPercentage);
        totalScore = 0;
        OnScoreChange?.Invoke(totalScore);
    }

    private void AddScore(int score, int ritualCount)
    {
        totalScore += score;
        Debug.Log("Total Score: " + totalScore);
        if (totalScore >= summoningSuccessFullThreshholds[ritualCount])
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
