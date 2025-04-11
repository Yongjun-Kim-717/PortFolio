using System;
using System.Collections;
using UnityEngine;

// * EnemyController 스크립트
//- 모든 Monster의 Base 클래스
//- 이동, 충돌, 사망 등 기본 동작 구현
public class EnemyController : BaseController
{
    // 멤버 컴포넌트
    protected Rigidbody _rigidbody;
    protected RectTransform _recttransform;
    protected Collider _collider;
    protected Animator _animator;

    // 사망 이펙트
    public GameObject DeathEffect { get; set; }

    // 몬스터 레벨
    public int Level { get; set; } = 1;

    // 스탯 멤버 변수 (스크립터블 오브젝트 값 복사)
    protected float _speed = 1.5f;
    protected float _maxHp = 20;
    protected float _currentHp = 20;
    protected float _atk = 1;
    protected float _exp = 10;
    protected int _size = 1;

    // 몬스터 공격력 프로퍼티
    public float Atk 
    {
        get { return _atk; }
        set { _atk = value; }
    }

    // 스탯 스크립터블 오브젝트
    protected EnemyStat _enemyStat;

    // 바라보는 방향 벡터
    protected Vector3 _faceDir = new Vector3(1,1,1);

    // * 초기화 함수
    //- Awake에서 실행
    //- 컴포넌트 연결 및 컴포넌트 값 설정 (Find 사용은 지양하자. GetComponentInChildren이 덜 무겁다.)
    protected override void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _recttransform = GetComponent<RectTransform>();
        _collider = GetComponent<Collider>();
        _animator = gameObject.FindChild<Animator>("UnitRoot");
        _rigidbody.useGravity = false;
        _rigidbody.freezeRotation = true;
        _rigidbody.linearVelocity = Vector3.zero;
        _recttransform.Rotate(45, 0, 0);

        Level = StageManager.Instance.CurrentMonsterLevel;
    }

    // * OnEnable 주기 함수
    //- 체력 초기화 메서드 추가 (오브젝트 풀링으로 활성화되며 재활용되기 때문)
    protected virtual void OnEnable()
    {
        SetStatus();
        ObjectManager.Instance.MonsterCount++;
    }

    // * 스탯 초기화 함수
    //- 레벨에 능력치 비례
    public void SetStatus()
    {
        if (_enemyStat != null)
        {
            _speed = _enemyStat.Speed * Level;
            _maxHp = _enemyStat.MaxHp * Level;
            Atk = _enemyStat.Atk * Level;
            _exp = _enemyStat.Exp * Level;
            _size = _enemyStat.Size * Level;
        }
        _currentHp = _maxHp;
    }

    // * FixedUpdate 주기함수
    //- 이동 기능
    protected virtual void FixedUpdate()
    {
        Move();
    }

    // * 이동 함수
    //- Player를 ObjectManager.Instance에서 직접 받아와 위치 정보 갱신
    //- 플레이어 위치를 향해 이동
    //- 플레이어를 향한 벡터의 x좌표 부호를 통해 Enemy의 방향 갱신 및 방향전환 기능 (모든 부위가 나누어져있어 flip불가능)
    void Move()
    {
        PlayerController player = ObjectManager.Instance.Player;
        if (player == null)
        {
            Debug.Log("캐릭터가 존재하지 않습니다.");
            return;
        }

        Vector3 targetPos = player.transform.position;
        Vector3 targetDir = (targetPos - transform.position).normalized;
        transform.position += targetDir * _speed * Time.deltaTime;
        _faceDir.x = (targetDir.x > 0) ? -1 : 1;
        _recttransform.localScale = _faceDir * _size;
        _animator.SetBool(Define.IsMove, true);
    }

    // * 데미지 계산 함수
    //- 충돌 시 발동 (이 부분도 결합도가 너무 높음. 외부에서 직접 실행하는 것은 지양하자 : 충돌 이벤트를 생성해서 이벤트에 구독하는 방식)
    //- 사망 후처리 진행 (단일 책임 원칙에 어긋남 )
    //- 현재는 사망 조건이 HP <= 0 뿐이지만 특정 사망 조건이 늘어날 수록 하드코딩이 됨
    //- Status와 Health를 클래스로 변경해서 책임을 분할해보자
    public bool GetDamage(float damage)
    {
        SetDamageText(damage);
        _currentHp -= damage;
        _animator.SetTrigger(Define.DamagedTrigger);
        if (_currentHp <= 0)
        {
            Dead();
            ObjectManager.Instance.Despawn(this);
        }
        return true;
    }

    // * 데미지 텍스트 생성 함수
    //- 데미지를 인자로 받아 해당 텍스트를 오브젝트 풀링으로 생성
    void SetDamageText(float damage)
    {
        DamageTextController text = PoolManager.Instance.GetObject<DamageTextController>(transform.position);
        text.Damage = damage;
    }

    // * 사망처리 함수
    //- 경험치 적용
    //- 사망 이펙트 생성 (이 부분도 만약 설정에서 사망 이펙트 미생성을 체크한다면 수정이 복잡함 차라리 event로 구독해서 수행하자)
    protected virtual void Dead()
    {
        SetExp(_exp);
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
    }

    // * 경험치 적용 함수
    //- 경험치 구슬 오브젝트 풀링으로 생성 및 관리
    void SetExp(float exp)
    {
        ExpController experience = PoolManager.Instance.GetObject<ExpController>(transform.position);
        experience.ExpAmount = exp;
    }
}
