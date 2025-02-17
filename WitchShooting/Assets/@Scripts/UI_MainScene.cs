using UnityEngine;
using UnityEngine.UI;

public class UI_MainScene : MonoBehaviour
{
    public Button StartButton;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButtonClick);   
    }

    void OnStartButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
