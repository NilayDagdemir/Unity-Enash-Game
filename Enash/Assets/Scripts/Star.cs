using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour
{
    IEnumerator magnetMovement;

    public static Star Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "EnemyWithAI")
        {
            magnetMovement = MagnetMovement(other.transform.position, other);
            StartCoroutine(magnetMovement);
        }
    }

    void IncreaseExperienceBar(Collider2D triggeredEnemy)
    {
        if (triggeredEnemy.tag == "Player")
            Player.Instance.IncreaseExperienceBar(Player.Instance.IncreaseAmount);

        else if (triggeredEnemy.tag == "EnemyWithAI")
        {
            EnemyWithAI enemyToIncreaseExp = triggeredEnemy.GetComponent<EnemyWithAI>();
            enemyToIncreaseExp.IncreaseExperience(enemyToIncreaseExp.IncreaseAmount);
        }
    }

    void GenerateNewPosForStar()
    {
        transform.position = Game.Instance.GenerateRandomPos(transform.position.z);
        gameObject.SetActive(true);
    }

    IEnumerator MagnetMovement(Vector3 posToGo, Collider2D triggeredEnemy)
    {
        while (Vector3.Distance(transform.position, posToGo) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posToGo, 0.9f);
            yield return null;
        }
        gameObject.SetActive(false);

        IncreaseExperienceBar(triggeredEnemy);

        GenerateNewPosForStar();
    }
}
