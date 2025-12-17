using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnInputHandler : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    [HideInInspector] public GameObject[] spawnedBags = new GameObject[3];
    private int currentBag = 0;
    private const int TOTAL_BAGTYPES = 3;

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
            {
                continue;
            }

            GameObject bag = GameManager.instance.pool.Get(i);
            bag.transform.position = spawnPoint[i+1].position;

            RiceBag riceBag = bag.GetComponent<RiceBag>();

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