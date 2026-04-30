using System;
using UnityEngine;

public class InfoCanvas : MonoBehaviour
{
    [SerializeField] private UIPanel _informationPanel;

    private void OnEnable()
    {
        GameEvents.PlayerDied += OnPlayerDeath;
    }
    private void OnDisable()
    {
        GameEvents.PlayerDied -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        _informationPanel.ShowPanel();
    }
}
