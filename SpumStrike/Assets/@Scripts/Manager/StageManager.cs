using System;
using System.Collections.Generic;
using UnityEngine;

// * StageManager 스크립트 
//- 각 몬스터 웨이브를 관리 (클리어, 오브젝트, 능력치 조정 등)
//- 스크립터블 오브젝트로 스테이지 정보를 저장했음.
//- 그러면 스크립터블 오브젝트를 받아와서 정보 변경을 하자
//- 일단 시작 이벤트를 만들어서 이제 로그인을 하고 로비화면에 왔을때 로비에서 시작버튼을 누르면 씬 전환이 이루어지고 바로 시작함
//- 시작하면 지금 화면처럼 캐릭터에게 줌인이 되면서 바로 스테이지 1시작
//- 스테이지 1의 보스몬스터를 잡았을 경우 클리어 처리가 되며 바로 다음 스폰시스템 작동 
//- 스폰시스템도 변경을 해야함
//- 이렇게 3개정도? 만들고 3스테이지 까지 클리어 하면 뭐... 클리어 점수같은거라도 해야하나
public class StageManager : Singleton<StageManager>
{
    private bool _isInit = false;

    private GameObject _spawnSystemObj;
    private SpawnSystem _spawnSystem;

    static private int _stageCount;
    static private bool _isClear = false;

    public event Action<int> OnStageCountChanged;
    public event Action OnStageCleared;
    public event Action OnStageEnded;

    // 스테이지 정보 저장 자료구조 StageType에 따라 각기 다른 Queue로 저장
    public Dictionary<Define.StageType, Queue<StageInfo>> StageDictionary = new Dictionary<Define.StageType, Queue<StageInfo>>();

    private StageInfo _currentStage;
    public int CurrentMonsterLevel;
    public int Score;

    public int StageCount
    {
        get { return _stageCount; }
        set 
        { 
            _stageCount = value;
            OnStageCountChanged?.Invoke(value);
        }
    }

    public bool IsClear
    {
        get { return _isClear; }
        set
        {
            _isClear = value;
            if (_isClear)
            {
                Debug.Log("클리어!");
            }
        }
    }

    // Awake 시에 호출 싱글톤 클래스 상속
    protected override void Initialize()
    {
        if(!_isInit)
        {
            base.Initialize();
            StageDictionary.Clear();
            StageCount = 0;
            CurrentMonsterLevel = 1;
            Score = 0;
            _isClear = false;
            _currentStage = null;
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

    // start 메서드에서 스테이지 정보를 저장한 SpawnSystem을 실행
    public void StartStage()
    {
        _spawnSystem = null;
        SetSpawnSystem();
    }

    // 스테이지 매니저 생성 위해 게임 매니저 스크립트에서 첫 호출될 메서드
    public void SetStage()
    {
        StageDictionary[Define.StageType.Normal] = new Queue<StageInfo>();
        for (int i = 0; i < ObjectManager.Instance.StageInfos.Count; i++)
        {
            StageDictionary[Define.StageType.Normal].Enqueue(ObjectManager.Instance.StageInfos[i]);
        }
    }

    // SpawnSystem 생성 및 스크립트 연결, stageInfo 저장 및 스폰시스템에 연결
    void SetSpawnSystem()
    {
        // SpawnSystem 생성 및 스크립트 연결
        if(!_spawnSystem)
            _spawnSystemObj = new GameObject("SpawnSystem");
        _spawnSystem = _spawnSystemObj.GetOrAddComponent<SpawnSystem>();

        // 첫 스테이지 시작
        StartNewStage();
    }

    // 스테이지 시작 메서드 이걸 게임 매니저로 확장해서 사용
    public void StartNewStage()
    {
        StageCount++;
        // stageInfo 추출 및 SpawnSystem에 전달
        if(StageDictionary[Define.StageType.Normal].Count > 0)
        {
            StageInfo stageInfo = StageDictionary[Define.StageType.Normal].Dequeue();
            _currentStage = stageInfo;
            SetStageInfoToSpawnSystem(stageInfo); //이걸 굳이 모듈화를 했어야 했나
        }
        else
        {
            // 남은 stageInfo가 없을 때 == 모든 스테이지 클리어
            OnStageEnded?.Invoke();
            return;
        }

        // 스포닝 실행
        _spawnSystem.StartSpawningPool();
    }

    // stageInfo 저장 및 스폰 시스템에 전달
    void SetStageInfoToSpawnSystem(StageInfo stageInfo)
    {
        if(!stageInfo)
        {
            Debug.Log("스테이지 정보가 존재하지 않습니다.");
            return;
        }
        CurrentMonsterLevel = stageInfo.EnemyLevel;
        _spawnSystem.GetComponent<SpawnSystem>().Setting(stageInfo);
    }

    // 스테이지 클리어 메서드
    public void ClearStage()
    {
        IsClear = true;
        Score += _currentStage.ClearScore;
        UIManager.Instance.Text_Score.text = Score.ToString();
        OnStageCleared?.Invoke();
    }
}