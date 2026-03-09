using System.Collections.Generic;
using UnityEngine;

public class ProjectileContainer
{
    public GameObject ProjectilePrefab;
    public float Damage = 1;
    public float Radius = 0;
    public float Speed = 1;
    //0 a yaklastikca mermi duz gider
    public float Spread = 0;
    public float Lifetime = 5f;
    public float CastDelay = 0;
    public bool IsTriggered = false;
    public List<ProjectileContainer> OnHitPayloads = new List<ProjectileContainer>();
    //kac kere castlencek
    public int CastCount = 1;
    public int FragmentCount = 0;
    public int SplitCount = 0;
    public float DamageMultiplier = 1;
    public float SpeedMultiplier = 1;
    public float AirSplitTime = 0;
    //
    public float RotationSpeed = 0;
    public bool IsChildProjectile = false;
    public float SplitSpreadAngle = 50f;
    public string VFXKey;
    public float KnockbackForce = 0;
    public float SlowMultiplier = 1f;
    public float SlowDuration = 0f;
    public bool IsHoming = false;
    public float HomingStrength = 0;
    public float HomingRange = 0f;
    public void SetMultipliers()
    {
        Damage *= DamageMultiplier;
        Speed *= SpeedMultiplier;
    }

    public void ResetConteiner()
    {
        ProjectilePrefab = null;
        Damage = 1;
        Radius = 0;
        Speed = 1;
        Spread = 0;
        Lifetime = 5f;
        CastDelay = 0;
        CastCount = 1;
        DamageMultiplier = 1;
        SpeedMultiplier = 1;
        IsTriggered = false;
        IsChildProjectile = false;
        FragmentCount = 0;
        AirSplitTime = 0;
        SplitCount = 0;
        RotationSpeed = 0;
        SplitSpreadAngle = 50f;
        VFXKey = null;
        KnockbackForce = 0;
        SlowMultiplier = 1f;
        SlowDuration = 0f;
        IsHoming = false;
        HomingStrength = 0;
        HomingRange = 0f;
        OnHitPayloads.Clear();
    }

    public ProjectileContainer CopyContainer()
    {
        ProjectileContainer newContainer = new ProjectileContainer();
        newContainer.Damage = Damage;
        newContainer.Speed = Speed;
        newContainer.Radius = Radius;
        newContainer.Spread = Spread;
        newContainer.Lifetime = Lifetime;
        newContainer.CastDelay = CastDelay;
        newContainer.CastCount = CastCount;
        newContainer.DamageMultiplier = DamageMultiplier;
        newContainer.SpeedMultiplier = SpeedMultiplier;
        newContainer.ProjectilePrefab = ProjectilePrefab;
        newContainer.SplitCount = SplitCount;
        newContainer.AirSplitTime = AirSplitTime;
        newContainer.RotationSpeed = RotationSpeed;
        newContainer.IsTriggered = IsTriggered;
        newContainer.FragmentCount = FragmentCount;
        newContainer.IsChildProjectile = IsChildProjectile;
        newContainer.SplitSpreadAngle = SplitSpreadAngle;
        newContainer.VFXKey = VFXKey;
        newContainer.KnockbackForce = KnockbackForce;
        newContainer.SlowMultiplier = SlowMultiplier;
        newContainer.SlowDuration = SlowDuration;
        newContainer.IsHoming = IsHoming;
        newContainer.HomingStrength = HomingStrength;
        newContainer.HomingRange = HomingRange;
        if (OnHitPayloads != null)
        {
            foreach (var payload in this.OnHitPayloads)
            {
                newContainer.OnHitPayloads.Add(payload.CopyContainer());
            }
        }
        return newContainer;
    }


}
