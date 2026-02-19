using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.GameStatesChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.GameStatesChanged -= OnGameStateChanged;
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
}
