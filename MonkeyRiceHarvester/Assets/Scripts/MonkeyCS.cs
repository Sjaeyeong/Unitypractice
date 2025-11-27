using UnityEngine;
using UnityEngine.EventSystems;

public class MonkeyCS : MonoBehaviour
{
    // [0] : Main Monkey, [1] ~ : Other Monkey
    [Header("# Monkey Identity")]
    public int monkeyID = 0;
    public string monkeyName = "쌀숭이";

    [Header("# Current Stats")]
    public long level = 1;
    public double damage = 1.0f;
    public float attackSpeed = 1.0f;

    public float attackCooldown;
    public float attackTimer;

    public TargetRice target;

    void Start()
    {
        attackCooldown = 1f / attackSpeed;
        target = FindObjectOfType<TargetRice>();

    }

    void Update()
    {
        if (!target)
            return;
        
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }


    }

    void Attack()
    {
        double totalDamage = damage;
    }


}
