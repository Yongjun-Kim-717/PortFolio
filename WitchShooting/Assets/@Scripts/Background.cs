using UnityEngine;

public class Backgorund : MonoBehaviour
{
    public GameObject[] Backgrounds;
    public float speed = 5f;


    void FixedUpdate()
    {
        for(int i = 0; i < Backgrounds.Length; i++)
        {
            // 아래로 이동
            Backgrounds[i].transform.position += Vector3.down * speed * Time.deltaTime;

            if(Backgrounds[i].transform.position.y < -8)
            {
                Backgrounds[i].transform.position = new Vector3(0, 15.5f, 0f);
            }
        }
    }
}
