using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "BossActions/WhirlingBlades")]
public class WhirlingBladesAction : BossActionBase
{
    private float _radius = 3f;
    public override void Execute(GameObject weapon, Transform center,System.Action onComplete)
    {
        GameObject pivot = new GameObject("Pivot");
        pivot.transform.position = center.position;
        for (int i = 0; i < 4; i++)
        {
            float angle = 90f - (i * 90f);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                center.position.x + _radius * Mathf.Cos(rad),
                center.position.y + _radius * Mathf.Sin(rad)
            );
            var clone = Object.Instantiate(weapon, pos, Quaternion.identity);
            clone.transform.SetParent(pivot.transform);
            clone.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
        pivot.transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Restart).SetEase(Ease.Linear).OnComplete(() => {
            Object.Destroy(pivot);
            onComplete?.Invoke();
        });

    }

    
}
