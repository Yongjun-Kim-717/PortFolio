using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    // 생성된 프리팹의 이동속도
    private float _speed = 2.0f;

    void Update()
    {
        this.gameObject.transform.position += new Vector3(-_speed, 0, 0) * Time.deltaTime;
        
        // 화면에서 벗어날 경우 삭제
        if(this.gameObject.transform.position.x < -5.0f)
        {
            Destroy(gameObject);
        }
    }
}
