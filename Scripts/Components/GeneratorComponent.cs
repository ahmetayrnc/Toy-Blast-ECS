using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

public enum GeneratorType
{
    Item,
    CellItem,
}

public enum GeneratorRadius
{
    All,
    Radius1,
}

public enum GoalEffectType
{
    Increase,
    Decrease,
}

[Game]
public class GeneratorComponent : IComponent
{
    public GeneratorType Type;
}

[Game]
public class ItemToGenerateComponent : IComponent
{
    public ItemTypeInBoard Type;
}

[Game]
public class CellItemToGenerateComponent : IComponent
{
    public CellItemTypeInBoard Type;
}

[Game]
public class GoalToGenerateComponent : IComponent
{
    public GoalType Type;
}

[Game, Event(EventTarget.Self, priority: 1)]
public class GeneratorClosedComponent : IComponent
{
}

[Game]
public class CanBeTargetedByGeneratorComponent : IComponent
{
}

[Game]
public class GoalEffectTypeComponent : IComponent
{
    public GoalEffectType Type;
}

[Game]
public class GeneratorRadiusComponent : IComponent
{
    public GeneratorRadius Value;
}

[Game]
public class GenerationAmountComponent : IComponent
{
    public int Value;
}

[Game]
public class BottomCellConsiderateComponent : IComponent
{
}

[Game]
public class WillGenerateComponent : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class ReservedCellsComponent : IComponent
{
    public List<int> ReservationIds;
}

[Game, Event(EventTarget.Self)]
public class ReservedItemsComponent : IComponent
{
    public List<Tuple<int, int>> Value;
}