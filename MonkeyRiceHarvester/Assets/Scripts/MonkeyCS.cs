using UnityEngine;

public class MonkeyCS : MonoBehaviour
{
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
}
