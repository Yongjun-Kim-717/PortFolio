using System.Collections;
using UnityEngine;

// * ExpController 스크립트
//- 몬스터 사망 시 해당 몬스터의 경험치량을 가진 경험치 오브젝트 생성
//- 오브젝트 풀링으로 관리하며 오브젝트 매니저에서 직접 정보 관리 -> 플레이어와의 거리를 통해 자석 시스템 구현해보자
//- 플레이어가 습득 시 플레이어의 경험치 상승
public class ExpController : BaseController, IGetAble
{
    // exp 수치 변수
    private float _expAmount = 20f;
    // exp 수치 프로퍼티
    public float ExpAmount { get { return _expAmount; } set { _expAmount = value; } }

    // 컴포넌트 변수
    private SphereCollider _spherecollider;
    private Rigidbody _rigidbody;

    // * 초기화 함수
    //- Awake에서 실행
    //- Top-Down 뷰에 맞게 회전
    //- 컴포넌트 연결 및 컴포넌트 속성정의
    protected override void Initialize()
    {
        transform.rotation = Quaternion.Euler(45f, 0, 0);
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
        _spherecollider = GetComponent<SphereCollider>();
        _spherecollider.isTrigger = true;
    }

    // * 충돌 감지 함수
    //- 플레이어의 습득 범위 오브젝트 감지 (코루틴 실행)
    //- 플레이어 오브젝트 감지 (장착, 풀링 환원)
    //- 지형 감지 (3D 환경에서 경험치 오브젝트 통통 튀는 애니메이션 적용 실패..ㅠㅠ)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.GetRangeTag))
        {
            StartCoroutine(GetItem(other.gameObject));
        }
        if (other.CompareTag(Define.GroundTag))
        {
            _rigidbody.useGravity = false;
            _rigidbody.linearVelocity = Vector3.zero;
        }
        if(other.CompareTag(Define.PlayerTag))
        {
            PlayerUnitManager.Instance.PlayerInfo.ExpUp(ExpAmount);
            ObjectManager.Instance.Despawn(this);
        }
    }

    #region Interface
    // * 아이템 흡수 기능
    //- IGetable 인터페이스 기능 구현
    //- 타겟에게 점점 끌려가는 흡수 기능 구현
    public IEnumerator GetItem(GameObject target)
    {
        float duration = 0.3f;                     // 전체 이동 시간
        float elapsed = 0;                         // 이동 시간 추적 : 증가 변수

        Vector3 startPos = transform.position;     // 시작 위치 : 객체 위치 
        Vector3 endPos = target.transform.position;// 도착 위치 : 매개 변수 target 위치

        // 전체 이동시간 만큼 부드럽게 이동
        while(elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos,endPos,elapsed/duration);
            elapsed += Time.deltaTime; 
            yield return null;
        }
    }
    #endregion
}