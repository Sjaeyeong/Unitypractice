using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    public void Init(float damage, int per)
    {
        this.damage = damage; // this.damage는 클래스의 damage, 오른쪽 damage는 매개변수의 damage
        this.per = per;
    }
}
