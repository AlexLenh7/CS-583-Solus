using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private HealthUI healthUI;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public Animator animator;
    private PlayerMovement playerMovement;
    [SerializeField] private AudioClip HurtSoundClip;

    void Start()
    {
        healthUI = FindObjectOfType<HealthUI>();
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();

        if (healthUI != null)
        {
            healthUI.OnHealthChanged();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthUI != null) healthUI.OnHealthChanged();
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        // Trigger hurt animation based on current movement direction
        animator.SetTrigger("Hurt");
        SoundFXManager.instance.PlaySoundFXClip(HurtSoundClip, transform, .5f);

        // Use the player's movement direction for hurt animation facing
        if (playerMovement != null && playerMovement.moveDirection.sqrMagnitude > 0.01f)
        {
            animator.SetFloat("HurtHorizontal", playerMovement.moveDirection.x);
            animator.SetFloat("HurtVertical", playerMovement.moveDirection.y);
        }
        else
        {
            // Default to last move direction
            animator.SetFloat("HurtHorizontal", playerMovement.lastMoveDirection.x);
            animator.SetFloat("HurtVertical", playerMovement.lastMoveDirection.y);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");
        isDead = true;
        animator.SetBool("isDead", true);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

}


