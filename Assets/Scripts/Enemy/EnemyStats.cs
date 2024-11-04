using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    [SerializeField] private AudioClip damageSoundClip;
    public static int totalEnemiesKilled = 0;
    private bool isDead = false;
    private EnemyMovement enemyMovement;

    public int initialMaxHealth;

    void Awake()
    {
        // Store the initial max health for reference
        initialMaxHealth = maxHealth;
        //dropManager = FindObjectOfType<PowerUpManager>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void InitializeHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;  // Prevent damage after death

        currentHealth -= damage;

        animator.Play("Undead_hurt", -1, 0f);

        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, .5f);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log("Enemy died");
        totalEnemiesKilled++;
        Debug.Log($"Total enemies killed: {totalEnemiesKilled}");

        // Disable colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        // Start death animation
        animator.SetBool("IsDead", true);
        
        // Disable movement but keep the component enabled for the HandleDeath animation event
        if (enemyMovement != null)
        {
            enemyMovement.StopMovement();
        }

        // Disable this component's update logic but keep it enabled for animation events
        this.enabled = false;
    }

    public static void ResetKillCount()
    {
        totalEnemiesKilled = 0;
    }
}
