using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;

    public int initialMaxHealth;
    void Awake()
    {
        // Store the initial max health for reference
        initialMaxHealth = maxHealth;
        //dropManager = FindObjectOfType<PowerUpManager>();
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
        currentHealth -= damage;

        animator.Play("Undead_hurt", -1, 0f);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");

        animator.SetBool("IsDead", true);

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        GetComponent<EnemyMovement>().enabled = false;
        this.enabled = false;

    }

}
