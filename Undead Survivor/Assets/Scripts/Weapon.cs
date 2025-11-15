using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // 무기의 아이디
    public int prefabId; // 무기의 프리팹 아이디
    public float damage;
    public int count;
    public float speed;

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // forward가 아닌 back인 이유는 스피드가 지금 -로 설정되어있기 때문

                break;
            // case 1:
            //     break;
            // case 2:
            //     break;
            default:
                break;
            
        }

        // Test code
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(2, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage += damage;
        this.count += count;

        if(id == 0)
            Batch();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();

                break;
            // case 1:
            //     break;
            // case 2:
            //     break;
            default:
                break;
            
        }
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
            bullet.localRotation = Quaternion.identity; // 회전의 초기값

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1); // 근접무기라 관통이 의미없기 때문에 -1로 설정하여 무한으로 설정함 / -1 is Infinity Per.
        }
    }
}
