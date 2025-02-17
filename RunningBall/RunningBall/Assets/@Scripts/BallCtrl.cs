using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallCtrl : MonoBehaviour
{
    public TMP_Text Text;
    public GameObject FailedPanel;
    public Button Restart;

    private Rigidbody rb;
    private Transform tr;

    private Vector3 _track1Pos = new Vector3(-5, 1, -38);
    private Vector3 _track2Pos = new Vector3(0, 1, -38);
    private Vector3 _track3Pos = new Vector3(5, 1, -38);
    private Vector3 _targetPos;

    private float _power = 15f;
    private float _speed = 6f;
    private int _score = 0;

    public Vector3 StartP = Vector3.zero;
    public Vector3 EndP;
    public Vector3 moveP;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        Text.text = $"Score : {_score}";
        _targetPos = _track2Pos;
    }

    void Update()
    {
        CheckMouseCursor();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            _score += 10;
            Text.text = $"Score : {_score}";
        }
        if(other.gameObject.CompareTag("Obstacle"))
        {
            Time.timeScale = 0.0f;
            FailedPanel.SetActive(true);
            Restart.onClick.AddListener(OnClickRestart);
        }
    }

    private void CheckMouseCursor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartP = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndP = Input.mousePosition;
            moveP = EndP - StartP;
            if (moveP.x > 40.0f)
            {
                if (tr.position.x >= _track1Pos.x && _track2Pos.x > tr.position.x)
                {
                    _targetPos = _track2Pos;
                }
                else if (tr.position.x >= _track2Pos.x)
                {
                    _targetPos = _track3Pos;
                }               
            }
            else if (moveP.x < -40.0f)
            {
                if (tr.position.x <= _track2Pos.x)
                {
                    _targetPos = _track1Pos;
                }
                else if (tr.position.x <= _track3Pos.x)
                {
                    _targetPos = _track2Pos;
                }
            }
        }
    }

    private void Move()
    {
        tr.Translate((_targetPos - tr.position).normalized * _speed * Time.deltaTime);
        if (Vector3.Distance(tr.position, _targetPos) < 0.1f)
        {
            tr.position = _targetPos;
        }
    }

    private void Jump()
    {
        if (moveP.y > 40.0f)
        {
            rb.AddForce(Vector3.up * _power, ForceMode.Impulse);
            moveP.y = 0;
        }
    }

    private void OnClickRestart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
