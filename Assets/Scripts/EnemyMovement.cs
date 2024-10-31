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
    
    // Start is called before the first frame update

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }


    void Update()
    {
        MoveTowardsPlayer();
        UpdateAnimation();
        FlipSprite();
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
}
