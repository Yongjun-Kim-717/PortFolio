using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ResultScene : MonoBehaviour
{
    public TMP_Text CurrentScoreText;
    public TMP_Text BestScoreText;

    public Button MainButton;
    public Button RetryButton;
  
    void Start()
    {
        MainButton.onClick.AddListener(OnMainButtonClick);
        RetryButton.onClick.AddListener(OnRetryButtonClick);

        CurrentScoreText.text = $"Current Score : {GameManager.Instance.Score}";
        BestScoreText.text = $"Best Score : {GameManager.Instance.BestScore}";

        GameManager.Instance.Score = 0;
    }

    void OnMainButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    void OnRetryButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
