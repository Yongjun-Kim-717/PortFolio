using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // 스크롤 속도
    public float Speed;
    // 배경을 담을 배열
    public Transform[] BackGrounds;
    // 게임 캐릭터를 담을 변수
    public GameObject Bird;

    // 배경 이미지 양 끝 x좌표
    private float _leftPosX = 0f;
    private float _rightPosX = 0f;

    // 게임 화면의 x, y 길이 절반
    private float _xScreenHalfSize;
    private float _yScreenHalfSize;

    void Start()
    {
        // 해상도에 맞게 비율 조정
        _yScreenHalfSize = Camera.main.orthographicSize;
        _xScreenHalfSize = _yScreenHalfSize * Camera.main.aspect;

        // 배경 이미지의 좌표 초기화
        _leftPosX = -(_xScreenHalfSize * 2);
        _rightPosX = _xScreenHalfSize * 2;
    }

    void Update()
    {
        for (int i = 0; i < BackGrounds.Length; i++)
        {
            //배경 이미지 x축 기준 좌측으로 Speed 만큼 이동
            BackGrounds[i].position += new Vector3(-Speed, 0, 0) * Time.deltaTime;

            // 배경 이미지가 게임화면으로부터 벗어날 때 위치 변경
            if (BackGrounds[i].position.x < _leftPosX)
            {
                Vector3 nextPos = BackGrounds[i].position;
                nextPos = new Vector3(nextPos.x + _rightPosX * 2, nextPos.y, nextPos.z);
                BackGrounds[i].position = nextPos;
            }
        }
    }
}
