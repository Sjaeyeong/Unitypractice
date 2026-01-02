using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// using TMPro;

public class HUD : MonoBehaviour
{
    public enum InfoType{
        Exp, Level, Kill, Time, Rice, Banana,
        redHP, blueHP, greenHP,
        redBagLevel, blueBagLevel, greenBagLevel,
        TotalDamage, MonkeyAtk, MonkeySpeed, MonkeyCrit, MonkeyCritDmg, MonkeyCount,
        FarmerRate
    }
    public InfoType type;

    Text myText;
    // TextMeshProUGUI myText;
    Slider mySlider;


    void Awake()
    {
        myText = GetComponent<Text>();
        // myText = GetComponent<TextMeshProUGUI>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        if (GameManager.instance == null)
            return;

        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp;
                if (mySlider != null && maxExp > 0)
                    mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("Kill : {0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                int hour = Mathf.FloorToInt(GameManager.instance.gameTime / 3600);
                int min = Mathf.FloorToInt(GameManager.instance.gameTime / 60);
                int sec = Mathf.FloorToInt(GameManager.instance.gameTime % 60);
                myText.text = string.Format("Time : {0:D2}:{1:D2}:{2:D2}", hour, min, sec);
                break;
            case InfoType.Rice:
                myText.text = string.Format("{0:N0}", GameManager.instance.rice);
                break;
            case InfoType.Banana:
                myText.text = string.Format("{0:N0}", GameManager.instance.banana);
                break;
            case InfoType.redHP:
                UpdateBagHP(0);
                break;
            case InfoType.blueHP:
                UpdateBagHP(1);
                break;
            case InfoType.greenHP:
                UpdateBagHP(2);
                break;

            case InfoType.redBagLevel:
                UpdateBagLevel(0, "빨강");
                break;
            case InfoType.blueBagLevel:
                UpdateBagLevel(1, "파랑");
                break;
            case InfoType.greenBagLevel:
                UpdateBagLevel(2, "초록");
                break;
            case InfoType.TotalDamage:
                myText.text = string.Format("ToTal Damage\n: {0:N0}", GameManager.instance.totalDamage);
                break;
            case InfoType.MonkeyAtk:
                UpdateMonkeyStat(0);
                break;
            case InfoType.MonkeySpeed:
                UpdateMonkeyStat(1);
                break;
            case InfoType.MonkeyCrit:
                UpdateMonkeyStat(2);
                break;
            case InfoType.MonkeyCritDmg:
                UpdateMonkeyStat(3);
                break;
            case InfoType.MonkeyCount:
                UpdateMonkeyStat(4);
                break;
        }
    }

    void UpdateBagHP(int index)
    {
        if (mySlider == null) return;
        
        if (GameManager.instance.activeRiceBag[index] != null)
        {
            float curHP = GameManager.instance.activeRiceBag[index].hp;
            float maxHP = GameManager.instance.activeRiceBag[index].maxHp;
            mySlider.value = curHP / maxHP;
        }
    }

    void UpdateBagLevel(int index, string colorName)
    {
        if (myText == null) return;

        if (GameManager.instance.activeRiceBag[index] != null)
        {
            myText.text = string.Format("{0} 쌀포대 Lv.{1:F0}", colorName, GameManager.instance.activeRiceBag[index].level);
        }
    }

    void UpdateMonkeyStat(int statIndex)
    {
        MonkeyCS refMonkey = GameManager.instance.activeMonkeys[0];

        switch (statIndex)
        {
            case 0: myText.text = string.Format("Bonust Atk\n: +{0:F1}", refMonkey.GetBaseDamageBonus()); break;
            case 1: myText.text = string.Format("Bonust Speed\n: +{0:F1}", refMonkey.GetBaseAttackSpeedBonus()); break;
            case 2: myText.text = string.Format("Bonust Crit\n: +{0:P0}", refMonkey.GetBaseCriticalChanceBonus()); break;
            case 3: myText.text = string.Format("Bonust CritDmg\n: +{0:P0}", refMonkey.GetBaseCriticalDamageBonus()); break;
            case 4:
                myText.text = string.Format("Bonus Ammo : + {0}", GameManager.instance.bonusAmmo);
                break;
        }
    }

}
