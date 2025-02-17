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

    //1. �÷��̾� ü�� �߰� �� UI ǥ�� (��)
    //2. �Ͻ� ���� ��� �߰� (��)
    //3. ��尡 ȭ�� �ٱ����� ������ �ʰ� ó�� (��)
    //4. HPȸ�� ������ �߰� (��)
    //5. ���, HPȸ���������� ������Ʈ Ǯ�� ���� (��)
    //6. ���׿� �߰� (��)
    //7. ���� �߰� (��)
}
