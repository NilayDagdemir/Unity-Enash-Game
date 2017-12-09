using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyWithAI> _enemiesWithAI;
    public int _easyEnemyAmount;
    public int _mediumEnemyAmount;
    public int _hardEnemyAmount;
    public int _enemyAIAmount;
    public GameObject EasyEnemy;
    public GameObject MediumEnemy;
    public GameObject HardEnemy;
    public GameObject EnemyWithAI;

    public static EnemyManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        _enemiesWithAI = new List<EnemyWithAI>();

        GenerateEnemies();

        GenerateAIEnemyNames();
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < _easyEnemyAmount; i++)
        {
            GameObject obj = Instantiate(EasyEnemy);
            EasyEnemy easyEnemy = obj.GetComponent<EasyEnemy>();
            easyEnemy.transform.position = Game.Instance.GenerateRandomPos(easyEnemy.transform.position.z);
        }
        for (int j = 0; j < _mediumEnemyAmount; j++)
        {
            GameObject obj = Instantiate(MediumEnemy);
            MediumEnemy mediumEnemy = obj.GetComponent<MediumEnemy>();
            mediumEnemy.transform.position = Game.Instance.GenerateRandomPos(mediumEnemy.transform.position.z);
        }

        for (int k = 0; k < _hardEnemyAmount; k++)
        {
            GameObject obj = Instantiate(HardEnemy);
            HardEnemy hardEnemy = obj.GetComponent<HardEnemy>();
            hardEnemy.transform.position = Game.Instance.GenerateRandomPos(hardEnemy.transform.position.z);
        }
        for (int t = 0; t < _enemyAIAmount; t++)
        {
            GameObject obj = Instantiate(EnemyWithAI);
            EnemyWithAI enemyAI = obj.GetComponent<EnemyWithAI>();
            enemyAI.transform.position = Game.Instance.GenerateRandomPos(enemyAI.transform.position.z);
            _enemiesWithAI.Add(enemyAI);
        }
    }

    void GenerateAIEnemyNames()
    {
        for (int i = 0; i < _enemiesWithAI.Count; i++)
        {
            switch (i)
            {
                case 0:
                    _enemiesWithAI[i].name = "JamesFuckinHetfield";
                    break;
                case 1:
                    _enemiesWithAI[i].name = "ChrispyChicken";
                    break;
                case 2:
                    _enemiesWithAI[i].name = "XxT.N.TxX";
                    break;
                case 3:
                    _enemiesWithAI[i].name = "Tanay";
                    break;
                case 4:
                    _enemiesWithAI[i].name = "Nilay";
                    break;
                case 5:
                    _enemiesWithAI[i].name = "BigMacTheory";
                    break;
                case 6:
                    _enemiesWithAI[i].name = "TheGreatTitsInTheSky";
                    break;
                case 7:
                    _enemiesWithAI[i].name = "Area51";
                    break;
            }
        }
    }
}

