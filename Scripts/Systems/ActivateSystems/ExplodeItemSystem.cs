using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ExplodeItemSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly Contexts _contexts;

    public ExplodeItemSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillExplode);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasWillExplode && !entity.isCantBeDestroyed && !entity.hasOrderedFakeItems;
    }

    public void Cleanup()
    {
        foreach (var entity in _contexts.game.GetGroup(GameMatcher.WillExplode).GetEntities())
        {
            entity.RemoveWillExplode();
        }
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            for (int i = 0; i < entity.willExplode.Count; i++)
            {
                DecreaseItemLayer(entity, entity.layer);
            }
        }
    }

    private void DecreaseItemLayer(GameEntity entity, LayerComponent layer)
    {
        var layerValue = layer.Value;

        entity.ReplaceLayer(Mathf.Max(0, layerValue - 1));

        if (layerValue > 0) return;

        if (entity.hasLayerResetting)
        {
            entity.ReplaceLayer(entity.layerResetting.OriginalLayer);
        }
        else
        {
            DestroyItem(entity);
        }

        if (entity.hasGenerator)
        {
            entity.isWillGenerate = true;
        }
    }

    private void DestroyItem(GameEntity entity)
    {
        entity.isWillBeDestroyed = true;

        if (!entity.isMultiBlock) return;

        if (entity.hasFakeItems)
        {
            foreach (var fakeItemId in entity.fakeItems.FakeItemIds)
            {
                var fakeItem = _contexts.game.GetEntityWithId(fakeItemId);
                if (fakeItem == null) continue;

                fakeItem.isWillBeDestroyed = true;
            }
        }
    }
}