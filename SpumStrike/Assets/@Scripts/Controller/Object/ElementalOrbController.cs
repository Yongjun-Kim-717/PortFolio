using System.Collections;
using UnityEngine;


// * 엘리멘탈 오브 컨트롤러 스크립트
//- 무기 정보 저장
//- 충돌 시 습득 및 무기 적용
public class ElementalOrbController : BaseController, IGetAble
{
    // 무기 정보 (Sctiptable Object)
    public Weapon Weapon;
    
    // 충돌 감지 컴포넌트
    private CapsuleCollider _capsuleCollider;

    // * 초기화 함수
    //- Awake에서 실행
    //- Top-Down 뷰에 맞게 회전
    //- 컴포넌트 연결 및 컴포넌트 속성정의
    protected override void Initialize()
    {
        transform.rotation = Quaternion.Euler(45, 0, 0);  
        _capsuleCollider = gameObject.GetOrAddComponent<CapsuleCollider>();
        _capsuleCollider.isTrigger = true;
    }

    // * 충돌 감지 함수
    //- 플레이어의 습득 범위 오브젝트 감지 (코루틴 실행)
    //- 플레이어 오브젝트 감지 (장착, 풀링 환원)
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.GetRangeTag))
        {
            StartCoroutine(GetItem(other.gameObject));
        }
        if (other.CompareTag(Define.PlayerTag))
        {
            ObjectManager.Instance.Player.EquipWeapon(Weapon);
            ObjectManager.Instance.Despawn(this);
        }
    }

    #region Interface
    // * 아이템 흡수 기능
    //- IGetable 인터페이스 기능 구현
    //- 타겟에게 점점 끌려가는 흡수 기능 구현
    public IEnumerator GetItem(GameObject target)
    {
        float duration = 0.3f;
        float elapsed = 0;

        Vector3 startPos = transform.position;
        Vector3 endPos = target.transform.position;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    #endregion
}
