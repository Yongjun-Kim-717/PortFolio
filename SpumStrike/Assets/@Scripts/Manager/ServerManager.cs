using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// * ServerManager 스크립트
//- 유니티 클라이언트와 서버간 연결 관리
//- 유저 ID, Password 정보 전달 및 저장, 데이터 불러오기 수행
public class ServerManager : Singleton<ServerManager>
{
    public event Action OnSignUpComplete;
    public event Action OnSignUpFailed;
    public event Action OnLogInComplete;
    public event Action OnLogInFailed;

    private string url_LogIn = "http://3.39.189.159:5000/api/login";
    private string url_SignUp = "http://3.39.189.159:5000/api/signup";
    private string url_SaveScore = "http://3.39.189.159:5000/api/save_score";

    private string jwtToken; // 토큰 저장 변수

    #region SignUp
    // User 회원가입 정보 서버 전달 및 결과 조회(post방식)
    public void UserSignUp(string userID, string userPassword)
    {
        StartCoroutine(SignUp(userID, userPassword));
    }

    IEnumerator SignUp(string userID, string userPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        form.AddField("userPassword",userPassword);

        using (UnityWebRequest request = UnityWebRequest.Post(url_SignUp, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("SignUp has been completely");
                OnSignUpComplete?.Invoke();
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }
    #endregion

    #region LogIn
    public void UserLogIn(string userID, string userPassword)
    {
        StartCoroutine (LogIn(userID, userPassword));
    }

    IEnumerator LogIn(string userID, string userPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        form.AddField("userPassword", userPassword);

        using (UnityWebRequest request = UnityWebRequest.Post(url_LogIn, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(jsonResponse);
                TokenResponse response = JsonUtility.FromJson<TokenResponse>(jsonResponse);

                jwtToken = response.token;
                Debug.Log("토큰 : " + jwtToken);
                Debug.Log("메시지 : " + response.message);
                OnLogInComplete?.Invoke();
            }
            else
            {
                Debug.Log("로그인 실패 : " + request.error);    
                OnLogInFailed?.Invoke();
            }
        }
    }
    #endregion

    #region SaveScore
    public void UserSaveScore(int score)
    {
        StartCoroutine(SaveScore(score));
    }

    IEnumerator SaveScore(int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("score", score);

        using(UnityWebRequest request = UnityWebRequest.Post(url_SaveScore, form))
        {
            // 헤더에 토큰 추가
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(jsonResponse);
            }
            else
            {
                Debug.Log("점수 저장 실패 : " + request.error);
            }
        }
    }
    #endregion
}

// 토큰 정보
public class TokenResponse
{
    public string message;
    public string token;
}