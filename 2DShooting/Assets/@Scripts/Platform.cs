using UnityEngine;

public class Platform : MonoBehaviour
{
    public float Speed = 0.5f;

    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.AddForce(Vector2.left * Speed, ForceMode2D.Impulse);
        Destroy(gameObject, 7.0f);
    }
}
