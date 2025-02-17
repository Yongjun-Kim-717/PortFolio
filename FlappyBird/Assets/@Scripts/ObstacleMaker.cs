using System.Collections;
using UnityEngine;

public class ObstacleMaker : MonoBehaviour
{
    // 생성될 Pipe의 프리팹을 담을 변수
    public GameObject PipePrefab;

    // 생성될 Coin의 프리팹을 담을 변수
    public GameObject CoinPrefab;

    // 파이프의 생성 위치 변수
    private float _generatePosX = 4.0f;
    private float _generatePosY = 9.0f;

    // 파이프의 길이 변수
    private float _length;

    void Start()
    {
        StartCoroutine(MakeRandomObstacle());
    }

    // 장애물 랜덤 생성 함수
    IEnumerator MakeRandomObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);

            // 랜덤 변수 지정
            float rand = Random.Range(-4.0f, 4.0f);

            // 랜덤 비율의 장애물 및 코인 생성
            Instantiate(PipePrefab, new Vector2(_generatePosX, _generatePosY + rand), Quaternion.AngleAxis(180, new Vector3(0, 0, 1))); // 위 파이프 생성
            Instantiate(PipePrefab, new Vector2(_generatePosX, -(_generatePosY - rand)), Quaternion.identity); // 아래 파이프 생성
            Instantiate(CoinPrefab, new Vector2(_generatePosX, rand), Quaternion.identity); // 코인 생성
        }
    }
}
