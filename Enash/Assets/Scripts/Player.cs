using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    float _health;

    public Vector3 PlayerPos
    {  get { return transform.position;  } }
    public int Level;
    public int _score;
    public float HitPower;
    public float IncreaseAmount;                    // exp increase amount
    public Image CoolDownBar;
    public Image ExperienceBar;
    public GameObject HealthBarProgress;
    public GameObject HealthBarBackground;
    public Canvas CoolDownCanvas;
    public Text LevelText;
    public Text ScoreText;
    public PlayerState PlayersState;
    public Action OnLevelIncreased;

    public static Player Instance
    {
        get; private set;
    } 

    void Awake()
    {
        Instance = this;
        Level = 0;
        ExperienceBar.fillAmount = 0;
        IncreaseAmount = 0.3f;
        HitPower = 0.2f;
        _score = Level * 10;
        ScoreText.text = "SCORE: " + _score;
        PlayerPrefs.SetInt("score", _score);
        PlayersState = PlayerState.Alive;
    }

    void OnEnable()
    {
        LevelText.text = "" + Level;
        _health = HealthBarProgress.transform.localScale.x;
    }

    void Update ()
    {
        CheckIfLevelIncreased();

        if (_health <= 0.1f)
        {
            HealthBarProgress.transform.localScale = new Vector3(0, HealthBarProgress.transform.localScale.y, HealthBarProgress.transform.localScale.z);
            Die();                              // player dies
        }
    }

    void FireOnLevelIncreased()
    {
        if (OnLevelIncreased != null)
            OnLevelIncreased();
    }

    public void IncreaseExperienceBar(float increaseAmount)
    {
        ExperienceBar.fillAmount += increaseAmount;
    }

    void CheckIfLevelIncreased()
    {
        if (ExperienceBar.fillAmount >= 1)
        {
            Level++;
            _score = Level * 10;                                          // Everytime user increase the level, gains 1 score
            PlayerPrefs.SetInt("score", _score);
            ScoreText.text = "SCORE: " + _score;
            LevelText.text = "" + Level;
            LeaderboardManager.Instance.FireOnScoreChanged(name, _score);
            ExperienceBar.fillAmount = 0;
            IncreaseAmount = IncreaseAmount / 2;                          // make the gaining of the experience amount harder
            FireOnLevelIncreased();
        }
    }

    public void DecreaseHealth (float decreaseAmount)
    {
       HealthBarProgress.transform.localScale -= new Vector3(decreaseAmount, 0, 0);
        _health = HealthBarProgress.transform.localScale.x;
    }

    public void UpgradeHealthBar(float increaseAmount)
    {
        Debug.Log("HEALTH BAR UPGRADED.");
        HealthBarBackground.transform.localScale += new Vector3(increaseAmount, 0, 0);
        // upgrading the health bar also adds a little health to the health bar
        HealthBarProgress.transform.localScale += new Vector3(increaseAmount/3, 0, 0);
        _health = HealthBarProgress.transform.localScale.x;
    }

    public void UpgradeAttack(float increaseAmount)
    {
        Debug.Log("ATTACK POWER UPGRADED.");
        HitPower += increaseAmount;
    }

    public void UpgradeCooldownBar(float coolDownIncreaseAmount)            // increase the decrease amount of the Cooldown bar so the bar could erase
    {                                                                       // from the scene more quickly
        Debug.Log("COOLDOWN BAR UPGRADED.");
        PlayerMovement.Instance.CooldownDecreaseAmount += coolDownIncreaseAmount;
    }

    public void IncreaseVelocity(float velocityIncreaseAmount)
    {
        Debug.Log("VELOCITY UPGRADED.");
        PlayerMovement.Instance.IncreaseSpeed(velocityIncreaseAmount);
    }

    public void IncreaseExpAmount(float expIncreaseAmount)
    {
        Debug.Log("EXP AMOUNT OF THE STAR UPGRADED.");
        IncreaseAmount += expIncreaseAmount;
    }

    void Die()
    {
        PlayersState = PlayerState.Dead;
        HealthBarBackground.SetActive(false);
        HealthBarProgress.SetActive(false);
        Game.Instance.ShowGameOverScreen();
        gameObject.SetActive(false);
    }
}
