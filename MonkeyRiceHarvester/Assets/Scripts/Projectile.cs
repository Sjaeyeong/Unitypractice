using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum MoveType { Straight, Parabola }
    [Header("Settings")]
    public MoveType moveType;
    public bool isPer;
    public float damage;
    public float speed = 10f;
    public float arcHeight = 2.0f; 

    private Vector3 startPos;
    private Vector3 targetPos;
    private float progress;
    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float dmg, Vector3 dirOrPos, MoveType type, bool per)
    {
        this.damage = dmg;
        moveType = type;
        isPer = per;
        progress = 0f;
        
        if (moveType == MoveType.Straight)
        {
            rigid.linearVelocity = dirOrPos.normalized * speed;
            float angle = Mathf.Atan2(dirOrPos.y, dirOrPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (moveType == MoveType.Parabola)
        {
            startPos = transform.position;
            targetPos = dirOrPos;
            rigid.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    void Update()
    {
        if (moveType == MoveType.Parabola)
        {
            float totalDist = Vector3.Distance(startPos, targetPos);
            float speedFactor = (totalDist > 0) ? speed / totalDist : 1f;

            progress += Time.deltaTime * speedFactor;

            if (progress >= 1.0f) progress = 1.0f;

            Vector3 linearPos = Vector3.Lerp(startPos, targetPos, progress);
            float height = Mathf.Sin(progress * Mathf.PI) * arcHeight;
            transform.position = linearPos + new Vector3(0, height, 0);

            if (progress >= 1.0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RiceBag"))
        {
            RiceBag bag = collision.GetComponent<RiceBag>();
            if (bag != null)
            {
                bag.TakeDamage(damage);
            }

            // 관통이 아니면 투사체 삭제 (Pool로 반환)
            if (!isPer)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // 화면 밖으로 나가면 비활성화
    void OnBecameInvisible() 
    {
        gameObject.SetActive(false);
    }
}