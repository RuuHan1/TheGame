using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    private List<XpData> xpsOnWorld = new();
    private List<Xp> xpsInnstance = new();
    [SerializeField]private GameObject xpPrefab;
    private void OnEnable()
    {
        GameEvents.EnemyDiedXp += SpawnXp;
    }
    private void OnDisable()
    {
        GameEvents.EnemyDiedXp -= SpawnXp;
    }
    private void SpawnXp(Vector3 vector, float arg2)
    {
        GameObject newXp = LeanPool.Spawn(xpPrefab, vector, Quaternion.identity);
        XpData newXpData = new XpData
        {
            position = vector,
            value = arg2,
            isCollected = false
        };
        xpsOnWorld.Add(newXpData);
        xpsInnstance.Add(newXp.GetComponent<Xp>());
        xpsInnstance[^1].index = xpsOnWorld.Count - 1;
        xpsInnstance[^1].XpManager = this;

    }

    public void XpCollected(int index,PlayerStats player)
    {
        XpData data = xpsOnWorld[index];
        if (data.isCollected) return;
        XpData xpData = xpsOnWorld[index];
        xpData.isCollected = true;
        GameEvents.XpCollected?.Invoke(xpData.value);
        player.CollectXp(xpData.value);
        LeanPool.Despawn(xpsInnstance[index].gameObject);
    }

}
