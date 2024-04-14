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

    enum demon
    {
        IMP,
        FIEND,
        LORD
    }

    enum failure
    {
        SHRIMP,
        FREAK,
        POTATO
    }


    private TextMeshProUGUI resultText;
    private void Start()
    {
        var manager = gameManager.GetComponent<GameManager>();
        manager.OnDeterminedCorrectPercentage += DisplayResultText;
        manager.OnEndOfRitual += DisplayEndOfStageText;
        manager.OnRemoveSigilAccuracyText += ClearResultText;
        manager.OnRitualStart += DisplayRitualStartText;
        manager.OnIntroEnd += ClearIntroText;
        manager.OnSigilIntroStart += DisplaySigilIntroText;
        manager.OnSigilIntroEnd += ClearSigilIntroText;

        resultText = GetComponent<TextMeshProUGUI>();
        resultText.text = "";
    }

    private void ClearSigilIntroText()
    {
        messageBox.SetActive(false);
        messageText.text = "";
    }

    private void DisplaySigilIntroText()
    {
        messageBox.SetActive(true);
        messageText.text = "Observe and memorize the sigil...";
    }

    private void ClearIntroText()
    {
        messageBox.SetActive(false);
        messageText.text = "";
        resultText.text = "";
    }

    private void DisplayRitualStartText(int ritualCount)
    {
        resultText.text = $"Summon forth this {(demon)ritualCount}!";
        messageBox.SetActive(true);
        messageText.text = "The ritual begins...";
    }

    private void ClearResultText()
    {
        resultText.text = "";
    }

    private void DisplayEndOfStageText(float totalPercentage, int ritualCount)
    {
        resultText.text = $"Summoning complete!\nTotal ritual power: {totalPercentage:0.00}%";
        messageBox.SetActive(true);

        if (totalPercentage >= 50f)
        {
            var adjective = totalPercentage switch
            {
                <= 55f => "pathetic",
                <= 60f => "anemic",
                <= 65f => "frail",
                <= 70f => "feisty",
                <= 75f => "mighty",
                <= 80f => "grand",
                <= 85f => "apocalyptic",
                > 85f => "ungodly",
                _ => ""
            };
            messageText.text = ritualCount == 0 ? $"You summoned an {(demon)ritualCount}.\nIt looks {adjective}." : $"You summoned a {(demon)ritualCount}.\nIt looks {adjective}.";
        }
        else
        {
            var secondSentence = ritualCount switch
            {
                0 => "How awkward...",
                1 => "What the hell!?",
                2 => "Embarrassing.",
                _ => "Are you serious?"
            };

            messageText.text = $"You summoned a {(failure)ritualCount}.\n{secondSentence}";
        }
    }

    private void DisplayResultText(float percentage)
    {
        resultText.text = $"Summoning Sigil Accuracy: {percentage:0.00}%";
    }
}
