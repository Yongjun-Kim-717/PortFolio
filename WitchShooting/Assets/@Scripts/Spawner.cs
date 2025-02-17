using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Meteor;
    public GameObject WarningLine;

    Vector3 _initPos_Enemy = new Vector3(-3, 6, 0);
    Vector3 _initPos_Meteor = new Vector3(0, 8, 0);

    void Start()
    {
        StartCoroutine(CoSpawnEnemy());
        StartCoroutine(CoSpawnMeteor());
    }

    IEnumerator CoSpawnEnemy()
    {
        Vector3 spawnPos = _initPos_Enemy;

        for(int i =0; i<5; i++)
        {
            spawnPos.x += 1;
            Instantiate(Enemy, spawnPos , Quaternion.identity);
        }

        yield return new WaitForSeconds(2.5f);
        StartCoroutine(CoSpawnEnemy());
    }

    IEnumerator CoSpawnMeteor()
    {
        yield return new WaitForSeconds(4.0f);

        Vector3 spawnPosLine = GameManager.Instance.PlayerPos;
        GameObject line = Instantiate(WarningLine, spawnPosLine , Quaternion.identity);
        
        yield return new WaitForSeconds(1.5f);

        Destroy(line);
        Vector3 spawnPosMeteor = GameManager.Instance.PlayerPos + _initPos_Meteor;
        GameObject meteor = Instantiate(Meteor, spawnPosMeteor , Quaternion.identity);

        StartCoroutine(CoSpawnMeteor());
    }
}
