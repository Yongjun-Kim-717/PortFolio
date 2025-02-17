using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Vector3 _startPos; // 마우스 클릭 시작위치
    Vector3 _endPos;   // 마우스 클릭 뗀 위치
    Vector3 _direction;// 마우스 이동 방향
    Vector3 _target;   // 이동할 목표 지점

    int _cnt = 0;      // 이동할 목표지점 번호
    bool _isGround = false; //땅에 닿았는지 여부

    Rigidbody _rigidbody;

    public UnityEngine.UI.Text ScoreText;

    public GameObject GameOverUI;

    int _score = 0;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MouseEvent();
        Move();
    }
    // 마우스 입력처리
    void MouseEvent()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition;
            _direction = _endPos - _startPos;

            if(_direction.x > 50)
            {
                _cnt = (_cnt > 0) ? 1 : _cnt + 1;
            }
            if(_direction.x < -50)
            {
                _cnt = (_cnt < 0) ? -1 : _cnt - 1;
            }
            if(_direction.y>50 && _isGround)
            {
                _rigidbody.AddForce(Vector3.up * 300);
                _isGround = false;
            }
        }
    }   
    // 이동
    void Move()
    {
        _target = transform.position;
        switch(_cnt)
        {
            case -1:
                _target.x = -3;
                break;
            case 0:
                _target.x = 0;
                break;
            case 1:
                _target.x = 3;
                break;
        }
        if(Vector3.Distance(_target, transform.position) > 0.01f)
        {
            transform.Translate((_target - transform.position).normalized * 5 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
        if(other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            _score += 10;
            ScoreText.text = _score.ToString();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }
}
