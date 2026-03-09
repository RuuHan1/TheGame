using UnityEngine;
[System.Serializable]
public struct EnemyData
{ 
    public Vector3 position;
    public float speed;
    public float health;
    public float damage;
    public bool isAlive;
    public float XpWorth;
    public float radius;
    public Vector3 velocity;
    public float slowMultiplier;  // 1 = normal, 0.5 = yarý hýz
    public float slowTimer;
}
