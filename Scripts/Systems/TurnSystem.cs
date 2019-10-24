using Entitas;

public class TurnSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private Turn _turn;

    private readonly IGroup<GameEntity> _turnSensitiveGroup;

    public TurnSystem(Contexts contexts)
    {
        _contexts = contexts;
        _turnSensitiveGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.TurnSensitive));
    }

    public void Execute()
    {
        _turn = _contexts.game.turn;

        if (ShouldIncrementTurn())
        {
            IncrementTurn();
        }

        _contexts.game.ReplaceTurn(_turn.DeadTurnCount, _turn.TurnCounterPrevValue, _turn.TurnCounter, _turn.TurnId);
    }

    private bool ShouldIncrementTurn()
    {
        if (_turn.TurnCounterPrevValue == 0)
        {
            return false;
        }

        if (_turn.TurnCounter != 0)
        {
            return false;
        }

        if (_turn.DeadTurnCount < 1)
        {
            _turn.DeadTurnCount++;
            return false;
        }

        _turn.DeadTurnCount = 0;

        return true;
    }

    private void IncrementTurn()
    {
        _turn.TurnCounterPrevValue = 0;
        _turn.TurnCounter = 0;
//        _turn.TurnId++;

        foreach (var item in _turnSensitiveGroup.GetEntities())
        {
            if (Equals(item.itemType.Value, ItemType.Duck))
            {
                item.isCantBeActivated = !item.isCantBeActivated;
            }

            if (item.hasConsecutionSensitive)
            {
                if (item.layer.Value == 0)
                {
                    if (item.consecutionSensitive.ActivatedTurnId != _turn.TurnId)
                    {
                        item.ReplaceLayer(item.layer.Value + 1);
                    }

//                    if (!item.hasWillExplode)
//                    {
//                        item.ReplaceLayer(item.layer.Value + 1);
//                    }
                }
            }

            if (item.hasColorChanging)
            {
                var colorQueue = item.colorChanging.ColorQueue;
                var prevColor = colorQueue.Peek();
                colorQueue.Dequeue();
                colorQueue.Enqueue(prevColor);
                var newColor = colorQueue.Peek();

                var colorDict = item.colorSensitive.colors;
                colorDict.Remove(prevColor);
                colorDict[newColor] = item.layer.Value + 1;

                item.ReplaceColorChanging(colorQueue);
                item.ReplaceColor(newColor);
                item.ReplaceColorSensitive(colorDict);
            }
        }
    }
}