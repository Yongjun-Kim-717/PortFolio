using UnityEngine;

public class Obstacle : MonoBehaviour
{

    void Update()
    {
        transform.Translate(Vector3.back * 10 * Time.deltaTime);
    }
}
