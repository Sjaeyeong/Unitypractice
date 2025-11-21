using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    [Header("# Spawn Setting")]
    public float[] spawnRateByLevel = {1.5f, 1.1f, 0.7f, 0.5f, 0.3f, 0.1f};
    public float levelTime;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
            
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);
        
        int rateIndex = Mathf.Min(level, spawnRateByLevel.Length - 1);
        float currentSpawnRate = spawnRateByLevel[rateIndex];

        if (timer > currentSpawnRate)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        int randomLev = Random.Range(0, level + 1);
        enemy.GetComponent<Enemy>().Init(spawnData[randomLev]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
    public int exp;
    public float scaleEnemy = 1f;
}
