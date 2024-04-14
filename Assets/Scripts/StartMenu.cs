using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private AudioSource buttonPressSound;
    public void StartGame()
    {
        buttonPressSound.Play();
        Debug.Log("Start Game");
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        buttonPressSound.Play();
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
