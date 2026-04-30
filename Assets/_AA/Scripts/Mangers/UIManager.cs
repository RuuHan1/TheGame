using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{

    
    [Header("Info Panel")]
    [SerializeField] private TextMeshProUGUI _infoPanelText;
    [SerializeField] private GameObject _infoTextBg;
    [SerializeField] private GameObject _endPanel;

    private Stack<UIPanel> _activePanels = new();
    [Header("References")]
    [Tooltip("Hiçbir panel açýk deðilken ESC'ye basýldýðýnda açýlacak varsayýlan menü.")]
    [SerializeField] private UIPanel _pauseMenuPanel;

    private void Start()
    {
    }
    private void OnEnable()
    {
        
        GameEvents.ToggleInfoPanel += OnPopUpInfoPanel;
        GameEvents.GameFinished_BossSpawner += OnGameFinished;
        GameEvents.AddPanelToStack += OnOpenPanel;
        GameEvents.EscapePressed += OnCancelPerformed;
    }

    private void OnDisable()
    {
        GameEvents.ToggleInfoPanel -= OnPopUpInfoPanel;
        GameEvents.GameFinished_BossSpawner -= OnGameFinished;
        GameEvents.AddPanelToStack -= OnOpenPanel;
        GameEvents.EscapePressed -= OnCancelPerformed;
    }

    private void OnPopUpInfoPanel(string info, bool state)
    {
        _infoTextBg.SetActive(state);
        _infoPanelText.text = info + "(Space)";
        // Örneðin, bir UI panel prefab'ý kullanarak:
        // GameObject popUpPanel = Instantiate(popUpPanelPrefab, uiCanvas.transform);
        // popUpPanel.GetComponent<PopUpPanel>().Initialize(info);
        // popUpPanel.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
    }
    private void OnGameFinished()
    {
        _endPanel.SetActive(true);
    }
    public void OnNewRunButtonClicked()
    {
        GameEvents.NewRunClicked_UIManager?.Invoke();
    }

    private void OnCancelPerformed()
    {
        if (_activePanels.Count > 0)
        {
            CloseTopPanel();
        }
        else
        {
            if (_pauseMenuPanel != null)
            {
                OnOpenPanel(_pauseMenuPanel);
            }

        }
    }

    public void OnOpenPanel(UIPanel newPanel)
    {
        if (_activePanels.Count > 0 && _activePanels.Peek() == newPanel) return;

        newPanel.ShowPanel();
        _activePanels.Push(newPanel);
        UpdatePauseState();
    }

    public void CloseTopPanel()
    {
        if (_activePanels.Count > 0)
        {
            UIPanel topPanel = _activePanels.Pop();
            topPanel.HidePanel();
            UpdatePauseState();
        }
    }
    private void UpdatePauseState()
    {
        bool isPaused = _activePanels?.Count > 0;
        GameEvents.GamePaused?.Invoke(isPaused);
    }
}
