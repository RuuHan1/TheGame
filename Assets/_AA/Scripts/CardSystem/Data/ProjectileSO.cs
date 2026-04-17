using UnityEngine;

//projectile cardlari base bulletin ozelliklerini tutarlar
public abstract class ProjectileSO : CardSO
{
    public GameObject ProjectilePrefab;
    public float Damage;
    public float Speed;
    public float CastDelay = 0.5f;
    public float Radius = 0;
    public VFXType VFXKey;
    public SfxType SfxType;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        container.ProjectilePrefab = ProjectilePrefab;
        container.Damage = Damage;
        container.Speed = Speed;
        container.CastDelay += CastDelay;
        container.Radius = Radius;
        container.VFXKey = VFXKey;
        container.SfxType = SfxType;
        if (CastDelay< 0) container.CastDelay = 0;
    }
    
}
