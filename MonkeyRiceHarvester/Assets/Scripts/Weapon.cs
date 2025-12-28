using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("# Weapon Info")]
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    public float crit;
    public float critDmg;

    [Header("# Base Value Backup")]
    private float baseDamageValue;
    public float baseSpeedValue;
    private float baseCritValue;
    private float baseCritDmgValue;

    private float burstDelay = 0.1f;
    float timer;
    public bool isAttacking = false;
    MonkeyCS monkey;

    void Awake()
    {
        monkey = GetComponentInParent<MonkeyCS>();

        baseDamageValue = damage;
        baseSpeedValue = speed;
        baseCritValue = crit;
        baseCritDmgValue = critDmg;

        UpgradeStats();
    }

    void Update()
    {
        if (!isAttacking) return;

        float cooldown = 1f / speed;
        timer += Time.deltaTime;

        if (timer > cooldown)
        {
            timer = 0f;
            StartCoroutine(FireBurstRoutine()); // 연사 코루틴
        }
    }

    public void UpgradeStats()
    {
        if (!monkey)
            return;

        damage = baseDamageValue + monkey.GetBaseDamageBonus();
        speed = baseSpeedValue + monkey.GetBaseAttackSpeedBonus();
        crit = Mathf.Min(baseCritValue + monkey.GetBaseCriticalChanceBonus(), 1.0f);
        critDmg = baseCritDmgValue + monkey.GetBaseCriticalDamageBonus();
    }

    public void SetAttackState(bool attack)
    {
        isAttacking = attack;
    }

    IEnumerator FireBurstRoutine()
    {
        if (monkey.target == null) yield break;

        int totalCount = count + GameManager.instance.bonusAmmo;

        for (int i = 0; i < totalCount; i++)
        {
            if (id != 1 && monkey.target == null) yield break;

            GameObject bulletObj = GameManager.instance.pool.Get(prefabId);
            bulletObj.transform.position = transform.position;
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();

            // 크리티컬 계산 (UnityEngine 명시)
            float finalDamage = damage;
            if (UnityEngine.Random.value <= crit) finalDamage *= critDmg;

            Vector2 launchVelocity;
            float gravity;
            bool isPen;

            // 2번째 원숭이 (삽) 로직 간소화
            if (id == 1) 
            {
                launchVelocity = Vector2.right * 15f; // 단순히 오른쪽 방향으로 고정 발사
                gravity = 0f;                        // 중력 없음 (일직선)
                isPen = true;                        // 관통 활성화
            }
            else // 1번(삼지창), 3번(바나나) 원숭이 로직
            {
                gravity = (id == 0) ? 1.2f : 2.2f;
                launchVelocity = CalculatePreciseVelocity(monkey.target.position, gravity);
                isPen = false;
            }
            
            bulletScript.Init(finalDamage, isPen, launchVelocity, gravity);

            yield return new WaitForSeconds(burstDelay); // 꼬리 물기 연사 효과
        }
    }

    Vector2 CalculatePreciseVelocity(Vector3 target, float gravityScale)
    {
        Vector2 diff = target - transform.position;
        float g = Mathf.Abs(Physics2D.gravity.y * gravityScale);

        if (gravityScale <= 0) return diff.normalized * 15f; // 직선 삽

        // 수평 거리에 따른 체공 시간 계산
        float time = Mathf.Sqrt(Mathf.Abs(diff.x) / g) * 1.5f; 
        float vx = diff.x / time;
        float vy = (diff.y / time) + (0.5f * g * time);

        return new Vector2(vx, vy);
    }

}
