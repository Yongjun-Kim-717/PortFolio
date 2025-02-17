using UnityEngine;

public class PlayerCrtl : MonoBehaviour
{
    public GameObject BulletPrefab_default;

    public float Speed = 5f;

    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = (Vector2.up * v) + (Vector2.right * h);

        tr.Translate(moveDir.normalized * Speed * Time.deltaTime);
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(BulletPrefab_default, new Vector3(tr.position.x,tr.position.y,0), Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Failed");
        }
    }
}
