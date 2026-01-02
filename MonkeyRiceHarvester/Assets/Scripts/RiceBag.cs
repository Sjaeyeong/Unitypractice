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

    public float baseHP;
    public int baseRice;
    public float baseExp;

    public RuntimeAnimatorController[] animCon;

    [Header("# HUD Link")]
    public Slider hudHpSlider;

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

        level++;

        coll.enabled = true;
        rigid.simulated = true;
        anim.SetBool("Destroy", false);
    }

    void LateUpdate()
    {
        if (hudHpSlider != null && hudHpSlider.gameObject.activeInHierarchy)
        {
            hudHpSlider.value = hp / maxHp; 
        }
    }

    public void ForceRecalculateHP()
    {
        float levelMult = 1f + (level * 0.1f);
        float currentBaseHp = baseHP * levelMult;

        exp = baseExp;
        rice = baseRice;

        if (level % 10 == 0)
        {
            anim.runtimeAnimatorController = animCon[1];
            maxHp = currentBaseHp * 1.2f;
            banana = level / 10;
        }
        else
        {
            anim.runtimeAnimatorController = animCon[0];
            maxHp = currentBaseHp;
            banana = 0;
            
        }

        hp = maxHp;
    }

    // public void Init(SpawnData data)
    // {
    //     baseExp = data.exp;
    //     baseHP = data.hp;
    //     baseRice = data.rice;
        
    // }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) 
            return;

        float damageTaken = collision.GetComponent<Bullet>().damage;
        hp -= damageTaken;
        
        if (GameManager.instance != null)
            GameManager.instance.totalDamage += damageTaken;

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

            if (GameManager.instance != null)
            {
                GameManager.instance.rice += rice;
                GameManager.instance.banana += banana;
            }
            
            anim.SetBool("Destroy", true);
            
        }

    }

    void Destroy()
    {
        gameObject.SetActive(false);

        if (GameManager.instance != null && GameManager.instance.isAutoSpawn)
        {
            SpawnInputHandler spawner = FindAnyObjectByType<SpawnInputHandler>();
            if(spawner != null)
                spawner.Invoke("Spawn", 0.7f);
        }
    }

}