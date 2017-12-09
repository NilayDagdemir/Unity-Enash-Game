using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    int _healthPoint;
    int _attackPoint;
    int _coolDownPoint;
    int _speedPoint;
    int _starExpPoint;
    int _propertyPoint;
    bool _newHighScore;
    float _randomX;
    float _randomY;
    static System.Random _randomizer;

    public Canvas PropertiesCanvas;
    public Canvas GameOverCanvas;
    public Text HighScoreText;
    public Text NewHighScoreText;
    public Text IncreasePropertyAmount;
    public Text HealthBarUpgradedAmount;
    public Text AttackUpgradeAmount;
    public Text CoolDownUpgradeAmount;
    public Text SpeedUpgradeAmount;
    public Text StarExpUpgradeAmount;

    public static Game Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;
        _randomizer = new System.Random();
        Player.Instance.OnLevelIncreased += LevelIncreased;
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("highscore"))
            _newHighScore = true;

        ShowHighScore();

        UpgradeAProperty();

        if (_propertyPoint == 0)
            PropertiesCanvas.gameObject.SetActive(false);

    }

    void LevelIncreased()
    {
        _propertyPoint++;
        IncreasePropertyAmount.text = "" + _propertyPoint + "x";
        PropertiesCanvas.gameObject.SetActive(true);
    }

    public Vector3 GenerateRandomPos(float zPos)
    {
        _randomX = _randomizer.Next((int)Boundaries.Instance._leftBoundary, (int)Boundaries.Instance._rightBoundary);
        _randomY = _randomizer.Next((int)Boundaries.Instance._bottomBoundary, (int)Boundaries.Instance._topBoundary);
        return new Vector3(_randomX, _randomY, zPos);
    }

    void UpgradeAProperty()
    {
        // every upgrade operation will end after 10 points for the same property
        if (_propertyPoint > 0 && _healthPoint <= 10 && Input.GetKeyDown(KeyCode.Q))                // upgrade health property
            UpgradeHealth();

        else if (_propertyPoint > 0 && _attackPoint <= 10 && Input.GetKeyDown(KeyCode.W))           // upgrade attack property
            UpgradeAttack();

        else if (_propertyPoint > 0 && _coolDownPoint <= 10 && Input.GetKeyDown(KeyCode.E))         // upgrade coolDown bar
            UpgradeCooldownBar();

        else if (_propertyPoint > 0 && _speedPoint <= 10 && Input.GetKeyDown(KeyCode.R))            // upgrade speed
            UpgradeSpeed();

        else if (_propertyPoint > 0 && _starExpPoint <= 10 && Input.GetKeyDown(KeyCode.A))          // upgrade star exp amount
            UpgradeStarExp();
    }

    void UpgradeHealth()
    {
        _healthPoint++;
        HealthBarUpgradedAmount.text = "" + _healthPoint + "x";
        Player.Instance.UpgradeHealthBar(0.02f);
        _propertyPoint--;
        IncreasePropertyAmount.text = "" + _propertyPoint + "x";
    }

    void UpgradeAttack()
    {
        _attackPoint++;
        AttackUpgradeAmount.text = "" + _attackPoint + "x";
        Player.Instance.UpgradeAttack(0.1f);
        _propertyPoint--;
        IncreasePropertyAmount.text = "" + _propertyPoint + "x";
    }

    void UpgradeCooldownBar()
    {
        _coolDownPoint++;
        CoolDownUpgradeAmount.text = "" + _coolDownPoint + "x";
        Player.Instance.UpgradeCooldownBar(0.003f);
        _propertyPoint--;
        IncreasePropertyAmount.text = "" + _propertyPoint + "x";
    }

    void UpgradeSpeed()
    {
        _speedPoint++;
        SpeedUpgradeAmount.text = "" + _speedPoint + "x";
        Player.Instance.IncreaseVelocity(0.02f);
        _propertyPoint--;
        IncreasePropertyAmount.text = "" + _propertyPoint + "x";
    }

    void UpgradeStarExp()
    {
        _starExpPoint++;
        StarExpUpgradeAmount.text = "" + _starExpPoint + "x";
        Player.Instance.IncreaseExpAmount(0.1f);
        _propertyPoint--;
        IncreasePropertyAmount.text = "" + _propertyPoint + "x";
    }

    void ShowHighScore()
    {
        HighScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("highscore");
    }

    public void ShowGameOverScreen()
    {
        if (_newHighScore)
        {
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("score"));
            NewHighScoreText.text = "HIGH SCORE! : " + PlayerPrefs.GetInt("highscore");
            NewHighScoreText.gameObject.SetActive(true);
            _newHighScore = false;
        }
        else
            NewHighScoreText.gameObject.SetActive(false);

        GameOverCanvas.gameObject.SetActive(true);      // Show the game over screen
    }
}
