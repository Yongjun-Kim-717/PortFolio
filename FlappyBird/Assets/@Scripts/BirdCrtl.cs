using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BirdCrtl : MonoBehaviour
{
    // Ŭ�� �� ���� �����ӵ�
    public float JumpSpeed;

    // ���� ����
    private int _score = 0;

    public TMP_Text msg;

    // rigidbody2D ������Ʈ ���� ����
    [SerializeField]
    private Rigidbody2D _rigidBody;

    // ���콺 ���� Ŭ�� �̺�Ʈ ���� ����
    private bool IsMouseClick => Input.GetMouseButtonDown(0);

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Jump();
    }

    void Jump()
    {
        if (IsMouseClick)
        {
            _rigidBody.AddForce(Vector2.up * JumpSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coin"))
        {
            _score++;
            msg.text = $"Score : {_score}";
            Destroy(collision.gameObject);
        }
    }
}
