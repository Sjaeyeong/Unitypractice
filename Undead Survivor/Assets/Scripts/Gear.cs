using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear(); // 처음 생성할 때 기능들을 부여
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear(); // 레벨업할 때도 기능들을 다시 부여
    }

    void ApplyGear()
    {
        switch(type){
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoes:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        
        foreach(Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }
    
    void SpeedUp()
    {
        float speed = 3;
        GameManager.instance.player.speed = speed + (speed * rate);
    }
}
