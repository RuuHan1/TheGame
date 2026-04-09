using Lean.Pool;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlotMachine : MonoBehaviour,IInteractable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        collision.TryGetComponent(out PlayerStats playerStats);
        if (playerStats != null)
        {
            Interact(new InputAction.CallbackContext());
        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        GameEvents.SlotMachineTaken?.Invoke();
        LeanPool.Despawn(gameObject);
    }




    //public void Interact()
    //{
    //    CardViewSO card = SpinWheel(_cardManager.AllCardViews);
    //    _cardManager.AddCardToHand(card);
    //}
}
