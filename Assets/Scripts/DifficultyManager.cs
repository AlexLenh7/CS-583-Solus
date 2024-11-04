using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public float difficultyIncreaseInterval = 120f; // 2 minutes
    public float maxDifficultyTime = 600f; // 10 minutes

    public int healthIncreasePerLevel = 10;
    public int damageIncreasePerLevel = 10;
    public float speedIncreasePerLevel = .05f;

    [SerializeField] private float gameTimer = 0f;
    [SerializeField] private int currentDifficultyLevel = 0;
    [SerializeField] private int maxDifficultyLevel;

    public Color[] difficultyColors = new Color[]
    {
        Color.white,    // Level 0 (Base)
        Color.green,     // Level 1
        Color.blue,    // Level 2
        Color.yellow,   // Level 3
        Color.red,      // Level 4
        Color.magenta   // Level 5 (Max)
    };

    void Start()
    {
        // Calculate max difficulty level (10 minutes / 2 minutes per level = 5 levels)
        maxDifficultyLevel = Mathf.FloorToInt(maxDifficultyTime / difficultyIncreaseInterval);
        //Debug.Log($"Difficulty system initialized. Max levels: {maxDifficultyLevel}");
    }

    void Update()
    {
        gameTimer += Time.deltaTime;
        
        // Calculate new difficulty level
        int newLevel = Mathf.Min(
            Mathf.FloorToInt(gameTimer / difficultyIncreaseInterval),
            maxDifficultyLevel
        );

        // If difficulty level has changed
        if (newLevel != currentDifficultyLevel)
        {
            currentDifficultyLevel = newLevel;
            //Debug.Log($"Difficulty increased to level {currentDifficultyLevel + 1} at time {gameTimer:F1}s");
        }
    }

    public void ApplyDifficultyToEnemy(GameObject enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        if (enemyStats != null && enemyAttack != null)
        {
            // Store original values
            int baseHealth = enemyStats.initialMaxHealth;
            int baseDamage = enemyAttack.attackDamage;
            float baseSpeed = enemyMovement.moveSpeed;

            // Calculate and apply increases
            int bonusHealth = healthIncreasePerLevel * currentDifficultyLevel;
            int bonusDamage = damageIncreasePerLevel * currentDifficultyLevel;
            float bonusSpeed = speedIncreasePerLevel * currentDifficultyLevel;

            enemyStats.maxHealth = baseHealth + bonusHealth;
            enemyAttack.attackDamage = baseDamage + bonusDamage;
            enemyMovement.moveSpeed = baseSpeed + bonusSpeed;

            // Initialize health after setting max health
            enemyStats.InitializeHealth();

            SpriteRenderer spriteRenderer = enemyStats.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && currentDifficultyLevel < difficultyColors.Length)
            {
                spriteRenderer.color = difficultyColors[currentDifficultyLevel];
            }

            // Debug.Log($"Enemy spawned at difficulty level {currentDifficultyLevel + 1}:");
            // Debug.Log($"Health: {baseHealth} + {bonusHealth} = {enemyStats.maxHealth}");
            // Debug.Log($"Damage: {baseDamage} + {bonusDamage} = {enemyAttack.attackDamage}");
        }
    }

    public float GetDifficultyProgress()
    {
        return Mathf.Min(gameTimer / maxDifficultyTime, 1f);
    }
}