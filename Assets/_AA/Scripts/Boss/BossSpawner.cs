using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private GameObject _Boss;
    private void OnEnable()
    {
        GameEvents.PlayerPosition += SetPlayerTarget;
        GameEvents.SpawnBoss_GameManager += OnSpawnBoss;
    }

    private void OnDisable()
    {
        GameEvents.PlayerPosition -= SetPlayerTarget;
        GameEvents.SpawnBoss_GameManager -= OnSpawnBoss;
    }

    private void SetPlayerTarget(Transform transform)
    {
        _target = transform;
    }

    private void OnSpawnBoss()
    {
        GameObject newBoss = Instantiate(_Boss, transform.position, Quaternion.identity);
        newBoss.GetComponent<Boss>().SetTarget(_target);
    }

}
