using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Timer timer;
    private bool scoreUpdated = false;
    
    // Points per second survived
    public int pointsPerSecond = 10;
    // Points per enemy killed
    public int pointsPerKill = 50;

    void Awake()
    {
        // Make sure we have all required components
        if (scoreText == null)
            Debug.LogError("ScoreManager: scoreText not assigned!");
        if (timer == null)
            Debug.LogError("ScoreManager: timer not assigned!");
    }

    void OnEnable()
    {
        if (!scoreUpdated)
        {
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        if (scoreUpdated) return;

        // Calculate time score
        int timeSurvivedScore = Mathf.FloorToInt(timer.elapsedTime) * pointsPerSecond;
        
        // Get total enemies killed from the static counter
        int killScore = EnemyStats.totalEnemiesKilled * pointsPerKill;
        
        // Calculate total score
        int totalScore = timeSurvivedScore + killScore;

        // Update the score text with detailed breakdown
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore:N0}";
            // scoreText.text += $"\n\nTime Survived: {Mathf.FloorToInt(timer.elapsedTime)}s (+{timeSurvivedScore:N0} pts)";
            // scoreText.text += $"\nEnemies Defeated: {EnemyStats.totalEnemiesKilled} (+{killScore:N0} pts)";
        }

        scoreUpdated = true;
    }

    public void ForceScoreUpdate()
    {
        scoreUpdated = false;
        UpdateScore();
    }
}
