using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level = 1;
    public int exp = 0;
    public int[] nextExp = {};

    [Header("# GameObject")]
    public PoolManager pool;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    
}
