using Entitas;
using UnityEngine;

public class RotorDeactivationSystem : IExecuteSystem
{
    private readonly Contexts _contexts;

    readonly IGroup<GameEntity> _activeRotorGroup;
    readonly IGroup<GameEntity> _cellGroup;

    private const int Distance = 1;
    private const int DistanceDestroyMultiplier = 5;

    public RotorDeactivationSystem(Contexts contexts)
    {
        _contexts = contexts;
        _activeRotorGroup = contexts.game.GetGroup(GameMatcher.ActiveRotor);
        _cellGroup = contexts.game.GetGroup(GameMatcher.Cell);
    }

    public void Execute()
    {
        if (_activeRotorGroup.count <= 0)
        {
            return;
        }

        var board = _contexts.game.board;
        var boardSize = board.Size;
        var width = boardSize.x;
        var height = boardSize.y;

        var targetRight = width - 1 + Distance;
        const int targetLeft = -Distance;
        var targetUp = height - 1 + Distance;
        const int targetDown = -Distance;

        var targetRightDestroy = width - 1 + Distance * DistanceDestroyMultiplier;
        const int targetLeftDestroy = -Distance * DistanceDestroyMultiplier;
        var targetUpDestroy = height - 1 + Distance * DistanceDestroyMultiplier;
        const int targetDownDestroy = -Distance * DistanceDestroyMultiplier;

        foreach (var activeRotor in _activeRotorGroup)
        {
            switch (activeRotor.activeRotor.Direction)
            {
                case RotorDirection.Right:
                    if (activeRotor.isOutsideTheBoard)
                    {
                        if (targetRightDestroy - activeRotor.position.value.x <= 0)
                        {
                            DestroyRotor(activeRotor);
                        }
                    }
                    else
                    {
                        if (targetRight - activeRotor.position.value.x <= 0)
                        {
                            RotorLeftBoard(activeRotor, activeRotor.activeRotor.Direction, activeRotor.position.value);
                        }
                    }

                    break;

                case RotorDirection.Left:
                    if (activeRotor.isOutsideTheBoard)
                    {
                        if (activeRotor.position.value.x - targetLeftDestroy <= 0)
                        {
                            DestroyRotor(activeRotor);
                        }
                    }
                    else
                    {
                        if (activeRotor.position.value.x - targetLeft <= 0)
                        {
                            RotorLeftBoard(activeRotor, activeRotor.activeRotor.Direction, activeRotor.position.value);
                        }
                    }

                    break;

                case RotorDirection.Up:
                    if (activeRotor.isOutsideTheBoard)
                    {
                        if (targetUpDestroy - activeRotor.position.value.y <= 0)
                        {
                            DestroyRotor(activeRotor);
                        }
                    }
                    else
                    {
                        if (targetUp - activeRotor.position.value.y <= 0)
                        {
                            RotorLeftBoard(activeRotor, activeRotor.activeRotor.Direction, activeRotor.position.value);
                        }
                    }

                    break;

                case RotorDirection.Down:
                    if (activeRotor.isOutsideTheBoard)
                    {
                        if (activeRotor.position.value.y - targetDownDestroy <= 0)
                        {
                            DestroyRotor(activeRotor);
                        }
                    }
                    else
                    {
                        if (activeRotor.position.value.y - targetDown <= 0)
                        {
                            RotorLeftBoard(activeRotor, activeRotor.activeRotor.Direction, activeRotor.position.value);
                        }
                    }

                    break;
            }
        }
    }

    private void RotorLeftBoard(GameEntity rotorEntity, RotorDirection direction, Vector2 pos)
    {
        foreach (var e in _cellGroup.GetEntities())
        {
            if (direction == RotorDirection.Left || direction == RotorDirection.Right)
            {
                if (e.gridPosition.value.y != (int) pos.y)
                {
                    continue;
                }
            }

            if (direction == RotorDirection.Up || direction == RotorDirection.Down)
            {
                if (e.gridPosition.value.x != (int) pos.x)
                {
                    continue;
                }
            }

            CellHelper.UnBlockFallAt(e);
        }

        WaitHelper.Reduce(WaitType.Input, WaitType.Turn, WaitType.CriticalAnimation);

        rotorEntity.isOutsideTheBoard = true;
    }

    private void DestroyRotor(GameEntity rotorEntity)
    {
        rotorEntity.isWillBeDestroyed = true;
    }
}