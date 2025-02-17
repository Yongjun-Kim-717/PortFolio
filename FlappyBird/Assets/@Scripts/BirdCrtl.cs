using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BirdCrtl : MonoBehaviour
{
    // 클릭 시 새의 점프속도
    public float JumpSpeed;

    // 점수 변수
    private int _score = 0;

    public TMP_Text msg;

    // rigidbody2D 컴포넌트 저장 변수
    [SerializeField]
    private Rigidbody2D _rigidBody;

    // 마우스 좌측 클릭 이벤트 변수 저장
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
