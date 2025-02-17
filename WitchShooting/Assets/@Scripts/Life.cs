using System.Collections;
using UnityEngine;

public class Life : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    void OnEnable()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        float posX = Random.Range(-50f, 50f);
        float posY = Random.Range(200f, 300f);
        _rigidbody2D.AddForce(new Vector3(posX, posY, 0));
        StartCoroutine(CoDeactivate());
    }

    IEnumerator CoDeactivate()
    {
        yield return new WaitForSeconds(10);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(transform.position.x, -2, 2);
        transform.position = currentPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
