using NUnit.Framework;
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
        anim.runtimeAnimatorController = animCon[data.spriteType];
        maxHp = data.hp;
        hp = data.hp;
    }

}
