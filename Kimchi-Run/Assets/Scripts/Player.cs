using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce;

    [Header("References")]
    public Rigidbody2D PlayerRigidBody;

    private bool isGrounded = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){  // 스페이스바가 눌렸는지 && 캐릭터가 땅에 있는지
            PlayerRigidBody.AddForceY(jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) { // OnCollisionEnter2D는 입력된 Collider가 플레이어의 Collider와 부딪히면 호출 됨
        if(collision.gameObject.name == "Platforms") {
            isGrounded = true;
        }
    }
}
