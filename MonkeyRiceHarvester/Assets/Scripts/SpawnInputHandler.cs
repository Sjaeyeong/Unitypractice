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
        // for (int i=0; i<TOTAL_BAGTYPES; i++)
        // {
        //     GameObject bag = GameManager.instance.pool.Get(i);
        //     bag.transform.position = spawnPoint[i+1].position;
        //     bag.GetComponent<RiceBag>().Init(spawnData[i]);
        // }
        GameObject redBag = GameManager.instance.pool.Get(0);
        GameObject blueBag = GameManager.instance.pool.Get(1);
        GameObject greenBag = GameManager.instance.pool.Get(2);

        redBag.transform.position = spawnPoint[1].position;
        blueBag.transform.position = spawnPoint[2].position;
        greenBag.transform.position = spawnPoint[3].position;

        redBag.GetComponent<RiceBag>().Init(spawnData[0]);
        blueBag.GetComponent<RiceBag>().Init(spawnData[1]);
        greenBag.GetComponent<RiceBag>().Init(spawnData[2]);
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