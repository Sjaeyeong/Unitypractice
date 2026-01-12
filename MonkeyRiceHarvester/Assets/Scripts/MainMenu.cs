using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "JYScene";
    public Button continueButton;

    void Start()
    {
        if (continueButton != null)
        {
            continueButton.interactable = PlayerPrefs.HasKey("Rice") || PlayerPrefs.HasKey("PlayerLevel");

        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 테스트 시 멈춤
        #else
            Application.Quit(); // 빌드된 게임 종료
        #endif
        Debug.Log("게임 종료");
    }
}
