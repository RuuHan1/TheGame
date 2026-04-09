using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossSpawner : MonoBehaviour, IInteractable
{
    private Transform _target;
    [SerializeField] private GameObject _Boss;
    private bool _isInfoPanelOpen = false;
    [SerializeField] private InputActionReference _interactAction;
    private bool _bossDefeated = false;
    private void OnEnable()
    {
        GameEvents.PlayerPosition += SetPlayerTarget;
        GameEvents.SpawnBoss_GameManager += OnSpawnBoss;
        _interactAction.action.performed += Interact;
        GameEvents.BossDefeated_Boss += OnBossDefeated;
    }

    private void OnDisable()
    {
        GameEvents.PlayerPosition -= SetPlayerTarget;
        GameEvents.SpawnBoss_GameManager -= OnSpawnBoss;
        _interactAction.action.performed -= Interact;
        GameEvents.BossDefeated_Boss += OnBossDefeated;
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
        if (_bossDefeated)
        {
            GameEvents.PopUpInfoPanel?.Invoke("Boss defeated! For upstair.", _isInfoPanelOpen);
            return;
        }
        else
        {
            GameEvents.PopUpInfoPanel?.Invoke("Spawn boss", _isInfoPanelOpen);

        }

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

    public void Interact(InputAction.CallbackContext context)
    {
        if (_isInfoPanelOpen && !_bossDefeated)
        {
            OnSpawnBoss();
        }
        if (_bossDefeated)
        {
            GameEvents.GameFinished_BossSpawner?.Invoke();
            GameEvents.GamePaused?.Invoke(true);
        }
    }
    private void OnBossDefeated()
    {
        // Boss yenildiðinde yapýlacak iþlemler (örneðin, kapýyý açmak)
        _bossDefeated = true;
        Debug.Log("Boss defeated! Door is now open.");
    }
}
