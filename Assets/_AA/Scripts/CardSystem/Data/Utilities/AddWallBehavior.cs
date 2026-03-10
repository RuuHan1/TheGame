using UnityEngine;

[CreateAssetMenu(fileName = "AddWallBehavior", menuName = "Card System/Utilities/AddWallBehavior")]
public class AddWallBehavior : UtilitySO
{

    [SerializeField] private float _wallStopDelay = 0.3f;
    [SerializeField] private bool _isWall = true;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        container.IsWall = _isWall;
        container.WallStopDelay = _wallStopDelay;
    }
}
