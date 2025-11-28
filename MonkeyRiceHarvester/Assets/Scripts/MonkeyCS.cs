using UnityEngine;

public class MonkeyCS : MonoBehaviour
{
    // [0] : Main Monkey, [1] ~ : Other Monkey
    [Header("# Monkey Info")]
    public int id; // 0: 기본, 1: 낫, 2: 삽
    public int prefabId;
    public float damage;
    public float speed;
    public bool isLive;

    float timer;
    public Animator weaponAnim;

    void Awake()
    {
        if (!weaponAnim)
            weaponAnim = GetComponentInChildren<Animator>(true);
    }

    void Start()
    {
        isLive = true;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        float delay = 1.0f / speed;

        timer += Time.deltaTime;

        if (timer > delay)
        {
            timer = 0f;
            Fire();
        }
    }

    void Fire()
    {
        RiceBag target = GetFirstActiveBag();

        // 타겟이 없거나 죽었으면 공격 안 함
        if (!target || id != 1)
            return;

        Vector3 targetPos = (target != null) ? target.transform.position : Vector3.zero;
        
        Vector3 fireDirOrPos = Vector3.zero;
        Projectile.MoveType moveType = Projectile.MoveType.Straight;
        bool isPiercing = false;

        switch (id)
        {
            case 0: // [기본] 포물선, 타겟 위치로 (관통X)
            case 2: // [삽 원숭이] 직선, 회전 필요
                // 타겟 방향 벡터 계산
                Vector3 dir = targetPos - transform.position;
                
                // Z축 회전 각도 계산 (무기가 X축(오른쪽)을 기본 정면으로 가정)
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                
                // [핵심] 계산된 회전 값 적용
                transform.rotation = Quaternion.Euler(0, 0, angle);

                if (id == 0)
                {
                    fireDirOrPos = targetPos; // 포물선은 목표 좌표
                    moveType = Projectile.MoveType.Parabola;
                    isPiercing = false;
                }
                else // id == 2
                {
                    fireDirOrPos = dir.normalized; // 직선은 방향 벡터
                    moveType = Projectile.MoveType.Straight;
                    isPiercing = false;
                }
                break;

            case 1: // [낫] 직선, 오른쪽으로 무조건 발사, 관통 O
                fireDirOrPos = Vector3.right; 
                moveType = Projectile.MoveType.Straight;
                isPiercing = true;
                break;
        }
        // 4. 애니메이션 재생
        if (weaponAnim != null) 
            weaponAnim.SetTrigger("Attack");

        GameObject bulletObj = PoolManager.instance.Get(prefabId);
        if (bulletObj != null)
        {
            bulletObj.transform.position = transform.position;
            Projectile p = bulletObj.GetComponent<Projectile>();
            p.Init(damage, fireDirOrPos, moveType, isPiercing);
        }


    }

    RiceBag GetFirstActiveBag()
    {
        if (GameManager.instance.bags == null) return null;

        foreach (RiceBag bag in GameManager.instance.bags)
        {
            // bags 배열 순서대로 확인 (0, 1, 2)
            if (bag.gameObject.activeSelf)
                return bag;
        }
        return null;
    }

}
