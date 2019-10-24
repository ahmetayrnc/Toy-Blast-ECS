using Entitas;
using UnityEngine;

public class RotorMovementSystem : IExecuteSystem
{
    readonly IGroup<GameEntity> _activeRotorGroup;

    private const float Speed = 12f;

    public RotorMovementSystem(Contexts contexts)
    {
        _activeRotorGroup = contexts.game.GetGroup(GameMatcher.ActiveRotor);
    }

    public void Execute()
    {
        if (_activeRotorGroup.count <= 0)
        {
            return;
        }

        foreach (var activeRotor in _activeRotorGroup)
        {
            switch (activeRotor.activeRotor.Direction)
            {
                case RotorDirection.Right:
                    activeRotor.position.value.x += Speed * Time.deltaTime;
                    activeRotor.ReplacePosition(activeRotor.position.value);
                    break;

                case RotorDirection.Left:
                    activeRotor.position.value.x -= Speed * Time.deltaTime;
                    activeRotor.ReplacePosition(activeRotor.position.value);
                    break;

                case RotorDirection.Up:
                    activeRotor.position.value.y += Speed * Time.deltaTime;
                    activeRotor.ReplacePosition(activeRotor.position.value);
                    break;

                case RotorDirection.Down:
                    activeRotor.position.value.y -= Speed * Time.deltaTime;
                    activeRotor.ReplacePosition(activeRotor.position.value);
                    break;
            }
        }
    }
}