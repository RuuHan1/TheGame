using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float Timer { get; private set; } = 0f;
    private bool _isSlowMotionActive = false;
    private bool _isGamePaused = false;
    private bool _isBossSpawned = false;
    private void OnEnable()
    {
        GameEvents.GamePaused += OnGameStateChanged;
        GameEvents.DecreaseTimeScale_EnemyManager += OnDecreaseTimeScale;
        GameEvents.NewRunClicked_UIManager += OnNewRunClicked;

    }


    private void Start()
    {
        StartCoroutine(TimerRoutine());
    }
    private void OnDisable()
    {
        GameEvents.GamePaused -= OnGameStateChanged;
        GameEvents.DecreaseTimeScale_EnemyManager -= OnDecreaseTimeScale;
        GameEvents.NewRunClicked_UIManager -= OnNewRunClicked;
    }
    private void Update()
    {
        Timer += Time.deltaTime;
    }
    private void OnDecreaseTimeScale()
    {
        if (_isSlowMotionActive) return;
        StartCoroutine(SlowMotionRoutine());
    }
    private void OnGameStateChanged(bool isPaused)
    {
        _isGamePaused = isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = _isSlowMotionActive ? 0.7f : 1f;
        }
    }
    private IEnumerator TimerRoutine()
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1f);
        while (true)
        {
            yield return oneSecond;
            GameEvents.SecondPassed?.Invoke();
        }
    }

    private IEnumerator SlowMotionRoutine()
    {
        if (_isGamePaused)
        {
            yield break;
        }
        Time.timeScale = 0.7f;
        yield return new WaitForSeconds(0.3f);
        if (_isGamePaused)
        {
            Time.timeScale = 0f;
            yield break;
        }
        Time.timeScale = 1f;
    }
    private void OnNewRunClicked()
    {
        SceneManager.LoadScene(0);
    }
}
