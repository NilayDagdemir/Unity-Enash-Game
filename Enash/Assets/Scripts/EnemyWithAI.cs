using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum AIState
{
    Moving,
    AttackingToPlayer,
    AttackingToCreep,
    Dead
}

public class EnemyWithAI : Enemy
{
    static System.Random randomizer;
    bool _firstEntered;
    int _randomNumber;
    int _randomNumberForProperty;
    int _level;
    int _healthPoint;
    int _damagePoint;
    int _speedPoint;
    int _starExpPoint;
    string _deadEnemy;
    GameObject _currentEnemyToAttack;
    static Action OnDeath;
    IEnumerator AttackCreepRoutine;
    AIState AIEnemyState;

    public int _score;
    public float _attackPower;
    public float IncreaseAmount;
    public float ExperienceAmount;
    public CircleAroundEnemy CircleAroundEnemyAI;

    new public static EnemyWithAI Instance
    { get; private set; }

    void Awake()
    {
        Instance = this;
        ExperienceAmount = 0;                   // initial experience amount
        ExperienceAmountToGive = 0.3f;
        _speedOfMovement = 3f;
        _attackPower = 0.1f;
        IncreaseAmount = 0.1f;
        _deadEnemy = "";
        GenerateRandomLevel();
        DiffType = DifficultyType.EnemyWithAI;
    }

    void Start()
    {
        LeaderboardManager.Instance.FireOnScoreChanged(name, _score);
    }

    void OnEnable()
    {
        _health = HealthProgressBar.transform.localScale.x;
        CircleAroundEnemyAI.OnTrigger += EnemyTriggered;
        CircleAroundEnemyAI.OnEnemyExit += EnemyExit;
        OnDeath += EnemyIsDead;
        StartCoroutine("Movement");
    }

    void Update()
    {
        if (ExperienceAmount >= 1)
        {
            _level++;
            if (_firstEntered)
                _score = _level * 10;
            else
                _score = _score + (_level * 10);
            LeaderboardManager.Instance.FireOnScoreChanged(name, _score);
            IncreaseAmount = IncreaseAmount / 2;       // decrease the increase amount of the experience
            ExperienceAmountToGive += ExperienceAmountToGive / 2;
            UpgradeProperty();
            ExperienceAmount = 0;
        }
    }
    
    void OnDisable()
    {
        _firstEntered = false;
        _level = 0;
        _score = _score / 2;
        LeaderboardManager.Instance.FireOnScoreChanged(name, _score);
        StopAllCoroutines();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AfterTrigger(other, Player.Instance.HitPower);
    }

    static int GenerateNewRandomNumber()
    {
        System.Random newRandomizer = new System.Random((int)DateTime.Now.Ticks); 
        int _randomNumber = newRandomizer.Next(1, 5);
        return _randomNumber;
    }

    void GenerateRandomLevel()
    {
        _firstEntered = true;
        _randomNumber = GenerateNewRandomNumber();
        _level = _randomNumber;
        _score = _level * 10;

        ExperienceAmountToGive = IncreaseAmount * _level;

        for (int i = 0; i < _level; i++)
            UpgradeProperty();
    }

    void EnemyTriggered(Enemy enemy)
    {
        if (enemy == null && _health >= 0.5f)          // if the enemy is the player && AI's health is enough to attack to player
        {
            if (AttackCreepRoutine != null)
                StopCoroutine(AttackCreepRoutine);          // if the AI is currently attacking to a creep, stop this and attack to player
            if (AIEnemyState != AIState.AttackingToPlayer)
            {
                StopCoroutine("Movement");
                StartCoroutine("AttackToPlayer");
            }
        }
        //if the enemy is not the player && is not currently attacking to the player && is not currently attacking to any creep
        if (enemy != null && AIEnemyState != AIState.AttackingToPlayer
            && AIEnemyState != AIState.AttackingToCreep)
        {
            _currentEnemyToAttack = enemy.gameObject;
            //AttackCreepRoutine = AttackToCreep(enemy);
            if (AttackCreepRoutine != null)
                StopCoroutine(AttackCreepRoutine);
            AttackCreepRoutine = AttackToCreep(enemy) as IEnumerator;
            StopCoroutine("Movement");
            StartCoroutine(AttackCreepRoutine);
        }

    }

    void EnemyExit(Enemy enemy)
    {
        if (enemy == null)      // the enemy that exit is the player
        {
            // if AI currently attacking to player, because of the player is exit, stop attacking the player
            if(AIEnemyState == AIState.AttackingToPlayer)
                StopCoroutine("AttackToPlayer");

            if (AIEnemyState != AIState.Moving && AIEnemyState != AIState.AttackingToCreep)
                StartCoroutine("Movement");                  // if AI is not moving now, start moving
        }

        // if the enemy is not the player && if the enemy that AI was attacking is the enemy that exit && AI currently attacking to this creep
        if (enemy != null && _currentEnemyToAttack == enemy.gameObject && AIEnemyState == AIState.AttackingToCreep)
        {
            if(AttackCreepRoutine != null)
                StopCoroutine(AttackCreepRoutine);
            if (AIEnemyState != AIState.Moving)
                StartCoroutine("Movement");
        }
    }

    void FireOnDeath()
    {
        if (OnDeath != null)
            OnDeath();
    }

    void EnemyIsDead()
    {
        if (AttackCreepRoutine != null)
            StopCoroutine(AttackCreepRoutine);

        if (_deadEnemy == "Player")
            this.enabled = false;

        else if (_deadEnemy != "Player" && AIEnemyState != AIState.Moving)
            StartCoroutine("Movement");
    }

    void UpgradeProperty()
    {
        randomizer = new System.Random();
        _randomNumberForProperty = randomizer.Next(0, 4);

        if (_randomNumberForProperty == 0 && _healthPoint < 2)
            UpgradeHealth();

        else if (_randomNumberForProperty == 1 && _damagePoint < 3)
            UpgradeDamage();

        else if (_randomNumberForProperty == 2 && _speedPoint < 3)
            UpgradeSpeed();

        else if (_randomNumberForProperty == 3 && _starExpPoint < 3)
            UpgradeStar();
    }

    public void IncreaseExperience(float increaseAmount)
    {
        ExperienceAmount += increaseAmount;
    }

    void UpgradeHealth()
    {
        HealthProgressBar.transform.localScale += new Vector3(IncreaseAmount, 0, 0);
        _health = HealthProgressBar.transform.localScale.x;
        _healthPoint++;
    }

    void UpgradeDamage()
    {
        _attackPower += IncreaseAmount;
        _damagePoint++;
    }

    void UpgradeSpeed()
    {
        _speedOfMovement += IncreaseAmount;
        _speedPoint++;
    }

    void UpgradeStar()
    {
        IncreaseAmount = IncreaseAmount * 2;
        _starExpPoint++;
    }

    public void DecreasePlayersHealth()
    {
        Player.Instance.DecreaseHealth(_attackPower);
    }

    public void LookAt(Vector3 posToLook)
    {
        float angle = Mathf.Atan2(posToLook.x - transform.position.x, posToLook.y - transform.position.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, 90 - angle);
    }

    void MovementTowardsPlayer()
    {
        // movement through the player
        transform.position = Vector3.MoveTowards(transform.position, Player.Instance.PlayerPos, _speedOfMovement * Time.deltaTime);
    }

    Vector3 GetTargetPos(Vector3 posToAttack)
    {
        LookAt(posToAttack);                    // look to the enemy that enemyAI is going to attack
        Vector3 targetPos = CalculateNewPos(posToAttack);
        targetPos = Boundaries.Instance.GetBoundaryPosition(targetPos);
        return targetPos;
    }

    void ChangePosToAttack(Vector3 targetPosToGo)
    {
        LookAt(targetPosToGo);
        Vector3 movementSpeedVector = Vector3.zero;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPosToGo, ref movementSpeedVector, Time.deltaTime * 4);
        transform.position = new Vector3(newPos.x, newPos.y, Player.Instance.PlayerPos.z);
        transform.position = Boundaries.Instance.GetBoundaryPosition(transform.position);
    }

    IEnumerator Movement()
    {
        AIEnemyState = AIState.Moving;
        while (Player.Instance.PlayersState != PlayerState.Dead)
        {
            // random movement (if the enemy and the player are far away from each other || the enemy's health is low)
            if (Vector3.Distance(transform.position, Player.Instance.PlayerPos) > 15f || _health <= 0.5f)
            {
                Vector3 _randomPos = Game.Instance.GenerateRandomPos(transform.position.z);
                while (Vector3.Distance(transform.position, Player.Instance.PlayerPos) > 15f || _health <= 0.5f)
                {
                    LookAt(_randomPos);
                    transform.position = Vector3.MoveTowards(transform.position, _randomPos, _speedOfMovement * Time.deltaTime);
                    if (transform.position == _randomPos)
                        _randomPos = Game.Instance.GenerateRandomPos(transform.position.z);         // if the enemy reached the random pos, generate a new one
                    yield return null;
                }
            }
            else if (Vector3.Distance(transform.position, Player.Instance.PlayerPos) > 5f
                    && Vector3.Distance(transform.position, Player.Instance.PlayerPos) <= 15f && _health > 0.5f)
            {
                //if enemy is close enough to follow the player && has enough health to follow the player
                LookAt(Player.Instance.PlayerPos);
                MovementTowardsPlayer();
            }
            yield return null;
        }
    }

    IEnumerator AttackToPlayer()
    {
        AIEnemyState = AIState.AttackingToPlayer;
        while (Player.Instance.PlayersState != PlayerState.Dead)
        {
            if (HealthProgressBar.transform.localScale.x >= 0.5f)
            {
                _randomNumber = randomizer.Next(0, 3);         // generate a random number between 0 and 1

                if (_randomNumber == 0  || _randomNumber == 1) // there is a %50 chance to attack and decrease player's health
                {
                    Vector3 newTargetPos = GetTargetPos(Player.Instance.PlayerPos);
                    LookAt(newTargetPos);
                    // after the enemy with AI close enough to dash towards the player
                    while (Vector3.Distance(transform.position, newTargetPos) > 0.01f)
                    {
                        ChangePosToAttack(newTargetPos);
                        yield return null;
                    }
                    DecreasePlayersHealth();                     // hit the player
                }
                else              // there is a %50 chance to miss the attack towards the player
                {
                    Vector3 randomTargetPos = CalculateRandomPosInsideARadius(Player.Instance.PlayerPos);
                    LookAt(randomTargetPos);

                    while (Vector3.Distance(transform.position, randomTargetPos) > 0.01f)
                    {
                        ChangePosToAttack(randomTargetPos);
                        yield return null;
                    }
                    yield return new WaitForSeconds(1f);          // if the enemyAI misses the attacking, then it needs to wait a little bit to dash again(cooldown)
                }
            }
            yield return null;
        }
        _deadEnemy = "Player";
        FireOnDeath();
    }

    IEnumerator AttackToCreep(Enemy creepToAttack)
    {
        AIEnemyState = AIState.AttackingToCreep;
        //if (name == "Yurdagul")
        //    Debug.Log("entered");
        while (!creepToAttack.IsDead)
        {
            if (creepToAttack.DiffType == DifficultyType.Easy)
                creepToAttack = creepToAttack as EasyEnemy;
            else if (creepToAttack.DiffType == DifficultyType.Medium)
                creepToAttack = creepToAttack as MediumEnemy;
            else if (creepToAttack.DiffType == DifficultyType.Hard)
                creepToAttack = creepToAttack as HardEnemy;
            else if (creepToAttack.DiffType == DifficultyType.EnemyWithAI)
                creepToAttack = creepToAttack as EnemyWithAI;

            Vector3 newTargetPos = GetTargetPos(creepToAttack.MyPos);
            Vector3 movementSpeedVector = Vector3.zero;

            while (Vector3.Distance(transform.position, newTargetPos) > 0.01f)
            {
                LookAt(newTargetPos);
                Vector3 newPos = Vector3.SmoothDamp(transform.position, newTargetPos, ref movementSpeedVector, Time.deltaTime * 4);
                transform.position = new Vector3(newPos.x, newPos.y, newTargetPos.z);
                transform.position = Boundaries.Instance.GetBoundaryPosition(transform.position);
                yield return null;
            }
            newTargetPos = GetTargetPos(creepToAttack.MyPos);           // the pos of the enemy has changed so take the new position of the enemy to attack
            creepToAttack.DecreaseEnemysHealth(_attackPower);
            yield return null;
        }
        GenerateNewPosForCreep(creepToAttack);      // after killing the creep, the creep needs to start at a different pos with a full health
        FireOnDeath();
    }

    void GenerateNewPosForCreep(Enemy creepToAttack)
    {
        if (creepToAttack.DiffType == DifficultyType.Easy)
            creepToAttack = creepToAttack as EasyEnemy;
        else if (creepToAttack.DiffType == DifficultyType.Medium)
            creepToAttack = creepToAttack as MediumEnemy;
        else if (creepToAttack.DiffType == DifficultyType.Hard)
            creepToAttack = creepToAttack as HardEnemy;
        else if (creepToAttack.DiffType == DifficultyType.EnemyWithAI)
            creepToAttack = creepToAttack as EnemyWithAI;
        // after killing the enemy, create one more
        creepToAttack.MyPos = Game.Instance.GenerateRandomPos(creepToAttack.MyPos.z);     // instantiate the enemy with full health and a different position
        creepToAttack.ResetHealthBar();
        // take experience from the death enemy
        IncreaseExperience(creepToAttack.ExperienceAmountToGive);
    }

    Vector3 CalculateRandomPosInsideARadius(Vector3 targetPos)
    {
        int _randomX = randomizer.Next((int)targetPos.x - 2, (int)targetPos.x + 2);
        int _randomY = randomizer.Next((int)targetPos.y - 2, (int)targetPos.y + 2);
        Vector3 newTargetPos = new Vector3(_randomX, _randomY, transform.position.z);
        newTargetPos = Boundaries.Instance.GetBoundaryPosition(newTargetPos);

        return newTargetPos;
    }

    public Vector3 CalculateNewPos(Vector3 targetPosition)          // I need to calculate the new pos which the enemy with AI will take, after attacking the player
    {
        Vector3 newPos = Vector3.zero;
        newPos.z = targetPosition.z;

        if (transform.position.x > targetPosition.x)
            newPos.x = targetPosition.x - 0.4f;

        else if (transform.position.x < targetPosition.x)
            newPos.x = targetPosition.x + 0.4f;

        if (transform.position.y > targetPosition.y)
            newPos.y = targetPosition.y - 0.4f;

        else if (transform.position.y < targetPosition.y)
            newPos.y = targetPosition.y + 0.4f;

        return newPos;
    }
}