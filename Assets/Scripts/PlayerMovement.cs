using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public Animator animator;
    // public Transform attackPoint; // Reference to attackPoint
    // public float attackPointOffset = 0.5f; // Distance to offset attackPoint
    [HideInInspector] public Vector2 lastMoveDirection;
    public bool canMove = true;

    public bool isDying = false;
    public float fadeDuration = 1f;

    private float activeMoveSpeed;
    public float dashSpeed = 5f;

    public float dashLength = 0.5f, dashCooldown = 1f;

    private float dashCounter;
    private float dashCoolCounter;

    // New variables for dash invulnerability
    private bool isDashing = false;
    public LayerMask enemyLayer; // Assign the enemy layer in the Unity Inspector
    private int initialCollisionLayer;
    private PlayerStats playerStats; // Reference to player stats if you have one

    void Start()
    {
        activeMoveSpeed = moveSpeed; // Set initial move speed
        initialCollisionLayer = gameObject.layer;
        playerStats = GetComponent<PlayerStats>();
    }
    
    void Update()
    {
        if (canMove && !isDying)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;

            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);

            if (Input.GetKeyDown(KeyCode.Space) && dashCoolCounter <= 0 && dashCounter <= 0)
            {
                StartDash();
            }

            // Handle dash duration and reset speed
            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    EndDash();
                }
            }

            // Handle dash cooldown countdown
            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            // Update last move direction
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                lastMoveDirection = moveDirection;
                animator.SetFloat("LastHorizontal", lastMoveDirection.x);
                animator.SetFloat("LastVertical", lastMoveDirection.y);
            }
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    void StartDash()
    {
        isDashing = true;
        activeMoveSpeed = dashSpeed;
        dashCounter = dashLength;
        
        // Ignore collisions with enemies during dash
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
        
        // Optional: Add visual feedback for dash
        StartCoroutine(DashEffect());
    }

    void EndDash()
    {
        isDashing = false;
        activeMoveSpeed = moveSpeed;
        dashCoolCounter = dashCooldown;
        
        // Re-enable collisions with enemies
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), false);
    }

    IEnumerator DashEffect()
    {
        // Optional: Add trail effect or other visual feedback during dash
        // You could enable a TrailRenderer here if you have one

        float elapsed = 0f;
        while (elapsed < dashLength)
        {
            elapsed += Time.deltaTime;
            // You could add additional visual effects here
            yield return null;
        }
    }

    public bool IsDashing
    {
        get { return isDashing; }
    }

    void FixedUpdate() 
    {
        if (canMove)
        {
            rb.velocity = moveDirection * activeMoveSpeed;
        } 
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void StopMovement()
    {
        canMove = false;
        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
    }

    public void HandleDeath()
    {
        isDying = true;
        StopMovement();
        animator.SetBool("isDead", true); // Set IsDead in the Animator
        animator.SetFloat("Speed", 0);    // Stop movement animations
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

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

    // void UpdateAttackPointPosition()
    // {
    //     if (attackPoint != null && lastMoveDirection != Vector2.zero)
    //     {
    //         // Position attackPoint a certain distance from the player in the direction of lastMoveDirection
    //         attackPoint.localPosition = lastMoveDirection * attackPointOffset;
    //     }
    // }
}


