using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public Animator animator;
    public Transform attackPoint; // Reference to attackPoint
    public float attackPointOffset = 0.5f; // Distance to offset attackPoint
    [HideInInspector] public Vector2 lastMoveDirection;
    public bool canMove = true;
    
    void Update()
    {
        if (canMove)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;

            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);

            // Update last move direction
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                lastMoveDirection = moveDirection;
                animator.SetFloat("LastHorizontal", lastMoveDirection.x);
                animator.SetFloat("LastVertical", lastMoveDirection.y);
            }

            // Update attackPoint position based on lastMoveDirection
            UpdateAttackPointPosition();
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    void FixedUpdate() 
    {
        if (canMove)
        {
            rb.velocity = moveDirection * moveSpeed;
        } 
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void UpdateAttackPointPosition()
    {
        if (attackPoint != null && lastMoveDirection != Vector2.zero)
        {
            // Position attackPoint a certain distance from the player in the direction of lastMoveDirection
            attackPoint.localPosition = lastMoveDirection * attackPointOffset;
        }
    }
}


