using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerTextUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameManager;
    private float timeRemaining;
    private bool timerIsRunning = false;

    private void Start()
    {
        gameManager.GetComponent<GameManager>().OnTimerStart += UpdateTimerText;
        timerText.text = "";
    }

    private void UpdateTimerText(float playerPaintTime)
    {
        timerIsRunning = true;
        timeRemaining = playerPaintTime;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 1)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer(timeRemaining);
            }
            else
            {
                timerText.text = "";
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }

    }

    private void UpdateTimer(float timeRemaining)
    {
        timeRemaining -= 1;
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
