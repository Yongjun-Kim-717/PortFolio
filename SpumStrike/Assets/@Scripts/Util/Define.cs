using System.Collections.Generic;
using UnityEngine;

public class Define
{
    #region Path
    public const string PlayerPath = "Characters/DragonKnight";
    public const string SkeletonPath = "Characters/Skeleton";
    public const string BossPath = "Characters/Skeleton_Boss";
    public const string DeathEffectPath = "Characters/Effect/DeathEffect";
    public const string SpawnEffectPath = "Characters/Effect/SpawnEffect";
    public const string BossSkillEffectPath = "Characters/Effect/DarkBoomEffect";
    public const string BossSkillTargetSignPath = "Objects/TargetSign";
    public const string DamageTextPath = "Objects/DamageText";
    public const string ExpPath = "Objects/Exp";
    public const string ChestPath = "Objects/Chest";
    public const string ElementalOrbPath = "Objects/ElementalOrbs";
    public const string LevelUpEffectPath = "Characters/Effect/LevelUpEffect";

    public const string HpBarPath = "UI/HPBar";

    public const string WeaponPath = "Status/Weapons";
    public const string ItemPath = "Status/Items";
    public const string SkeletonStatusPath = "Status/Enemies/Skeleton";
    public const string BossStatusPath = "Status/Enemies/Boss";
    public const string StageInfoPath = "Status/StageInfo";
    #endregion

    #region Tag
    public const string EnemyTag = "Enemy";
    public const string PlayerTag = "Player";
    public const string ExpTag = "EXP";
    public const string GroundTag = "Ground";
    public const string GetRangeTag = "GetRange";
    public const string SwordAuraTag = "SwordAura";
    #endregion

    #region Animator
    public static string IsMove = "1_Move";
    public static string AttackTrigger = "2_Attack";
    public static string DamagedTrigger = "3_Damaged";
    public static string DeathTrigger = "4_Death";
    public static string IsDeath = "isDeath";
    #endregion

    #region LevelUpStatus
    public const float AtkStatus = 10;
    public const float MaxHpStatus = 10;
    public const float DefStatus = 10;
    public const int MaxExpStatus = 10;
    #endregion

    #region Weapons
    public enum Weapons
    {
        PlaneSword = 0,
        FireSword = 1,
        IceSword = 2,
        ElectricSword = 3
    }
    #endregion

    #region Value
    public enum ItemValue
    {
        Common = 0,
        Uncommon,
        Rare,
        Epic,
        Legendary = 4,
    }
    #endregion

    #region Color
    public static Dictionary<ItemValue, Color> ItemValueColors = new Dictionary<ItemValue, Color>()
    {
        { ItemValue.Common, Util.HexCodeToColor("#FFFFFF")},
        { ItemValue.Uncommon, Util.HexCodeToColor("#00FF00") },
        { ItemValue.Rare, Util.HexCodeToColor("#0064FF") },
        { ItemValue.Epic, Util.HexCodeToColor("#FF00FF") },
        { ItemValue.Legendary, Util.HexCodeToColor("#FF0A00") }
    };
    #endregion

    #region StatOption
    public enum Option
    {
        Atk = 0,
        MaxHp = 1,
        Defense = 2,
        Speed = 3,
        AttackSpeed = 4,
        AttackRange = 5,
    }
    #endregion

    #region StageType
    public enum StageType
    {
        Normal = 0,
        Reward,
        Special
    }
    #endregion
}