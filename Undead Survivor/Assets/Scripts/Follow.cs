using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position); // WorldToScreenPoint : 월드 상의 오브젝트 위치를 스크린 좌표로 변환
        
    }

}
