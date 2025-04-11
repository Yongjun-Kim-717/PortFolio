using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
public class Item : ScriptableObject
{
    public Define.ItemValue Value;  //아이템 밸류
    public string Name;             //아이템 이름
    public string ToolTipText;      //툴팁 텍스트
    public Define.Option Option;    //업그레이드 옵션
    public float Figure;            //업그레이드 수치
    public Sprite SpriteImage;      //스프라이트 이미지

    public float Weight;            //아이템 랜덤 가중치 

    //특정 밸류 색 리턴 메서드
    public Color GetItemColor()
    {
        return Define.ItemValueColors[Value];
    }
}
