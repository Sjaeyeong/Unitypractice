using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    // public float maxGameTime;

    [Header("# Core Info")]
    public float hp;
    public float maxHp;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600};

    [Header("# GameObject")]
    public PoolManager pool;
    public Core core;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart()
    {
        hp = maxHp;
        core.gameObject.SetActive(true);

        Resume();

    }

    public void GameOver()
    {
        
    }

    void Update()
    {
        if (!isLive)
            return;
        gameTime += Time.deltaTime;

        // if (gameTime > maxGameTime)
        // {
        //     gameTime = maxGameTime;
        //     GameVictory();
        // }
    }

    public void GetExp(int amount)
    {
        if(!isLive)
            return;
        
        exp += amount;

        // 만약 다음 레벨 경험치 데이터가 없다면 레벨업 중단 (에러 방지)
        if (level < nextExp.Length && exp >= nextExp[level]) {
            level++;
            exp = 0;
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;

    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;

    }
}
