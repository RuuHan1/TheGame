using UnityEngine;

//projectile cardlari base bulletin ozelliklerini tutarlar
public abstract class ProjectileSO : CardSO
{
    public GameObject ProjectilePrefab;
    public float Damage;
    public float Speed;
    public float CastDelay = 0.5f;


    public override void UpdateContainer(ProjectileContainer container, WeaponSO weapon)
    {
        container.ProjectilePrefab = ProjectilePrefab;
        container.Damage = Damage;
        container.Speed = Speed;
        container.CastDelay += CastDelay;
        if (CastDelay< 0) container.CastDelay = 0;
    }
    
}
