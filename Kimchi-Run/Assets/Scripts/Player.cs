using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce;

    [Header("References")]
    public Rigidbody2D PlayerRigidBody;

    public BoxCollider2D PlayerCollider;
    public Animator PlayerAnimator;

    private bool isGrounded = true;
    public int lives = 3;
    public bool isInvincible = false;
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
            PlayerAnimator.SetInteger("state", 1);
        }
    }

    void KillPlayer()
    {
        PlayerCollider.enabled = false;
        PlayerAnimator.enabled = false;
        PlayerRigidBody.AddForceY(jumpForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision) { // OnCollisionEnter2D는 입력된 Collider가 플레이어의 Collider와 부딪히면 호출 됨
        if(collision.gameObject.name == "Platforms") {
            if (!isGrounded) { // 게임을 막 시작했을 땐 state를 2로 바꾸고 싶지 않기 때문에 조건문 추가
                PlayerAnimator.SetInteger("state", 2);
            }
            isGrounded = true;
        }
    }

    void Hit()
    {
        lives -= 1;
        if(lives == 0)
        {
            KillPlayer();
        }
    }

    void Heal()
    {
        lives = Mathf.Min(3, lives + 1); // 어떤 이유든지 3으로 설정 = 3과 lives+1중 최소로 반환
    }

    void StartInvincible()
    {
        isInvincible = true;
        Invoke("StopInvincible", 5f);
    }

    void StopInvincible()
    {
        isInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "enemy")
        {
            Destroy(collider.gameObject);
            if(!isInvincible) {
                Hit();
            }
        }
        else if(collider.gameObject.tag == "food")
        {
            Destroy(collider.gameObject);
            Heal();
        }
        else if(collider.gameObject.tag == "golden")
        {
            Destroy(collider.gameObject);
            StartInvincible();
        }
    }
}
