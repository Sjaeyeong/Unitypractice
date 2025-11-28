using UnityEngine;

public class SpawnInputHandler : MonoBehaviour
{
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 또는 터치 감지
        if (Input.GetMouseButtonDown(0))
        {
            // GameManager가 준비되었고, 빨강 포대가 죽어있다면 수동 소환 시도
            if (!GameManager.instance)
            {
                int numBags = GameManager.instance.bags.Length; 
                
                for (int i = 0; i < numBags; i++)
                {
                    // GameManager.ManualSpawn(i)는 포대 i가 '해금되었고' '현재 죽어있는' 경우에만 소환을 실행합니다.
                    GameManager.instance.ManualSpawn(i);
                }
            }
        }
    }
}