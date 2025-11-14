using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

  void FixedUpdate()
  {
    if (!isLive)
        return;

    Vector2 dirVec = target.position - rigid.position; // 방향 벡터 = 타겟 위치 - 현재 위치
    Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // 단위 벡터 * 속도 * 시간 (프레임 영향으로 결과가 달라지지 않기위해 FixedDeltaTime사용)
    rigid.MovePosition(rigid.position + nextVec);
    rigid.linearVelocity = Vector2.zero;
  }

  void LateUpdate()
  {
    if (!isLive)
        return;

    spriter.flipX = target.position.x < rigid.position.x;
  }
}
