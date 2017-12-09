using UnityEngine;
using System;

public class CircleAroundEnemy : MonoBehaviour
{
    Enemy _enemyToAttack;

    public Action<Enemy> OnTrigger;
    public Action<Enemy> OnEnemyExit;

    public static CircleAroundEnemy Instance { get; private set; }

    void FireOnTrigger(Enemy targetEnemy)
    {
        if (OnTrigger != null)
            OnTrigger(targetEnemy);
    }

    void FireOnExit(Enemy targetEnemy)
    {
        if (OnEnemyExit != null)
            OnEnemyExit(targetEnemy);
    }

    void Awake()
    {
        Instance = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            FireOnTrigger(null);

        else if (other.tag == "Enemy" || (other.tag == "EnemyWithAI" && other.name != GetComponentInParent<EnemyWithAI>().name))
        {
            _enemyToAttack = other.GetComponent<Enemy>();
            FireOnTrigger(_enemyToAttack);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
            FireOnExit(null);

        else if (other.tag == "Enemy" || (other.tag == "EnemyWithAI" && other.name != GetComponentInParent<EnemyWithAI>().name))
        {
            _enemyToAttack = other.GetComponent<Enemy>();
            FireOnExit(_enemyToAttack);
        }
    }
}
