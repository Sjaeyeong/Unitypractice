using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩 보관 변수
    public GameObject[] prefabs;
    
    // 풀 담당 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; // 풀 초기화

        for (int i = 0; i < pools.Length; i++) // 풀 안의 리스트도 초기화
        {
        pools[i] = new List<GameObject>();
        }
        // Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고(비활성화된) 있는 게임오브젝트 접근
        foreach (GameObject item in pools[index])
        {
        if (!item.activeSelf)
        {
            // 발견하면 select 변수에 할당       
            select = item;
            select.SetActive(true);
            break;
        }
        }

        // 못찾으면?
        if (!select)
        {
        // 새롭게 생성하고 select 변수에 할당
        select = Instantiate(prefabs[index], transform);
        pools[index].Add(select); // 생성한 게임오브젝트를 해당 풀 리스트에 추가
        }

        return select;
    }

}
