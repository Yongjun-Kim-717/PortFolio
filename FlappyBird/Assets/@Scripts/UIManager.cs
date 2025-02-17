using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // 버튼을 연결할 변수
    public Button startButton;

    private UnityAction action;

    void Start()
    {       
        // UnityAction을 사용한 이벤트 연결 방식
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("Game");
    }

}
