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

    [Header("Target Logic")]
    public Transform[] targets;
    public int currentTargetIndex = 0;

    float timer;
    MonkeyCS monkey;

    void Awake()
    {
        FindSpawnPoint();
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

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 0.5f;
                break;
            case 1:

                break;
            default:

                break;
        }
    }

    void FindSpawnPoint()
    {
        targets = GetComponentsInChildren<Transform>();
    }

    void Fire()
    {
        if ()
    }

}
