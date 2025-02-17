using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // ��ư�� ������ ����
    public Button startButton;

    private UnityAction action;

    void Start()
    {       
        // UnityAction�� ����� �̺�Ʈ ���� ���
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("Game");
    }

}
