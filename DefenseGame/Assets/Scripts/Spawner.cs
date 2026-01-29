using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    [Header("# Spawn Setting")]
    public float[] spawnRateByLevel = {1.5f, 1.1f, 0.7f, 0.5f, 0.3f, 0.1f};
    public float levelTime;
    public float spawnRadius = 18f;

    int level;
    float timer;

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
        Vector2 spawnDir = Random.insideUnitCircle.normalized;
        enemy.transform.position = (Vector3)(spawnDir * spawnRadius);

        int randomDataIdx = Random.Range(0, level + 1);
        enemy.GetComponent<Enemy>().Init(spawnData[randomDataIdx]);
    }

    [System.Serializable]
    public class SpawnData
    {
        public float spawnTime;
        public int spriteType;
        public int hp;
        public float speed;
        public int exp;
        public int damage;
    }
}
