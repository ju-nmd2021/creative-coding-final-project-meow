using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Debug = UnityEngine.Debug;
using Pathfinding;
using Unity.VisualScripting.FullSerializer;

public class NpcMovement : MonoBehaviour
{
    // NPC
    private Transform parentTransform;
    private IAstarAI ai;
    private Vector3 relVelocity;
    // General
    public bool movementLocked = false;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        // AI

        ai = GetComponentInParent<IAstarAI>();
        parentTransform = transform;
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
        relVelocity = parentTransform.InverseTransformDirection(ai.velocity);
      
        movementInput = relVelocity;
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
                animator.SetInteger("direction", 1);
            }
            else if (movementInput.y > 0)
            {
                animator.SetInteger("direction", 2);
            }
            else if (movementInput.y < 0)
            {
                animator.SetInteger("direction", 3);
            }
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
}
