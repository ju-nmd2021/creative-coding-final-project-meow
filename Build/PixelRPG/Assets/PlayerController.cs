//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections;
//using Debug = UnityEngine.Debug;

//public class PlayerController : MonoBehaviour
//{
//    public bool movementLocked = true;
//    public float moveSpeed = 1f;
//    public float collisionOffset = 0.05f;
//    public ContactFilter2D movementFilter;
//    public SwordAttack swordAttack;
//    Vector2 movementInput;
//    Rigidbody2D rb;
//    Animator animator;
//    SpriteRenderer spriteRenderer;
//    Transform childTransform;
//    Vector2 rightAttackOffset;
//    Vector2 topAttackOffset;
//    Vector2 bottomAttackOffset;

//    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
//    // Start is called before the first frame update
//    void Start()
//    {
 
//        childTransform = transform.Find("SwordHitbox");
//        rightAttackOffset = childTransform.position;
//        topAttackOffset = new Vector2(0, 0);
//        bottomAttackOffset = new Vector2(0, -0.15f);
//        rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        spriteRenderer = GetComponent<SpriteRenderer>();
//    }

//    private void resetTriggers()
//    {
//        animator.SetBool("isMoving", false);
//    }

//    enum Direction: int
//    {
//        Right = 1,
//        Up = 2,
//        Down = 3
//    }

//    private void Update()
//    {
//        if (movementLocked)
//        {
//            // Don't move
//            return;
//        }
//        if (movementInput != Vector2.zero)
//        {
//            bool success = TryMove(movementInput);

//            if (!success && movementInput.x > 0)
//            {
//                success = TryMove(new Vector2(movementInput.x, 0));
//            }

//            if (!success && movementInput.y > 0)
//            {
//                TryMove(new Vector2(0, movementInput.y));
//            }

//                resetTriggers();
//            animator.SetBool("isMoving", true);

//            if (movementInput.x != 0)
//            {
//                animator.SetInteger("direction", 1);
//                // animator.SetBool("isMovingX", true);
//            } else if (movementInput.y > 0)
//            {
//                animator.SetInteger("direction", 2);
//                // animator.SetBool("isMovingYTop", true);
//            } else if (movementInput.y < 0)
//            {
//                animator.SetInteger("direction", 3);
//             //   animator.SetBool("isMovingYBottom", true);
//            }
//        }
//        else
//        {
//            resetTriggers();
//        }
//        // Update sprite direction
//        if (movementInput.x < 0)
//        {
//            spriteRenderer.flipX = true;
//            childTransform.localPosition = new Vector3(-(rightAttackOffset.x), rightAttackOffset.y);
//        } else if (movementInput.x > 0) 
//        {
//            spriteRenderer.flipX = false;
//            childTransform.localPosition = rightAttackOffset;
//        } else if(movementInput.y < 0)
//        {
//            childTransform.localPosition = bottomAttackOffset;
//        } else if (movementInput.y > 0)
//        {
//            childTransform.localPosition = topAttackOffset;
//        }
//    }

//    private bool TryMove(Vector2 direction)
//    {
//        if (direction != Vector2.zero)

//        {
//            int count = rb.Cast(
//                direction,
//                movementFilter,
//                castCollisions,
//                moveSpeed * Time.fixedDeltaTime + collisionOffset);

//            if (count == 0)
//            {
//                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        else {
//            return false;
//        }
//    }

//    void OnMove(InputValue value)
//    {
//        movementInput = value.Get<Vector2>();
//    }

//    private void OnFire()
//    {
//       print("here");
//        animator.SetTrigger("swordAttack");
//   }

//    public void SwordAttack()
//    {
//        lockMovement();
//        swordAttack.Attack();
//    }

//    public void EndSwordAttack()
//    {
//        unlockMovement();
//        swordAttack.stopAttack();
//    }

//    private void lockMovement()
//    {
//        movementLocked = true;
//    }

//    private void unlockMovement() {
//        movementLocked = false;
//    }
//}
