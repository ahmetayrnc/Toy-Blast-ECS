using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using Color = EntitasBlast.Color;
using EventType = Entitas.CodeGeneration.Attributes.EventType;

[Game]
public class Id : IComponent
{
    [PrimaryEntityIndex] public int Value;
}

[Game, Event(EventTarget.Self)]
public class PositionComponent : IComponent
{
    public Vector2 value;
}

[Game]
public class ViewComponent : IComponent
{
    public IView value;
}

[Game, Event(EventTarget.Self)]
public class HintComponent : IComponent
{
    public HintType Value;
}

[Game, Event(EventTarget.Self)]
public class GridPositionComponent : IComponent
{
    [EntityIndex] public Vector2Int value;
}

[Game]
public class MatchGroupComponent : IComponent
{
    [EntityIndex] public int Id;
    public int Count;
}

[Game, Event(EventTarget.Self)]
public class LayerComponent : IComponent
{
    public int Value;
}

[Game]
public class FallingComponent : IComponent
{
    public Vector3 From;
    public Vector3 Target;
    public float Speed;
}

[Game]
public class ColorComponent : IComponent
{
    public Color Value;
}

public enum Axis
{
    Vertical,
    Horizontal
}

[Game]
public class ItemAxisComponent : IComponent
{
    public Axis Value;
}

public enum MatchType
{
    BlueCube,
    RedCube,
    YellowCube,
    GreenCube,
    Positive,
    Invalid
}

[Game]
public class MatchTypeComponent : IComponent
{
    public MatchType Value;
}

[Game]
public class CanFall : IComponent
{
}

[Game]
public class CanBeActivatedByTouch : IComponent
{
}

[Game]
public class RemoverSensitive : IComponent
{
}

[Game]
public class CanBeActivatedByNearMatch : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class ColorSensitive : IComponent
{
    public Dictionary<Color, int> colors;
}

[Game]
public class TurnSensitive : IComponent
{
}

[Game, Event(EventTarget.Self, EventType.Removed), Event(EventTarget.Self)]
public class CantBeActivated : IComponent
{
}

public enum ActivationReason
{
    Touch,
    Tnt,
    Rotor,
    NearMatch,
    God,
    PuzzlePuzzle,
    Puzzle,
    Bottom
}

[Game]
public class Activation : IComponent
{
    public Queue<ActivationReason> ActivationQueue;
}

[Game]
public class ActiveGodTouch : IComponent
{
}

public enum PuzzleComboType
{
    PuzzleRotor,
    PuzzleTnt
}

[Game]
public class PuzzleCombo : IComponent
{
    public PuzzleComboType ComboType;
}

public enum RotorDirection
{
    Left,
    Right,
    Up,
    Down
}

[Game]
public class ActiveRotor : IComponent
{
    public RotorDirection Direction;
    public Vector2Int LastPassedCell;
}

[Game]
public class ActiveTouch : IComponent
{
}

[Game]
public class RemoverId : IComponent
{
    public int Value;
}

[Unique, Game]
public class RemoverIdCount : IComponent
{
    public int Value;
}

[Game]
public class WillSpawnCellItem : IComponent
{
    public CellItemTypeInBoard Type;
    public int GridX;
    public int GridY;
}

[Game]
public class CanBeActivatedByInnerMatch : IComponent
{
}

[Game]
public class CantBeActivatedByPositiveItem : IComponent
{
}

public class CanStopItemActivation : IComponent
{
}

[Game]
public class CanStopItemFall : IComponent
{
}

[Game, Event(EventTarget.Self, priority: 2)]
public class WillBeDestroyed : IComponent
{
}

[Game, Unique]
public class Turn : IComponent
{
    public int DeadTurnCount;
    public int TurnCounterPrevValue;
    public int TurnCounter;
    public int TurnId;
}

public class Removers : IComponent
{
    public HashSet<int> Set;
}

[Unique]
public class Dirty : IComponent
{
}

[Game]
public class Variant : IComponent
{
    public int Id;
}

[Game]
public class CanBeActivatedByBottom : IComponent
{
}

[Game]
public class ConsecutionSensitive : IComponent
{
    public int ActivatedTurnId;
}

[Game]
public class ColorChanging : IComponent
{
    public Queue<Color> ColorQueue;
}

[Game]
public class LayerTypeChanging : IComponent
{
}

[Game]
public class FallStateChanging : IComponent
{
}

[Game]
public class RemoverSensitivityChanging : IComponent
{
}

[Game]
public class MultiBlock : IComponent
{
}

[Game]
public class InnerMatchItem : IComponent
{
}

[Game]
public class ColorCopying : IComponent
{
}

[Game]
public class SubItem : IComponent
{
}

[Game]
public class DestroyedSoap : IComponent
{
}

[Game, Unique, Event(EventTarget.Any)]
public class LevelComponent : IComponent
{
    public int LevelNo;
}

[Game, Unique]
public class ItemGroupsComponent : IComponent
{
    public Group[] Groups;
}

[Game]
public class CantBeDestroyed : IComponent
{
}

[Game]
public class CollectableGenerator : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class WillExplode : IComponent
{
    public int Count;
}

[Game]
public class PositiveItem : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class Activated : IComponent
{
}

[Game]
public class ColorMatch : IComponent
{
    public int TouchedCubeId;
}

[Game]
public class PositiveItemMatch : IComponent
{
    public int TouchedPositiveItemId;
}

[Game]
public class TntTnt : IComponent
{
    public int Radius;
}

[Game]
public class FakeItem : IComponent
{
    public int RealItemId;
}

[Game]
public class FakeItems : IComponent
{
    public List<int> FakeItemIds;
}

[Game]
public class OrderedFakeItemsComponent : IComponent
{
    public Stack<int> FakeItemIds;
}

[Game, Unique]
public class CellsDirty : IComponent
{
}

[Game]
public class PuzzlePuzzle : IComponent
{
}

[Game]
public class RotorRotor : IComponent
{
}

[Game]
public class TntRotor : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class Merging : IComponent
{
    public Vector2Int To;
}

[Game, Event(EventTarget.Self)]
public class MergeMaster : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class CreatedFromMatch : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class TntExplosionStarted : IComponent
{
}

[Game]
public class TntExplosionEnded : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class ExplodeAnimationStartedComponent : IComponent
{
}

[Game]
public class ExplodeAnimationEndedComponent : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class SpawnAnimationStartedComponent : IComponent
{
}

[Game]
public class SpawnAnimationEndedComponent : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class MatchAnimationStartedComponent : IComponent
{
}

[Game]
public class MatchAnimationEndedComponent : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class ComboTypeComponent : IComponent
{
    public ComboType Value;
}

[Game, Event(EventTarget.Self)]
public class PuzzleTargetedCubes : IComponent
{
    public List<int> cubeIds;
}

[Game]
public class PosItemsToActivate : IComponent
{
    public List<int> PosItemIds;
}

[Game]
public class OutsideTheBoard : IComponent
{
}

[Game]
public class CollectedAtLayer : IComponent
{
}

[Game]
public class CollectedAtEnd : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class CollectionStartedComponent : IComponent
{
    public int Amount;
}

[Game]
public class CollectionEndedComponent : IComponent
{
    public GoalType GoalType;
    public int Amount;
}

[Game, Unique]
public class WantedBees : IComponent
{
    public int Amount;
}

[Game, Unique, Event(EventTarget.Any)]
public class RemainingMoves : IComponent
{
    public int Amount;
}

[Game]
public class GoalTypeComponent : IComponent
{
    public GoalType Value;
}

[Game]
public class ReservedForCellItem : IComponent
{
}

[Game]
public class CreatedFromGeneratorComponent : IComponent
{
}

[Game]
public class LayerResettingComponent : IComponent
{
    public int OriginalLayer;
}

[Game]
public class CantHaveBubbleOnTopComponent : IComponent
{
}

[Game]
public class CannonDirectionComponent : IComponent
{
    public CannonDirection Value;
}

[Game]
public class BottomCellBlockingComponent : IComponent
{
}

[Game, Unique]
public class BottomCellBlockingDirtyComponent : IComponent
{
}

[Game]
public class ItemDirectionComponent : IComponent
{
    public ItemDirection Value;
}

[Game]
public class CandyFakeItemComponent : IComponent
{
}

[Game, Event(EventTarget.Self)]
public class ExplosionStartedComponent : IComponent
{
    public int Count;
}

[Game]
public class ExplosionEndedComponent : IComponent
{
    public int Count;
}

[Game, Event(EventTarget.Self)]
public class ShuffleStartedComponent : IComponent
{
    public bool ChangedColor;
    public Vector2Int Pos;
}

[Game]
public class ShuffleEndedComponent : IComponent
{
    public Vector2Int Pos;
}

[Game, Unique]
public class CriticalAnimationCounterComponent : IComponent
{
    public int Value;
}

[Game, Unique]
public class CollectAnimationCounterComponent : IComponent
{
    public int Value;
}

[Game, Unique, Event(EventTarget.Any)]
public class GameplayStateComponent : IComponent
{
    public GameplayState Value;
}

[Game, Unique]
public class CantBeShuffledComponent : IComponent
{
}

public enum GameplayState
{
    Play,
    Waiting,
    Determination,
    Win,
    Lose,
    ShuffleLose
}

public enum ItemDirection
{
    Right,
    Left,
    Up,
    Down
}

public enum CannonDirection
{
    Right,
    Left,
}