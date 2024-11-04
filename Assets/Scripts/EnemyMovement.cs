using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    public float moveSpeed;
    public Animator animator;
    private Vector2 movement;

    private Vector2 moveDirection;

    private bool isDying = false;
    private bool isStopped = false;
    public float fadeDuration = 1f;
    
    private PowerUpManager dropManager;
    // Start is called before the first frame update

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Awake()
    {
        dropManager = FindObjectOfType<PowerUpManager>();
    }

    public void StopMovement()
    {
        isStopped = true;
    }

    public void ResumeMovement()
    {
        isStopped = false;
    }

    void Update()
    {   
        if (!isDying && !isStopped)
        {
            MoveTowardsPlayer();
            UpdateAnimation();
            FlipSprite();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to the player
        moveDirection = (player.position - transform.position).normalized;

        // Move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void UpdateAnimation()
    {
        // Set the direction in the animator
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Speed", moveDirection.sqrMagnitude); // Magnitude to control idle/moving states
    }

    void FlipSprite()
    {
        // Flip based on the direction of movement in the world space
        if (moveDirection.x < 0) 
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        } 
        else if (moveDirection.x > 0) 
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }
    }

    public void HandleDeath()
    {
        isDying = true;
        animator.SetBool("IsDead", true); // Set IsDead in the Animator
        animator.SetFloat("Speed", 0);    // Stop movement animations
        if (dropManager != null)
        {
            dropManager.TryDropPowerup(transform.position);
        }
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        float fadeAmount;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            fadeAmount = 1 - (t / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAmount);
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        Destroy(gameObject);
    }
}
