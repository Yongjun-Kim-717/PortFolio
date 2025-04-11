using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// *WorldSpace UI Manager 스크립트
//- 월드 공간 캔버스에 존재하는 UI 오브젝트 관리 스크립트입니다.
//- 체력바를 Dictionary로 관리한다.
public class WorldSpaceUIManager : Singleton<WorldSpaceUIManager>
{
    private Dictionary<string, GameObject> _hpBarList = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> hpBarList
    {
        get { return _hpBarList; }
    }

    public Image Image_HP_Player;

    private bool _isInit = false;

    protected override void Initialize()
    {
        if(!_isInit)
        {
            base.Initialize();
            Image_HP_Player = GameObject.Find("Image - HPGauge").GetComponent<Image>();
            Image_HP_Player.fillAmount = PlayerUnitManager.Instance.PlayerInfo.CurrentHp / PlayerUnitManager.Instance.PlayerInfo.MaxHp;
            PlayerUnitManager.Instance.PlayerInfo.OnCurrentHpChanged += (hp) => { HPBarSet(hp); };
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

    void HPBarSet(float hp)
    {
        Image_HP_Player.fillAmount = hp / PlayerUnitManager.Instance.PlayerInfo.MaxHp;
    }

    protected override void Clear()
    {
        base.Clear();
        hpBarList.Clear();
    }
}
