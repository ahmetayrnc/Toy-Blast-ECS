using System.Collections.Generic;
using Entitas;
using ItemExtensions;
using UnityEngine;

public class ActivateGodTouchSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateGodTouchSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ActiveGodTouch);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActiveGodTouch;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ProcessGodTouch(entity.gridPosition.value, entity.removerId.Value);
            entity.Destroy();
        }
    }

    private void ProcessGodTouch(Vector2Int touchPos, int removerId)
    {
        var item = _contexts.game.GetItemWithPosition(touchPos);

        if (item == null || item.hasFalling)
        {
            return;
        }

        ActivatorHelper.TryActivateItemWithPositive(touchPos, removerId, ActivationReason.God);
    }
}