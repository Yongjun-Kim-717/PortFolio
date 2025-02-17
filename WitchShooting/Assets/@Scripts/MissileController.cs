using System.Collections;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(CoDeactivate());
    }

    IEnumerator CoDeactivate()
    {
        yield return new WaitForSeconds(10);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * 5 * Time.deltaTime);
    }
}
