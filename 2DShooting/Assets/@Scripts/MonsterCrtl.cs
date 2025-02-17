using UnityEngine;
using TMPro;

public class MonsterCrtl : MonoBehaviour
{
    public float MaxHp = 100.0f;

    static int _score = 0;
    private float _currentHp;

    public TMP_Text ScoreText;

    void Start()
    {
        _currentHp = MaxHp;
        Destroy(gameObject, 10.0f);
        ScoreText = GameObject.Find("Text - Score").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        transform.Translate(Vector3.left * 5f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            _currentHp -= 50f;
            if(_currentHp <= 0)
            {
                _score += 10;
                ScoreText.text = $"Score : {_score}";
                Destroy(gameObject);
            }
        }
    }
}
