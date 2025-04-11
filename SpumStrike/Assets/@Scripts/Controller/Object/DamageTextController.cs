using TMPro;
using UnityEngine;

// * DamageTextController 스크립트
//- 생성 시 Vector3.up으로 이동 및 일정 시간 지나면 오브젝트 풀링 환원
public class DamageTextController : BaseController
{
    // 속도 관련 변수
    private float _moveUpSpeed;
    private float _alphaDownSpeed;

    // 컴포넌트 변수
    private TextMeshPro _text;
    private Color _alphaColor;

    // 데미지 정보를 받을 변수
    public float Damage;

    // * 초기화 함수
    //- Awake에서 실행
    protected override void Initialize()
    {
        _moveUpSpeed = 2.0f;
        _alphaDownSpeed = 2.0f;

        _text = gameObject.GetOrAddComponent<TextMeshPro>();
    }

    // * 풀링 활성화 OnEnable 주기함수
    //- alpha 값 초기화
    //- 일정 시간 뒤 파괴 (코루틴을 활용하자)
    private void OnEnable()
    {
        _alphaColor.a = 255f;
        Invoke("DestroyThis", 1.0f);
    }

    // * Update 주기함수
    //- 데미지 텍스트 이동 기능
    //- 데미지 텍스트 투명도 조절 기능
    private void Update()
    {
        _text.text = Damage.ToString();

        // 3D 공간 기준 위쪽 (0,1,0) 방향으로 이동
        transform.Translate(new Vector3(0, _moveUpSpeed * Time.deltaTime, 0));

        // 색(투명도) 변환
        _alphaColor.a = Mathf.Lerp(_alphaColor.a, 0, Time.deltaTime * _alphaDownSpeed);
        _text.color = _alphaColor;
    }

    // * 풀링 환원 함수
    private void DestroyThis()
    {
        ObjectManager.Instance.Despawn(this);
    }
}
