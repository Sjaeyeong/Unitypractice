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
        if (target != null && target.gameObject.activeSelf)
        {
            RiceBag rb = target.GetComponent<RiceBag>();
            if (rb != null && rb.isLive) return; 
        }

        // 2. 새로운 타겟 찾기 (HP 우선 -> 거리 차선)
        target = null;
        float maxRatio = -1f;
        float minDist = Mathf.Infinity;

        if (spawner == null || spawner.spawnedBags == null) return;

        foreach (GameObject bagObject in spawner.spawnedBags)
        {
            if (bagObject != null && bagObject.activeSelf)
            {
                RiceBag riceBag = bagObject.GetComponent<RiceBag>();
                
                // 살아있는 가방 중에서만 검사
                if (riceBag != null && riceBag.isLive)
                {
                    // 체력 비율 계산 (0.0 ~ 1.0)
                    float currentRatio = riceBag.hp / riceBag.maxHp;
                    float currentDist = Vector2.Distance(transform.position, bagObject.transform.position);

                    // 우선순위 1: 체력 비율이 더 높은 가방을 발견한 경우
                    if (currentRatio > maxRatio)
                    {
                        maxRatio = currentRatio;
                        minDist = currentDist;
                        target = bagObject.transform;
                    }
                    // 우선순위 2: 체력 비율이 같다면(예: 모두 풀피), 더 가까운 가방 선택
                    else if (Mathf.Approximately(currentRatio, maxRatio))
                    {
                        if (currentDist < minDist)
                        {
                            minDist = currentDist;
                            target = bagObject.transform;
                        }
                    }
                }
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
        BonusAttackSpeed = bonusValue;
        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }

    public void SetCriticalChance(float bonusValue)
    {
        BonusCriticalChance = bonusValue;
        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }

    public void SetCriticalDamage(float bonusValue)
    {
        BonusCriticalDamage = bonusValue;
        if (weapon != null)
        {
            weapon.UpgradeStats();
        }
    }
}
