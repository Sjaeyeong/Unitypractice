using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager pool;
    public MonkeyCS monkey;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

}
