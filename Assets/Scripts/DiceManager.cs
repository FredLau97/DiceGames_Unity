using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiceManager : Singleton<DiceManager>
{
    private int registeredDiceValues;
    private int rollCount;
    private List<IDice> diceList;
    
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform[] dicePositions;

    [field: SerializeField] public int NumberOfDice { get; private set; }

    public event Action OnRollDice;
    public event Action OnResetDice;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        diceList = new List<IDice>();
        rollCount = 0;
        
        for (var i = 0; i < NumberOfDice; i++)
        {
            var dice = Instantiate(dicePrefab, dicePositions[i].position, Quaternion.identity, dicePositions[i]) as IDice;
            dice.Initialize(dicePositions[i].position);
            diceList.Add(dice);
        }
    }

    public void RollDice()
    {
        OnRollDice?.Invoke();
    }

    public void ResetDie()
    {
        OnResetDice?.Invoke();
    }
}
