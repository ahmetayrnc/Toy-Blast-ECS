using Entitas;
using UnityEngine;

public class ActiveRotorProcessor : IExecuteSystem
{
    private readonly Contexts _contexts;

    public ActiveRotorProcessor(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        foreach (var entity in _contexts.game.GetGroup(GameMatcher.ActiveRotor).GetEntities())
        {
            ProcessActiveRotor(entity, entity.position.value, entity.activeRotor, entity.removerId.Value);
        }
    }

    private void ProcessActiveRotor(GameEntity entity, Vector2 rotorPos, ActiveRotor activeRotor, int removerId)
    {
        int x = 0;
        int y = 0;
        switch (activeRotor.Direction)
        {
            case RotorDirection.Down:
                x = (int) (rotorPos.x);
                y = (int) (rotorPos.y + 0.1f);
                break;
            case RotorDirection.Up:
                x = (int) (rotorPos.x);
                y = (int) (rotorPos.y + 1 - 0.1f);
                break;
            case RotorDirection.Left:
                x = (int) (rotorPos.x + 0.1f);
                y = (int) (rotorPos.y);
                break;
            case RotorDirection.Right:
                x = (int) (rotorPos.x + 1 - 0.1f);
                y = (int) (rotorPos.y);
                break;
        }

        if (!InBounds(x, y))
        {
            return;
        }

        var pos = new Vector2Int(x, y);
        if (pos.Equals(activeRotor.LastPassedCell))
        {
            return;
        }

        entity.ReplaceActiveRotor(activeRotor.Direction, pos);

        ActivatorHelper.TryActivateItemWithPositive(pos, removerId, ActivationReason.Rotor);
    }

    private bool InBounds(int x, int y)
    {
        if (x < 0 || x >= _contexts.game.board.Size.x)
            return false;

        if (y < 0 || y >= _contexts.game.board.Size.y)
            return false;

        return true;
    }
}