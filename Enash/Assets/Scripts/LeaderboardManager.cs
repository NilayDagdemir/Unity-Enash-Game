using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class LeaderboardManager : MonoBehaviour
{
    EnemyWithAI enemyAI;
    public Action<string, int> OnScoreChanged;

    public Text ScoresText;
    Dictionary<string, int> leaderboardList;

    public static LeaderboardManager Instance
    { get; private set; }

    void Awake()
    {
        Instance = this;
        leaderboardList = new Dictionary<string, int>();
        OnScoreChanged += ScoreChanged;
        ScoresText.text = "";
    }

    void Start()
    {
        leaderboardList.Add(Player.Instance.name, Player.Instance._score);

        for (int i = 0; i < EnemyManager.Instance._enemyAIAmount; i++)
        {
            enemyAI = EnemyManager.Instance._enemiesWithAI[i];
            leaderboardList.Add(enemyAI.name, enemyAI._score);
        }
        ScoreChanged("", 0);
    }

    public void FireOnScoreChanged(string name, int newScore)
    {
        if (OnScoreChanged != null)
            OnScoreChanged(name, newScore);
    }

    void ScoreChanged(string name, int newScore)
    {
        if (name != "")
            leaderboardList[name] = newScore;

        var sortedList = from pair in leaderboardList
                         orderby pair.Value descending
                         select pair;

        string newScoresText = "";
        int i = 1;

        foreach (KeyValuePair<string, int> pair in sortedList)
        {
            newScoresText += i + ". " + pair.Key + ": " + pair.Value + "\r\n\n";
            i++;
        }
        if (ScoresText != null)
            ScoresText.text = newScoresText;
    }
}