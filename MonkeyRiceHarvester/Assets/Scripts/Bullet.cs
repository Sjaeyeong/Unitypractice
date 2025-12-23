using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public bool isPenetrating;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, bool isPenetrating, Vector2 velocity, float gravity)
    {
        this.damage = damage;
        this.isPenetrating = isPenetrating;
        
        rigid.gravityScale = gravity;
        rigid.linearVelocity = velocity;
    }

    void Update()
    {
        // 총알이 이동 방향을 바라보도록 회전
        if (rigid.linearVelocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rigid.linearVelocity.y, rigid.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("RiceBag"))
            return;

        if (!isPenetrating)
        {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        
        gameObject.SetActive(false);
    }
}
