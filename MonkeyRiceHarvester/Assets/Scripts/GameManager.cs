using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // 재화가 변경될때 마다 호출됨. UIManager에서 UI갱신
    public event Action OnCurrentChanged;

    [Header("# Money Rice")]
    public double currentRice = 0;
    public double currentBanana = 0;


    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void AddRice(double rice)
    {
        if (rice > 0)
        {
            currentRice += rice;
            OnCurrentChanged?.Invoke();
        }
    }

    public void AddBanana(double banana)
    {
        if (banana > 0)
        {
            currentRice += banana;
            OnCurrentChanged?.Invoke();
        }
    }

    public bool SubRice(double rice)
    {
        if (currentRice >= rice)
        {
            currentRice -= rice;
            OnCurrentChanged?.Invoke();
            return true;
        }

        return false;
    }

    public bool SubBanana(double banana)
    {
        if (currentBanana >= banana)
        {
            currentRice -= banana;
            OnCurrentChanged?.Invoke();
            return true;
        }

        return false;
    }

}
