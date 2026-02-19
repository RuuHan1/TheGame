using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/New Enemy", order = 1)]
public class EnemyStatsSO : ScriptableObject
{
    public float MaxHealth = 100f;
    public float MoveSpeed = 5f;
    public float Damage = 10f;
    public float XpValue = 5f;
    public GameObject EnemyPrefab;
}
