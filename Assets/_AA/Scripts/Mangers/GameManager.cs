using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int Timer { get; private set; } = 0;
    private bool _isSlowMotionActive = false;
    private bool _isGamePaused = false;
    private void OnEnable()
    {
        GameEvents.GamePaused += OnGameStateChanged;
        GameEvents.DecreaseTimeScale_EnemyManager += OnDecreaseTimeScale;
        GameEvents.PlayerDied += OnPlayerDeath;

    }

    private void OnDisable()
    {
        GameEvents.GamePaused -= OnGameStateChanged;
        GameEvents.DecreaseTimeScale_EnemyManager -= OnDecreaseTimeScale;
        GameEvents.PlayerDied -= OnPlayerDeath;

    }

    private void OnPlayerDeath()
    {
        OnGameStateChanged(true);
    }


    private void Start()
    {
        StartCoroutine(TimerRoutine());
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
            Timer++;
            GameEvents.SecondPassed?.Invoke(Timer);
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
