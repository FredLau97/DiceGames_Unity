using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour, IDice
{
    private DiceControls controls;
    private DiceVisuals visuals;

    private DiceManager diceManager;
    

    private void OnEnable()
    {
        diceManager.OnRollDice += Roll;
    }
    
    private void OnDisable()
    {
        diceManager.OnRollDice -= Roll;
    }

    private void Awake()
    {
        diceManager = DiceManager.Instance;
        controls = GetComponent<DiceControls>();
        visuals = GetComponent<DiceVisuals>();
    }

    public void Initialize(Vector3 position)
    {
        controls.SetStartPosition(position);
    }

    public void Roll()
    {
        controls.Roll();
    }

    public int GetRollValue()
    {
        float yDot, xDot, zDot;
        int rollValue = -1;

        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
        xDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
        zDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

        rollValue = yDot switch
        {
            1 => 3,
            -1 => 4,
            _ => rollValue
        };
        rollValue = xDot switch
        {
            1 => 5,
            -1 => 2,
            _ => rollValue
        };
        rollValue = zDot switch
        {
            1 => 6,
            -1 => 1,
            _ => rollValue
        };

        return rollValue;
    }

    public void Click()
    {
        visuals.ToggleOutline();
    }
}