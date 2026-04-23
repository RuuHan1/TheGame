using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static bool _isInitialized;
    private ISceneState _currentState;
    private void Awake()
    {
        if (_isInitialized)
        {
            Destroy(gameObject);
            return;
        }
        _isInitialized = true;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        _currentState?.Tick();
    }
    public void ChangeState(ISceneState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.Enter();
    }
}
