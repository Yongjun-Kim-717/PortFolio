using System.Collections.Generic;
using UnityEngine;

public class EmemyController : MonoBehaviour
{
    public GameObject Effect;

    int _currentHp = 3;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.Translate(Vector3.down * 3 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Missile"))
        {
            _currentHp--;
            _animator.SetTrigger("Hit");
            collision.gameObject.SetActive(false); // 오브젝트 풀링으로 관리하므로 비활성화
            if(_currentHp <= 0)
            {
                GameManager.Instance.Score += 5;
                Instantiate(Effect, transform.position, Quaternion.identity);
                MakeCoin();
                MakeLife();
                Destroy(gameObject);
            }
        }
    }

    #region COINOBJECTPOOLING
    public GameObject Coin;
    List<GameObject> coinList = GameManager.Instance.CoinList;
    // CoinPool => GameManager Script에서 생성 : 현재 스크립트에 생성할 경우 몬스터 생성마다 CoinPool이 생성됨
    // CoinList => GameManager Script에서 생성 : 현재 스크립트에 생성할 경우 몬스터 생성마다 CoinList가 생성됨

    void MakeCoin()
    {
        int rnd = Random.Range(0, 4);
        for (int i = 0; i < rnd; i++)
        {
            GetCoin();
        }
    }

    GameObject GetCoin()
    {
        for (int i = 0; i < coinList.Count; i++)
        {
            if (!coinList[i].gameObject.activeSelf)
            {
                coinList[i].SetActive(true);
                coinList[i].transform.position = transform.position;
                return coinList[i];
            }
        }
        GameObject coin = Instantiate(Coin);
        coin.transform.parent = GameManager.Instance.CoinPool.transform;
        coin.transform.position = transform.position;
        coinList.Add(coin);
        return coin;
    }
    #endregion

    #region LIFEOBJECTPOOLING
    public GameObject Life;
    List<GameObject> lifeList = GameManager.Instance.LifeList;

    void MakeLife()
    {
        int rnd = Random.Range(0, 3);
        for (int i = 0; i < rnd; i++)
        {
            GetLife();
        }
    }

    GameObject GetLife()
    {
        for (int i = 0; i < lifeList.Count; i++)
        {
            if (!lifeList[i].gameObject.activeSelf)
            {
                lifeList[i].SetActive(true);
                lifeList[i].transform.position = transform.position;
                return lifeList[i];
            }
        }
        GameObject life = Instantiate(Life);
        life.transform.parent = GameManager.Instance.LifePool.transform;
        life.transform.position = transform.position;
        lifeList.Add(life);
        return life;
    }
    #endregion
}
