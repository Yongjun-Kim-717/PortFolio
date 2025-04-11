using UnityEngine;
using UnityEngine.EventSystems;
public class UI_JoyStick : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject JoyStick;
    public GameObject Handler;

    private Vector2 _moveDir;
    private Vector2 _touchPos;
    private Vector2 _originPos;
    private float _radius;

    protected override void Initialize()
    {
        base.Initialize();
        _originPos = JoyStick.transform.position;
        _radius = JoyStick.GetComponent<RectTransform>().sizeDelta.y / 2;
        SetActiveJoyStick(false);
    }

    void SetActiveJoyStick(bool isActive)
    {
        JoyStick.SetActive(isActive);
        Handler.SetActive(isActive);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetActiveJoyStick(true);

        _touchPos = Input.mousePosition;
        JoyStick.transform.position = Input.mousePosition;
        Handler.transform.position = Input.mousePosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragPos = eventData.position;

        _moveDir = (dragPos - _touchPos).normalized;
        float distance = (dragPos - _touchPos).sqrMagnitude;

        Vector3 newPos;
        // 조이스틱이 반지름 안에 있는 경우
        if (distance < _radius)
        {
            newPos = _touchPos + (_moveDir * distance);
        }
        else
        {
            newPos = _touchPos + (_moveDir * _radius);
        }
        Handler.transform.position = newPos;

        PlayerUnitManager.Instance.MoveDir = _moveDir;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _moveDir = Vector2.zero;
        Handler.transform.position = _originPos;
        JoyStick.transform.position = _originPos;
        PlayerUnitManager.Instance.MoveDir = _moveDir;
        SetActiveJoyStick(false);
    }

}