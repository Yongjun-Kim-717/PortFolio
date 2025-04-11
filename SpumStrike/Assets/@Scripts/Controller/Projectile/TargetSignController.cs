using System.Collections;
using UnityEngine;

// * TargetSignController 스크립트
//- 타겟을 따라다니다가 캐스팅 시간 종료 시 위치 고정 및 스킬 발현
public class TargetSignController : BaseController
{
    private bool _isStop = false;
    protected override void Initialize()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void Start()
    {
        StartCoroutine(FixedAndDestroy());
    }

    private void Update()
    {
        Move(ObjectManager.Instance.Player.transform.position);
    }

    void Move(Vector3 targetPos)
    {
        if(!_isStop)
            transform.position = targetPos;
    }

    // 생성 직후 플레이어의 위치에 있다가 1초 캐스팅 타임 지나면 위치 고정 및 삭제
    // 하드 코딩으로 시간 고정해둔 상태(BossSkill의 CastingTime을 받아서 해결할 것)
    IEnumerator FixedAndDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        _isStop = true;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
