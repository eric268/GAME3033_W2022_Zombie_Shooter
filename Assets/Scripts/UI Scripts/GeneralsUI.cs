using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralsUI : MonoBehaviour
{
    public PlayerController playerController;

    public void OnResumePressed()
    {
        playerController.isPaused = false;
        GameManager.instance.EnableCursor(false);
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void OnRestartPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnCreditsPressed()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
