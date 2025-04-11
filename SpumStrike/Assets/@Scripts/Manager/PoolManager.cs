using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // 오브젝트 풀링 리스트
    Dictionary<System.Type, List<GameObject>> _poolList;
    // 오브젝트 풀링 관리 변수
    Dictionary<System.Type, GameObject> _parentObject;

    private bool _isInit = false;

    protected override void Initialize()
    {
        if(!_isInit)
        {
            base.Initialize();
            _poolList = new Dictionary<System.Type, List<GameObject>>();
            _parentObject = new Dictionary<System.Type, GameObject>();
            _isInit = true;
        }
    }

    public void ClearOption()
    {
        _isInit = false;
    }

    public void EnsureInitialized()
    {
        Initialize();
    }

    // * 오브젝트 풀링을 통한 생성 메서드
    //- 제네릭을 활용 BaseController를 상속받는 Controller 오브젝트 풀링 기능
    public T GetObject<T>(Vector3 spawnPos) where T : BaseController 
    {
        System.Type type = typeof(T);

        // 타입 검사
        if(type.IsSubclassOf(typeof(BaseController)))
        {
            // 오브젝트 풀 리스트에 해당 타입의 오브젝트 리스트가 존재하는지 검사
            if(_poolList.ContainsKey(type))
            {
                // 존재할 시 해당 타입 풀 리스트의 모든 오브젝트 검사
                for(int i = 0; i < _poolList[type].Count; i++)
                {
                    // 풀 리스트의 오브젝트 활성화 여부 검사 및 비활성화 객체를 활성화 및 좌표 초기화(생성 개념)
                    if(!_poolList[type][i].activeSelf)
                    {
                        _poolList[type][i].SetActive(true);
                        _poolList[type][i].transform.position = spawnPos;

                        return _poolList[type][i].GetComponent<T>();
                    }
                }

                // 존재하지 않을 시 오브젝트 매니저의 Spawn 메서드로 동적 생성 및 풀링 관리 오브젝트로 부모 연결, 풀 리스트에 추가
                T obj = ObjectManager.Instance.Spawn<T>(spawnPos);
                obj.transform.SetParent(_parentObject[type].transform, false);
                _poolList[type].Add(obj.gameObject);
                return obj;
            }
            // 오브젝트 풀 리스트에 해당 타입의 오브젝트 리스트가 존재하지 않을 시
            else
            {
                // 풀링 관리 오브젝트에 해당 타입 오브젝트가 존재하지 않을 시 동적 생성 및 추가
                if (!_parentObject.ContainsKey(type))
                {
                    GameObject go = new GameObject(type.Name);
                    _parentObject.Add(type, go);
                }
                // 동적으로 오브젝트 생성 후 풀링리스트 동적 생성 및 추가
                var obj = ObjectManager.Instance.Spawn<T>(spawnPos);
                obj.transform.SetParent(_parentObject[type].transform, false);
                List<GameObject> newList = new List<GameObject>();
                newList.Add(obj.gameObject);
                _poolList.Add(type, newList);
                return obj;
            }
        }
        return null;
    }
}
