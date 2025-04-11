using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// * ObjectManager 스크립트
//- 멤버 변수
// PlayerController 변수 (프로퍼티)
// 프리팹 리소스 로드 및 저장 변수
// Enemies 프로퍼티 (공격 대상 확인 여부에 사용)
// 무기(scriptable object)를 로드 후 장착 및 설정
public class ObjectManager : Singleton<ObjectManager>
{
    #region Event
    public event Action<int> OnMonsterCountChanged;
    #endregion

    #region PlayerObj
    private PlayerController _player;
    public PlayerController Player { get { return _player; } }
    #endregion

    #region EnemyObj
    // 공격 대상 탐색을 위한 자료구조
    public HashSet<BaseController> Enemies { get; set; } = new HashSet<BaseController>();

    static private int _monsterCount = 0;
    public int MonsterCount
    {
        get { return _monsterCount; }
        set
        {
            _monsterCount = value;
            OnMonsterCountChanged?.Invoke(_monsterCount);
        }
    }
    #endregion

    #region Resources
    private GameObject _playerResource;
    private GameObject _skeletonResource;
    private GameObject _bossResource;
    private GameObject _deathResource;
    private GameObject _spawnResource;
    private GameObject _skillBossResource;
    private GameObject _skillBossTargetSignResource;
    private GameObject _damageTextResource;
    private GameObject _expResource;
    private GameObject _hpBarResource;
    private GameObject _chestResource;
    private GameObject _levelupResource;

    private Dictionary<Define.Weapons, Weapon> _weaponDictionary = new Dictionary<Define.Weapons, Weapon>();

    public List<GameObject> Orbs = new List<GameObject>();
    public List<Item> Items = new List<Item>();
    public List<StageInfo> StageInfos = new List<StageInfo>();

    public void ResourceLoad()
    {
        _monsterCount = 0;
        _playerResource = Resources.Load<GameObject>(Define.PlayerPath);
        _skeletonResource = Resources.Load<GameObject>(Define.SkeletonPath);
        _bossResource = Resources.Load<GameObject>(Define.BossPath);
        _deathResource = Resources.Load<GameObject>(Define.DeathEffectPath);
        _spawnResource = Resources.Load<GameObject>(Define.SpawnEffectPath);
        _skillBossResource = Resources.Load<GameObject>(Define.BossSkillEffectPath);
        _skillBossTargetSignResource = Resources.Load<GameObject>(Define.BossSkillTargetSignPath);
        _damageTextResource = Resources.Load<GameObject>(Define.DamageTextPath);
        _expResource = Resources.Load<GameObject>(Define.ExpPath);
        _hpBarResource = Resources.Load<GameObject>(Define.HpBarPath);
        _chestResource = Resources.Load<GameObject>(Define.ChestPath);
        _levelupResource = Resources.Load<GameObject>(Define.LevelUpEffectPath);

        ElementalOrbResourceLoad();

        WeaponResourceLoad();
        
        UpgradeItemResourceLoad();

        StageInfoResourceLoad();    
    }

    private void ResourceClear()
    {
        _playerResource = null;
        _skeletonResource = null;
        _bossResource = null;
        _deathResource = null;
        _spawnResource = null;
        _skillBossResource = null;
        _skillBossTargetSignResource = null;
        _damageTextResource = null;
        _expResource = null;
        _hpBarResource = null;
        _chestResource = null;
        _levelupResource = null;

        Enemies.Clear();
        _player = null;
        _weaponDictionary.Clear();
        Orbs.Clear();
        Items.Clear();
        StageInfos.Clear();

        // 참조되지 않는 유니티 리소스 해제(Unused 플래그 설정)
        Resources.UnloadUnusedAssets();
        // 가비지 컬렉터 실행(메모리 정리)
        System.GC.Collect();
    }

    // * 무기 리소스 로드 함수
    //- Scriptable Object로 만들어진 Weapon을 모두 Load 후, 반복문으로 Dictionary에 해당 key값에 맞는 무기 저장
    //- 무기 key값으로 해당 무기에 접근 가능
    private void WeaponResourceLoad()
    {
        Weapon[] allWeapons = Resources.LoadAll<Weapon>(Define.WeaponPath);

        foreach (Weapon weapon in allWeapons)
        {
            _weaponDictionary[weapon.WeaponType] = weapon;
        }
    }

    // * 엘리멘탈 오브 리소스 로드 함수
    //- 모든 엘리멘탈 오브 리소스를 로드하고 몬스터 사망 시 랜덤 오브를 생성해야함
    private void ElementalOrbResourceLoad()
    {
        GameObject[] allOrbs = Resources.LoadAll<GameObject>(Define.ElementalOrbPath);

        foreach (GameObject orb in allOrbs)
        {
            Orbs.Add(orb);
        }
    }

    // * 업그레이드 아이템 리소스 로드 함수
    //- 모든 아이템 리소스를 로드 및 리스트 저장
    private void UpgradeItemResourceLoad()
    {
        Item[] allItems = Resources.LoadAll<Item>(Define.ItemPath);

        foreach (Item item in allItems)
        {
            Items.Add(item);
        }
    }

    // * 스테이지 정보 리소스 로드 함수
    //- 모든 스테이지 정보 리소스를 로드 및 저장
    private void StageInfoResourceLoad()
    {
        StageInfo[] stageInfos = Resources.LoadAll<StageInfo>(Define.StageInfoPath);

        foreach(StageInfo stageInfo in stageInfos)
        {
            StageInfos.Add(stageInfo);
        }
    }
    #endregion

    #region InitAndClear
    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void Clear()
    {
        base.Clear();
        ResourceClear();
    }
    #endregion

    #region ObjectSpawn
    // 오브젝트 생성 Spawn 함수
    public T Spawn<T>(Vector3 spawnPos) where T : BaseController
    {
        Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            // 플레이어 캐릭터 생성 및 컨트롤러 스크립트 연결 및 플레이어 정보 저장
            GameObject obj = Instantiate(_playerResource, spawnPos, Quaternion.identity);
            PlayerController playerController = obj.GetOrAddComponent<PlayerController>();
            _player = playerController;
            // 플레이어 레벨 업 이펙트 연결
            _player.LevelUpEffect = _levelupResource;
            // 플레이어 기본 무기 장착
            _player.EquipWeapon(_weaponDictionary[Define.Weapons.PlaneSword]);
            // 체력바 생성
            SetHPBar(obj);
            // 카메라 설정
            Camera.main.GetOrAddComponent<CameraController>();
            // 스폰 장소
            Instantiate(_spawnResource, Vector3.zero, Quaternion.identity);
            return playerController as T;
        }
        if (type == typeof(SkeletonController))
        {
            // 스켈레톤 캐릭터 생성 및 컨트롤러 스크립트 연결
            GameObject obj = Instantiate(_skeletonResource, spawnPos, Quaternion.identity);
            SkeletonController skeletonController = obj.GetOrAddComponent<SkeletonController>();
            // 스켈레톤 캐릭터의 컨트롤러 스크립트 멤버 변수인 DeathEffect에 리소스 할당
            skeletonController.DeathEffect = _deathResource;
            // 몬스터 리스트에 저장
            Enemies.Add(skeletonController);
            return skeletonController as T;
        }
        if (type == typeof(BossController))
        {
            // 보스 캐릭터 생성 및 컨트롤러 스크립트 연결
            GameObject obj = Instantiate(_bossResource, spawnPos, Quaternion.identity);
            BossController bossController = obj.GetOrAddComponent<BossController>();
            // 보스 캐릭터 리소스에 스킬 스크립트와 타겟 지정 이펙트 스크립트 연결
            _skillBossResource.GetOrAddComponent<BossSkillController>();
            _skillBossTargetSignResource.GetOrAddComponent<TargetSignController>();
            // 보스 캐릭터 컨트롤러 스크립트 멤버 변수에 리소스 할당
            bossController.DeathEffect = _deathResource;
            bossController.BossSkill = _skillBossResource;
            bossController.BossSkillTargetSign = _skillBossTargetSignResource;
            // 몬스터 리스트에 보스 캐릭터 추가 (공격 대상 인식 메서드에서 활용)
            Enemies.Add(bossController);
            return bossController as T;
        }
        if (type == typeof(DamageTextController))
        {
            // 데미지 텍스트 생성 및 스크립트 연결
            GameObject obj = Instantiate(_damageTextResource, spawnPos, Quaternion.identity);
            DamageTextController damageText = obj.GetOrAddComponent<DamageTextController>();
            return damageText as T;
        }
        if (type == typeof(ExpController))
        {
            // 경험치 오브젝트 생성 및 스크립트 연결
            GameObject obj = Instantiate(_expResource, spawnPos, Quaternion.identity);
            ExpController expController = obj.GetOrAddComponent<ExpController>();
            return expController as T;
        }
        if (type == typeof(ElementalOrbController))
        {
            // 랜덤한 Elemental Orb 생성 기능
            int num = UnityEngine.Random.Range(0, Orbs.Count);
            GameObject obj = Instantiate(Orbs[num], spawnPos, Quaternion.identity);
            ElementalOrbController elementalOrb = obj.GetOrAddComponent<ElementalOrbController>();
            return elementalOrb as T;
        }
        if (type == typeof(ChestController))
        {
            GameObject obj = Instantiate(_chestResource, spawnPos, Quaternion.identity);
            ChestController chestController = obj.GetOrAddComponent<ChestController>();
            Enemies.Add(chestController);
            return chestController as T;
        }
        return null;
    }

    // 오브젝트 비활성화 함수 (오브젝트 풀링 대상)
    public void Despawn<T>(T obj) where T : BaseController
    {
        Type type = typeof(T);
        if(Util.IsTypeEnemy(type))
        {
            MonsterCount--;
            Debug.Log("몬스터 사망");
        }
        obj.gameObject.SetActive(false);
    }
    #endregion

    #region SearchObject
    //public GameObject GetNearestTarget(float distance = 20f)
    //{
    //    var targetList = Enemies.Where(enemy => enemy.gameObject.activeSelf).ToList();
    //    // 거리순으로 정렬하여 가장 가까운 적
    //    var target = targetList.OrderBy(enemy => (Player.Center - enemy.transform.position).sqrMagnitude).FirstOrDefault();

    //    if ((target.transform.position - Player.Center).sqrMagnitude > distance)
    //    {
    //        return null;
    //    }
    //    return target.gameObject;
    //}

    // 매개변수 범위 내 적 탐지 메서드(기준 점 플레이어 캐릭터) => 추후에 캐릭터 이외의 요소를 추가할 수도 있으니 매개변수로 게임 오브젝트를 추가해 확장성 높이자
    //public bool IsEnemyInAttackRange(float range)
    //{
    //    var targetList = Enemies.Where(enemy => enemy.gameObject.activeSelf).ToList();
    //    // 거리순으로 정렬하여 가장 가까운 적
    //    var target = targetList.OrderBy(enemy => (Player.Center - enemy.transform.position).sqrMagnitude).FirstOrDefault();
    //    if (target != null)
    //    {
    //        if ((target.transform.position - Player.Center).sqrMagnitude <= range)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;

    //}
    #endregion

    #region HPBar
    // 체력바 생성 함수
    private void SetHPBar(GameObject obj)
    {
        // World Space Canvas 생성 (체력바 생성용) 및 기본 설정
        GameObject hpBarObj = new GameObject("UI_HPBar");
        hpBarObj.transform.SetParent(obj.transform, false);
        Canvas canvas = hpBarObj.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        // HPBar 프리팹 생성 및 기본 설정
        GameObject hpBar = Instantiate(_hpBarResource);
        hpBar.transform.SetParent(canvas.transform, false);
        hpBar.transform.position = _player.transform.position + Vector3.up * 1.1f;

        // HPBar 이벤트 설정을 위해 Manager스크립트에 저장
        WorldSpaceUIManager.Instance.EnsureInitialized();
        WorldSpaceUIManager.Instance.hpBarList.Add(Define.PlayerTag, hpBar);
    }
    #endregion

    // 가중치 랜덤 아이템 반환 메서드
    public Item GetRandomItem()
    {
        float sumOfWeight = 0;

        for(int i=0; i<Items.Count; i++)
        {
            sumOfWeight += Items[i].Weight;
        }

        float randomWeight = UnityEngine.Random.Range(0,sumOfWeight);

        for(int i=0; i<Items.Count; i++)
        {
            if(Items[i].Weight > randomWeight)
                return Items[i];
            randomWeight -= Items[i].Weight;
        }

        return Items[Items.Count-1];
    }
}
