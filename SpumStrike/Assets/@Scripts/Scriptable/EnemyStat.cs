using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyStatus")]
public class EnemyStat : ScriptableObject
{
    public float Speed = 1.5f;     // 이동속도
    public float MaxHp = 20;       // 최대 체력
    public float Atk = 1;          // 공격력
    public float Exp = 10;         // 드랍 경험치
    public int Size = 1;           // 크기
}
