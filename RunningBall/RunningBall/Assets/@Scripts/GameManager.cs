using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] Objects;
    public GameObject[] Tracks;
    
    void Start()
    {
        for(int i=0; i<Tracks.Length; i++)
        {
            StartCoroutine(GenerateObject(Tracks[i], Objects));
        }
    }

    IEnumerator GenerateObject(GameObject track, GameObject[] gameObjects)
    {
        while(true)
        {
            yield return new WaitForSeconds(2.0f);

            int index = Random.Range(0, gameObjects.Length);

            Instantiate(gameObjects[index], new Vector3((float)track.transform.position.x, (float)track.transform.position.y + 1.5f, (float)track.transform.position.z + 50f), Quaternion.identity);
        }     
    }
}
