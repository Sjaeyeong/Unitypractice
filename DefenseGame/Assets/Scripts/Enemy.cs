using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("# Enemy Info")]
    public float hp;
    public float speed;
    public int exp;
    public float maxHp;
    public float damage;
    bool isLive;

    [Header("# Visuals")]
    public Sprite[] enemySprites;

    Rigidbody2D target;
    Rigidbody2D rb;
    Collider2D coll;
    SpriteRenderer spriter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();

    }

    void OnEnable()
    {
        if (GameManager.instance.core != null)
            target = GameManager.instance.core.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rb.simulated = true;
        spriter.sortingOrder = 2;
        hp = maxHp;
    }

    void FixedUpdate()
    {
        if (!isLive || !GameManager.instance.isLive || target == null)
            return;

        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);

        float angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    public void Init(Spawner.SpawnData data)
    {
        speed = data.speed;
        maxHp = data.hp;
        hp = data.hp;
        exp = data.exp;
        damage = data.damage;

        if (data.spriteType < enemySprites.Length)
        {
            spriter.sprite = enemySprites[data.spriteType];
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLive) return;

    // 1. 총알에 맞았을 때
    if (collision.CompareTag("Bullet"))
    {
        TakeDamage(collision.GetComponent<Bullet>().damage);
    }
    // 2. Core(Player)에 부딪혔을 때
    else if (collision.CompareTag("Player"))
    {
        // GameManager의 체력을 깎음 (기존 Core.cs 로직을 여기로 이전)
        GameManager.instance.hp -= damage; 

        // 데미지를 주고 적은 즉시 사망(풀로 반환)
        Die();
    }

    }

    public void TakeDamage(float damage)
    {
        if (!isLive) return;

        hp -= damage;

        // 히트 시각 피드백 (선택 사항: 나중에 애니메이션이나 색상 변경 추가)
        // StartCoroutine(HitColorRoutine()); 

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Knockback(Vector2 dir)
    {
        // 순간적인 힘을 가함 (기존 속도 무시)
        rb.AddForce(dir * 3f, ForceMode2D.Impulse);
    }

    void Die()
    {
        isLive = false;
        coll.enabled = false;
        rb.simulated = false;

        GameManager.instance.kill++;
        GameManager.instance.GetExp(exp);

        gameObject.SetActive(false);
    }
}
