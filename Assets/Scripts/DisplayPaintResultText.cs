using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPaintResultText : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private TextMeshProUGUI messageText;


    private static ScoreTranslator scoreTranslator = new ScoreTranslator();
    private List<int> summoningSuccessFullThreshholds;
    private TextMeshProUGUI resultText;
    private void Start()
    {
        var manager = gameManager.GetComponent<GameManager>();
        manager.OnDeterminedCorrectPercentage += DisplayResultText;
        manager.OnEndOfRitual += DisplayEndOfStageText;
        manager.OnRemoveSigilAccuracyText += UpdateResultText;
        manager.OnRitualStart += DisplayRitualStartText;
        manager.OnIntroEnd += ClearIntroText;
        manager.OnSigilIntroStart += DisplaySigilIntroText;
        manager.OnSigilIntroEnd += ClearSigilIntroText;

        summoningSuccessFullThreshholds = manager.GetSummoningSuccessFullThreshholds();
        resultText = GetComponent<TextMeshProUGUI>();
        resultText.text = "";
    }

    private void ClearSigilIntroText()
    {
        messageBox.SetActive(false);
        messageText.text = "";
    }

    private void DisplaySigilIntroText(int sigilCount)
    {
        messageBox.SetActive(true);
        messageText.text = $"Memorize and paint the sigil... {sigilCount}/5";
    }

    private void ClearIntroText()
    {
        messageBox.SetActive(false);
        messageText.text = "";
        //resultText.text = "";
    }

    private void DisplayRitualStartText(int ritualCount)
    {
        var summonOptions = ScoreTranslator.TranslateSummonOptions(ritualCount);
        //resultText.text = $"Summon forth this {summonOptions.Item1}!";
        UpdateResultText(ritualCount);
        
        messageBox.SetActive(true);

        var numStr = ritualCount switch
        {
            0 => "first",
            1 => "second",
            2 => "last",
            _ => "next"
        };
        messageText.text = $"The {numStr} ritual begins...";
    }

    private void UpdateResultText(int numberOfRituals)
    {
        var thresholdStr = numberOfRituals switch
        {
            0 => "50%",
            1 => "60%",
            2 => "70%",
            _ => "A lot"
        };
        resultText.text = $"Total power needed:\n{thresholdStr}";
    }

    private void DisplayEndOfStageText(float totalPercentage, int ritualCount)
    {
        resultText.text = $"Ritual complete\nTotal power: {totalPercentage:0.00}%";
        messageBox.SetActive(true);
        var summonOptions = ScoreTranslator.TranslateSummonOptions(ritualCount);
        if (totalPercentage / 100f >= summoningSuccessFullThreshholds[ritualCount] / 500f)
        {
            var adjective = ScoreTranslator.TranslateAdjectiveOptions(totalPercentage);
            messageText.text = ritualCount == 0 ? $"You summoned an {summonOptions.Item1}.\nIt looks {adjective}." : $"You summoned a {summonOptions.Item1}.\nIt looks {adjective}.";
        }
        else
        {
            var secondSentence = ScoreTranslator.TranslateSecondSentenceOptions(ritualCount);
            messageText.text = $"You summoned a {summonOptions.Item2}.\n{secondSentence}";
        }
    }

    private void DisplayResultText(float percentage)
    {
        resultText.text = $"Summoning Sigil Accuracy: {percentage:0.00}%";
    }
}
