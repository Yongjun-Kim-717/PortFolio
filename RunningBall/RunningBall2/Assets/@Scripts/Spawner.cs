using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject Coin;


    void Start()
    {
        StartCoroutine(CoSpawnItem());
    }


    IEnumerator CoSpawnItem()
    {
        // 0�̻� 10�̸��� ���� - int������ ���� ���� �� �̻� �̸�, float���� ���� ���� �� �̻� ����
        int random = Random.Range(0, 10);
        if(random < 7)
        {
            Instantiate(Obstacle, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(Coin, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(CoSpawnItem());
    }
}
