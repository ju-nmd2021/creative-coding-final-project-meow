using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Debug = UnityEngine.Debug;
using Pathfinding;

public class MovementController : MonoBehaviour
{
    // General
    public LayerMask obstacleLayer;
    public bool movementLocked = false;
    public int direction = 1;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public float knockbackForce = 1f;
    public float knockbackDuration = 0.2f;
    private float knockbackTimer;
    private Vector2 attackDirection = Vector2.right;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public int stumbledTimes;
    // Start is called before the first frame update
    void Start()
    {
        // General
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void resetTriggers()
    {
        animator.SetBool("isMoving", false);
    }

    enum Direction : int
    {
        Right = 1,
        Up = 2,
        Down = 3
    }

    private void Update()
    {

        if (knockbackTimer > 0)
        {
            if (IsObstacle())
            {
                knockbackTimer = 0;
                rb.velocity = Vector2.zero;
                return;
            }
            // Apply the knockback force in the desired direction
            rb.velocity = new Vector2(attackDirection.x * knockbackForce, rb.velocity.y);
            knockbackTimer -= Time.deltaTime;

          // CheckForObstacles();
        } else
        {
            rb.velocity = Vector2.zero;
        }

        if (movementLocked)
        {
            // Don't move
            return;
        }
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success && movementInput.x > 0)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success && movementInput.y > 0)
            {
                TryMove(new Vector2(0, movementInput.y));
            }

            resetTriggers();
            animator.SetBool("isMoving", true);

            if (movementInput.x != 0)
            {
                direction = 1;
            }
            else if (movementInput.y > 0)
            {
                direction = 2;
            }
            else if (movementInput.y < 0)
            {
                direction = 3;
            }
            animator.SetInteger("direction", direction);
        }
        else
        {
            resetTriggers();
        }
        // Update sprite direction
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
         }
    }

    private bool IsObstacle()
    {
        // Calculate the desired position after knockback
        Vector2 desiredPosition = rb.position + rb.velocity * Time.deltaTime;

        // Check for obstacles in the desired position
        if (Physics2D.OverlapCircle(desiredPosition, 0.1f, obstacleLayer))
        {
            // If an obstacle is in the way, adjust the position to prevent getting stuck
            //  rb.position = desiredPosition;
            return true;
        }
        return false;
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)

        {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
                return true;
            }
            else
            {
                stumbledTimes += 1;
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

      public void ApplyKnockback(Vector2 direction)
    {
        attackDirection = direction;
        // Reset the velocity and set the timer to apply the knockback effect
        rb.velocity = Vector2.zero;
        knockbackTimer = knockbackDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stumbledTimes += 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("enter");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("col");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collide");
    }
}
