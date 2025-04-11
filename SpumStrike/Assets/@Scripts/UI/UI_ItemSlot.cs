using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// * 아이템 슬롯 UI
//- 활성화 조건 : 레벨 업하면 UpGrade UI가 활성화되며 같이 자동 활성화
//- 활성화 될 때 잠금 flag가 false라면 자동 랜덤 아이템 설정 및 UI 초기화, true라면 고정
//- 비활성화 조건 : UpGrade UI에서 아이템 클릭 후 선택 버튼 클릭 시 아이템 슬롯은 자동 비활성화
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private Item _upgradeItem;

    public Image SelectMarker;
    public GameObject SpritePanel;
    public GameObject SpriteImage;
    public TMP_Text ItemName;
    public TMP_Text ItemDescription;

    // 액션으로하기엔...

    // setslot item 메서드 item 종류가 한 종류이기에 의존성 역전 원칙을 지키지 않았음
    // 하지만 추후 아이템 종류를 세분화하고 늘리고 싶다면 interface로 구현해 변경할 것
    // 아이템 랜덤 설정도 해당 메서드에서 수행하지 말고 외부에서 수행 후 아이템만 받아올 것
    public void SetSlot(Item upgradeItem)
    {
        // 랜덤 아이템 설정 및 초기화
        // 이때 다른 슬롯의 아이템과 중복 문제 및 한계 논의
        // 스크립트는 미리 붙여두고 런타임에 아이템 정보만 받아오는게 맞음
        // 그런데 활성화 될 때 아이템 정보를 초기화해야하는데..?
        //_upgradeItem = ObjectManager.Instance.Items[Random.Range(0, ObjectManager.Instance.Items.Count)];
        _upgradeItem = upgradeItem;
        UpdateSlot();
    }

    // 아이템 슬롯 변경 메서드
    public void UpdateSlot()
    {
        SpritePanel.GetComponent<Image>().color = _upgradeItem.GetItemColor();
        SpriteImage.GetComponent<Image>().sprite = _upgradeItem.SpriteImage;
        ItemName.text = _upgradeItem.Name;
        ItemName.color = _upgradeItem.GetItemColor();
        ItemDescription.text = _upgradeItem.ToolTipText;
        ItemDescription.color = _upgradeItem.GetItemColor();
    }

    // 이미지를 띄우고 클릭 시 선택하는 것까지가 기능이다.
    #region IPointerHandler
    // 해당 UI에 마우스 클릭 시 효과(사운드 추가)
    public void OnPointerDown(PointerEventData eventData)
    {
        SelectMarker.gameObject.SetActive(true);
    }

    // 해당 UI에 마우스 클릭 후 뗐을 시 효과(선택 마커 출력)
    public void OnPointerUp(PointerEventData eventData)
    {
        SelectMarker.gameObject.SetActive(false);
        UpgradeUIManager.Instance.OnSelectItem?.Invoke(_upgradeItem);
        UpgradeUIManager.Instance.SetCanvasOff();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SelectMarker.gameObject.SetActive(false);
    }
    #endregion
}
