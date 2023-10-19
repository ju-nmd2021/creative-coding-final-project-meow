using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    private LevelCreator levelCreator;
    private int health = 8;
    public AIPath aipath;
    Animator animator;
    public int Health
    {
        get { return health; }
        set {
            health = value;
            print($"health: {health}");
            if (health <= 0) {
               Die();
           }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        levelCreator = GameObject.Find("LevelCreator").GetComponent<LevelCreator>();
        animator = GetComponent<Animator>();
    }

    void Die()
    {
        aipath.canMove = false;
        animator.SetBool("isAnimating", false);
        animator.SetTrigger("death");
    }

    public void DeleteSelf()
    {
         StartCoroutine(StartDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(2);
        levelCreator.enemiesAmount--;
        Destroy(gameObject);
    }
}
