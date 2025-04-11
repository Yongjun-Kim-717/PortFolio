using UnityEngine;
using UnityEngine.UI;

public class UI_Check : UI_Base
{
    public Button Button_OK;

    protected override void Initialize()
    {
        base.Initialize();
        Button_OK.onClick.AddListener(OnClickButtonOK);
    }

    void OnClickButtonOK()
    {
        gameObject.SetActive(false);
    }
}
