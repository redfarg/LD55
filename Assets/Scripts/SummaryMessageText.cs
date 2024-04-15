using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummaryMessageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject scoreTracker;
    [SerializeField] private GameObject messageBox;
    private static ScoreTranslator ScoreTranslator = new ScoreTranslator();
    private void Start()
    {
        scoreTracker.GetComponent<ScoreTracker>().OnFinalScore += DisplaySummaryMessage;
    }

    private void DisplaySummaryMessage(List<float> aggregatedScores)
    {
        messageBox.SetActive(false);
        var message = "All rituals are complete.\n\nYou summoned: \n\n";
        for (var scoreIndex = 0; scoreIndex < aggregatedScores.Count; scoreIndex++)
        {
            var summonOptions = ScoreTranslator.TranslateSummonOptions(scoreIndex);

            if (SummoningWasSuccessful(aggregatedScores, scoreIndex))
            {
                var adjective = ScoreTranslator.TranslateAdjectiveOptions(aggregatedScores[scoreIndex]);
                message += $"{adjective} {summonOptions.Item1}\t\t{aggregatedScores[scoreIndex]:0.00}%.\n";
            }
            else
            {
                message += $"{summonOptions.Item2}\t\t{aggregatedScores[scoreIndex]:0.00}%\n";
            }
        }
        messageText.text = message;
    }

    private static bool SummoningWasSuccessful(List<float> aggregatedScores, int scoreIndex)
    {
        return (aggregatedScores[scoreIndex] >= 50f && scoreIndex == 0) ||
            (aggregatedScores[scoreIndex] >= 60f && scoreIndex == 1) ||
            (aggregatedScores[scoreIndex] >= 75f && scoreIndex == 2);
    }
}
