using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Unique, Game, Event(EventTarget.Any)]
public class Board : IComponent
{
    public Vector2Int Size;
    public bool IsFallActive;
    public bool IsTouchActive;
    public bool IsHintActive;
    public int TouchBlockCounter;
    public int FallBlockCounter;
    public int HintBlockCounter;
}

[Game, Unique]
public class IdCounter : IComponent
{
    public int Value;
}

[Game, Unique]
public class ReservationIdCounterComponent : IComponent
{
    public int Value;
}