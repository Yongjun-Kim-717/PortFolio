using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_Login : UI_Base
{
    public GameObject LoginPanel;
    public GameObject SignUpSuccessPanel;
    public GameObject FailedPanel;

    public TMP_InputField InputField_ID;
    public TMP_InputField InputField_Password;

    public TMP_Text Text_Title;

    public Button Button_Login;
    public Button Button_SignUp;
    public Button Button_OK;

    private string _userID;
    private string _userPassword;

    protected override void Initialize()
    {
        base.Initialize();
        Button_SignUp.onClick.AddListener(SetUserSignUp);
        Button_Login.onClick.AddListener(SetUserLogIn);
        Button_OK.onClick.AddListener(PopDown);
        ServerManager.Instance.OnLogInFailed += PopUp;
        if(FailedPanel.activeSelf == true)
            PopDown();
    }

    // 회원가입
    private void SetUserSignUp()
    {
        _userID = InputField_ID.text;
        _userPassword = InputField_Password.text;
        ServerManager.Instance.UserSignUp(_userID, _userPassword);
    }

    private void SetUserLogIn()
    {
        _userID = InputField_ID.text;
        _userPassword = InputField_Password.text;
        ServerManager.Instance.UserLogIn(_userID, _userPassword);
    }

    // 액션의 종류에 따라 UI 텍스트 변경
    private void PopUp()
    {
        FailedPanel.SetActive(true);
    }

    private void PopDown()
    {
        FailedPanel.SetActive(false);
    }
}
