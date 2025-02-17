using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed = 5f;

    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.AddForce(Vector2.right * BulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 5.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            Destroy(gameObject);
        }
    }
}
