using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("# Info")]
    public int activeTargetCount = 1;

    public static GameManager instance;
    public PoolManager pool;
    public MonkeyCS monkey;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;

    }

}
