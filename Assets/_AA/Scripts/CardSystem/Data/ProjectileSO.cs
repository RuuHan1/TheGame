using UnityEngine;

//projectile cardlari base bulletin ozelliklerini tutarlar
public abstract class ProjectileSO : CardSO
{
    public GameObject ProjectilePrefab;
    public float Damage;
    public float Speed;
    public float CastDelay = 0.5f;
    public float Radius = 0;
    public string VFXKey;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        container.ProjectilePrefab = ProjectilePrefab;
        container.Damage = Damage;
        container.Speed = Speed;
        container.CastDelay += CastDelay;
        container.Radius = Radius;
        container.VFXKey = VFXKey;
        if (CastDelay< 0) container.CastDelay = 0;
    }
    
}
