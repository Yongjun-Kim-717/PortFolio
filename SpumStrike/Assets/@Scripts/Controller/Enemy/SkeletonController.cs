using UnityEngine;

// * SkeletonController 스크립트
//- 능력치 초기화
public class SkeletonController : EnemyController
{
    // * 초기화 함수
    //- Awake에서 실행
    //- 능력치 정보 로드 및 전달
    protected override void Initialize()
    {
        _enemyStat = Resources.Load<EnemyStat>(Define.SkeletonStatusPath);
        base.Initialize();
    }
}
