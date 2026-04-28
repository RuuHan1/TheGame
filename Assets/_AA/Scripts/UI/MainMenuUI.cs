using UnityEngine;

public class MainMenuUI : UIPanel
{
    public void OnPlayClick()
    {
        int diffuculty = 1;
        GameEvents.PlayButtonClicked?.Invoke(diffuculty);
    }
}
