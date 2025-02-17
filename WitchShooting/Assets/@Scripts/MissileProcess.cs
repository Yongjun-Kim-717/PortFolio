using UnityEngine;
using System.Collections.Generic;
public class MissileProcess : MonoBehaviour
{
    public GameObject Missile;

    public List<GameObject> MissileList = new List<GameObject>();

    GameObject _missilePool;

    float _interval = 0.1f;
    float _coolTime = 0;

    void Start()
    {
        _missilePool = new GameObject("MissilePool");
    }

    // Update is called once per frame
    void Update()
    {
        _coolTime += Time.deltaTime;

        if(Input.GetKey(KeyCode.Space))
        {
            if(_coolTime > _interval)
            {
                _coolTime = 0;
                GetMissile();
            }
        }
    }

    GameObject GetMissile()
    {
        for(int i=0; i<MissileList.Count; i++)
        {
            if(!MissileList[i].gameObject.activeSelf)
            {
                MissileList[i].SetActive(true);
                MissileList[i].transform.position = transform.position + new Vector3(0, 1, 0);
                return MissileList[i];
            }
        }
        GameObject missile = Instantiate(Missile);
        missile.transform.parent = _missilePool.transform;
        missile.transform.position = transform.position + new Vector3(0, 1, 0);
        MissileList.Add(missile);
        return missile;
    }
}
