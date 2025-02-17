using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image[] Lifes;

    public float Speed = 3f;

    private int _currentHp;

    private void Start()
    {
        _currentHp = GameManager.Instance.MaxHp;
    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            float h = Input.GetAxisRaw("Horizontal");

            transform.Translate(Vector3.right * h * Speed * Time.deltaTime);

            Vector3 currentPos = transform.position;
            currentPos.x = Mathf.Clamp(transform.position.x, -2, 2);
            transform.position = currentPos;
            GameManager.Instance.PlayerPos = currentPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("Meteor"))
        {
            _currentHp--;
            Lifes[_currentHp].gameObject.SetActive(false);
            if(_currentHp <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
            }
        }
        if(collision.CompareTag("Life"))
        {
            if(_currentHp < 3)
            {
                Lifes[_currentHp].gameObject.SetActive(true);
                _currentHp++;
            }
        }
    }

    //1. 플레이어 체력 추가 및 UI 표시 (완)
    //2. 일시 정지 기능 추가 (완)
    //3. 골드가 화면 바깥으로 나가지 않게 처리 (완)
    //4. HP회복 아이템 추가 (완)
    //5. 골드, HP회복아이템을 오브젝트 풀로 관리 (완)
    //6. 메테오 추가 (완)
    //7. 몬스터 추가 (완)
}
