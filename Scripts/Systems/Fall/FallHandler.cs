using Entitas;
using UnityEngine;

public class FallHandler : IExecuteSystem
{
    private readonly Contexts _contexts;

    readonly IGroup<GameEntity> _fallingGroup;
    private const float Acceleration = 55f;
    private const float MaxVelocity = 30f;

    public FallHandler(Contexts contexts)
    {
        _contexts = contexts;
        _fallingGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Falling)
            .NoneOf(GameMatcher.WillBeDestroyed, GameMatcher.Activation));
    }

    public void Execute()
    {
        var board = _contexts.game.board;
        var isFallActive = board.IsFallActive;

        if (!isFallActive)
        {
            return;
        }

        foreach (var e in _fallingGroup.GetEntities())
        {
            AssignNewVelocity(e);

            if (ReachedTarget(e))
            {
                e.ReplacePosition(e.falling.Target);
                e.RemoveFalling();
            }
            else
            {
                MoveDown(e);
            }
        }
    }

    private void AssignNewVelocity(GameEntity entity)
    {
        var speed = entity.falling.Speed;
        speed += Acceleration * Time.deltaTime;
        speed = Mathf.Min(speed, MaxVelocity);
        entity.ReplaceFalling(entity.falling.From, entity.falling.Target, speed);
    }

    private bool ReachedTarget(GameEntity entity)
    {
        return entity.position.value.y - entity.falling.Target.y - entity.falling.Speed * Time.deltaTime <= 0;
    }

    private void MoveDown(GameEntity entity)
    {
        var overlapped = false;
        foreach (var fallingItem in _contexts.game.GetEntities(GameMatcher.AllOf(GameMatcher.Falling)))
        {
            if (fallingItem == entity) continue;
            if (fallingItem.gridPosition.value.x != entity.gridPosition.value.x) continue;
            if (entity.position.value.y < fallingItem.position.value.y) continue;
            if (fallingItem.position.value.y + 1 < entity.position.value.y) continue;

            var belowItemVelocity = fallingItem.falling.Speed;
            entity.falling.Speed = belowItemVelocity;
            overlapped = true;
            break;
        }

        if (overlapped) return;

        var newY = entity.position.value.y - entity.falling.Speed * Time.deltaTime;
        entity.ReplacePosition(new Vector2(entity.position.value.x, newY));
    }
}