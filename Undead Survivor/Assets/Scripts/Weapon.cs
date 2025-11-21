using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // ������ ���̵�
    public int prefabId; // ������ ������ ���̵�
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
            
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // forward�� �ƴ� back�� ������ ���ǵ尡 ���� -�� �����Ǿ��ֱ� ����

                break;
            // case 1:
            //     break;
            // case 2:
            //     break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
            
        }

        // Test code
        // if (Input.GetButtonDown("Jump"))
        // {
        //     LevelUp(10, 1);
        // }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage += damage * Character.Damage;
        this.count += count;

        if(id == 0)
            Batch();
        
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero; // player�ȿ��� ��ġ�� ����� �ϱ� ������ local���

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed; // Melee Weapon
                Batch();

                break;
            // case 1:
            //     break;
            // case 2:
            //     break;
            default:
                speed = 0.5f * Character.WeaponRate; // Range weapon
                break;
            
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
    }

    void Batch()
    {

        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }


            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity; // ȸ���� �ʱⰪ

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // ��������� ������ �ǹ̾��� ������ -1�� �����Ͽ� �������� ������ / -1 is Infinity Per.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // ��ǥ�� ���͸� �־� ȸ���� ������
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
