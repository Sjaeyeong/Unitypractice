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

        GameObject bulletObj = GameManager.instance.pool.Get(prefabId);
        if (bulletObj == null) {
            Debug.LogError($"Pool에서 {prefabId}번 프리팹을 가져오지 못했습니다.");
            return;
        }

        Transform bulletTransform = bulletObj.transform;
        bulletTransform.position = transform.position;
        
        // 방향 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        bulletTransform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. Bullet 스크립트 참조 확인 (가장 유력한 에러 지점)
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet == null) {
            Debug.LogError($"{bulletObj.name} 프리팹에 Bullet.cs 스크립트가 없습니다!");
            return;
        }

        // 무기별 관통 설정
        int per = (id == 2 || id == 6) ? 99 : (id == 4 ? 2 : 0);

        // 스탯 주입
        bullet.Init(damage, per, dir, speed);
    }
}