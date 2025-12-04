using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RiceBag : MonoBehaviour
{
    [Header("# Bag Info")]
    public float exp;
    public float hp;
    public float maxHp;
    public int level;
    public bool isLive;
    public int rice;
    public int banana;

    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;
    Collider2D coll;

    public SpawnData[] spawnData;


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
        level++;
        if (level % 10 == 0)
        {
            anim.runtimeAnimatorController = animCon[1];
            maxHp = hp * 1.2f;
        }
        else
        {
            anim.runtimeAnimatorController = animCon[0];
            maxHp = hp;
            
        }
        hp = maxHp;
        coll.enabled = true;
        rigid.simulated = true;
        anim.SetBool("Destroy", false);
    }

    public void Init(SpawnData data)
    {
        exp = data.exp;
        hp = data.hp;
        maxHp = data.maxHP;
        rice = data.rice;
        banana = data.banana;
        
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

            // level++;
            GameManager.instance.kill++;
            GameManager.instance.GetExp(exp);
            
            anim.SetBool("Destroy", true);
            
        }

    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

}
