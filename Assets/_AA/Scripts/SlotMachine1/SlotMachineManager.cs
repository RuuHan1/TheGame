using Lean.Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotMachineManager : MonoBehaviour
{
    [SerializeField] private FloorSO _floorData;
    [SerializeField] private CardLibrarySO _cardLibrary;
    [Header("Slot Machine Spawn Settings")]
    [SerializeField] private GameObject _slotMachinePrefab;
    [SerializeField] private float _spawnDelay = 5f;
    [SerializeField] private float _spawnRadius = 10f;
    private float _spawnTimer;
    private float _diedEnemyCounter;

    private Transform _playerTransform;
    private void OnEnable()
    {
        GameEvents.SlotMachineTaken += OnSlotMachineTaken;
        GameEvents.PlayerPosition += OnPlayerTransformAssigned;
        GameEvents.EnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        GameEvents.SlotMachineTaken -= OnSlotMachineTaken;
        GameEvents.PlayerPosition -= OnPlayerTransformAssigned;
        GameEvents.EnemyDied -= OnEnemyDied;
    }
    private void OnEnemyDied(int obj)
    {
        _diedEnemyCounter++;
        if (_diedEnemyCounter >= 15)
        {
            SpawnSlotMachine();
            _diedEnemyCounter = 0;
        }
    }

    private void Start()
    {
        _spawnTimer = _spawnDelay; // Initialize the spawn timer
    }
    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f)
        {
            SpawnSlotMachine();
            _spawnTimer = _spawnDelay; 
        }
    }
    private void OnPlayerTransformAssigned(Transform transform)
    {
        _playerTransform = transform;
    }


    private void OnSlotMachineTaken()
    {
        CardViewSO newCard = SpinWheel(_cardLibrary.CardViews);
        if (newCard != null)
        {
            GameEvents.CardAwarded?.Invoke(newCard);
        }
    }

    private CardRarity GetCardRarity()
    {
        FloorSO floor = _floorData;
        int dice = Random.Range(0, (int)floor.GetTotalRatio() + 1);
        if (dice <= floor.RarityRatioList[0].weight)
        {
            return CardRarity.Common;
        }
        else if (dice <= floor.RarityRatioList[1].weight)
        {
            return CardRarity.Uncommon;
        }
        else if (dice <= floor.RarityRatioList[2].weight)
        {
            return CardRarity.Rare;
        }
        else if (dice <= floor.RarityRatioList[3].weight)
        {
            return CardRarity.Legendary;
        }
        return CardRarity.Common;

    }

    private CardType GetCardType()
    {
        var cardTypes = _floorData.CardTypes;

        if (cardTypes == null || cardTypes.Count == 0)
        {
            Debug.LogError("CardTypes listesi boþ veya null.");
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, cardTypes.Count);

        return cardTypes[randomIndex];
    }



    public CardViewSO SpinWheel(IReadOnlyList<CardViewSO> list)
    {
        CardType targetType = GetCardType();
        CardRarity targetRarity = GetCardRarity();

        var validCardViews = list
            .Where(x => x.CardData.CardType == targetType && x.CardData.CardRarity == targetRarity);
        int count = validCardViews.Count();

        if (count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, count);
            CardViewSO selectedCardView = validCardViews.ElementAt(randomIndex);

            Debug.Log($"Rarity : {selectedCardView.CardData.CardRarity} / Type : {selectedCardView.CardData.CardType}");
            return selectedCardView;
        }

        Debug.LogWarning("Kriterlere uygun kart bulunamadý.");
        return null;
    }

    public Vector2 GetRandomPointInCircle()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
        return (Vector2)_playerTransform.position + randomOffset;
    }
    private void SpawnSlotMachine()
    {
        LeanPool.Spawn(_slotMachinePrefab, GetRandomPointInCircle(), Quaternion.identity,this.transform);
    }
}
