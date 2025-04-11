using System;
using System.Collections;
using UnityEngine;

// * 스폰 시스템
//- 이것도 몬스터 수를 파악해야 하므로 싱글톤으로 제작해야겠다.
//- 보스 몬스터 사망 시 클리어 이벤트 발생
//- 클리어 시 다음 웨이브 시작
//- 플레이어 위치는 그대로
//- 몬스터 웨이브 시작
public class SpawnSystem : MonoBehaviour
{
    Coroutine _coSpawningPool;

    WaitForSeconds _spawnInterval;

    private int _enemyCount;

    private void Awake()
    {
        ObjectManager.Instance.Spawn<PlayerController>(Vector3.zero); // 플레이어 캐릭터 생성 메서드를 몬스터 생성 코루틴과 분리
    }

    // StageInfo 스크립터블 오브젝트를 StageManager에서 받아 데이터 설정 진행
    public void Setting(StageInfo stageinfo)
    {
        _enemyCount = stageinfo.EnemyCount;
        _spawnInterval = new WaitForSeconds(stageinfo.SpawnInterval);
    }

    // StageManager에서 직접 SpawnSystem을 다루자
    // 코루틴 변수를 두고 해당 변수의 null체크를 통해 스포닝 메서드의 중복 실행을 방지
    public void StartSpawningPool()
    {
        if (_coSpawningPool == null)
        {
            _coSpawningPool = StartCoroutine(CoSpawningPool());
        }
    }

    // * 적 스폰 코루틴 함수
    //- 일반 몬스터 지속 생성
    //- 일정 카운트 이상 넘어가면 보스 몬스터 생성
    //- 보스 몬스터 처치 시 클리어
    IEnumerator CoSpawningPool()
    {
        int count = 0;

        while (true)
        {
            yield return _spawnInterval;

            Vector3 spawnPos = GetRandomPositionAround(Vector3.zero); //캐릭터 위치 기준으로 할 경우 캐릭터 위치에 따라 필드 외부에서 몬스터 생성 가능성 존재

            PoolManager.Instance.EnsureInitialized();
            PoolManager.Instance.GetObject<SkeletonController>(spawnPos);

            if (count < 3)
                PoolManager.Instance.GetObject<ChestController>(GetRandomPositionAround(ObjectManager.Instance.Player.transform.position, 5f, 20f));

            count++;

            if(count >= _enemyCount)
            {
                ObjectManager.Instance.Spawn<BossController>(new Vector3(0, 0, 0));
                _coSpawningPool = null;
                break;
            }
        }
    }

    // 랜덤 스폰 좌표 설정 메서드
    public Vector3 GetRandomPositionAround(Vector3 origin, float minDistance = 1f, float maxDistance = 20f)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Rad2Deg;
        float distance = UnityEngine.Random.Range(minDistance, maxDistance);

        float offsetX = Mathf.Cos(angle) * distance;
        float offsetZ = Mathf.Sin(angle) * distance;

        Vector3 pos = origin + new Vector3(offsetX, 0, offsetZ);

        return pos;
    }
}
