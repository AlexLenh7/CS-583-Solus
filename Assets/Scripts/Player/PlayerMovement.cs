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
    public float dashCoolCounter;

    // New variables for dash invulnerability
    private bool isDashing = false;
    public LayerMask enemyLayer; // Assign the enemy layer in the Unity Inspector
    private int initialCollisionLayer;
    private PlayerStats playerStats; // Reference to player stats if you have one

    public GameManager gameManager;

    private TrailRenderer trailRenderer;
    
    // Ghost Trail variables
    public float ghostTrailInterval = 0.05f; // How often to spawn ghost images
    public float ghostTrailDuration = 0.3f; // How long each ghost image lasts
    public Color ghostTrailColor = new Color(1f, 1f, 1f, 0.5f); // Ghost image color/transparency
    private bool isCreatingGhostTrail = false;
    private SpriteRenderer playerSprite;

    public DashBar dashBar;

    [SerializeField] private AudioClip DeathSoundClip;
    [SerializeField] private AudioClip DodgeSoundClip;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false; // Disable on start
        }

        playerSprite = GetComponent<SpriteRenderer>();
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

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
                if (dashBar != null)
                {
                    // Update the dash bar fill amount
                    float fillAmount = 1 - (dashCoolCounter / dashCooldown);
                    dashBar.SetFill(fillAmount);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && dashCoolCounter <= 0 && dashCounter <= 0)
            {
                StartDash();
                SoundFXManager.instance.PlaySoundFXClip(DodgeSoundClip, transform, .5f);
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

        if (dashBar != null)
        {
            dashBar.SetFill(0f);
        }
        
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }

        if (!isCreatingGhostTrail)
        {
            StartCoroutine(CreateGhostTrail());
        }

        // Ignore collisions with enemies during dash
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
    }

    IEnumerator CreateGhostTrail()
    {
        isCreatingGhostTrail = true;

        while (isDashing)
        {
            GameObject ghost = new GameObject("GhostTrail");
            ghost.transform.position = transform.position;
            ghost.transform.rotation = transform.rotation;
            ghost.transform.localScale = transform.localScale;

            SpriteRenderer ghostSprite = ghost.AddComponent<SpriteRenderer>();
            ghostSprite.sprite = playerSprite.sprite;
            ghostSprite.sortingOrder = playerSprite.sortingOrder;
            ghostSprite.color = ghostTrailColor;

            StartCoroutine(FadeOutGhost(ghost, ghostSprite));

            yield return new WaitForSeconds(ghostTrailInterval);
        }

        isCreatingGhostTrail = false;
    }

    IEnumerator FadeOutGhost(GameObject ghost, SpriteRenderer ghostSprite)
    {
        Color startColor = ghostSprite.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < ghostTrailDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / ghostTrailDuration;
            
            ghostSprite.color = Color.Lerp(startColor, endColor, normalizedTime);
            
            yield return null;
        }

        Destroy(ghost);
    }

    void EndDash()
    {
        isDashing = false;
        activeMoveSpeed = moveSpeed;
        dashCoolCounter = dashCooldown;
        
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

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
        SoundFXManager.instance.PlaySoundFXClip(DeathSoundClip, transform, .5f);
        isDying = true;
        StopMovement();
        animator.SetBool("isDead", true); // Set IsDead in the Animator
        animator.SetFloat("Speed", 0);    // Stop movement animations
        dashBar.Hide();
        StartCoroutine(FadeOutAndDestroy());
        gameManager.gameOver();
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
        //gameObject.SetActive(false);
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


