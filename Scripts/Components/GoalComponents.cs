using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique, Event(EventTarget.Any)]
public class GoalComponent : IComponent
{
    public Dictionary<GoalType, int> Goals;
}

[Game, Unique, Event(EventTarget.Any)]
public class GoalProgressComponent : IComponent
{
    public Dictionary<GoalType, int> Progress;
}

[Game, Unique, Event(EventTarget.Any)]
public class GoalForGeneratorsComponent : IComponent
{
    public Dictionary<GoalType, int> Goals;
}

[Game]
public class GoalDependentComponent : IComponent
{
}