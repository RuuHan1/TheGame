using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    private List<XpData> xpsOnWorld = new();
    private List<Xp> xpsInnstance = new();
    [SerializeField]private GameObject xpPrefab;
    [SerializeField] private Transform xpPool;
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
        GameObject newXp = LeanPool.Spawn(xpPrefab, vector, Quaternion.identity,xpPool);
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


    //hatali
    public void XpCollected(int index, PlayerStats player)
    {
        XpData data = xpsOnWorld[index];
        if (data.isCollected) return;
        XpData xpData = xpsOnWorld[index];
        xpData.isCollected = true;
        GameEvents.XpCollected?.Invoke(xpData.value);
        //MoveXp(data, player.transform, index);
        LeanPool.Despawn(xpsInnstance[index].gameObject);
    }
    //public void MoveXp(XpData data, Transform playerTransform, int index)
    //{
    //    if (data.isCollected) return; // çift tetiklenmeyi engelle
    //    data.isCollected = true;

    //    GameObject xpObject = xpsInnstance[index].gameObject;

    //    xpObject.transform.DOKill(); // önceki tween varsa temizle

    //    xpObject.transform.DOMove(playerTransform.position, 0.6f)
    //        .SetEase(Ease.InCubic)
    //        .OnComplete(() =>
    //        {
    //            if (xpObject == null) return; // null guard
    //            GameEvents.XpCollected?.Invoke(data.value);
    //            LeanPool.Despawn(xpObject);
    //        });
    //}


}
