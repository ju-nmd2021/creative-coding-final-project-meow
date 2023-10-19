using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public int damage = 2;
    public AtackControllerNPC controller;
    // private bool canReachPlayer = false;
    private GameObject player;

    private void Start()
    {
        // on start
    }

    public void TryAttack(Vector2 direction)
    {
        if (player == null)
        {
            return;
        }
        StatsPlayer stats = player.GetComponent<StatsPlayer>();
        stats.Health -= damage;
        MovementController movement = player.GetComponent<MovementController>();
        movement.ApplyKnockback(direction);
    }

    private void Knockback()
    {
       Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
        print(rigidbody);
        if (rigidbody != null)
        {
            Vector2 bounceDirection = Vector2.up * 5;
            rigidbody.AddForce(bounceDirection, ForceMode2D.Impulse);
        }
    }


    public void Attack()
    {
        swordCollider.enabled = true;
    }
    public void stopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            controller.Attack();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
