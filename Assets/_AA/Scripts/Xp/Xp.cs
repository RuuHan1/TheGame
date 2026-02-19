using UnityEngine;

public class Xp : MonoBehaviour, ICollectable
{
    [HideInInspector] public int index;
    [HideInInspector]public XpManager XpManager;
    public void Collect(PlayerStats player)
    {
        XpManager.XpCollected(index,player);
    }

}
