using UnityEngine;

public class Coin : MonoBehaviour
{ 
    void Update()
    {
        transform.Rotate(new Vector3 (15,30,45) * 5 * Time.deltaTime);
        transform.Translate(Vector3.back * 10 * Time.deltaTime, Space.World); //로컬 좌표계로 이동 시 Rotate로 로컬좌표계의 방향이 전환되었기에 이상한 결과물이 나오므로 translate의 두번째 인자에 Space.World추가
    }
}
