using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    private ScoreManager scoreManager;

    void Awake()
    {
        // Find the ScoreManager component
        scoreManager = gameOverUI.GetComponentInChildren<ScoreManager>();
        
        if (scoreManager == null)
            Debug.LogError("GameManager: ScoreManager not found in gameOverUI!");
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        
        // Force the score to update when game over screen is shown
        if (scoreManager != null)
        {
            scoreManager.ForceScoreUpdate();
        }
    }

    public void Restart()
    {
        EnemyStats.ResetKillCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart successful");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Title Screen");
        Debug.Log("Back to Title screen");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit successful");
    }
}
