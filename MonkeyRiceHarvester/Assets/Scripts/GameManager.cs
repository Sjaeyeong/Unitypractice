using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public float totalDamage;

    [Header("# Banana Shop Info")]
    public bool isAutoSpawn = false;
    public int maxBagcount = 1;
    public int towerMonkeyCount = 0; // max is 2
    public bool bagUpgrade = false;
    public int bonusAmmo = 0; // max is 3
    public int bonusMonkey = 0; // max is 2

    [Header("# Extra Monkey Prefabs/Objects")]
    public GameObject[] extraMonkeys;

    [Header("# Farmer Setting")]
    public GameObject[] farmerMonkeys;
    public float farmerRate;
    float riceBuffer;

    [Header("# RiceBag Info")]
    public RiceBag[] activeRiceBag = new RiceBag[3];
    
    [Header("# Shopt Info")]
    public bool[] isBagTypeUnlocked = new bool[3] { true, false, false};

    [Header("# HUD References")]
    public GameObject hudBlueBarParent;
    public GameObject hudGreenBarParent;
    public Slider hudRedSlider;
    public Slider hudBlueSlider;
    public Slider hudGreenSlider;

    public GameObject shopWindow;
    public GameObject dataWindow;

    public PoolManager pool;
    public List<MonkeyCS> activeMonkeys = new List<MonkeyCS>();
    

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        level = 1;

        UpdateHUDHealthBars();
        UpdateFarmerState();

        MonkeyCS[] startingMonkeys = FindObjectsByType<MonkeyCS>(FindObjectsSortMode.None);
        activeMonkeys.AddRange(startingMonkeys);

    }

    void Update()
    {
        gameTime += Time.deltaTime;
        nextExp = CalculateNextExp(level);

        if (farmerRate > 0)
        {
            riceBuffer += farmerRate * Time.deltaTime;
            if (riceBuffer >= 1.0f)
            {
                int gain = Mathf.FloorToInt(riceBuffer);
                rice += gain;
                riceBuffer -= gain;
            }
        }

    }

    public void GetExp(float amount)
    {
        exp += amount;

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

    public void UpdateFarmerState()
    {
        for (int i = 0; i < farmerMonkeys.Length; i++)
        {
            farmerMonkeys[i].SetActive(i < towerMonkeyCount);
        }
    }

    public void toggleWindow(GameObject targetWindow)
    {
        if (!targetWindow)
            return;
        
        bool isActive = targetWindow.activeSelf;
        targetWindow.SetActive(!isActive);
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

    public void UpdateHUDHealthBars()
    {
        if (hudBlueBarParent != null)
            hudBlueBarParent.SetActive(maxBagcount >= 2);

        if (hudGreenBarParent != null)
            hudGreenBarParent.SetActive(maxBagcount >= 3);
    }

}
