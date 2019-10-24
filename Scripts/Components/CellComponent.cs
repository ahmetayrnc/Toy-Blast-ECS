using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

public enum CellState
{
    Empty,
    Full,
    Invalid,
}

[Game]
public class CellComponent : IComponent
{
    public bool CanLetItemActivate;
    public bool CanLetItemFall;
}

[Game]
public class CellIdComponent : IComponent
{
    [PrimaryEntityIndex] public Tuple<int, int> Value;
}

[Game]
public class CellStateComponent : IComponent
{
    public CellState Value;
}

[Game]
public class CanAcceptFallComponent : IComponent
{
    public bool Value;
    public int Counter;
}

[Game]
public class BottomCellComponent : IComponent
{
}

[Game]
public class HasAccessToBottom : IComponent
{
}