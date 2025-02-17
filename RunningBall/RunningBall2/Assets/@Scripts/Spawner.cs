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
        // 0이상 10미만의 난수 - int형으로 범위 지정 시 이상 미만, float으로 난수 지정 시 이상 이하
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
