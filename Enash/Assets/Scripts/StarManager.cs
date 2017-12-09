using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarManager : MonoBehaviour
{ 
    public int _starAmount;
    public GameObject Star;

    public static StarManager Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;

        InstantiateStars();
    }

    void InstantiateStars()
    {
        for (int i = 0; i < _starAmount; i++)
        {
            GameObject obj = Instantiate(Star);
            obj.transform.position = Game.Instance.GenerateRandomPos(obj.transform.position.z);
        }
    }
}
