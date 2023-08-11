using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class DiceVisuals : MonoBehaviour, IOutline
{
    private DiceManager diceManager;
    [field: SerializeField] public Outline Outline { get; private set; }
    [field: SerializeField] public Color OutlineColor { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float OutlineWidth { get; private set; }

    private void OnEnable()
    {
        diceManager.OnResetDice += DisableOutline;
    }
    
    private void OnDisable()
    {
        diceManager.OnResetDice -= DisableOutline;
    }

    private void Awake()
    {
        diceManager = DiceManager.Instance;
        Outline = GetComponent<Outline>();
        Outline.OutlineColor = OutlineColor;
        Outline.OutlineWidth = OutlineWidth;
        Outline.enabled = false;
    }

    public void ToggleOutline()
    {
        Outline.enabled = !Outline.enabled;
    }

    private void DisableOutline() => Outline.enabled = false;
}
