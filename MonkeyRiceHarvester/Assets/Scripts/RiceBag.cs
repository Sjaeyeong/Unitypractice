using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RiceBag : MonoBehaviour
{
    [Header("# Bag Info")]
    public float hp;
    public float maxHp;
    public int level = 1;
    public bool isLive;

    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;
    Collider2D coll;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    void OnEnable() // 다시 소환될때 적용될 코드
    {
        isLive = true;
        hp = maxHp;
        coll.enabled = true;
        rigid.simulated = true;
        anim.SetBool("Destroy", false);
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
            anim.SetTrigger("Hit");

        }

        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;

            level++;
            
            anim.SetBool("Destroy", true);
            Destroy();
            
        }

    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

}
