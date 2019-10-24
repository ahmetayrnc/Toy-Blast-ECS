using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ActivateTouchSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public ActivateTouchSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ActiveTouch);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActiveTouch;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ProcessTouch(entity.gridPosition.value);
            entity.Destroy();
        }
    }

    private void ProcessTouch(Vector2Int touchPos)
    {
        if (!InBounds(touchPos))
        {
            return;
        }

        var entitySet = _contexts.game.GetEntitiesWithGridPosition(touchPos);
        GameEntity cellItem = null;
        GameEntity item = null;
        foreach (var entity in entitySet)
        {
            if (entity.isCellItem)
            {
                cellItem = entity;
            }

            if (entity.isItem)
            {
                item = entity;
            }
        }

        if (item == null)
        {
            return;
        }

        if (item.hasFalling)
        {
            return;
        }

        if (!item.hasMatchType)
        {
            return;
        }

        MatchType matchType = item.matchType.Value;
        MatchGroupComponent matchGroup = item.matchGroup;

        if (matchGroup.Id == -1)
        {
            return;
        }

        if (matchType != MatchType.Positive && matchGroup.Count < 2)
        {
            return;
        }

        if (cellItem != null && cellItem.isCanStopItemActivation)
        {
            return;
        }

        ActivatorHelper.ActivateItem(item, ActivationReason.Touch);
        MoveHelper.ReduceMove();
        _contexts.game.turn.TurnId++;
    }

    private bool InBounds(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= _contexts.game.board.Size.x)
            return false;

        if (pos.y < 0 || pos.y >= _contexts.game.board.Size.y)
            return false;

        return true;
    }
}