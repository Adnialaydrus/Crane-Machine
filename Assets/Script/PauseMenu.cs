using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button exitButton;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false);

        // Attach button click events
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
        pauseMenuUI.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
        pauseMenuUI.SetActive(false);
    }

    void ExitGame()
    {
        Application.Quit(); // Quit the application
    }
}