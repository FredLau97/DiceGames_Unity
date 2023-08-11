using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiceManager : Singleton<DiceManager>
{
    private List<IDice> allDiceList;
    private List<IDice> readableDiceList;
    
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform[] dicePositions;

    [field: SerializeField] public int NumberOfDice { get; private set; }

    public event Action OnRollDice;
    public event Action OnResetDice;
    public event Action<List<IDice>> OnAllDiceRolled;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        allDiceList = new List<IDice>();
        readableDiceList = new List<IDice>();

        for (var i = 0; i < NumberOfDice; i++)
        {
            var dice = Instantiate(dicePrefab, dicePositions[i].position, Quaternion.identity, dicePositions[i]) as IDice;
            dice.Initialize(dicePositions[i].position);
            ((Dice)dice).OnDiceReadyToRead += RegisterDiceAsReadyToRead;
            allDiceList.Add(dice);
        }
    }

    private void RegisterDiceAsReadyToRead(IDice dice)
    {
        if (readableDiceList.Contains(dice))
        {
            readableDiceList[readableDiceList.IndexOf(dice)] = dice;
        }
        else
        {
            readableDiceList.Add(dice);
        }

        if (readableDiceList.Count != NumberOfDice) return;
        
        OnAllDiceRolled?.Invoke(readableDiceList);
        foreach (var readableDice in readableDiceList)
        {
            Debug.Log(readableDice.GetRollValue());
        }
    }

    public void RollDice()
    {
        OnRollDice?.Invoke();
    }

    public void ResetDie()
    {
        readableDiceList.Clear();
        OnResetDice?.Invoke();
    }
}
