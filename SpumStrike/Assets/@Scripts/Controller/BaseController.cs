using UnityEngine;

// * BaseController 스크립트
//- 모든 Controller의 부모 클래스
//- 초기화 시점 지정
//- 초기화 흐름이 자동으로 지정되다 보니 관리를 중요시 해야함
public abstract class BaseController : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }

    protected abstract void Initialize();
}
