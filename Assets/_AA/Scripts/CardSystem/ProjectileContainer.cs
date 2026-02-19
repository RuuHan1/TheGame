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
    public float Lifetime = 5;
    public float CastDelay = 0;
    public bool IsTriggered = false;
    public List<ProjectileContainer> OnHitPayloads = new List<ProjectileContainer>();
    //kac kere castlencek
    public int CastCount = 1;
    public int SplitCount = 0;
    public float DamageMultiplier = 1;
    public float SpeedMultiplier = 1;
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
        Lifetime = 5;
        CastDelay = 0;
        CastCount = 1;
        DamageMultiplier = 1;
        SpeedMultiplier = 1;
        IsTriggered = false;
        SplitCount = 0;
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
        newContainer.IsTriggered = IsTriggered;
        newContainer.SplitCount = SplitCount;
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
