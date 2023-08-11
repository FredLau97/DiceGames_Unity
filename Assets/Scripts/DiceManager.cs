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
    
    [SerializeField] private int numberOfDie;
    [SerializeField] private Dice[] diceArray;
    [SerializeField] private Transform[] dicePositions;
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private int[] diceValues;
    [SerializeField] private TextMeshProUGUI diceSumText;

    [SerializeField] public List<DiceValueCollection> diceValueCollections;

    public delegate void RollDice();
    public delegate void ResetDice();
    public delegate void EnableDiceOutline();
    public delegate void DisableDiceOutline();
    public event RollDice OnRollDice;
    public event ResetDice OnResetDice;
    public event EnableDiceOutline OnEnableDiceOutline;
    public event DisableDiceOutline OnDisableDiceOutline;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        diceArray = new Dice[numberOfDie];
        diceValues = new int[numberOfDie];
        diceValueCollections = new List<DiceValueCollection>();
        rollCount = 0;
        
        for (var i = 0; i < numberOfDie; i++)
        {
            var dice = Instantiate(dicePrefab, dicePositions[i].position, Quaternion.identity, dicePositions[i]);
            dice.Initialize(dicePositions[i].position);
            diceArray[i] = dice;
            diceValueCollections.Add(new DiceValueCollection()
            {
                CollectionValue = i + 1,
                DiceInTrial = new List<int>()
            });
            dice.OnAnnounceRoll += RegisterDiceRoll;
        }
    }

    private void RegisterDiceRoll(int rollvalue)
    {
        diceValues[registeredDiceValues] = rollvalue;
        var collection = diceValueCollections.Find(c => c.CollectionValue == rollvalue);
        collection.DiceInTrial[rollCount]++;
        
        registeredDiceValues++;

        if (registeredDiceValues < numberOfDie) return;

        OnEnableDiceOutline?.Invoke();
        var rollSum = GetSumOfRoll();
        rollCount++;
        diceSumText.SetText($"{rollSum}");
    }

    private int GetSumOfRoll()
    {
        var sum = 0;
        
        foreach (var diceValue in diceValues)
        {
            sum += diceValue;
        }

        return sum;
    }

    public void RollDie()
    {
        for (var i = 0; i < diceValueCollections.Count; i++)
        {
            diceValueCollections[i].DiceInTrial.Add(0);
        }
        
        registeredDiceValues = 0;
        OnRollDice?.Invoke();
    }

    public void ResetDie()
    {
        OnResetDice?.Invoke();
        OnDisableDiceOutline?.Invoke();
        registeredDiceValues = 0;
        Array.Clear(diceValues, 0, numberOfDie);
        diceSumText.SetText("0");
    }
}

[Serializable]
public class DiceValueCollection
{
    public int CollectionValue;
    public List<int> DiceInTrial;
}
