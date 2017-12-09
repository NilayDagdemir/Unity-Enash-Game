using UnityEngine;
using System.Collections;

public enum DifficultyType
{
    Easy,
    Medium,
    Hard,
    EnemyWithAI
}

public class Enemy : MonoBehaviour
{
    protected Vector3 _randomPos;
    protected IEnumerator _randomMove;
    protected float _health;

    public float _speedOfMovement;
    // it is the experience amount to give, if somebody kills this enemy
    public float ExperienceAmountToGive;
    public GameObject HealthProgressBar;
    public bool IsDead;
    public DifficultyType DiffType;

    public Vector3 MyPos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public static Enemy Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;
        _randomPos = Vector3.zero;
    }

    public void AfterTrigger(Collider2D obj, float decreaseAmount)
    {
        if (obj.gameObject.name == "Player" && PlayerMovement.Instance.HasClicked)
        {
            PlayerMovement.Instance.ResetCooldown();

            DecreaseEnemysHealth(decreaseAmount);               // if the player has entered to the enemy's boundaries & clicked to the mouse
                                                                // then decrease the health of the enemy
            if (_health < 0.1f)         // if the enemy has dead
            {
                Player.Instance.IncreaseExperienceBar(ExperienceAmountToGive);               // if the enemy is dead, then increase the experience bar of the player
                Die();
            }
        }
    }

    public void Die()
    {
        IsDead = true;
        this.gameObject.SetActive(false);
        transform.position = Game.Instance.GenerateRandomPos(transform.position.z);     // instantiate the enemy with full health and a different position
        ResetHealthBar();
    }

    public void DecreaseEnemysHealth(float decreaseAmount)
    {
        // decrease the health of the enemy
        HealthProgressBar.transform.localScale -= new Vector3(decreaseAmount, 0, 0);
        _health = HealthProgressBar.transform.localScale.x;

        if (_health < 0.1f)
            IsDead = true;
    }

    public void ResetHealthBar()
    {
        if(gameObject.name == "EnemyEasy(Clone)")
            HealthProgressBar.transform.localScale = new Vector3(1, 1, 1);
        else if(gameObject.name == "EnemyMedium(Clone)")
            HealthProgressBar.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        else if (gameObject.name == "EnemyHard(Clone)")
            HealthProgressBar.transform.localScale = new Vector3(0.8f, 0.7f, 0.7f);
        else // if the enemy is enemy with AI
            HealthProgressBar.transform.localScale = new Vector3(1.3f, 1.3f, 0.4f);
        this.gameObject.SetActive(true);
        HealthProgressBar.SetActive(true);
        IsDead = false;
    }
}
