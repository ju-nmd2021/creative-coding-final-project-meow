using Pathfinding;
using System.Collections;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public int damage = 5;
    public float bounceForce = 0.001f;
    public float knockbackDuration = 0.5f;
    private Vector3 knockbackDirection;
    public float knockbackForce = 0.0f;
    public int timesAttackedSuccessfully = 0;

    private void Start()
    {

    }

  
    public void Attack()
    {;
        swordCollider.enabled = true;
    }
    public void stopAttack()
    {
        swordCollider.enabled = false;
    }

    public void ApplyKnockback(Vector3 direction, AIPath ai)
    {
        // Calculate the knockback direction and store it

        knockbackDirection = direction.normalized;

        // Start the knockback Coroutine
        StartCoroutine(DoKnockback(ai));
    }

    private IEnumerator DoKnockback(AIPath ai)
    {
        float timer = 0f;

        while (timer < knockbackDuration)
        {

            // Calculate the movement for this frame in the opposite direction
            Vector3 knockbackMovement = -knockbackDirection * (damage / 3) * Time.deltaTime;

            // Use your custom Move function to apply the movement
            ai.Move(knockbackMovement);

            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            StatsController enemy = other.GetComponent<StatsController>();
            if (enemy != null) {
                Rigidbody2D enemyRigidbody = other.GetComponent<Rigidbody2D>();
                
                if (enemyRigidbody == null) {
                    return;
                }
                AIPath ai = other.GetComponent<AIPath>();
                Vector2 bounceDirection = getAttackDirection();
                ApplyKnockback(bounceDirection, ai);
                // ai.Move(bounceDirection);
                enemy.Health -= damage;
                timesAttackedSuccessfully += 1;
            }
            // Deal enemy dmg
        }
    }

    private Vector2 getAttackDirection()
    {
        Vector3 relativePos = transform.InverseTransformPoint(transform.parent.position);
        print(relativePos);

        if (relativePos == Vector3.zero)
        {
            // return "top";
            return Vector2.down;
        }
        else if (relativePos.x == -0.11f)
        {
            //  return "left";
            return Vector2.left;
        }
        else if (relativePos.x == 0.11f)
        {
            // return "right";
            return Vector2.right;
            //  } else if (relativePos == new Vector3(0, -0.15f, 0))
        } else if (relativePos.x == 0 &&  relativePos.y == 0.15f)
        {
           return Vector2.up;
        }
        return Vector2.right;
    }

    
}
