using System.Linq;

public class WaitHelper
{
    static WaitHelper()
    {
    }

    private WaitHelper()
    {
    }

    public static WaitHelper Instance { get; } = new WaitHelper();


    public static bool Has(params WaitType[] types)
    {
        return types.Any(type => type.Has());
    }

    public static void Reduce(params WaitType[] types)
    {
        foreach (var type in types)
        {
            type.Reduce();
        }
    }

    public static void Increase(params WaitType[] types)
    {
        foreach (var type in types)
        {
            type.Increase();
        }
    }
}

public abstract class WaitType : Enumeration
{
    public static readonly WaitType FallingItem = new FallingItemType(0);
    public static readonly WaitType Hint = new HintType(3);
    public static readonly WaitType Input = new InputType(4);
    public static readonly WaitType Fall = new FallType(5);
    public static readonly WaitType Turn = new TurnType(6);
    public static readonly WaitType CriticalAnimation = new CriticalAnimationType(7);
    public static readonly WaitType WillSpawnItem = new WillSpawnItemType(8);
    public static readonly WaitType WillBeDestroyedItem = new WillBeDestroyedItemType(9);
    public static readonly WaitType CollectAnimation = new CollectAnimationType(10);
    public static readonly WaitType Activation = new ActivationType(11);

    private WaitType(int value, string name) : base(value, name)
    {
    }

    public abstract bool Has();
    public abstract void Reduce();
    public abstract void Increase();

    private class FallingItemType : WaitType
    {
        public FallingItemType(int value) : base(value, "FallingItem")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.GetGroup(GameMatcher.Falling).count > 0;
        }

        public override void Reduce()
        {
        }

        public override void Increase()
        {
        }
    }

    private class WillSpawnItemType : WaitType
    {
        public WillSpawnItemType(int value) : base(value, "Fill")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.GetGroup(GameMatcher.WillSpawnItem).count > 0;
        }

        public override void Reduce()
        {
        }

        public override void Increase()
        {
        }
    }

    private class WillBeDestroyedItemType : WaitType
    {
        public WillBeDestroyedItemType(int value) : base(value, "WillBeDestroyed")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.GetGroup(GameMatcher.WillBeDestroyed).count > 0;
        }

        public override void Reduce()
        {
        }

        public override void Increase()
        {
        }
    }

    private class ItemReservationType : WaitType
    {
        public ItemReservationType(int value) : base(value, "ItemReservation")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.GetGroup(GameMatcher.ItemReservation).count > 0;
        }

        public override void Reduce()
        {
        }

        public override void Increase()
        {
        }
    }

    private class CellItemReservationType : WaitType
    {
        public CellItemReservationType(int value) : base(value, "ItemReservation")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.GetGroup(GameMatcher.CellItemReservation).count > 0;
        }

        public override void Reduce()
        {
        }

        public override void Increase()
        {
        }
    }

    private class HintType : WaitType
    {
        public HintType(int value) : base(value, "Hint")
        {
        }

        public override bool Has()
        {
            return !Contexts.sharedInstance.game.board.IsHintActive;
        }

        public override void Reduce()
        {
            Contexts.sharedInstance.game.board.HintBlockCounter--;
            if (Contexts.sharedInstance.game.board.HintBlockCounter != 0) return;

            Contexts.sharedInstance.game.board.IsHintActive = true;
            Contexts.sharedInstance.game.isDirty = true;
        }

        public override void Increase()
        {
            Contexts.sharedInstance.game.board.HintBlockCounter++;
            Contexts.sharedInstance.game.board.IsHintActive = false;
        }
    }

    private class InputType : WaitType
    {
        public InputType(int value) : base(value, "Input")
        {
        }

        public override bool Has()
        {
            return !Contexts.sharedInstance.game.board.IsTouchActive;
        }

        public override void Reduce()
        {
            Contexts.sharedInstance.game.board.TouchBlockCounter--;
            if (Contexts.sharedInstance.game.board.TouchBlockCounter != 0) return;

            Contexts.sharedInstance.game.board.IsTouchActive = true;
        }

        public override void Increase()
        {
            Contexts.sharedInstance.game.board.TouchBlockCounter++;
            Contexts.sharedInstance.game.board.IsTouchActive = false;
        }
    }

    private class FallType : WaitType
    {
        public FallType(int value) : base(value, "Fall")
        {
        }

        public override bool Has()
        {
            return !Contexts.sharedInstance.game.board.IsFallActive;
        }

        public override void Reduce()
        {
            Contexts.sharedInstance.game.board.FallBlockCounter--;
            if (Contexts.sharedInstance.game.board.FallBlockCounter == 0)
            {
                Contexts.sharedInstance.game.board.IsFallActive = true;
            }
        }

        public override void Increase()
        {
            Contexts.sharedInstance.game.board.FallBlockCounter++;
            Contexts.sharedInstance.game.board.IsFallActive = false;
        }
    }

    private class TurnType : WaitType
    {
        public TurnType(int value) : base(value, "Turn")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.turn.TurnCounter > 0;
        }

        public override void Reduce()
        {
            Contexts.sharedInstance.game.turn.TurnCounterPrevValue = Contexts.sharedInstance.game.turn.TurnCounter;
            Contexts.sharedInstance.game.turn.TurnCounter--;
        }

        public override void Increase()
        {
            Contexts.sharedInstance.game.turn.TurnCounterPrevValue = Contexts.sharedInstance.game.turn.TurnCounter;
            Contexts.sharedInstance.game.turn.TurnCounter++;
        }
    }

    private class CriticalAnimationType : WaitType
    {
        public CriticalAnimationType(int value) : base(value, "CriticalAnimation")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.criticalAnimationCounter.Value > 0;
        }

        public override void Reduce()
        {
            Contexts.sharedInstance.game.criticalAnimationCounter.Value--;
        }

        public override void Increase()
        {
            Contexts.sharedInstance.game.criticalAnimationCounter.Value++;
        }
    }

    private class CollectAnimationType : WaitType
    {
        public CollectAnimationType(int value) : base(value, "CollectAnimation")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game.collectAnimationCounter.Value > 0;
        }

        public override void Reduce()
        {
            Contexts.sharedInstance.game.collectAnimationCounter.Value--;
        }

        public override void Increase()
        {
            Contexts.sharedInstance.game.collectAnimationCounter.Value++;
        }
    }

    private class ActivationType : WaitType
    {
        public ActivationType(int value) : base(value, "CollectAnimation")
        {
        }

        public override bool Has()
        {
            return Contexts.sharedInstance.game
                       .GetGroup(GameMatcher.AnyOf(GameMatcher.Activation, GameMatcher.Activated,
                           GameMatcher.WillExplode)).count > 0;
        }

        public override void Reduce()
        {
        }

        public override void Increase()
        {
        }
    }
}