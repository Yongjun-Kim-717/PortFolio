using UnityEngine;

public class Coin : MonoBehaviour
{ 
    void Update()
    {
        transform.Rotate(new Vector3 (15,30,45) * 5 * Time.deltaTime);
        transform.Translate(Vector3.back * 10 * Time.deltaTime, Space.World); //���� ��ǥ��� �̵� �� Rotate�� ������ǥ���� ������ ��ȯ�Ǿ��⿡ �̻��� ������� �����Ƿ� translate�� �ι�° ���ڿ� Space.World�߰�
    }
}
