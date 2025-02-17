using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button ReStartButton;

    void Start()
    {
        ReStartButton.onClick.AddListener(OnClickReStart);
    }

    void OnClickReStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
