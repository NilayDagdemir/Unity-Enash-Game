using UnityEngine;
using System.Collections;

public class EasyEnemy : Enemy
{
    void Awake ()
    {
        _speedOfMovement = 2.5f;
        ExperienceAmountToGive = 0.2f;
        DiffType = DifficultyType.Easy;
    }

    void OnEnable()
    {
        _health = HealthProgressBar.transform.localScale.x;
        StartCoroutine("MoveRandomly");
    }

    void OnDisable()
    {
        StopCoroutine("MoveRandomly");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AfterTrigger(other, Player.Instance.HitPower);
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
