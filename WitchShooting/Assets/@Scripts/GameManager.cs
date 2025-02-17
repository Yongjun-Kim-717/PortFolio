using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SingleTon
    // * 싱글톤 패턴
    //- 객체의 인스턴스가 오직 1개인 패턴
    //- 메모리 낭비를 방지한다.
    //- 다른 클래스 간의 데이터 공유가 쉽다.
    private static GameManager s_instance = null;
    public static GameManager Instance
    {
        get
        {
            if (s_instance == null)
                return null;
            return s_instance;
        }
    }

    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // 값이 있다면 (중복되었다면)
        {
            Destroy(gameObject);
        }
        if(_coinPool == null)
        {
            _coinPool = new GameObject("CoinPool");
        }
        if(_lifePool == null)
        {
            _lifePool = new GameObject("LifePool");
        }
    }
    #endregion

    #region HPSCORE
    private int _score = 0;
    private int _MaxHp = 3;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            if (_score > PlayerPrefs.GetInt("Score"))
            {
                PlayerPrefs.SetInt("Score", _score);
            }
        }
    }

    public int MaxHp
    {
        get { return _MaxHp; }
    }

    public int BestScore { get => PlayerPrefs.GetInt("Score"); }
    #endregion

    #region PlAYERPOS
    private static Vector3 _playerPos;

    public Vector3 PlayerPos
    {
        get { return _playerPos; }
        set { _playerPos = value; }
    }

    #endregion

    #region COINPOOL
    private static GameObject _coinPool;

    [SerializeField]
    private List<GameObject> _coinList = new List<GameObject>();
    public GameObject CoinPool
    {
        get { return _coinPool; }
    }

    public List<GameObject> CoinList
    {
        get { return _coinList; }
    }
    #endregion

    #region LIFEPOOL
    private static GameObject _lifePool;

    [SerializeField]
    private List<GameObject> _lifeList = new List<GameObject>();
    public GameObject LifePool
    {
        get { return _lifePool; }
    }

    public List<GameObject> LifeList
    {
        get { return _lifeList; }
    }
    #endregion
}
