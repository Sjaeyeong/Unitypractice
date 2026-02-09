using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Range, Special } // 우리 기획은 모두 원거리이므로 간단하게 분류

    [Header("# Main Info")]
    public int itemId;          // 무기 번호 (0~6)
    public string itemName;     // 무기 이름 (예: 오리지널 서클)
    [TextArea]
    public string itemDesc;     // 무기 설명 (예: ○ 단발 사격...)
    public Sprite itemIcon;     // UI에 표시될 아이콘

    [Header("# Level Data")]
    public float baseDamage;    // 초기 공격력
    public int baseCount;       // 초기 발사 수
    public float baseFireRate;  // 연사 속도 (Weapon.fireRate)
    public float baseSpeed; // 탄환 속도 (Weapon.bulletSpeed)

    [Header("# Weapon Setting")]
    public GameObject projectile; // 이 무기가 사용할 총알 프리팹
}