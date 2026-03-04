using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/Card Library")]
public class CardLibrarySO : ScriptableObject
{
    [SerializeField] private List<CardViewSO> _cardViews;
    [HideInInspector] public IReadOnlyList<CardViewSO> CardViews => _cardViews;
}
