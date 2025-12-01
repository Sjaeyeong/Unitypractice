using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RiceBag : MonoBehaviour
{
    [Header("# Bag Info")]
    public float hp;
    public float maxHp;
    public int level;

    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    bool isLive;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void OnEnable() // 다시 소환될때 적용될 코드
    {
        isLive = true;
        hp = maxHp;

    }

    public void Init(SpawnData data)
    {
        if (data.level % 10 == 0)
        {
            anim.runtimeAnimatorController = animCon[1];
            maxHp = data.hp * 1.2f;
        }
        else
        {
            anim.runtimeAnimatorController = animCon[0];
            maxHp = data.hp;
            
        }

        hp = data.hp;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        hp -= collision.GetComponent<Bullet>().damage;

        if (hp > 0)
        {
            
        }

        else
        {
            
        }

    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

}
