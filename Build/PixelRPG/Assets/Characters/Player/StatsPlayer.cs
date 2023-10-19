using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPlayer : MonoBehaviour
{
    private int health = 10;
    Animator animator;
    public Image healthBar;
    public int Health
    {
        get { return health; }
        set {
            health = value;
            print($"Player's health: {health}");
            int hp = Mathf.Clamp( health, 0, 100 );
            healthBar.fillAmount = health / 10f;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void SetAlive()
    {
        animator.Play("player_idle");
        GetComponent<MovementController>().movementLocked = false;
    }

    private void Die()
    {
        animator.Play("player_death");
        GetComponent<MovementController>().movementLocked = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnDeath()
    {
        GameObject.Find("LevelCreator").GetComponent<LevelCreator>().ShowStartAgain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
