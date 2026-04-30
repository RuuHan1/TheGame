using System;
using TMPro;
using UnityEngine;

public class TimerUI : UIPanel
{
    [Header("Timer UI")]
    [SerializeField] private TMP_Text _timerText;
    private void OnEnable()
    {
        GameEvents.SecondPassed += OnSecondPassed;
    }
    private void OnDisable()
    {
        GameEvents.SecondPassed -= OnSecondPassed;
    }
    private void OnSecondPassed(int timer)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        string formattedTime = timeSpan.ToString(@"mm\:ss");
        _timerText.text = formattedTime;
    }
}
