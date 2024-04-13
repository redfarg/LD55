using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPaintResultText : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
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

    private void DisplayEndOfStageText()
    {
        resultText.text = "First Summon Complete!";
    }

    private void DisplayResultText(float percentage)
    {
        resultText.text = $"Summoning Sigil Accuracy: {percentage:0.00}%";
    }
}
