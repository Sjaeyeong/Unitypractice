using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeItem
    {
        public string statName;
        public string displayName;
        public int currentLv = 1;
        public int baseCost;
        public int costIncreasePerLv;
        public float baseValue;
        public float valueIncreasePerLv;

        public Text levelText;
        public Text costText;
        public Button upgradeButton;
    }

        // [System.Serializable]
    // public class ShopItem
    // {
    //     public int bagIndex;
    //     public string itemName;
    //     public int bananaCost;
    // }

    // [Header("# Shop Items")]
    // public ShopItem[] shopItems;

    [Header("# Upgrade Items")]
    public UpgradeItem monkeyDamage;
    public UpgradeItem attackSpeed;
    public UpgradeItem criticalChance;
    public UpgradeItem criticalDamage;

    public MonkeyCS monkeyData;
    public Text riceText;
    public Text bananaText;

    void Awake()
    {
        monkeyData = FindAnyObjectByType<MonkeyCS>();
    }

    void Start()
    {
        InitializeUpgrade(monkeyDamage, "Damage");
        InitializeUpgrade(attackSpeed, "AttackSpeed");
        InitializeUpgrade(criticalChance, "CriticalChance");
        InitializeUpgrade(criticalDamage, "CriticalDamage");

        UpdateCurrencyUI();
    }

    public void UpdateCurrencyUI()
    {
        if (GameManager.instance != null)
        {
            if(riceText != null)
                riceText.text = GameManager.instance.rice.ToString("N0"); 
            if(bananaText != null)
                bananaText.text = GameManager.instance.banana.ToString("N0");
        }
    }

    void InitializeUpgrade(UpgradeItem item, string statName)
    {
        item.statName = statName;
        item.upgradeButton.onClick.AddListener(() => BuyUpgrade(item));
        UpdateUpgradeUI(item);
    }

    void UpdateUpgradeUI(UpgradeItem item)
    {
        item.levelText.text = $"{item.displayName} Lv.{item.currentLv}";

        long nextCost = item.baseCost + (item.currentLv * item.costIncreasePerLv);
        item.costText.text = nextCost.ToString("NO");

        if (GameManager.instance != null){
            item.upgradeButton.interactable = GameManager.instance.banana >= nextCost;
        }
    }

    public void BuyUpgrade(UpgradeItem item)
    {
        if (GameManager.instance == null || monkeyData == null)
            return;

        int cost =  item.baseCost + (item.currentLv * item.costIncreasePerLv);

        if (GameManager.instance.banana >= cost)
        {
            GameManager.instance.banana -= cost;
            UpdateCurrencyUI();

            item.currentLv++;

            float newValue = item.baseValue + (item.currentLv * item.valueIncreasePerLv);

            UpdateMonkeyStat(item.statName, newValue);

            UpdateUpgradeUI(item);
            Debug.Log($"Upgraded {item.statName} to Level {item.currentLv}.");
        }
        else
        {
            Debug.Log($"Not enough Banana to upgrade {item.statName}. Required: {cost}");
        }
    }

    void UpdateMonkeyStat(string statName, float newValue)
    {
        switch (statName)
        {
            case "Damage":
                monkeyData.SetDamage(newValue);
                break;
            case "AttackSpeed":
                monkeyData.SetAttackSpeed(newValue);
                break;
            case "CriticalChance":
                monkeyData.SetCriticalChance(newValue);
                break;
            case "CriticalDamage":
                monkeyData.SetCriticalDamage(newValue);
                break;
        }
    }

    // public void PurchaseBag(int itemIndex)
    // {
    //     if (itemIndex < 0 || itemIndex >= shopItems.Length)
    //         return;

    //     ShopItem item = shopItems[itemIndex];
    //     int bagIndex = item.bagIndex;

    //     if (GameManager.instance.isBagTypeUnlocked[bagIndex])
    //     {

    //     }
    // }



}
