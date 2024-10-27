using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    public Animator animator;
    private Vector2 lastMoveDirection;
    public bool canMove = true;
    public float attackStagger = 0.2f;
    // Start is called before the first frame update

    // Update is called once per frame
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

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                lastMoveDirection = moveDirection;
                animator.SetFloat("LastHorizontal", moveDirection.x);
                animator.SetFloat("LastVertical", moveDirection.y);
            }
        }
        else
        {
            moveDirection = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAttackAnimation();
        }

    }

    void FixedUpdate() 
    {
        // Only move the player if not attacking (you could allow movement while attacking if needed)
        if (canMove)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        } 
        else
        {
            rb.velocity = Vector2.zero; // Stop movement while attacking
        }
    }

    void PlayAttackAnimation()
    {
        canMove = false;
        animator.SetTrigger("Attack");
        // animator.SetBool("IsAttacking", true);
        // rb.velocity = lastMoveDirection * attackStagger;
        animator.SetFloat("LastHorizontal", lastMoveDirection.x);
        animator.SetFloat("LastVertical", lastMoveDirection.y);
    }

    // void StopAttacking()
    // {
    //     animator.SetBool("IsAttacking", false);
    // }

    void FinishAttack()
    {
        canMove = true;
        // rb.velocity = Vector2.zero;
    }

}