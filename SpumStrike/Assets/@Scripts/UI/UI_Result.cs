using UnityEngine;
using UnityEngine.UI;

public class UI_Result : UI_Base
{
    public Button Button_OK;
    public Button Button_Replay;

    protected override void Initialize()
    {
        base.Initialize();
        Button_OK.onClick.AddListener(OnClickButtonOK);
        Button_Replay.onClick.AddListener(OnClickButtonReplay);
    }

    void OnClickButtonOK()
    {
        ServerManager.Instance.UserSaveScore(StageManager.Instance.Score);
        UIManager.Instance.CheckUI.SetActive(true);
    }

    void OnClickButtonReplay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
