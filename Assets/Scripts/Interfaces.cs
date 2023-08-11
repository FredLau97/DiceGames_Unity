using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    public void Click();
}

public interface IDice : IClickable
{
    public void Initialize(Vector3 startPosition);
    public void Roll();
    public int GetRollValue();
}

public interface IOutline
{
    public Outline Outline { get; }
    public Color OutlineColor { get; }
    public float OutlineWidth { get; }
    public void ToggleOutline();
}
