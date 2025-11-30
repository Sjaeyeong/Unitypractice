using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class SpawnInputHandler : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    private int currentBag = 0;
    private const int TOTAL_BAGTYPES = 3;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject bag = GameManager.instance.pool.Get(0);
        bag.transform.position = spawnPoint[currentBag].position;
    }

}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public int level;
    public float hp;
    public float exp;
    public int rice;
    public int banana;
}