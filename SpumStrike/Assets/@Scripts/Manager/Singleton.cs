using UnityEngine;
using UnityEngine.SceneManagement;

// * 싱글톤 객체
//- 객체 생성 시 오직 하나만 생성될 수 있으며 씬 변경에도 파괴되지 않는 객체를 구현
//- 씬 변경시 Clear 메서드로 특정 변수 초기화 가능
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance = null;

    // * Instance 프로퍼티 (getter) : Instance가 첫 호출될 때 _instance 생성
    //- 생성 시 @Managers 그룹 탐색 후 생성 및 DontDestroyOnLoad 설정
    //- _instance == null 이면 동적으로 생성 후 제네릭 스크립트 연결
    //- 생성한 매니저 오브젝트를 @Managers의 자식으로 설정
    //- _instance에 생성한 매니저 오브젝트 할당
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject manager = GameObject.Find("@Managers");
                if (manager == null)
                {
                    manager = new GameObject("@Managers");
                    DontDestroyOnLoad(manager);
                }
                _instance = FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    T component = obj.AddComponent<T>();
                    obj.transform.parent = manager.transform;
                    _instance = component;
                }
            }
            return _instance;
        }
    }
    // * Awake 주기함수
    //- Awake시기에 _instance 체크 및 DontDestroyOnLoad 설정
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }
    // * OnEnable 주기함수
    //- 초기화 함수 실행
    private void OnEnable()
    {
        Initialize();
    }
    // * 초기화 함수
    //- sceneLoaded 이벤트에 OnSceneChanged 함수 구독 (중복 구독 고려)
    protected virtual void Initialize()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
        SceneManager.sceneLoaded += OnSceneChanged;
    }
    // * OnSceneChanged 함수
    //- Scene 변경 시 Clear 함수 실행
    protected void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        Clear();
    }
    // * Clear 함수
    protected virtual void Clear()
    {

    }
}
