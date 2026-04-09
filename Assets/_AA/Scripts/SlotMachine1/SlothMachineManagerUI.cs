using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlothMachineManagerUI : MonoBehaviour
{
    [Header("Wheels")]
    [Tooltip("Sahnedeki 3 Whell scriptini buraya sürükleyin.")]
    [SerializeField] private List<Whell> _wheels;

    [Header("UI References")]
    [SerializeField] private GameObject _SlothMachinePanel;
    [SerializeField] private GameObject _CardPrefab;
    [SerializeField] private InputActionReference _interactAction;
    private bool _isSpinning = false;
    private void OnEnable()
    {
        _interactAction.action.Enable();
        _interactAction.action.performed += OnCloseAnimationPanel;
        GameEvents.WhellSpinned_SlothMachineManager += HandleSpinEvent;
    }

    private void OnDisable()
    {
        GameEvents.WhellSpinned_SlothMachineManager -= HandleSpinEvent;
        _interactAction.action.performed -= OnCloseAnimationPanel;
    }

    private void HandleSpinEvent(CardType[] targets, CardViewSO viewSO)
    {
        GameEvents.GamePaused?.Invoke(true); // Oyun durumunu "duraklatýlmýþ" olarak deðiþtir
        StartCoroutine(SpinAllWheelsRoutine(targets, viewSO));
    }

    private IEnumerator SpinAllWheelsRoutine(CardType[] targets, CardViewSO viewSO)
    {
        _isSpinning = true;
        _SlothMachinePanel.SetActive(true);
        _CardPrefab.SetActive(false);

        // 1. Tüm çarklarý ayný anda döndürmeye baþla
        foreach (var wheel in _wheels)
        {
            wheel.Spin();
        }

        // Baþlangýç beklemesi
        yield return new WaitForSecondsRealtime(1.5f);

        // --- SIRALI DURDURMA MANTIÐI ---

        // ADIM 1: 1. Wheel (Indeks 0) -> Listenin 1. elemaný (targets[0])
        _wheels[0].StopAt(targets[0]);
        yield return new WaitForSecondsRealtime(0.7f); // Çarklar arasý gecikme

        // ADIM 2: 3. Wheel (Indeks 2) -> Listenin 2. elemaný (targets[1])
        _wheels[2].StopAt(targets[1]);
        yield return new WaitForSecondsRealtime(0.7f);

        // ADIM 3: 2. Wheel (Indeks 1) -> Listenin 3. elemaný (targets[2])
        _wheels[1].StopAt(targets[2]);

        // Tüm çarklarýn tamamen durmasý için son bir bekleme
        yield return new WaitForSecondsRealtime(1.0f);

        // Sonuç kartýný göster
        _CardPrefab.SetActive(true);
        CardVisualizer visualizer = _CardPrefab.GetComponent<CardVisualizer>();
        if (visualizer != null)
        {
            visualizer.Setup(viewSO);
        }

        _isSpinning = false;
    }
    private void OnCloseAnimationPanel(InputAction.CallbackContext context)
    {
        if (_isSpinning)
        {
            return;
        }
        GameEvents.GamePaused?.Invoke(false);
        _SlothMachinePanel.SetActive(false);


    }
}
