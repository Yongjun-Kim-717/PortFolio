using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    // ������ �������� �̵��ӵ�
    private float _speed = 2.0f;

    void Update()
    {
        this.gameObject.transform.position += new Vector3(-_speed, 0, 0) * Time.deltaTime;
        
        // ȭ�鿡�� ��� ��� ����
        if(this.gameObject.transform.position.x < -5.0f)
        {
            Destroy(gameObject);
        }
    }
}
