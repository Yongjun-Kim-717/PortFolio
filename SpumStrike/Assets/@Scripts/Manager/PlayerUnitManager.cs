using System;
using UnityEngine;

// * PlayerUnitManager 스크립트
//- 플레이어 정보 (능력치 : 프로퍼티 구현)
//- PlayerUnitManager 형식의 싱글톤 클래스를 상속받음
//- 조이스틱 연결 (조이스틱의 조작 -> MoveDir을 변경 -> Set 프로퍼티 내 Action 이벤트를 활용해 플레이어 이동 구현)
//- 플레이어 초기 능력치 생성 - 아직 고쳐야할 것
//- 게임 진행 및 UI 동기화

public class PlayerInfo
{
    public PlayerInfo()
    {
        Atk = 20;
        MaxHp = 100;
        CurrentHp = 100;
        Defense = 10;
        Speed = 3;
        AttackSpeed = 1;
        AttackRange = 15;
        MaxExp = 100;
        Exp = 0;
        Level = 0;
    }

    public event Action<float> OnCurrentHpChanged;
    public event Action<float> OnExpChanged;
    public event Action<int> OnLevelChanged;

    public float Atk { get; set; }
    public float MaxHp { get; set; }
    public float Defense { get; set; }
    public float Speed { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public float MaxExp { get; set; }

    private float _currentHp;
    public float CurrentHp 
    {
        get { return _currentHp; }
        set
        {
            _currentHp = value;
            OnCurrentHpChanged?.Invoke(value);
        }
    }

    public float Exp { get; set; }

    private int _level;
    public int Level { get; set; }

    public void ExpUp(float expAmount)
    {
        Exp += expAmount;
        if (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            LevelUp(); 
        }
        OnExpChanged?.Invoke(Exp);
    }

    private void LevelUp()
    {
        _level++;

        Atk += Define.AtkStatus;
        MaxHp += Define.MaxHpStatus;
        Defense += Define.DefStatus;
        MaxExp += Define.MaxExpStatus;

        CurrentHp = MaxHp;
        OnLevelChanged?.Invoke(_level);
    }

    public void Clear()
    {
        OnCurrentHpChanged = null;
        OnExpChanged = null;
        OnLevelChanged = null;
    }
}

public class PlayerUnitManager : Singleton<PlayerUnitManager>
{
    private bool _isInit = false;
    
    protected override void Initialize()
    {
        if(!_isInit)
        {
            base.Initialize();
            InitializePlayerInfo();
            _isInit = true;
        }
    }

    private void Start()
    {
        UpgradeUIManager.Instance.OnSelectItem += (item) => { UpgradePlayerStatus(item); };
    }

    protected override void Clear()
    {
        base.Clear();
        PlayerInfo.Clear();
        UpgradeUIManager.Instance.OnSelectItem -= UpgradePlayerStatus;
    }

    public void ClearOption()
    {
        _isInit = false;
    }

    public void EnsureInitialized()
    {
        Initialize();
    }

    #region Joystick
    public event Action<Vector2> OnMoveDirChanged;

    Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(value);
        }
    }
    #endregion

    // 플레이어의 능력치
    #region PlayerInfo
    public PlayerInfo PlayerInfo;
    private void InitializePlayerInfo()
    {
        PlayerInfo = new PlayerInfo();
    }

    // 아이템을 통한 플레이어 스테이터스 증가 메서드
    private void UpgradePlayerStatus(Item item)
    {
        float figure = item.Figure / 100;
        switch(item.Option)
        {
            case Define.Option.Atk:
                PlayerInfo.Atk += PlayerInfo.Atk * figure;
                break;
            case Define.Option.MaxHp:
                PlayerInfo.MaxHp += PlayerInfo.MaxHp * figure;
                break;
            case Define.Option.Defense:
                PlayerInfo.Defense += PlayerInfo.Defense * figure;
                break;
            case Define.Option.Speed:
                PlayerInfo.Speed += PlayerInfo.Speed * figure;
                break;
            case Define.Option.AttackSpeed:
                PlayerInfo.AttackSpeed += PlayerInfo.AttackSpeed * figure;
                ObjectManager.Instance.Player.GetComponent<AttackSystem>().ApplyAttackSpeed(PlayerInfo.AttackSpeed);
                break;
            case Define.Option.AttackRange:
                PlayerInfo.AttackRange += PlayerInfo.AttackRange * figure;
                break;
            default:
                break;
        }
        Debug.Log(item + "장착!" + item.Option + figure + "증가");
    }
    #endregion
}
