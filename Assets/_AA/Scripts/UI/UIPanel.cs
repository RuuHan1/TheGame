using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public virtual void ShowPanel()
    {
        gameObject.SetActive(true);
    }
    public virtual void HidePanel()
    {
        gameObject.SetActive(false);
    }
    public virtual void AddPanelToStack()
    {
        GameEvents.AddPanelToStack(this);
    }
}
