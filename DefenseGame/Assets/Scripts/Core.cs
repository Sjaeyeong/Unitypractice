using System.Data;
using UnityEngine;

public class Core : MonoBehaviour
{
    [Header("# Core Info")]
    public float hp = 100f;

    public void TakeDamae(float damage)
    {
        hp -= damage;
        if (hp <=0)
            Debug.Log("Game Over");
    }
}
