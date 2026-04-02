using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BossActions/StellBloom")]
public class StellBloomAction : BossActionBase
{
    private float _radius = 3f;
    private float _duration = 3f;

    private float _force = 20f;
    public override void Execute(GameObject weapon, Transform center, System.Action onComplete)
    {
        GameObject pivot = new GameObject("Pivot");
        pivot.transform.position = center.position;
        List<GameObject> clones = new();
        float dice = Random.Range(0f, 45f);
        for (int i = 0; i < 8; i++)
        {
            float angle = dice  + i * (360f / 8);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                center.position.x + _radius * Mathf.Cos(rad),
                center.position.y + _radius * Mathf.Sin(rad)
            );

            var clone = Object.Instantiate(weapon, pos, Quaternion.identity);
            clone.transform.SetParent(pivot.transform);
            clone.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
            clones.Add(clone);
        }
        foreach (var clone in clones)
        {
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (clone.transform.position - center.position).normalized;
                rb.AddForce(direction * _force, ForceMode2D.Impulse);
                
            }
        }
        DOVirtual.DelayedCall(_duration, () =>
        {
            Object.Destroy(pivot);
            onComplete?.Invoke(); 
        });
    }
}

