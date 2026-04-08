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
    private float _spawnRate = 20;

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
        if (_diedEnemyCounter >= _spawnRate)
        {
            SpawnSlotMachine();
            _diedEnemyCounter = 0;
            _spawnRate += 5; // Increase the spawn rate for the next slot machine
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
        var (newCard,spins) = SpinWheel(_cardLibrary.CardViews);
        GameEvents.WhellSpinned_SlothMachineManager?.Invoke(spins,newCard);


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
        float total = _floorData.GetTotalTypeRatio();
        float roll = UnityEngine.Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var ratio in _floorData.TypeRatioList)
        {
            cumulative += ratio.Weight;
            if (roll <= cumulative)
                return ratio.Type;
        }

        return _floorData.TypeRatioList[^1].Type; // fallback
    }



    public (CardViewSO card, CardType[] spins) SpinWheel(IReadOnlyList<CardViewSO> list)
    {
        CardType[] spins = new CardType[3];
        for (int i = 0; i < spins.Length; i++)
            spins[i] = GetCardType();

        CardType targetType = spins[2];
        int matchCount = (spins[0] == spins[2] ? 1 : 0) + (spins[1] == spins[2] ? 1 : 0);
        CardRarity targetRarity = matchCount switch
        {
            2 => CardRarity.Rare,
            1 => CardRarity.Uncommon,
            _ => CardRarity.Common
        };

        var validCards = list
            .Where(x => x.CardData.CardType == targetType && x.CardData.CardRarity == targetRarity)
            .ToList();

        if (validCards.Count > 0)
        {
            CardViewSO selected = validCards[UnityEngine.Random.Range(0, validCards.Count)];
            return (selected, spins); 
        }

        var fallback = list
            .Where(x => x.CardData.CardType == targetType)
            .OrderByDescending(x => x.CardData.CardRarity)
            .FirstOrDefault();

        return (fallback, spins);
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
