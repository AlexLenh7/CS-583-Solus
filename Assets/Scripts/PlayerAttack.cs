using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public LayerMask enemyLayers;
    private PlayerMovement playerMovement;
    public Transform attackPoint;
    private PolygonCollider2D attackCollider;

    public Transform attackColliderRight;
    public Transform attackColliderLeft;
    public Transform attackColliderUp;
    public Transform attackColliderDown;

    public float attackStagger = 0.2f;
    public int attackDamage = 20;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        //attackCollider = attackPoint.GetComponent<PolygonCollider2D>();

        attackColliderRight.gameObject.SetActive(false);
        attackColliderLeft.gameObject.SetActive(false);
        attackColliderUp.gameObject.SetActive(false);
        attackColliderDown.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check for attack input
        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.canMove)
        {
            TriggerAttack();
        }
    }

    void TriggerAttack()
    {
        playerMovement.canMove = false; // Disable movement during the attack
        animator.SetTrigger("Attack");

        // Enable the appropriate collider based on last move direction
        EnableAttackCollider();

        // Perform attack if an active collider is found
        PolygonCollider2D activeCollider = GetActiveCollider();
        if (activeCollider != null)
        {
            List<Collider2D> hitEnemies = new List<Collider2D>();
            Physics2D.OverlapCollider(activeCollider, new ContactFilter2D { layerMask = enemyLayers }, hitEnemies);
            
            // Damage the enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyStats>()?.TakeDamage(attackDamage);
            }
        }
        else
        {
            Debug.LogWarning("No active collider found for the attack direction.");
        }

        // Disable colliders after the attack
        DisableAllAttackColliders();
    }

    void EnableAttackCollider()
    {
        DisableAllAttackColliders(); // Ensure all colliders are off

        
        if (playerMovement.lastMoveDirection == Vector2.right)
        {
            attackColliderRight.gameObject.SetActive(true);
            Debug.Log("Attack Collider Right activated.");
        }
        else if (playerMovement.lastMoveDirection == Vector2.left)
        {
            attackColliderLeft.gameObject.SetActive(true);
            Debug.Log("Attack Collider Left activated.");
        }
        else if (playerMovement.lastMoveDirection == Vector2.up)
        {
            attackColliderUp.gameObject.SetActive(true);
            Debug.Log("Attack Collider Up activated.");
        }
        else if (playerMovement.lastMoveDirection == Vector2.down)
        {
            attackColliderDown.gameObject.SetActive(true);
            Debug.Log("Attack Collider Down activated.");
        }
    }

    PolygonCollider2D GetActiveCollider()
    {
        if (attackColliderRight.gameObject.activeSelf) return attackColliderRight.GetComponent<PolygonCollider2D>();
        if (attackColliderLeft.gameObject.activeSelf) return attackColliderLeft.GetComponent<PolygonCollider2D>();
        if (attackColliderUp.gameObject.activeSelf) return attackColliderUp.GetComponent<PolygonCollider2D>();
        if (attackColliderDown.gameObject.activeSelf) return attackColliderDown.GetComponent<PolygonCollider2D>();
        
        return null;
    }

    void DisableAllAttackColliders()
    {
        Debug.Log("Disabling all attack colliders.");
        attackColliderRight.gameObject.SetActive(false);
        attackColliderLeft.gameObject.SetActive(false);
        attackColliderUp.gameObject.SetActive(false);
        attackColliderDown.gameObject.SetActive(false);
    }

        void OnDrawGizmos()
    {
        if (attackColliderRight.gameObject.activeSelf)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackColliderRight.position, 0.1f); // or draw the collider shape
        }
        if (attackColliderLeft.gameObject.activeSelf)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackColliderLeft.position, 0.1f);
        }
        if (attackColliderUp.gameObject.activeSelf)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackColliderUp.position, 0.1f);
        }
        if (attackColliderDown.gameObject.activeSelf)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackColliderDown.position, 0.1f);
        }
    }

    public void FinishAttack()
    {
        playerMovement.canMove = true; // Re-enable movement
        DisableAllAttackColliders();
    }
}


