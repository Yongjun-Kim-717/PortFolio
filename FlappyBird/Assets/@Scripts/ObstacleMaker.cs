using System.Collections;
using UnityEngine;

public class ObstacleMaker : MonoBehaviour
{
    // ������ Pipe�� �������� ���� ����
    public GameObject PipePrefab;

    // ������ Coin�� �������� ���� ����
    public GameObject CoinPrefab;

    // �������� ���� ��ġ ����
    private float _generatePosX = 4.0f;
    private float _generatePosY = 9.0f;

    // �������� ���� ����
    private float _length;

    void Start()
    {
        StartCoroutine(MakeRandomObstacle());
    }

    // ��ֹ� ���� ���� �Լ�
    IEnumerator MakeRandomObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);

            // ���� ���� ����
            float rand = Random.Range(-4.0f, 4.0f);

            // ���� ������ ��ֹ� �� ���� ����
            Instantiate(PipePrefab, new Vector2(_generatePosX, _generatePosY + rand), Quaternion.AngleAxis(180, new Vector3(0, 0, 1))); // �� ������ ����
            Instantiate(PipePrefab, new Vector2(_generatePosX, -(_generatePosY - rand)), Quaternion.identity); // �Ʒ� ������ ����
            Instantiate(CoinPrefab, new Vector2(_generatePosX, rand), Quaternion.identity); // ���� ����
        }
    }
}
