using UnityEngine;

// * AttackEffectController 스크립트
//- 플레이어의 공격 이펙트 컨트롤러
public class AttackEffectController : BaseController
{
    private Transform _playerTransform; // 플레이어 transform

    // * transform 초기화 함수
    //- player transform을 전달받아 저장
    public void InitializeTransform(Transform transform)
    {
        _playerTransform = transform;
    }

    // * 초기화 함수
    //- Awake에서 실행
    //- ObjectManager의 Player의 transform을 저장
    protected override void Initialize()
    {
        InitializeTransform(ObjectManager.Instance.Player.transform);
    }

    // * Update 주기함수
    //- Player의 position 추적 이동
    private void Update()
    {
        if( _playerTransform != null )
            transform.position = _playerTransform.position;
    }
}
