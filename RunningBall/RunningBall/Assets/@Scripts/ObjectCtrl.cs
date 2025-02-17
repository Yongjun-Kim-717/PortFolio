using UnityEngine;

// 게임 매니저에서 트랙 끝에 생성한 오브젝트

public class ObjectCrtl : MonoBehaviour
{
    public float Speed = 60f;

    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    void Update()
    {
        transform.position += Vector3.back * Speed * 5 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("Coin") && other.gameObject.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }
    }
}
