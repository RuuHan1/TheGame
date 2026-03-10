using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Timer { get; private set; } = 0f;
    private bool _isSlowMotionActive = false;
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
        }
        else
        {
            Time.timeScale = 1f;

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
        Time.timeScale = 0.7f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1f;
        _isSlowMotionActive = false;
    }

}
