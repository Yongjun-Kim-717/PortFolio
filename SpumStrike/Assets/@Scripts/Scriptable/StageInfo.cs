using UnityEngine;

[CreateAssetMenu(menuName = "Game/StageInfo")]
public class StageInfo : ScriptableObject
{
    public Define.StageType StageType;  // 스테이지 종류(일반, 보상, 특수)
    public int ClearScore;              // 스테이지 클리어 점수
    public int EnemyCount;              // 적 카운트
    public int EnemyLevel;              // 적 레벨
    public float SpawnInterval;         // 스폰 간격
}
