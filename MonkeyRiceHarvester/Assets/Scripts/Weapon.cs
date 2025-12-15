using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;

    public float damage;
    public int count;
    public float speed;
    public float crit;
    public float critDmg;
    public int per;

    [Header("# Base Value Backup")]
    private float baseDamageValue;
    private float baseSpeedValue;
    private float baseCritValue;
    private float baseCritDmgValue;

    [Header("Target Logic")]
    public Transform[] targets;

    float timer;
    public bool isAttacking = false;
    MonkeyCS monkey;

    void Awake()
    {
        monkey = GetComponentInParent<MonkeyCS>();
        FindSpawnPoint();

        baseDamageValue = damage;
        baseSpeedValue = speed;
        baseCritValue = crit;
        baseCritDmgValue = critDmg;

        UpgradeStats();
    }

    void Update()
    {
        float cooldown = 1f / speed;

        switch (id)
        {
            case 0:
                timer += Time.deltaTime;

                if (timer > cooldown)
                {
                    timer = 0f;
                    Fire();
                }

                break;
            case 1:
                timer += Time.deltaTime;

                if (timer > cooldown)
                {
                    timer = 0f;
                    Fire();
                }

                break;
            default:
                timer += Time.deltaTime;

                if (timer > cooldown)
                {
                    timer = 0f;
                    Fire();
                }

                break;
        }
    }

    public void LevelUP(float damage)
    {
        this.damage = damage;

        
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

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 1.2f;
                break;
            case 1:
                speed = 1.5f;
                break;
            default:
                speed = 0.5f;
                break;
        }
    }

    public void SetAttackState(bool attack)
    {
        isAttacking = attack;
    }

    void FindSpawnPoint()
    {
        targets = GetComponentsInChildren<Transform>();
    }

    void Fire()
    {
        if (!isAttacking)
            return;
        if(id != 2)
        {
            Vector3 dir = transform.right;

            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }
        else
        {
            Vector3 targetPos = monkey.target.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }
    }

}
