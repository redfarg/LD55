using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPaintResultText : MonoBehaviour
{
    [SerializeField] private GameObject tilemapManager;
    private TextMeshProUGUI resultText;
    private void Start()
    {
        tilemapManager.GetComponent<TilemapManager>().OnDeterminedCorrectPercentage += DisplayResultText;
        resultText = GetComponent<TextMeshProUGUI>();
        resultText.text = "";
    }

    private void DisplayResultText(float percentage)
    {
        resultText.text = $"Summoning Ritual Accuracy: {percentage:0.00}%";
    }
}
