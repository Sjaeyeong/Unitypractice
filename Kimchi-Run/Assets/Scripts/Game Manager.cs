using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake() // start보다 먼저 유니티에 의해 실행되는 함수
    {
        if(Instance == null) // gamemanger 인스턴스를 다른 곳에서도 공유해서 사용할 수 있음
        {
            Instance = this;
        }   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
