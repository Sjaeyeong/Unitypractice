using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // 재화가 변경될때 마다 호출됨. UIManager에서 UI갱신
    [Header("# Game Info")]
    public int level = 1; // 전체 게임 레벨
    public double rice = 0;
    public int banana = 0;
    public bool isLive;

    [Header("# Rice Bags (Assign in Inspector)")]
    // 0: 빨강(Red), 1: 파랑(Blue), 2: 초록(Green)
    public RiceBag[] bags; 
    
    // 해금 여부 (0번 빨강은 기본 해금)
    public bool[] isUnlocked = { true, false, false }; 

    [Header("# Spawn Logic")]
    // 포대가 현재 죽어있어 소환을 기다리는지 기록
    public bool[] isBagDead = { false, false, false }; 
    public bool isAutoSpawnUnlocked = false; // 자동 소환 해금 여부
    public float autoSpawnInterval = 0.5f;   // 자동 소환 주기
    private float autoSpawnTimer;

    [Header("# Object Pool")]
    public PoolManager pool;


    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        // 게임 시작 시 해금된 포대 모두 소환
        for (int i = 0; i < bags.Length; i++)
        {
            if (isUnlocked[i])
            {
                bags[i].id = i; 
                SpawnBagInternal(i); // SpawnBagInternal 내부 함수를 사용하여 소환
            }
            else
            {
                bags[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // 자동 소환 로직
        if (isAutoSpawnUnlocked)
        {
            autoSpawnTimer += Time.deltaTime;
            if (autoSpawnTimer >= autoSpawnInterval)
            {
                autoSpawnTimer = 0f;
                TryAutoSpawnDeadBag();
            }
        }
    }

    // 포대가 죽었을 때 호출 (소환하지 않고 상태만 기록)
    public void OnRiceBagDead(int bagId)
    {
        if (bagId < 0 || bagId >= bags.Length) return;

        // 1. 포대가 죽었음을 기록
        isBagDead[bagId] = true;
        
        // 2. 0번(빨강) 포대가 죽었으면 레벨업 처리
        if (bagId == 0)
        {
            level++;
            
            // 10레벨 단위 갱신이 필요하면 즉시 실행
            if (level % 10 == 0 || (level - 1) % 10 == 0) 
            {
                RefreshAllBags();
                // 레벨업 시에는 모든 포대가 Init()을 통해 부활 처리되므로, 죽음 상태를 해제
                // 다른 포대들도 RefreshAllBags에서 Init이 호출되므로 죽음 상태 해제
                for (int i = 0; i < isBagDead.Length; i++) isBagDead[i] = false;
                return; 
            }
        }
    }
    
    // [수동 소환] 마우스/터치 클릭 시 호출될 함수
    public void ManualSpawn(int bagId)
    {
        if (bagId < 0 || bagId >= bags.Length) return;
        
        // 해금되었고, 현재 죽어있는 포대만 소환 가능
        if (isUnlocked[bagId] && isBagDead[bagId])
        {
            SpawnBagInternal(bagId);
        }
    }

    // [자동 소환] 아이템 해금 시 Update에서 호출
    void TryAutoSpawnDeadBag()
    {
        // 죽어있는 포대 중 가장 낮은 인덱스(빨강)부터 소환 시도
        for (int i = 0; i < bags.Length; i++)
        {
            if (isUnlocked[i] && isBagDead[i])
            {
                SpawnBagInternal(i);
                break; // 자동 소환은 한 틱에 하나씩만 처리
            }
        }
    }
    
    // 내부 소환 처리 함수 (실제 Init 호출)
    void SpawnBagInternal(int bagId)
    {
        bags[bagId].Init(level);
        isBagDead[bagId] = false; // 소환 완료했으므로 죽음 상태 해제
    }

    // 모든 포대 상태 갱신 (황금 변신용)
    void RefreshAllBags()
    {
        for (int i = 0; i < bags.Length; i++)
        {
            if (isUnlocked[i])
            {
                bags[i].Init(level);
            }
        }
    }
    
    // 바나나로 자동 소환 아이템 구매
    public void UnlockAutoSpawn()
    {
        int cost = 500; // 예시 가격
        if (!isAutoSpawnUnlocked && banana >= cost)
        {
            banana -= cost;
            isAutoSpawnUnlocked = true;
            // Debug.Log("자동 소환 해금 완료! 이제 포대가 자동으로 리젠됩니다.");
        }
    }

    public void GetReward(double riceAmt, int bananaAmt)
    {
        rice += riceAmt;
        banana += bananaAmt;
    }

}
