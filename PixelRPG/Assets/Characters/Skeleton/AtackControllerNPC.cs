using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class AtackControllerNPC : MonoBehaviour
{
    // Start is called before the first frame update
    public NpcMovement movementController;
   // public EnemyAttack swordAttack;
    Animator animator;
    public bool isAttacking = false;

    Transform hitBoxTransform;
    Vector2 rightAttackOffset;
    Vector2 topAttackOffset;
    Vector2 bottomAttackOffset;
    public int attackCooldown = 3;
    public EnemyAttack enemyAttack;
    private Vector2 attackDirection = Vector2.right;
    void Start()
    {
        animator = GetComponent<Animator>();
        hitBoxTransform = transform.Find("SwordHitbox");
        rightAttackOffset = hitBoxTransform.localPosition;
        topAttackOffset = new Vector2(0, 0);
        bottomAttackOffset = new Vector2(0, -0.15f);
    }
    
    public void SwordAttack()
    {
        gameObject.GetComponentInChildren<EnemyAttack>().TryAttack(attackDirection);
    }

    public void Attack()
    {
        if (isAttacking == false)
        {
        animator.SetTrigger("swordAttack");
        }
    }

    public void DealDamage()
    {

    }

    public void EndSwordAttack()
    {
        print(0);
    }

    public void enableAttack()
    {
        animator.SetBool("isAnimating", false);
        isAttacking = true;
    }

    public void disabledAttack()
    {
        animator.SetBool("isAnimating", true);
        StartCoroutine(StartCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if (movementController.movementInput.x < 0)
        {
            hitBoxTransform.localPosition = new Vector3(-(rightAttackOffset.x), rightAttackOffset.y);
            attackDirection = Vector2.left;
        }
        else if (movementController.movementInput.x > 0)
        {
            hitBoxTransform.localPosition = rightAttackOffset;
            attackDirection = Vector2.right;
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

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
