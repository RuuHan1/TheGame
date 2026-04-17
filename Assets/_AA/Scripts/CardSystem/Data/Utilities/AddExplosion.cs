using UnityEngine;
[CreateAssetMenu(fileName = "New Utility", menuName = "Card System/Utility/Add Explosion")]
public class AddExplosion : UtilitySO
{

    [SerializeField] private float explosionRadius = 0;
    [SerializeField] private VFXType VFXKey;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        if(container.Radius == 0)
        {
            container.Radius = explosionRadius;
            container.VFXKey = VFXKey;
        }
        else if (container.Radius > 0)
        {
            container.Radius += explosionRadius/3;
        }


    }
}
