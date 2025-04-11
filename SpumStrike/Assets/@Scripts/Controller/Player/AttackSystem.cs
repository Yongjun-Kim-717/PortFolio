using System.Collections;
using UnityEngine;

// * AttackSystem 스크립트
//- 플레이어 용 공격 시스템 스크립트
//- 자동 공격 수행 조건 설정 (공격 속도에 따른 변화)
//- AttackSystem에 조금 더 많은 책임을 부여했어야 했다. 지금은 너무 PlayerController에서 많은 걸 처리함.
public class AttackSystem : MonoBehaviour
{
    // 공격 코루틴 변수
    Coroutine _coAttack;

    // 공격 시간 (공격 속도 반비례)
    float _attackTime = 2.0f;

    // 쿨타임 (공격 시간)
    WaitForSeconds _coolTime;

    // * Start 주기 함수
    //- 공격 코루틴 함수 실행
    void Start()
    {
        _coolTime = new WaitForSeconds(_attackTime);
        if(_coAttack == null)
        {
            _coAttack = StartCoroutine(CoAttack());
        }    
    }

    // * 공격 속도 적용 함수
    //- 공격 속도에 따라 달라진 공격 시간(쿨타임) 적용
    public void ApplyAttackSpeed(float attackSpeed)
    {
        _attackTime /= attackSpeed;
        _coolTime = new WaitForSeconds(_attackTime);
    }

    // * 공격 코루틴
    //- 쿨타임마다 플레이어의 공격 수행
    IEnumerator CoAttack()
    {
        while(true)
        {
            yield return _coolTime;
            ObjectManager.Instance.Player.Attack();
        }
    }
}
