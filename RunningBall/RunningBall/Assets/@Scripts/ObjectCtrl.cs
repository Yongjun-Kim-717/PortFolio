using UnityEngine;

// ���� �Ŵ������� Ʈ�� ���� ������ ������Ʈ

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
