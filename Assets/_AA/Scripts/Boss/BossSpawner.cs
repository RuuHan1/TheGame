using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private GameObject _Boss;
    private bool _isInfoPanelOpen = false;
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

    public void ToggleInfoPanel()
    {
        
        GameEvents.PopUpInfoPanel?.Invoke("Spawn boss",_isInfoPanelOpen);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();

        if (player != null && !_isInfoPanelOpen)
        {
            _isInfoPanelOpen = true;
            ToggleInfoPanel();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            _isInfoPanelOpen = false;
            ToggleInfoPanel();
        }
    }

}
