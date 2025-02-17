using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{
    public Button RestartButton;

    void Start()
    {
        RestartButton.onClick.AddListener(OnRestartButtonClick);
    }

    void OnRestartButtonClick()
    {
        Time.timeScale = 1.0f;
        // 현재 씬 번호
        int sceneIdx = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(sceneIdx);
    }

}
