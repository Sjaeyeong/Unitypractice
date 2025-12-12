using UnityEngine;

public class MonkeyCS : MonoBehaviour
{
    [Header ("# Upgrade Stats")]
    [SerializeField] float BonusDamage = 0f;
    [SerializeField] float BonusAttackSpeed = 0f;
    [SerializeField] float BonusCriticalChance = 0f;
    [SerializeField] float BonusCriticalDamage = 0f;

    public RuntimeAnimatorController[] animCon;
    public Weapon weapon;

    public SpawnInputHandler spawner;

    public bool hasTarget = false;
    [HideInInspector] public Transform target;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        spawner = FindAnyObjectByType<SpawnInputHandler>();

        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }

    void Update()
    {
        Scan();

        if (target != null)
        {
            anim.SetBool("Attack", true);
            weapon.SetAttackState(true);
        }
        else
        {
            anim.SetBool("Attack", false);
            weapon.SetAttackState(false);
        }
    }

    void Scan()
    {
        target = null;

        if (spawner == null || spawner.spawnedBags == null)
            return;
        
        for (int i=0; i< spawner.spawnedBags.Length; i++)
        {
            GameObject bagObject = spawner.spawnedBags[i];

            if (bagObject != null && bagObject.activeSelf)
            {
                target = bagObject.transform;
                break;
            }
        }
    }

    public float GetBaseDamageBonus() { return BonusDamage; }
    public float GetBaseAttackSpeedBonus() { return BonusAttackSpeed; }
    public float GetBaseCriticalChanceBonus() { return BonusCriticalChance; }
    public float GetBaseCriticalDamageBonus() { return BonusCriticalDamage; }

    public void SetDamage(float bonusValue)
    {
        BonusDamage = bonusValue;
        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }

    public void SetAttackSpeed(float bonusValue)
    {
        BonusDamage = bonusValue;
        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }

    public void SetCriticalChance(float bonusValue)
    {
        BonusDamage = bonusValue;
        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }

    public void SetCriticalDamage(float bonusValue)
    {
        BonusDamage = bonusValue;
        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }
}
