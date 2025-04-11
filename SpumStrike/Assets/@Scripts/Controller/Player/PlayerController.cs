using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// * PlayerController 스크립트
//- 이동, 무기 장착, 공격, 피격, 이펙트, 후처리 담당
public class PlayerController : BaseController
{
    // 상태 플래그
    private bool _isDead = false;

    // 공격 관련 멤버 변수
    private Weapon _playerWeapon;
    public GameObject AttackEffect;

    // 컴포넌트
    private Vector3 _moveDir;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsulecollider;
    private Animator _animator;
    private RectTransform _recttransform;
    private AttackSystem _attackSystem;
    private Transform _playerBody;

    // 피격 효과 관련 변수
    private SpriteRenderer[] _spriterenderers;
    private Color _originColor = Color.white;
    private Color _hitColor = new Color(1f, 0f, 0f, 0.7f);
    private float _blinkDuration = 0.2f;

    // 레벨 업 이펙트
    public GameObject LevelUpEffect { get; set; }

    // 캐릭터 형상 방향 변수
    private Vector3 _faceDir = new Vector3(1, 1, 1);

    // * 캐릭터 이동 방향 프로퍼티
    //- 조이스틱과 연동
    public Vector3 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir.x = value.x;
            _moveDir.y = 0;
            _moveDir.z = value.y;
            _moveDir = _moveDir.normalized;
            if (_moveDir.x < 0)
                _faceDir.x = 1;
            else if (_moveDir.x > 0)
                _faceDir.x = -1;
        }
    }

    // * 초기화 함수
    //- Awake에서 실행
    //- 컴포넌트 연결 및 속성 설정
    //- 이동, 레벨업 이벤트 구독
    //- Find는 줄이자..
    protected override void Initialize()
    {
        _playerBody = gameObject.FindChild<Transform>("UnitRoot");
        _spriterenderers = _playerBody.gameObject.FindChild<Transform>("Root").gameObject.GetComponentsInChildren<SpriteRenderer>();
        _recttransform = GetComponent<RectTransform>();
        _animator = gameObject.FindChild<Animator>("UnitRoot");
        _attackSystem = gameObject.GetOrAddComponent<AttackSystem>();
        _capsulecollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.freezeRotation = true;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.mass = 2000;
        _recttransform.Rotate(45, 0, 0);
        PlayerUnitManager.Instance.OnMoveDirChanged += (dir) => { MoveDir = dir; };
        PlayerUnitManager.Instance.PlayerInfo.OnLevelChanged += (level) => { LevelUp(); };
    }

    // * FixedUpdate 주기함수
    //- 이동 기능 수행
    void FixedUpdate()
    {
        Move();
    }

    // * 이동 함수
    //- 사망 여부 체크
    //- rigidbody 기반 이동
    //- 이동 애니메이션 트리거 적용
    void Move()
    {
        if (_isDead)
            return;
        _rigidbody.linearVelocity = Vector3.zero;
        float speed = PlayerUnitManager.Instance.PlayerInfo.Speed;
        Vector3 moveDir = ObjectManager.Instance.Player.transform.position + _moveDir * speed * Time.deltaTime;
        _rigidbody.MovePosition(moveDir);
        _playerBody.localScale = _faceDir;
        _animator.SetBool(Define.IsMove, _moveDir != Vector3.zero);
    }

    // * 무기 장착 함수
    //- Scriptable Object로 구현된 무기 장착 기능
    //- 무기 이펙트 저장 및 스크립트 연결
    public void EquipWeapon(Weapon weapon)
    {
        _playerWeapon = weapon;
        AttackEffect = _playerWeapon.AttackEffect;
        AttackEffect.GetOrAddComponent<AttackEffectController>();
    }

    // * 공격 함수
    //- 사망 여부 체크
    //- 공격 이펙트 생성 코루틴 실행
    public void Attack()
    {
        if (_isDead)
            return;
        StartCoroutine(GetEffect(transform.position));
    }

    // * 공격 이펙트 생성 코루틴
    //- 인자로 받은 pos에 공격 이펙트 방향 및 크기 설정 후 생성
    IEnumerator GetEffect(Vector3 pos)
    {
        yield return null;
        _animator.SetTrigger(Define.AttackTrigger);
        float attackRange = PlayerUnitManager.Instance.PlayerInfo.AttackRange;
        Vector3 effectDir = new Vector3(_faceDir.x, 1, 1);
        AttackEffect.transform.localScale = effectDir * attackRange / 5;
        Instantiate(AttackEffect, transform.position, Quaternion.AngleAxis(90f, Vector3.left));
    }

    // * 충돌 감지 함수
    //- 적 오브젝트 충돌 감지
    //- 사망 조건 체크와 후처리는 Health 클래스를 구현해 책임 분할을 해보자
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(Define.EnemyTag))
        {
            float damage = collision.gameObject.GetComponent<EnemyController>().Atk;
            GetDamage(damage);
            if(PlayerUnitManager.Instance.PlayerInfo.CurrentHp <= 0)
            {
                Dead();
            }
        }
    }

    // * 투사체 충돌 감지 함수
    //- 보스 스킬 투사체 충돌 감지 (태그를 세분화 하자)
    //- 마찬가지로 Health 클래스를 따로 관리해 책임 분할
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.EnemyTag))
        {
            float damage = other.GetComponent<BossSkillController>().Atk;
            GetDamage(damage);
            if (PlayerUnitManager.Instance.PlayerInfo.CurrentHp <= 0)
            {
                Dead();
            }
        }
    }

    // * 데미지 계산 함수
    //- PlayerUnitManager(플레이어 캐릭터 정보 관리 매니저) Instance에 접근해 Hp 정보 변경
    //- 피격 애니메이션 트리거 실행
    //- 피격 효과 코루틴 실행
    public void GetDamage(float damage)
    {
        PlayerUnitManager.Instance.PlayerInfo.CurrentHp -= damage;
        _animator.SetTrigger(Define.DamagedTrigger);
        StartCoroutine(BlinkEffect());
    }

    // * 피격 효과 코루틴
    //- 그림자 제외 플레이어 캐릭터 프리팹의 모든 컴포넌트 순회 및 색 변경
    IEnumerator BlinkEffect()
    {
        foreach (var renderer in _spriterenderers)
        {
            renderer.color = _hitColor;
        }

        yield return new WaitForSeconds(_blinkDuration);

        foreach (var renderer in _spriterenderers)
        {
            renderer.color = _originColor;
        }
    }

    // * 레벨업 이펙트 생성 함수
    //- 레벨 업 이펙트를 Top-Down 뷰에 맞는 회전 값으로 생성
    void LevelUp()
    {
        Instantiate(LevelUpEffect, transform.position, Quaternion.Euler(45, 0, 0));
    }

    // * 사망 함수
    //- 사망 플래그 설정
    //- 사망 애니메이션 트리거 설정 
    //- 사망 후처리 코루틴 실행
    void Dead()
    {
        _isDead = true;
        _animator.SetTrigger(Define.DeathTrigger);
        StartCoroutine(AfterDead());
    
    }

    // * 사망 후처리 코루틴
    //- 일정 시간 후 GameManager의 사망으로 인한 게임 종료 메서드 수행 (이 부분도 추후 기능이 많아진다면 이벤트 처리를 하는 것이 나아보인다.)
    IEnumerator AfterDead()
    {
        yield return new WaitForSeconds(1.0f);
        // 사망구현
        GameManager.Instance.GameEndByDie();
    }
}
