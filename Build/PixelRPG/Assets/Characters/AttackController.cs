using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour

{

    Animator animator;
    public MovementController movementController;

    public SwordAttack swordAttack;
    Transform hitBoxTransform;
    Vector2 rightAttackOffset;
    Vector2 topAttackOffset;
    Vector2 bottomAttackOffset;
    public int timesAttacked = 0;
    // Start is called before the first frame update
    void Start()
    {
        hitBoxTransform = transform.Find("SwordHitbox");
        rightAttackOffset = hitBoxTransform.localPosition;
        topAttackOffset = new Vector2(0, 0);
        bottomAttackOffset = new Vector2(0, -0.15f);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (movementController.movementInput.x < 0)
        {
            hitBoxTransform.localPosition = new Vector3(-(rightAttackOffset.x), rightAttackOffset.y);
        }
        else if (movementController.movementInput.x > 0)
        {
            hitBoxTransform.localPosition = rightAttackOffset;
        }
        else if (movementController.movementInput.y < 0)
        {
            hitBoxTransform.localPosition = bottomAttackOffset;
        }
        else if (movementController.movementInput.y > 0)
        {
            hitBoxTransform.localPosition = topAttackOffset;
        }
    }

    private void OnFire()
    {
        animator.SetTrigger("swordAttack");
        timesAttacked += 1;
    }

    public void SwordAttack()
    {
        lockMovement();
        swordAttack.Attack();
        }
    public void EndSwordAttack()
    {
        unlockMovement();
        swordAttack.stopAttack();
    }

    private void lockMovement()
    {
        movementController.movementLocked = true;
    }

    private void unlockMovement()
    {
        movementController.movementLocked = false;
    }
}
