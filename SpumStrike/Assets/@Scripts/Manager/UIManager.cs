using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// * UIManager 스크립트
//- 싱글톤 패턴으로 구현
//- 스테이지 텍스트 (클리어 조건 달성 시 클리어 UI 제공 및 스테이지 텍스트 변경)
//- 체력바 (체력 변경 시 체력바 동기화)
//- 경험치바 (경험치 변경 시 경험치바 동기화)
//- UIManager 스크립트에서 체력바를 관리 못하나?
//- SpawnSystem에서 플레이어 오브젝트에 World Space Canvas를 생성하고 체력바를 생성한다. 
//- SpawnSystem에서 생성한 체력바를 받아온다??

public class UIManager : Singleton<UIManager>
{
    private bool _isInit = false;

    public TMP_Text Text_Wave;
    public TMP_Text Text_MonsterCount;
    public TMP_Text Text_Level;
    public TMP_Text Text_Score;

    public GameObject Exp_Gauge;
    public RectTransform Image_EXP;

    public GameObject FailedUI;
    public GameObject ClearUI;

    public GameObject CheckUI;

    protected override void Initialize()
    {
        if(!_isInit)
        {
            FailedUI = GameObject.Find("UI_Failed");
            FailedUI.SetActive(false);

            ClearUI = GameObject.Find("UI_Clear");
            ClearUI.SetActive(false);

            CheckUI = GameObject.Find("UI_Check");
            CheckUI.SetActive(false);

            Text_Wave = GameObject.Find("Text - Wave").GetComponent<TMP_Text>();
            Text_MonsterCount = GameObject.Find("Text - MonsterCount").GetComponent<TMP_Text>();
            Text_Level = GameObject.Find("Text - Level").GetComponent<TMP_Text>();
            Text_Score = GameObject.Find("Text - Score").GetComponent<TMP_Text>();

            Exp_Gauge = GameObject.Find("Image - EXPGauge");
            Image_EXP = Exp_Gauge.GetComponent<RectTransform>();

            PlayerUnitManager.Instance.PlayerInfo.OnExpChanged += (exp) => { EXPBarSet(exp); };
            PlayerUnitManager.Instance.PlayerInfo.OnLevelChanged += (level) => { LevelTextSet(level); };
            StageManager.Instance.OnStageCountChanged += (count) => { WaveCountSet(count); };
            ObjectManager.Instance.OnMonsterCountChanged += (count) => { MonsterCountSet(count); };
            _isInit = true;
        }
    }

    public void ClearOption()
    {
        _isInit = false;
        PlayerUnitManager.Instance.PlayerInfo.OnExpChanged -= EXPBarSet;
        PlayerUnitManager.Instance.PlayerInfo.OnLevelChanged -= LevelTextSet;
        StageManager.Instance.OnStageCountChanged -= WaveCountSet;
        ObjectManager.Instance.OnMonsterCountChanged -= MonsterCountSet;
    }

    public void EnsureInitialized()
    {
        Initialize();
    }

    #region MainUI
    public void UISet()
    {
        Debug.Log("UISet시작, 초기화 시작");
        EXPBarSet(PlayerUnitManager.Instance.PlayerInfo.Exp);
        Text_Wave.text = "WAVE 0";
        Text_MonsterCount.text = "0";
        Text_Level.text = "0";
    }

    public void EXPBarSet(float exp)
    {
        float currentWidth = exp / PlayerUnitManager.Instance.PlayerInfo.MaxExp * 995;
        Image_EXP.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
    }

    public void LevelTextSet(int level)
    {
        Text_Level.text = $"{level}";
    }

    public void MonsterCountSet(int monsterCount)
    {
        Text_MonsterCount.text = $"{monsterCount}";
    }

    public void WaveCountSet(int waveCount)
    {
        Text_Wave.text = $"Wave {waveCount}";
    }
    #endregion
}
