using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("# Bullet Info")]
    public float damage;
    public int per;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    public void Init(float damage, int per, Vector3 dir, float speed)
    {
        this.damage = damage;
        this.per = per;

        rb.linearVelocity = dir * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        // 1. 데미지 전달
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // 2. 무기별 특수 효과 처리 (ID나 특정 조건을 통해 확장)
        // 예: ◎ 더블 서클 (폭발) 로직이 여기에 들어갈 수 있습니다.
        // if (isExplosive) ExecuteExplosion();

        // 3. 관통 및 소멸 로직
        if (per == 99) {
            // 무한 관통 (라인 피어서, 문메랑 등) - 아무것도 하지 않음
            return;
        }

        per--; // 관통 횟수 차감

        if (per < 0)
        {
            // 속도를 멈추고 풀(Pool)로 반환
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

}
