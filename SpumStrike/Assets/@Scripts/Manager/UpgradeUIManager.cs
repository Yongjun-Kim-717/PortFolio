using System;
using System.Collections;
using UnityEngine;

// * 업그레이드 UI 매니저
//- 레벨 업 시 활성화되는 캔버스 관리
//- 랜덤 업그레이드 아이템 생성?
public class UpgradeUIManager : Singleton<UpgradeUIManager>
{
    private bool _isInit = false;

    // Upgrade UI Manager 멤버 변수
    public GameObject UpgradeUI;        // 업그레이드 UI 캔버스 변수 : 플레이어 캐릭터 레벨업 시 활성화 레벨업 액션에 활성화 메서드를 추가하자. 

    // Upgrade Item 선택 이벤트 액션
    public Action<Item> OnSelectItem;

    // 초기화 함수 재정의
    protected override void Initialize()
    {
        if(!_isInit)
        {
            base.Initialize();
            UpgradeUI = GameObject.Find("UI_Upgrade");
            UpgradeUI.GetOrAddComponent<UI_UpgradeUICanvas>();
            PlayerUnitManager.Instance.PlayerInfo.OnLevelChanged += (level) => { SetCanvasOn(level); };
            _isInit = true;
        }
    }


    public void ClearOption()
    {
        _isInit = false;
    }

    public void EnsureInitialized()
    {
        Initialize();
    }

    // 업그레이드 UI 캔버스 활성화 메서드
    // 레벨 업 액션 구독
    void SetCanvasOn(int level)
    {
        StartCoroutine(SetCanvasActive(true));
    }
    
    public void SetCanvasOff()
    {
        StartCoroutine(SetCanvasActive(false));
    }

    IEnumerator SetCanvasActive(bool isActive)
    {
        yield return new WaitForSecondsRealtime(isActive ? 1 : 0);
        Time.timeScale = isActive ? 0 : 1;
        UpgradeUI.SetActive(isActive);
    }

    protected override void Clear()
    {
        base.Clear();
        PlayerUnitManager.Instance.PlayerInfo.OnLevelChanged -= SetCanvasOn;
    }
}