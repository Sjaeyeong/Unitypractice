using System;
using System.Collections;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;
    // public RectTransform noticeRect;
    // public Vector3 noticeStart = new Vector3(100, -15, 0);
    // public Vector3 noticeEnd = new Vector3(-5, -15, 0);
    // public float slideDuration = 0.5f;

    
    enum Achieve { UnlockPotato, UnlockBean }
    Achieve[] achieves;
    WaitForSecondsRealtime wait; // timescale에 영항을 받지않음 (멈추지않는 시간)

    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(3);

        if (!PlayerPrefs.HasKey("MyData")) // 업적 저장용
        {
            Init();
        }

    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1); // playerprefs는 사용자의 기기에 간단한 데이터를 영구 저장, 불러오는 역할

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
        

    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;


        switch (achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 10;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

        for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

        StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
    //     noticeRect.anchoredPosition = noticeStart;

    //     float elapsed = 0f;

    // // 2. 슬라이드 인 애니메이션 (화면 안으로 진입)
    // // 이 루프가 SetActive(true) 직후에 실행되어 애니메이션을 시작합니다.
    //     while (elapsed < slideDuration)
    //     {
    //         elapsed += Time.unscaledDeltaTime; 
    //         float t = elapsed / slideDuration; // 0.0에서 1.0으로 진행

    //         // Lerp를 사용하여 시작 위치에서 목표 위치로 부드럽게 이동
    //         noticeRect.anchoredPosition = Vector3.Lerp(
    //             noticeStart, 
    //             noticeEnd, 
    //             t
    //         );
    //         yield return null; // 다음 프레임까지 대기
    //     }
    //     // 애니메이션이 정확히 목표 위치에서 끝나도록 보장
    //     noticeRect.anchoredPosition = noticeEnd;
        
        yield return wait;

        uiNotice.SetActive(false);
    }

}
