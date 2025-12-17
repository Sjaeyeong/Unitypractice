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

        [Header("Realtime Info")]
        public float totalStat;

        public Text levelText;
        public Text costText;
        public Button upgradeButton;
    }

        [System.Serializable]
    public class ShopItem
    {
        public int bagIndex;
        public string itemName;
        public int bananaCost;
        public Button purchaseButton;
        public GameObject unlockedText;
    }

    [Header("# Shop Items")]
    public ShopItem[] shopItems;

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

        InitializeBagPurchases();
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
        UpdateMonkeyStat(item.statName, item.totalStat);
    }

    void InitializeBagPurchases()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            ShopItem item = shopItems[i];

            if (i == 0) continue;

            if (GameManager.instance != null)
            {
                if (GameManager.instance.isBagTypeUnlocked[item.bagIndex])
                {
                    item.purchaseButton.gameObject.SetActive(false);
                    item.unlockedText.SetActive(true);
                }
                else
                {
                    item.purchaseButton.onClick.AddListener(() => PurchaseBag(item));
                    UpdateBagPurchaseUI(item);
                }
            }
        }
    }

    void UpdateUpgradeUI(UpgradeItem item)
    {
        item.levelText.text = $"{item.displayName} Lv.{item.currentLv}";

        long nextCost = item.baseCost + (item.currentLv * item.costIncreasePerLv);
        item.costText.text = nextCost.ToString("0");

        item.totalStat = item.baseValue + item.currentLv * item.valueIncreasePerLv;

        // if (GameManager.instance != null){
        //     item.upgradeButton.interactable = GameManager.instance.rice >= nextCost;
        // }
    }

    void UpdateBagPurchaseUI(ShopItem item)
    {
        if (GameManager.instance != null)
        {
            item.purchaseButton.interactable = (GameManager.instance.banana >= item.bananaCost);
        }
    }

    public void BuyUpgrade(UpgradeItem item)
    {
        if (GameManager.instance == null || monkeyData == null)
            return;

        int cost =  item.baseCost + (item.currentLv * item.costIncreasePerLv);

        if (GameManager.instance.rice >= cost)
        {
            GameManager.instance.rice -= cost;
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

    public void PurchaseBag(ShopItem item)
    {
        if (GameManager.instance == null) return;

        if (GameManager.instance.banana >= item.bananaCost)
        {
            GameManager.instance.banana -= item.bananaCost;

            GameManager.instance.isBagTypeUnlocked[item.bagIndex] = true;
            GameManager.instance.SaveBagUnlockState();

            item.purchaseButton.gameObject.SetActive(false);
            item.unlockedText.SetActive(true);
            UpdateCurrencyUI();
        }
        else
        {
            UpdateBagPurchaseUI(item);
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


}
