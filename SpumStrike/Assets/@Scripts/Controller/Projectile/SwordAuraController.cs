using UnityEngine;

// * SwordAuraController 스크립트
//- 플레이어 공격 투사체 부착 스크립트
//- 공격 무기 데이터와 collider 속성 설정
public class SwordAuraController : BaseController
{
    private CapsuleCollider _capsuleCollider;
    public Weapon WeaponData;

    protected override void Initialize()
    {
        Debug.Log("콜라이더 추가");
        _capsuleCollider = gameObject.GetOrAddComponent<CapsuleCollider>();
        _capsuleCollider.isTrigger = true;
    }

    // * 공격 이펙트 충돌 감지 함수
    //- 충돌 감지 역할 외에 GetDamage 함수 직접 실행, 이펙트 생성 등 너무 많은 책임 존재 이벤트 형식으로 분할 제발하자..
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.EnemyTag))
        {
            other.gameObject.GetComponent<EnemyController>().GetDamage(PlayerUnitManager.Instance.PlayerInfo.Atk);
            GameObject hitEffect = WeaponData.HitEffect;
            Instantiate(hitEffect, other.transform.position, Quaternion.identity);
        }
    }
}
