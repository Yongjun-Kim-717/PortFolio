using UnityEngine;

// * ChestController 스크립트
//- 매 라운드마다 정해진 개수만큼 상자가 생성됨
//- 상자에는 랜덤 종류의 Elemental Orb가 있고 상자 파괴 시 해당 orb 생성
//- 오브젝트 풀로 관리해서 매 스테이지마다 상자를 초기화하는게 낫지 않을까
public class ChestController : BaseController
{
    // * 초기화 함수
    //- x축 기준 45도 만큼 기울여 2D 에셋인 Box를 Top-Down 뷰에 맞게 조정
    protected override void Initialize()
    {
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    // * 충돌 감지 함수
    //- 플레이어의 공격 이펙트인 SwordAura 충돌 감지
    //- 오브 생성 및 풀링 환원 (오브라는 특정된 오브젝트만 생성가능하지만 좀 더 확장해서 controller를 생성할 수 있게 하는건 어땠을까..)
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.SwordAuraTag))
        {
            PoolManager.Instance.GetObject<ElementalOrbController>(transform.position);
            ObjectManager.Instance.Despawn(this);
        }
    }
}
