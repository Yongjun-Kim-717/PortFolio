using System.Collections;
using UnityEngine;

// * BossController 스크립트
//- EnemyController 상속
//- Boss 캐릭터의 크기 조정
//- Boss 캐릭터의 스킬 공격 추가
public class BossController : EnemyController
{
    // 스킬 공격의 쿨타임과 캐스팅 타임 (Scriptable Object에서 관리하도록 변경 예정)
    private WaitForSeconds _coolTime = new WaitForSeconds(3.0f);
    private WaitForSeconds _castingTime = new WaitForSeconds(2.0f);

    // 보스 스킬 오브젝트
    public GameObject BossSkill { get; set; }
    public GameObject BossSkillTargetSign { get; set; }

    // * 초기화 메서드
    //- Awake에서 수행
    protected override void Initialize()
    {
        //보스 스테이터스 Scriptable Object 로드 (ObjectManager에서 보스 리스트를 관리하고 정보를 받아오도록 변경)
        _enemyStat = Resources.Load<EnemyStat>(Define.BossStatusPath);  
        base.Initialize();
    }

    // * Start 주기함수
    //- 스킬 공격 코루틴 실행
    private void Start()
    {
        StartCoroutine(CoSkillAttack());
    }

    // * 사망 메서드 재정의
    //- 스테이지 클리어 메서드 수행 (의존성이 너무 높음. 인터페이스 or 이벤트를 도입해 결합도를 낮추자)
    protected override void Dead()
    {
        base.Dead();
        StageManager.Instance.ClearStage(); 
    }

    // * 보스 스킬 공격 코루틴 함수
    //- 타겟 지정 및 타겟 표시 생성(타겟 추적은 targetSignController 스크립트)
    //- 캐스팅 시간이 끝나면 타겟 표식 오브젝트가 사라지고 해당 위치에 스킬 오브젝트가 생성됨
    IEnumerator CoSkillAttack()
    {
        while(true)
        {
            yield return _coolTime;
            _animator.SetTrigger(Define.AttackTrigger);
            Vector3 spawnPos = ObjectManager.Instance.Player.transform.position;
            GameObject objTargetSign = Instantiate(BossSkillTargetSign, spawnPos, Quaternion.identity);

            yield return _castingTime; 
            GameObject objSkill = Instantiate(BossSkill, objTargetSign.transform.position, Quaternion.identity);
            objSkill.GetComponent<BossSkillController>().Atk = Atk;
        }
    }
}
