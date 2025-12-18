using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnInputHandler : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    [HideInInspector] public GameObject[] spawnedBags = new GameObject[3];
    private const int TOTAL_BAGTYPES = 3;

    float autoSpawnTimer = 0f;
    float autoSpawnDelay = 2.0f;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        Spawn();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0))
        {
            Spawn();
        }

        if (GameManager.instance != null && GameManager.instance.isAutoSpawn) {
            autoSpawnTimer += Time.deltaTime;
            if (autoSpawnTimer >= autoSpawnDelay) {
                autoSpawnTimer = 0f;
                Spawn();
            }
        }
    }

    void Spawn()
    {
        if (GameManager.instance == null)
            return;

        for (int i=0; i<TOTAL_BAGTYPES; i++)
        {
            if (!GameManager.instance.isBagTypeUnlocked[i])
                continue;
            
            GameObject existingBag = spawnedBags[i];

            if (existingBag != null && existingBag.activeSelf)
                continue;

            GameObject bag = GameManager.instance.pool.Get(i);
            bag.transform.position = spawnPoint[i+1].position;

            RiceBag riceBag = bag.GetComponent<RiceBag>();

            if (i == 0) riceBag.hudHpSlider = GameManager.instance.hudRedSlider;
            else if (i == 1) riceBag.hudHpSlider = GameManager.instance.hudBlueSlider;
            else if (i == 2) riceBag.hudHpSlider = GameManager.instance.hudGreenSlider;

            riceBag.baseExp = spawnData[i].exp;
            riceBag.baseHP = spawnData[i].hp;
            riceBag.baseRice = spawnData[i].rice;
            
            riceBag.ForceRecalculateHP();
            
            spawnedBags[i] = bag;

            GameManager.instance.activeRiceBag[i] = bag.GetComponent<RiceBag>();
        }
    }

}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public int level;
    public float hp;
    public float maxHP;
    public float exp;
    public int rice;
    public int banana;
}