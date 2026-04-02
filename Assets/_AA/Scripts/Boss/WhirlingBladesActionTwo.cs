using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BossActions/WhirlingBladesTwo")]
public class WhirlingBladesActionTwo : BossActionBase
{
    private float _radius = 3f;
    private float _duration = 5f;
    //private float _moveDistance = 10f;
    private float _force = 10f;
    public override void Execute(GameObject weapon, Transform center, System.Action onComplete)
    {
        GameObject pivot = new GameObject("Pivot");
        pivot.transform.position = center.position;
        List<GameObject> clones = new();
        for (int i = 0; i < 8; i++)
        {
            float angle = i * (360f / 8);
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
        foreach (GameObject clone in clones)
        {
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                clone.transform.SetParent(null);
                Vector3 direction = (clone.transform.position - center.position).normalized;
                rb.AddForce(direction * _force,ForceMode2D.Impulse);
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    clone.transform.SetParent(pivot.transform);
                });
            }
        }
        DOVirtual.DelayedCall(1.5f, () =>
        {
            pivot.transform.DORotate(new Vector3(0, 0, 240), _duration, RotateMode.FastBeyond360).SetLoops(1, LoopType.Restart).SetEase(Ease.Linear).OnComplete(() => {
                Object.Destroy(pivot);
                onComplete?.Invoke();
            });
        });

    }


}

