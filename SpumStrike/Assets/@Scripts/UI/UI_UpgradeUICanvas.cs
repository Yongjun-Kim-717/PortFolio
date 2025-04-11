using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeUICanvas : UI_Base
{
    // Item UI 리스트
    public GameObject _itemSlots;

    // 초기화 함수 재정의
    protected override void Initialize()
    {
        base.Initialize();
        _itemSlots = GameObject.Find("Panel - List");
    }

    // 활성화 시 아이템 랜덤 생성
    // 활성화 시 잠금된 리스트를 제외하고 새로운 아이템 등장
    private void OnEnable()
    {
        SetItemSlot(_itemSlots);
    }

    // 아이템 슬롯 세팅 메서드
    // 이 메서드에서 Active true 변경 및 해당 아이템 슬롯 활성화 및 초기화
    private void SetItemSlot(GameObject ItemList)
    {
        for (int i = 0; i < ItemList.transform.childCount; i++)
        {
            ItemList.transform.GetChild(i).gameObject.SetActive(true);
            ItemList.transform.GetChild(i).GetComponent<UI_ItemSlot>().SetSlot(ObjectManager.Instance.GetRandomItem());
        }
    }
}