using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // ��ũ�� �ӵ�
    public float Speed;
    // ����� ���� �迭
    public Transform[] BackGrounds;
    // ���� ĳ���͸� ���� ����
    public GameObject Bird;

    // ��� �̹��� �� �� x��ǥ
    private float _leftPosX = 0f;
    private float _rightPosX = 0f;

    // ���� ȭ���� x, y ���� ����
    private float _xScreenHalfSize;
    private float _yScreenHalfSize;

    void Start()
    {
        // �ػ󵵿� �°� ���� ����
        _yScreenHalfSize = Camera.main.orthographicSize;
        _xScreenHalfSize = _yScreenHalfSize * Camera.main.aspect;

        // ��� �̹����� ��ǥ �ʱ�ȭ
        _leftPosX = -(_xScreenHalfSize * 2);
        _rightPosX = _xScreenHalfSize * 2;
    }

    void Update()
    {
        for (int i = 0; i < BackGrounds.Length; i++)
        {
            //��� �̹��� x�� ���� �������� Speed ��ŭ �̵�
            BackGrounds[i].position += new Vector3(-Speed, 0, 0) * Time.deltaTime;

            // ��� �̹����� ����ȭ�����κ��� ��� �� ��ġ ����
            if (BackGrounds[i].position.x < _leftPosX)
            {
                Vector3 nextPos = BackGrounds[i].position;
                nextPos = new Vector3(nextPos.x + _rightPosX * 2, nextPos.y, nextPos.z);
                BackGrounds[i].position = nextPos;
            }
        }
    }
}
