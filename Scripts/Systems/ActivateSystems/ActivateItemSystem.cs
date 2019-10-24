using Entitas;
using UnityEngine;

public class ActivateItemSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _willActivates;
    private readonly Contexts _contexts;

    public ActivateItemSystem(Contexts contexts)
    {
        _contexts = contexts;
        _willActivates = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Activation));
    }

    public void Execute()
    {
        foreach (var entity in _willActivates.GetEntities())
        {
            var activationQueue = entity.activation.ActivationQueue;
            while (activationQueue.Count != 0)
            {
                var activationReason = activationQueue.Dequeue();
                ProcessActivation(activationReason, entity);
            }

            entity.RemoveActivation();
        }
    }

    private void ProcessActivation(ActivationReason activationReason, GameEntity entity)
    {
        switch (activationReason)
        {
            case ActivationReason.Touch:
                if (entity.matchGroup.Count >= 2)
                {
                    if (entity.isPositiveItem)
                    {
                        var match = _contexts.game.CreateEntity();
                        match.AddPositiveItemMatch(entity.id.Value);
                        ActivateItem(match);
                    }
                    else
                    {
                        var match = _contexts.game.CreateEntity();
                        match.AddColorMatch(entity.id.Value);
                        ActivateItem(match);
                    }

                    break;
                }

                ActivateItem(entity);
                break;

            case ActivationReason.PuzzlePuzzle:
                if (entity.isPositiveItem && !Equals(entity.itemType.Value, ItemType.Puzzle))
                {
                    ActivateItem(entity);
                    break;
                }

                ExplodeItem(entity);

                break;

            case ActivationReason.Tnt:
            case ActivationReason.Rotor:
                if (entity.isPositiveItem)
                {
                    ActivateItem(entity);
                    break;
                }

                ExplodeItem(entity);

                break;

            case ActivationReason.God:
            case ActivationReason.Bottom:
            case ActivationReason.NearMatch:
                ExplodeItem(entity);
                break;

            case ActivationReason.Puzzle:
                ActivateItem(entity);
                break;
            default:
                Debug.LogError("No Behaviour for activation Reason defined");
                break;
        }
    }

    private void ExplodeItem(GameEntity entity)
    {
        if (entity.hasWillExplode)
        {
            entity.ReplaceWillExplode(entity.willExplode.Count + 1);
        }
        else
        {
            entity.AddWillExplode(1);
        }
    }

    private void ActivateItem(GameEntity entity)
    {
        entity.isActivated = true;
    }
}