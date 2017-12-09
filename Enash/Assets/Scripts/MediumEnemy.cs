using UnityEngine;
using System.Collections;

public class MediumEnemy : Enemy
{
    void Awake()
    {
        _speedOfMovement = 1.0f;
        ExperienceAmountToGive = 0.4f;
        DiffType = DifficultyType.Medium;
    }

    void OnEnable()
    {
        _health = HealthProgressBar.transform.localScale.x;
        StartCoroutine("MoveRandomly");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AfterTrigger(other, Player.Instance.HitPower / 6);
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            _randomPos = Game.Instance.GenerateRandomPos(transform.position.z);
            while (transform.position != _randomPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, _randomPos, _speedOfMovement * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }
}
