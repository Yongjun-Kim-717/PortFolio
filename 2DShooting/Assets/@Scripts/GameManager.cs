using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MonsterPrefab;
    public GameObject GroundPrefab;

    void Start()
    {
        StartCoroutine(MonsterWave(MonsterPrefab));
        StartCoroutine(GenerateGround(GroundPrefab));
    }

    IEnumerator MonsterWave(GameObject monster)
    {
        yield return new WaitForSeconds(3.0f);

        float posY = Random.Range(-4f, 4f);
        Instantiate(monster, new Vector3(10, posY, 0), Quaternion.identity);

        StartCoroutine(MonsterWave(monster));
    }

    IEnumerator GenerateGround(GameObject ground)
    {
        yield return new WaitForSeconds(5.0f);

        float posY = Random.Range(-4f, 4f);
        Instantiate(ground, new Vector3(10, posY, 0), Quaternion.identity);

        StartCoroutine(MonsterWave(ground));
    }
}
