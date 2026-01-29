using System.Data;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Scanner scanner;

    Rigidbody2D rb;
    SpriteRenderer spriter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {
        // 체력 체크 및 게임 오버 로직을 Update나 별도 함수로 관리
        if (GameManager.instance.hp <= 0 && GameManager.instance.isLive)
        {
            // 무기들 비활성화
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GameManager.instance.GameOver();
        }
    }
}
