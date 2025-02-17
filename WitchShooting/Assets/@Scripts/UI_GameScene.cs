using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameScene : MonoBehaviour
{
    public TMP_Text ScoreText;
    public Button StopButton;
    public Button ContinueButton;
    public GameObject StoppedPanel;
    int _tempScore;

    private void Start()
    {
        StopButton.onClick.AddListener(OnStopButtonClick);
        ContinueButton.onClick.AddListener(OnContinueButtonClick);
    }

    void Update()
    {
        if(_tempScore != GameManager.Instance.Score)
        {
            _tempScore = GameManager.Instance.Score;
            ScoreText.text = GameManager.Instance.Score.ToString();
        }
    }

    void OnStopButtonClick()
    {
        Time.timeScale = 0;
        StoppedPanel.SetActive(true);
    }

    void OnContinueButtonClick()
    {
        Time.timeScale = 1;
        StoppedPanel.SetActive(false);
    }
}
