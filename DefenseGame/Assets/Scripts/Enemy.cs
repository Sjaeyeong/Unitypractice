using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("# Enemy Info")]
    public float hp;
    public float speed;

    Rigidbody2D rb;
    Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
}
