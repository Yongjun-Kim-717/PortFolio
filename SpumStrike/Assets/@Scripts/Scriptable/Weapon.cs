using UnityEngine;

[CreateAssetMenu(menuName = "Game/Weapon")]
public class Weapon : ScriptableObject
{
    public Define.Weapons WeaponType;
    public GameObject AttackEffect;
    public GameObject HitEffect;

    // 오브젝트 충돌 감지를 위한 태그 초기화
    private void OnEnable()
    {
        AttackEffect.tag = Define.SwordAuraTag;
    }
}
