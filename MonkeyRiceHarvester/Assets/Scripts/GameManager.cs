using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public RiceBag[] activeRiceBag = new RiceBag[3];
    
    [Header("# Shopt Info")]
    public bool[] isBagTypeUnlocked = new bool[3] { true, false, false};

    public GameObject shopWindow;
    public GameObject dataWindow;

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

    public void toggleWindow(GameObject targetWindow)
    {
        if (!targetWindow)
            return;
        
        bool isActive = targetWindow.activeSelf;
        targetWindow.SetActive(!isActive);
    }

    public bool UnlockedBagType(int bagIndex)
    {
        if (bagIndex < 0 || bagIndex >= isBagTypeUnlocked.Length)
            return false;

        if (isBagTypeUnlocked[bagIndex])
            return true;

        isBagTypeUnlocked[bagIndex] = true;
        SaveBagUnlockState();

        return true;
    }

    public void SaveBagUnlockState()
    {
        for (int i = 0; i < isBagTypeUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt($"BagUnlock_{i}", isBagTypeUnlocked[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadBagUnlockState()
    {
        for (int i = 0; i < isBagTypeUnlocked.Length; i++)
        {
            isBagTypeUnlocked[i] = (PlayerPrefs.GetInt($"BagUnlock{i}", isBagTypeUnlocked[i] ? 1 : 0) == 1);
        }

        isBagTypeUnlocked[0] = true; // red always unlocked
    }

}
