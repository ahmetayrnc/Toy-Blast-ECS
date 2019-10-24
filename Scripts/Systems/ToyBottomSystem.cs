using System;
using System.Collections.Generic;
using Entitas;

public class ToyBottomSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private Turn _turn;

    readonly IGroup<GameEntity> _toyGroup;

    public ToyBottomSystem(Contexts contexts)
    {
        _contexts = contexts;
        _toyGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.CanBeActivatedByBottom)
            .NoneOf(GameMatcher.Activation, GameMatcher.Falling));
    }

    public void Execute()
    {
        foreach (var toy in _toyGroup.GetEntities())
        {
            var toyPos = toy.gridPosition.value;

            var cell = _contexts.game.GetEntityWithCellId(new Tuple<int, int>(toyPos.x, toyPos.y));
            var cellItem = _contexts.game.GetEntityWithCellItemId(new Tuple<int, int>(toyPos.x, toyPos.y));

            if (!cell.isBottomCell) continue;

            if (cellItem != null && cellItem.isCanStopItemActivation) continue;

            ActivateItem(toy, ActivationReason.Bottom);
        }
    }

    private void ActivateItem(GameEntity entity, ActivationReason reason)
    {
        entity.isCanBeActivatedByBottom = false;

        Queue<ActivationReason> activationQueue;
        if (entity.hasActivation)
        {
            activationQueue = entity.activation.ActivationQueue;
            activationQueue.Enqueue(reason);
            entity.ReplaceActivation(activationQueue);
        }
        else
        {
            activationQueue = new Queue<ActivationReason>();
            activationQueue.Enqueue(reason);
            entity.AddActivation(activationQueue);
        }
    }
}