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
        var message = "Your summons: \n\n";
        for (var score = 0; score < aggregatedScores.Count; score++)
        {
            var summonOptions = ScoreTranslator.TranslateSummonOptions(score);

            if (score > 50f)
            {
                var adjective = ScoreTranslator.TranslateAdjecticeOptions(aggregatedScores[score]);
                message += $"{adjective} {summonOptions.Item1}        {aggregatedScores[score]:0:00}%.\n";
            }
            else
            {
                message += $"{summonOptions.Item2}        {aggregatedScores[score]:0:00}%\n";
            }
        }
        messageText.text = message;
    }
}
