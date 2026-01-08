using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public enum SpecialType
    {
        AutoSpawn,
        BagCount,
        TowerMonkey,
        UpgradeBag,
        BonusAmmo,
        ExtraMonkey
    }

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

        [Header("# Realtime Info")]
        public float totalStat;

        public Text levelText;
        public Text costText;
        public Button upgradeButton;
    }

        [System.Serializable]
    public class SpecialItem
    {
        [Header("# Settings")]
        public string itemName;
        public SpecialType type;
        public int bananaCost;
        public int costIncreasePerLv;
        public int maxLevel;

        [HideInInspector]
        public int currentLevel = 0;

        [Header("# UI References")]
        public Button purchaseButton;
        public Text costText;

        public int GetCurrentCost()
        {
            return bananaCost + (currentLevel * costIncreasePerLv);
        }
    }

    [Header("# Rice Shop Items")]
    public UpgradeItem monkeyDamage;
    public UpgradeItem attackSpeed;
    public UpgradeItem criticalChance;
    public UpgradeItem criticalDamage;

    [Header("# Banana Shop Items")]
    public SpecialItem[] specialItems;

    [Header("# Global References")]
    // public MonkeyCS monkeyData;
    public Text riceText;
    public Text bananaText;

    void Awake()
    {
        // monkeyData = FindAnyObjectByType<MonkeyCS>();
    }

    void Start()
    {
        InitializeUpgrade(monkeyDamage, "Damage");
        InitializeUpgrade(attackSpeed, "AttackSpeed");
        InitializeUpgrade(criticalChance, "CriticalChance");
        InitializeUpgrade(criticalDamage, "CriticalDamage");

        InitializeSpecialShop();

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

    // ========================================================================
    // 쌀 상점 로직 (Stats)
    // ========================================================================

    void InitializeUpgrade(UpgradeItem item, string statName)
    {
        item.statName = statName;

        item.upgradeButton.onClick.RemoveAllListeners();
        item.upgradeButton.onClick.AddListener(() => BuyUpgrade(item));

        UpdateUpgradeUI(item);
        // UpdateMonkeyStat(item.statName, item.totalStat);
        foreach (MonkeyCS m in GameManager.instance.activeMonkeys)
        {
            UpdateMonkeyStat(m, item);
        }
    }

    void UpdateUpgradeUI(UpgradeItem item)
    {
        if (item.levelText != null)
            item.levelText.text = $"{item.displayName} Lv.{item.currentLv}";

        long nextCost = item.baseCost + (item.currentLv * item.costIncreasePerLv);

        if (item.levelText != null)
            item.costText.text = nextCost.ToString("0");

        item.totalStat = item.baseValue + item.currentLv * item.valueIncreasePerLv;

        // if (GameManager.instance != null){
        //     item.upgradeButton.interactable = GameManager.instance.rice >= nextCost;
        // }
    }
    public void BuyUpgrade(UpgradeItem item)
    {
        if (GameManager.instance == null) return;

        long cost = item.baseCost + (item.currentLv * item.costIncreasePerLv);

        if (GameManager.instance.rice >= cost)
        {
            GameManager.instance.rice -= (int)cost;
            UpdateCurrencyUI();

            item.currentLv++;

            UpdateUpgradeUI(item);
            // UpdateMonkeyStat(item.statName, item.totalStat);
            foreach (MonkeyCS m in GameManager.instance.activeMonkeys)
            {
                UpdateMonkeyStat(m, item);
            }

            Debug.Log($"Upgraded {item.statName} to Level {item.currentLv}.");
        }
        else
        {
            Debug.Log("Not enough Rice!");
        }
    }

    void UpdateMonkeyStat(MonkeyCS m, UpgradeItem item)
    {
        if (m == null) return;

        switch (item.statName)
        {
            case "Damage":
                m.SetDamage(item.totalStat);
                break;
            case "AttackSpeed":
                m.SetAttackSpeed(item.totalStat);
                break;
            case "CriticalChance":
                m.SetCriticalChance(item.totalStat);
                break;
            case "CriticalDamage":
                m.SetCriticalDamage(item.totalStat);
                break;
        }
    }

    // ========================================================================
    //  바나나 상점 로직 (Special)
    // ========================================================================

    void InitializeSpecialShop()
    {
        foreach (var item in specialItems)
        {
            SyncSpecialItemLevel(item);
            UpdateSpecialUI(item);

            item.purchaseButton.onClick.RemoveAllListeners();
            item.purchaseButton.onClick.AddListener(() => PurchaseSpecial(item));
        }
    }
    void SyncSpecialItemLevel(SpecialItem item)
    {
        if (GameManager.instance == null) return;

        switch (item.type)
        {
            case SpecialType.AutoSpawn:
                item.currentLevel = GameManager.instance.isAutoSpawn ? 1 : 0;
                break;
            case SpecialType.BagCount:
                item.currentLevel = GameManager.instance.maxBagcount - 1;
                break;
            case SpecialType.TowerMonkey:
                item.currentLevel = GameManager.instance.towerMonkeyCount;
                break;
            case SpecialType.UpgradeBag:
                item.currentLevel = GameManager.instance.bagUpgrade ? 1 : 0;
                break;
            case SpecialType.BonusAmmo:
                item.currentLevel = GameManager.instance.bonusAmmo;
                break;
            case SpecialType.ExtraMonkey:
                item.currentLevel = GameManager.instance.bonusMonkey;
                break;
        }
    }

    void UpdateSpecialUI(SpecialItem item)
    {
        if (item.purchaseButton == null) return;

        bool isMax = item.currentLevel >= item.maxLevel;

        if (isMax)
        {
            if (item.costText != null) item.costText.text = "MAX LEVEL";
        }
        else
        {
            if (item.costText != null)
                item.costText.text = item.GetCurrentCost().ToString("N0");
    }
    item.purchaseButton.interactable = true;
    }

    public void PurchaseSpecial(SpecialItem item)
    {
        if (GameManager.instance == null) return;
        if (item.currentLevel >= item.maxLevel) return;

        int currentCost = item.GetCurrentCost();

        if (GameManager.instance.banana >= currentCost)
        {
            GameManager.instance.banana -= currentCost;
            UpdateCurrencyUI();

            item.currentLevel++;

            ApplySpecialEffect(item.type);

            UpdateSpecialUI(item);

            Debug.Log($"Purchased {item.type}. Current Level: {item.currentLevel}");
        }
        else
        {
            Debug.Log("Not enough Banana!");
        }
    }

    void ApplySpecialEffect(SpecialType type)
    {
        switch (type)
        {
            case SpecialType.AutoSpawn:
                GameManager.instance.isAutoSpawn = true;
                break;

            case SpecialType.BagCount:
                GameManager.instance.maxBagcount++;

                if (GameManager.instance.maxBagcount <= 3)
                {
                    int unlockIndex = GameManager.instance.maxBagcount - 1;

                    if (unlockIndex < GameManager.instance.isBagTypeUnlocked.Length)
                    {
                        GameManager.instance.isBagTypeUnlocked[unlockIndex] = true;
                        GameManager.instance.SaveBagUnlockState();

                        FindAnyObjectByType<SpawnInputHandler>().Spawn();
                    }
                }

                GameManager.instance.UpdateHUDHealthBars();
                break;

            case SpecialType.TowerMonkey:
                GameManager.instance.towerMonkeyCount++;
                
                // 1, 2번째 구매까지는 생산량도 늘리고 외형도 소환
                if (GameManager.instance.towerMonkeyCount <= GameManager.instance.farmerMonkeys.Length)
                {
                    GameManager.instance.farmerRate += 5.0f;
                    GameManager.instance.UpdateFarmerState();
                }
                else 
                {
                    GameManager.instance.farmerRate += 10.0f; // 업그레이드 효율은 더 높게 설정 가능
                }
                break;

            case SpecialType.UpgradeBag:
                GameManager.instance.bagUpgrade = true;
                break;

            case SpecialType.BonusAmmo:
                GameManager.instance.bonusAmmo++;
                break;

            case SpecialType.ExtraMonkey:
                GameManager.instance.bonusMonkey++;
                int monkeyIdx = GameManager.instance.bonusMonkey - 1;

                if (monkeyIdx < GameManager.instance.extraMonkeys.Length)
                {
                    GameObject newMonkeyObj = GameManager.instance.extraMonkeys[monkeyIdx];
                    newMonkeyObj.SetActive(true);

                    MonkeyCS newMonkeyScript = newMonkeyObj.GetComponent<MonkeyCS>();
                    if (newMonkeyScript != null)
                    {
                        GameManager.instance.activeMonkeys.Add(newMonkeyScript);

                        UpdateMonkeyStat(newMonkeyScript, monkeyDamage);
                        UpdateMonkeyStat(newMonkeyScript, attackSpeed);
                        UpdateMonkeyStat(newMonkeyScript, criticalChance);
                        UpdateMonkeyStat(newMonkeyScript, criticalDamage);
                    }
                }
                break;
        }
    }
}
