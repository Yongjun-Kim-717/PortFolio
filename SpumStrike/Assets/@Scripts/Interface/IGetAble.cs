using System.Collections;
using UnityEngine;

// * IGetAble 인터페이스
//- 아이템 습득 기능 구현
public interface IGetAble
{
    IEnumerator GetItem(GameObject target);
}
