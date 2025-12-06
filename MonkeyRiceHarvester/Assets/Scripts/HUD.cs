using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// using TMPro;

public class HUD : MonoBehaviour
{
    public enum InfoType{ Exp, Level, Kill, Time, Rice, Banana, redHP, blueHP, greenHP, redBagLevel, blueBagLevel, greenBagLevel }
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


        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp;
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:

                break;
            case InfoType.Time:
                
                break;
            case InfoType.Rice:
                myText.text = string.Format("{0:F0}", GameManager.instance.rice);
                break;
            case InfoType.Banana:
                myText.text = string.Format("{0:F0}", GameManager.instance.banana);
                break;
            case InfoType.redHP:
                float curRedHP = GameManager.instance.activeRiceBag[0].hp;
                float maxRedHp = GameManager.instance.activeRiceBag[0].maxHp;
                mySlider.value = curRedHP / maxRedHp;
                break;
            case InfoType.blueHP:
                float curBlueHP = GameManager.instance.activeRiceBag[1].hp;
                float maxBlueHp = GameManager.instance.activeRiceBag[1].maxHp;
                mySlider.value = curBlueHP / maxBlueHp;
                break;
            case InfoType.greenHP:
                float curGreenHP = GameManager.instance.activeRiceBag[2].hp;
                float maxGreenHp = GameManager.instance.activeRiceBag[2].maxHp;
                mySlider.value = curGreenHP / maxGreenHp;
                break;
            case InfoType.redBagLevel:
                myText.text = string.Format("빨강 쌀포대 Lv.{0:F0}", GameManager.instance.activeRiceBag[0].level);
                break;
            case InfoType.blueBagLevel:
                myText.text = string.Format("파랑 쌀포대 Lv.{0:F0}", GameManager.instance.activeRiceBag[1].level);
                break;
            case InfoType.greenBagLevel:
                myText.text = string.Format("초록 쌀포대 Lv.{0:F0}", GameManager.instance.activeRiceBag[2].level);
                break;
            // case InfoType.NULL:

            //     break;

        }
    }


}
