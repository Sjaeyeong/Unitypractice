using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("# Weapon Settings")]
    public int id;          // 무기 ID (0~6)
    public int prefabId;    // 사용할 총알 프리팹 인덱스
    public float damage;    // 데미지
    public int count;       // 발사 수
    public float speed;     // 탄속
    public float fireRate;  // 연사 속도

    float timer;
    Scanner scanner;

    void Awake()
    {
        scanner = GetComponentInParent<Scanner>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > fireRate) {
            timer = 0;
            Fire();
        }
    }

    public void Init(ItemData data)
    {
        // ... 기본 세팅 로직 ...
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        fireRate = data.baseFireRate;
        speed = data.baseSpeed;
        // 무기 종류별로 두 가지 속도를 다르게 세팅
        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }
    }

    void Fire()
    {
        if (scanner == null || scanner.nearestTarget == null)
            return;

        Vector3 targetPos = scanner.nearestTarget.position;
        Vector2 dir = (targetPos - transform.position).normalized;

        int per = 0;
        switch (id)
        {
            case 2: // [■■■■■] 라인 피어서
            case 6: // ☽ 문메랑
                per = 99; break;
            case 4: // ◑ 헤비 서클
                per = 2; break;
        }

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        bullet.rotation = Quaternion.Euler(0, 0, angle);
        
        bullet.GetComponent<Bullet>().Init(damage, per, dir, speed);
    }
}