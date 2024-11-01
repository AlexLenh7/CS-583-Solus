using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask playerLayer;
    public float attackCooldown = 1f;
    public bool showDebugGizmos = true;

    private Transform player;
    private Animator animator;
    private bool canAttack = true;
    public PolygonCollider2D attackCollider;
    private float lastAttackTime;
    private bool isAttackAnimationPlaying;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        animator = GetComponent<Animator>();

        attackCollider = transform.Find("AttackCollider")?.GetComponent<PolygonCollider2D>();
        
        attackCollider.enabled = false;
        attackCollider.isTrigger = true;
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        if (!canAttack || player == null || isAttackAnimationPlaying) return;

        // Check if enough time has passed since last attack
        if (Time.time - lastAttackTime < attackCooldown) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        if (isAttackAnimationPlaying) yield break;

        canAttack = false;
        isAttackAnimationPlaying = true;
        lastAttackTime = Time.time;
        
        // Trigger the attack animation
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("Attack");

        //Wait for the full animation
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // Reset states
        animator.SetBool("isAttacking", false);
        isAttackAnimationPlaying = false;
        
        // Add a small buffer before allowing next attack
        yield return new WaitForSeconds(0.1f);
        canAttack = true;
    }

    // Called by animation event at the start of attack frames
    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            
            // Check for player in range when enabling collider
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
                attackCollider.bounds.center,
                attackCollider.bounds.extents.magnitude,
                playerLayer
            );

            foreach (Collider2D playerCollider in hitPlayers)
            {
                // Check if the collider is set to interact with the enemy layer
                if ((playerLayer.value & (1 << playerCollider.gameObject.layer)) != 0 && playerCollider.CompareTag("Player"))
                {
                    PlayerStats playerStats = playerCollider.GetComponent<PlayerStats>();
                    if (playerStats != null)
                    {
                        //Vector2 attackDirection = (playerCollider.transform.position - transform.position).normalized;
                        //playerStats.TakeDamage(attackDamage, attackDirection);
                        playerStats.TakeDamage(attackDamage);
                        Debug.Log($"Enemy dealt {attackDamage} damage to player");
                        break; // Exit after hitting one valid collider to prevent double damage
                    }
                }
            }
        }
    }

    // Called by animation event at the end of attack frames
    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;
        
        // Draw attack detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw the actual attack collider area when it's enabled
        if (attackCollider != null && attackCollider.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackCollider.bounds.center, attackCollider.bounds.size);
        }
    }
}




