using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Vector3 _startPos; // ���콺 Ŭ�� ������ġ
    Vector3 _endPos;   // ���콺 Ŭ�� �� ��ġ
    Vector3 _direction;// ���콺 �̵� ����
    Vector3 _target;   // �̵��� ��ǥ ����

    int _cnt = 0;      // �̵��� ��ǥ���� ��ȣ
    bool _isGround = false; //���� ��Ҵ��� ����

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
    // ���콺 �Է�ó��
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
    // �̵�
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
