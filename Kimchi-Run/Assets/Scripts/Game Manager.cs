using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Intro,
    Playing,
    Dead
}

public class GameManager : MonoBehaviour

{
    public static GameManager Instance;
    public GameState State = GameState.Intro;
    public float PlayStartTime;
    public int Lives = 3;

    [Header("References")]
    public GameObject IntroUI;
    public GameObject DeadUI;
    public GameObject EnemySpawner;
    public GameObject FoodSpawner;
    public GameObject GoldSpawner;
    public Player PlayerScript;
    public TMP_Text scoreText;
    
    

    void Awake() // start보다 먼저 유니티에 의해 실행되는 함수
    {
        if(Instance == null) // gamemanger 인스턴스를 다른 곳에서도 공유해서 사용할 수 있음
        {
            Instance = this;
        }   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IntroUI.SetActive(true);
    }

    float CalculateScore()
    {
        return Time.time - PlayStartTime;
    }

    void SaveHighScore()
    {
        int score = Mathf.FloorToInt(CalculateScore()); 
        int currentHighScore = PlayerPrefs.GetInt("highScore"); // 현재최고점수를 사용자의 컴퓨터 디스크에 데이터를 저장할 수 있게 해줌 highScore 인자로 동기적으로
        if(score > currentHighScore) // 점수가 현재점수보다 더 클경우 최고점수를 갱신해줌
        {
            PlayerPrefs.SetInt("highScore", score); // highScore을 최고점수인 score로 변환해줌
            PlayerPrefs.Save();
        }
    }

    int GetHighScore()
    {
        return PlayerPrefs.GetInt("highScore");
    }

    // Update is called once per frame
    void Update()
    {
        if(State == GameState.Playing)
        {
            scoreText.text = "Score : " + Mathf.FloorToInt(CalculateScore());
        }
        else if(State == GameState.Dead)
        {
            scoreText.text = "High Score : " + GetHighScore();
        }
        if(State == GameState.Intro && Input.GetKeyDown(KeyCode.Space))
        {
            State = GameState.Playing;
            IntroUI.SetActive(false);
            EnemySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldSpawner.SetActive(true);
            PlayStartTime = Time.time;
        }
        if(State == GameState.Playing && Lives == 0)
        {
            PlayerScript.KillPlayer();
            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldSpawner.SetActive(false);
            DeadUI.SetActive(true);
            SaveHighScore();
            State = GameState.Dead;
        }
        if(State == GameState.Dead && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
