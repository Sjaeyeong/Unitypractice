using NUnit.Framework;
using UnityEngine;

public class RiceBag : MonoBehaviour
{
    [Header("# Settings")]
    public int id; // 0: 기본(빨강), 1: 파랑, 2: 초록 3: 황금
    public int level;
    public double maxHP;
    public double currentHP;
    [Header("# Visuals (Assign 4 Sprites)")]
    // [0]: 빨강, [1]: 파랑, [2]: 초록, [3]: 황금
    public Sprite[] sprites;

    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;

    bool isDead;
    bool isGolden;


    void Awake()
    {
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    public void Init(int newLevel)
    {
        level = newLevel;
        isDead = false;
        coll.enabled = true; // 피격 가능 상태로 전환

        // 1. 황금 포대 여부 확인
        isGolden = level % 10 == 0;

        // 2. 스프라이트 설정 (3번 인덱스가 황금)
        if (sprites.Length >= 4)
        {
            if (isGolden)
                spriter.sprite = sprites[3]; 
            else
                spriter.sprite = sprites[id]; 
        }

        // 3. 체력 계산
        double healthMultiplier = isGolden ? 1.5f : 1.0f; // 황금포대의 체력은 기본의 1.5배
        maxHP = 100 * Mathf.Pow(1.2f, level - 1) * healthMultiplier;
        currentHP = maxHP;

        gameObject.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;
        
        currentHP -= damage;


        if (anim != null)
            anim.SetTrigger("Hit");
        
        if (currentHP <= 0)
        {
            Destroy();
        }

    }

    void Destroy()
    {
        isDead = true;
        coll.enabled = false;
        
        // 보상 계산 및 지급
        double riceReward = maxHP * 0.1f;
        int bananaReward = 0;

        if (isGolden)
        {
            riceReward *= 10;
            bananaReward = level / 10;
        }

        GameManager.instance.GetReward(riceReward, bananaReward);
        
        gameObject.SetActive(false);

        // 매니저에게 죽음을 알림
        GameManager.instance.OnRiceBagDead(id);
        
    }



}
