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
    public GameObject settingWindow;

    public PoolManager pool;
    public List<MonkeyCS> activeMonkeys = new List<MonkeyCS>();

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        
        activeMonkeys.Clear();

        MonkeyCS[] startingMonkeys = FindObjectsByType<MonkeyCS>(FindObjectsSortMode.None);
        activeMonkeys.AddRange(startingMonkeys);
    }

    void Start()
        {
            LoadGame();

            UpdateHUDHealthBars();
            UpdateFarmerState();
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

    public void toggleSettings()
    {
        if (!settingWindow)
            return;

        bool isSettingActive = !settingWindow.activeSelf;
        settingWindow.SetActive(isSettingActive);

        Time.timeScale = isSettingActive ? 0f : 1f;
    }

    public void SaveGame()
    {
        // 1. 기본 재화 및 플레이어 성장도
        PlayerPrefs.SetInt("Rice", rice);
        PlayerPrefs.SetInt("Banana", banana);
        PlayerPrefs.SetInt("PlayerLevel", level); // 플레이어의 레벨
        PlayerPrefs.SetFloat("PlayerExp", exp);
        PlayerPrefs.SetFloat("gameTime", gameTime);
        PlayerPrefs.SetFloat("Damage", totalDamage);

        // 2. Rice Shop (기초 스탯 레벨) - ShopManager의 UpgradeItem 단계
        ShopManager shop = FindAnyObjectByType<ShopManager>();
        if (shop != null)
        {
            PlayerPrefs.SetInt("Lv_Damage", shop.monkeyDamage.currentLv);
            PlayerPrefs.SetInt("Lv_AtkSpeed", shop.attackSpeed.currentLv);
            PlayerPrefs.SetInt("Lv_CritChance", shop.criticalChance.currentLv);
            PlayerPrefs.SetInt("Lv_CritDamage", shop.criticalDamage.currentLv);
        }

        // 3. Banana Shop (특수 업그레이드 상태)
        PlayerPrefs.SetInt("IsAutoSpawn", isAutoSpawn ? 1 : 0);
        PlayerPrefs.SetInt("MaxBagCount", maxBagcount);
        PlayerPrefs.SetInt("TowerMonkeyCount", towerMonkeyCount);
        PlayerPrefs.SetInt("BagUpgrade", bagUpgrade ? 1 : 0);
        PlayerPrefs.SetInt("BonusAmmo", bonusAmmo);
        PlayerPrefs.SetInt("BonusMonkey", bonusMonkey);

        // 4. 가방 해금 상태 (배열)
        for (int i = 0; i < isBagTypeUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt("BagUnlocked_" + i, isBagTypeUnlocked[i] ? 1 : 0);
        }

        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("Rice")) return;

        // 1. 기본 재화 복구
        rice = PlayerPrefs.GetInt("Rice");
        banana = PlayerPrefs.GetInt("Banana");
        level = PlayerPrefs.GetInt("PlayerLevel");
        exp = PlayerPrefs.GetFloat("PlayerExp");
        gameTime = PlayerPrefs.GetFloat("gameTime");
        totalDamage = PlayerPrefs.GetFloat("Damage");

        // 2. Banana Shop 상태 복구
        isAutoSpawn = PlayerPrefs.GetInt("IsAutoSpawn") == 1;
        maxBagcount = PlayerPrefs.GetInt("MaxBagCount");
        towerMonkeyCount = PlayerPrefs.GetInt("TowerMonkeyCount");
        bagUpgrade = PlayerPrefs.GetInt("BagUpgrade") == 1;
        bonusAmmo = PlayerPrefs.GetInt("BonusAmmo");
        bonusMonkey = PlayerPrefs.GetInt("BonusMonkey");

        // 3. 가방 해금 상태 복구
        for (int i = 0; i < isBagTypeUnlocked.Length; i++)
        {
            isBagTypeUnlocked[i] = PlayerPrefs.GetInt("BagUnlocked_" + i) == 1;
        }

        // 4. 상점 UI 및 실제 스탯 동기화
        ShopManager shop = FindAnyObjectByType<ShopManager>();
        if (shop != null)
        {
            // Rice Shop 레벨 적용
            shop.monkeyDamage.currentLv = PlayerPrefs.GetInt("Lv_Damage", 1);
            shop.attackSpeed.currentLv = PlayerPrefs.GetInt("Lv_AtkSpeed", 1);
            shop.criticalChance.currentLv = PlayerPrefs.GetInt("Lv_CritChance", 1);
            shop.criticalDamage.currentLv = PlayerPrefs.GetInt("Lv_CritDamage", 1);

            // 상점 UI 갱신 (Initialize를 다시 호출하여 데이터 동기화)
            shop.UpdateCurrencyUI();
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // [중요] 씬 이동 전 시간 속도 복구
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // 실제 메인메뉴 씬 이름 입력
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
