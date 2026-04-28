using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : UIPanel
{
    [SerializeField] private Image _loadingFillImage;

    private void OnEnable()
    {
        GameEvents.LoadingProgressUpdated_LoadingState += OnLoadingProgressUpdated;
    }

    

    private void OnDisable()
    {
        GameEvents.LoadingProgressUpdated_LoadingState -= OnLoadingProgressUpdated;
    }
    private void OnDestroy()
    {
        GameEvents.LoadingProgressUpdated_LoadingState -= OnLoadingProgressUpdated;
    }
    private void OnLoadingProgressUpdated(float progress)
    {
        if (_loadingFillImage != null)
        {
            _loadingFillImage.fillAmount = progress;
        }
    }
}
