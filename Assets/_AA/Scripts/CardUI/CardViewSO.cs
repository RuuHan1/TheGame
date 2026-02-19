using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "CardView",menuName = "newView")]
public class CardViewSO : ScriptableObject
{
    [SerializeField] public CardSO CardData;
    [TextArea] public string CardName;
    public string CardDescription;
    [SerializeField] public Image CardBgImage;
    [SerializeField] public Image CardFgImage;

}
