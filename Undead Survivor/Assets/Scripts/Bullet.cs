using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage; // this.damage는 클래스의 damage, 오른쪽 damage는 매개변수의 damage
        this.per = per;

        if (per > -1)
        {
            rigid.linearVelocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        per--;

        if (per == -1)
        {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false); // object가 pool으로 관리되기 때문에 Destroy 사용금지

        }
    }
}
