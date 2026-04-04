using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Timer { get; private set; } = 0f;
    private bool _isSlowMotionActive = false;
    private bool _isGamePaused = false;
    private bool _isBossSpawned = false;
    private void OnEnable()
    {
        GameEvents.GameStatesChanged += OnGameStateChanged;
        GameEvents.DecreaseTimeScale_EnemyManager += OnDecreaseTimeScale;
    }


    private void Start()
    {
        StartCoroutine(TimerRoutine());
    }
    private void OnDisable()
    {
        GameEvents.GameStatesChanged -= OnGameStateChanged;
        GameEvents.DecreaseTimeScale_EnemyManager -= OnDecreaseTimeScale;
    }
    private void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= 300f && !_isBossSpawned)
        {
            _isBossSpawned = true;
            GameEvents.SpawnBoss_GameManager?.Invoke();
        }
    }
    private void OnDecreaseTimeScale()
    {
        if (_isSlowMotionActive) return;
        StartCoroutine(SlowMotionRoutine());
    }
    private void OnGameStateChanged(bool obj)
    {
        if (obj)
        {
            Time.timeScale = 0f;
            _isGamePaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            _isGamePaused = false;
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

}
