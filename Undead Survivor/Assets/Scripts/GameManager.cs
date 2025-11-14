using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    public float maxGameTime = 2 * 10f; // 게임 최대 시간
    public PoolManager pool;
    public Player player;

  void Awake()
  {
    instance = this;
  }

      void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

}
