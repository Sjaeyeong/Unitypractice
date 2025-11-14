using UnityEngine;

public class Reposition : MonoBehaviour
{
  Collider2D coll;

  void Awake()
  {
    coll = GetComponent<Collider2D>();
  }
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void OnTriggerExit2D(Collider2D collision)
  {
    if (!collision.CompareTag("Area"))
      return;

      Vector3 playerPos = GameManager.instance.player.transform.position;
      Vector3 myPos = transform.position;
      float diffx = Mathf.Abs(playerPos.x - myPos.x);
      float diffy = Mathf.Abs(playerPos.y - myPos.y);

      Vector3 playerDir = GameManager.instance.player.inputVec;
      float dirX = playerDir.x < 0 ? -1 : 1;
      float dirY = playerDir.y < 0 ? -1 : 1;

      switch (transform.tag)
        {
        case "Ground":
            if (diffx > diffy)
            {
                transform.Translate(Vector3.right * dirX * 40);
            }
            else if (diffx < diffy)
            {
                transform.Translate(Vector3.up * dirY * 40);
            }
            break;
        case "Enemy":
            if (coll.enabled)
            {
              transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f)); // 플레이어가 보는 화면 끝에서 소환되게끔 설정
            }
            break;
        }
  }
}
