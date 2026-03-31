using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private GameObject _Boss;
    private void OnEnable()
    {
        GameEvents.PlayerPosition += SetPlayerTarget;
    }

    private void OnDisable()
    {
        GameEvents.PlayerPosition -= SetPlayerTarget;
    }
    private void Start()
    {
        SpawnBoss();
    }
    private void SetPlayerTarget(Transform transform)
    {
        _target = transform;
    }

    private void SpawnBoss()
    {
        GameObject newBoss = Instantiate(_Boss, transform.position, Quaternion.identity);
        newBoss.GetComponent<Boss>().SetTarget(_target);
    }

}
