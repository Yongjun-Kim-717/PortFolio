using UnityEngine;

// * CameraController 스크립트
//- 플레이어 추적 이동
public class CameraController : BaseController
{
    // 타겟 컴포넌트 == 플레이어
    public Transform Target;

    // 위치 정보
    private Vector3 _offset = new Vector3(0, 7, -7);
    private Vector3 _movePos;

    // 카메라 속도
    private float _moveSpeed = 3f;

    // * 초기화 함수
    //- Awake에서 실행
    //- ObjectManager의 Player transform 정보 타겟에 저장
    protected override void Initialize()
    {
        Target = ObjectManager.Instance.Player.transform;
    }

    // * FixedUpdate 주기함수
    //- 타겟을 향해 부드러운 이동 (Lerp 활용)
    private void FixedUpdate()
    {
        _movePos = Target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, _movePos, _moveSpeed * Time.deltaTime);
    }
}
