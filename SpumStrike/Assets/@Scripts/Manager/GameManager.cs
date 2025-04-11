using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// * 게임 매니저 스크립트
//- 게임의 전반적인 흐름 관리 (시작, 중지, 종료 등)
//- 로그인 창이 있고, 로그인 시 로비 씬으로 이동
//- 로비씬에서 게임 start 버튼 클릭시 게임 시작
public class GameManager : Singleton<GameManager>
{
    protected override void Initialize()
    {
        base.Initialize();
        ServerManager.Instance.OnLogInComplete += GameSet;
    }

    protected override void Clear()
    {
        base.Clear();
    }

    // 로그인 성공 시 게임 scene으로 이동 및 세팅
    void GameSet()
    {
        StartCoroutine(LoadGameScene());
    }

    // 게임 Scene 비동기 로드 (사실 비동기로 할 필요는 없는 것 같다. 다른 이슈로 생겨난 오류 고치는 중에 시도해본 비동기 로드 일단 문제없이 돌아가서 수정 x)
    IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        ObjectManager.Instance.ResourceLoad();
        GameStart();
    }

    // 게임 시작 버튼 클릭 시 호출 스테이지 시작
    // 매니저 생성 및 초기화
    void GameStart()
    {
        Time.timeScale = 1f;

        PlayerUnitManager.Instance.EnsureInitialized();

        UIManager.Instance.EnsureInitialized();
        UIManager.Instance.UISet();

        LoadStage();

        UpgradeUIManager.Instance.EnsureInitialized();
        UpgradeUIManager.Instance.UpgradeUI.SetActive(false);
    }

    // 스테이지 정보 로딩
    void LoadStage()
    {
        StageManager.Instance.EnsureInitialized();
        StageManager.Instance.SetStage();
        StageManager.Instance.OnStageCleared += StageCleared;
        StageManager.Instance.OnStageEnded += GameEndByClear;
        StageManager.Instance.StartStage();
    }

    // 스테이지 클리어 이벤트에 구독
    void StageCleared()
    {
        StageManager.Instance.StartNewStage();
    }

    // 최종 스테이지 클리어로 인한 게임 종료 후처리
    public void GameEndByClear()
    {
        UIManager.Instance.ClearOption();
        UpgradeUIManager.Instance.ClearOption();
        WorldSpaceUIManager.Instance.ClearOption();
        PoolManager.Instance.ClearOption();
        PlayerUnitManager.Instance.ClearOption();
        StageManager.Instance.ClearOption();
        Time.timeScale = 0;
        UIManager.Instance.ClearUI.SetActive(true);
    }

    // 플레이어 캐릭터 죽음으로 인한 게임 종료 후처리
    public void GameEndByDie()
    {
        UIManager.Instance.ClearOption();
        UpgradeUIManager.Instance.ClearOption();
        WorldSpaceUIManager.Instance.ClearOption();
        PoolManager.Instance.ClearOption();
        PlayerUnitManager.Instance.ClearOption();
        StageManager.Instance.ClearOption();
        Time.timeScale = 0;
        UIManager.Instance.FailedUI.SetActive(true);
    }
}
