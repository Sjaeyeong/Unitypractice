using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public float gameTime;

    [Header("# Player Info")]
    public int level;
    public int kill;
    public float exp;
    public float nextExp;
    public int rice;
    public int banana;

    [Header("# RiceBag Info")]
    // public RiceBag[] riceBag;

    public RiceBag[] activeRiceBag = new RiceBag[3];

    public int activeTargetCount = 1;

    public PoolManager pool;
    public MonkeyCS[] monkey;
    

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        level = 1;

    }

    void Update()
    {
        gameTime += Time.deltaTime;
        nextExp = CalculateNextExp(level);

    }

    public void GetExp(float amount)
    {
        exp += 30;

        if (exp >= nextExp)
        {
            level++;
            exp -= nextExp;


        }
    }

    public static float CalculateNextExp(int currentlevel)
    {
        float coefficient = 30f;
        float exponent = 2.2f;

        float requiredExp = coefficient * Mathf.Pow(currentlevel, exponent);

        return Mathf.Floor(requiredExp);
    }

    public void GameStart(int id)
    {
        
    }

}
