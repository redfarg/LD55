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
        var message = "";
        for (var score = 0; score < aggregatedScores.Count; score++)
        {
            var summonOptions = ScoreTranslator.TranslateSummonOptions(score);

            if (score > 50f)
            {
                var adjective = ScoreTranslator.TranslateAdjecticeOptions(aggregatedScores[score]);
                message += $"You summoned a {adjective} {summonOptions.Item1} with {aggregatedScores[score]} accuracy.\n";
            }
            else
            {
                message += $"You summoned {summonOptions.Item2} with {aggregatedScores[score]} accuracy.\n";
            }
        }
        Debug.Log(message);
        messageText.text = message;
    }
}
