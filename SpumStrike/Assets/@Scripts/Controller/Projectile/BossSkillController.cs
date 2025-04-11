using UnityEngine;

// * BossSkillController 스크립트
//- 보스 스킬 투사체 부착 스크립트
//- 공격력과 collider 속성 설정
public class BossSkillController : BaseController
{
    private CapsuleCollider _capsuleCollider;

    private float _atk;

    public float Atk 
    { 
        get { return _atk; }
        set { _atk = value; }
    }

    protected override void Initialize()
    {
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        _capsuleCollider = gameObject.GetOrAddComponent<CapsuleCollider>();
        _capsuleCollider.center = new Vector3(0, 0, 1);
        _capsuleCollider.radius = 1.5f;
        _capsuleCollider.height = 1.0f;
        _capsuleCollider.direction = 2;
    }
}
