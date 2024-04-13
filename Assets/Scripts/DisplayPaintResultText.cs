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



    private TextMeshProUGUI resultText;
    private void Start()
    {
        var manager = gameManager.GetComponent<GameManager>();
        manager.OnDeterminedCorrectPercentage += DisplayResultText;
        manager.OnEndOfRituals += DisplayEndOfStageText;
        manager.OnRemoveSigilAccuracyText += ClearResultText;

        resultText = GetComponent<TextMeshProUGUI>();
        resultText.text = "";
    }

    private void ClearResultText()
    {
        resultText.text = "";
    }

    private void DisplayEndOfStageText(float totalPercentage, int ritualCount)
    {
        resultText.text = $"Summoning complete!\nTotal ritual power: {totalPercentage:0.00}%";
        messageBox.SetActive(true);

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
        messageText.text = ritualCount == 0 ?  $"You summoned an {(demon)ritualCount}.\nIt looks {adjective}." : $"You summoned a {(demon)ritualCount}.\nIt looks {adjective}.";
    }

    private void DisplayResultText(float percentage)
    {
        resultText.text = $"Summoning Sigil Accuracy: {percentage:0.00}%";
    }
}
