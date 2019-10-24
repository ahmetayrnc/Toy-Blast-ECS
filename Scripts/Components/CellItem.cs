using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class CellItem : IComponent
{
}

[Game]
public class CellItemIdComponent : IComponent
{
    [PrimaryEntityIndex] public Tuple<int, int> Value;
}

[Game]
public class CellItemTypeComponent : IComponent
{
    public CellItemType Value;
}

[Game]
public class CellItemReservation : IComponent
{
    [PrimaryEntityIndex] public int ReservationId;
    public CellItemTypeInBoard CellItemType;
}

[Game]
public class CellItemReservationCompletedComponent : IComponent
{
}