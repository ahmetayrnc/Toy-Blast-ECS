using Entitas;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class EmitInputSystem : IInitializeSystem, IExecuteSystem
{
    readonly Contexts _contexts;
    private InputEntity _leftMouseEntity;
    private InputEntity _rightMouseEntity;

    public EmitInputSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
    }

    public void Execute()
    {
        var board = _contexts.game.board;
        var isTouchActive = board.IsTouchActive;

        if (!isTouchActive)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch(0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleTouch(1);
        }
    }

    private void HandleTouch(int type)
    {
        // mouse position
        Debug.Assert(Camera.main != null, "Camera.main != null");
        Vector2 rawMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (rawMousePos.x < 0 || rawMousePos.y < 0)
        {
            return;
        }

        var mousePos = new Vector2Int((int) rawMousePos.x, (int) rawMousePos.y);

        var idCount = _contexts.game.removerIdCount.Value;

        var activeTouch = _contexts.game.CreateEntity();
        if (type == 0)
        {
            activeTouch.isActiveTouch = true;
        }
        else
        {
            activeTouch.isActiveGodTouch = true;
        }

        activeTouch.AddGridPosition(new Vector2Int(mousePos.x, mousePos.y));
        activeTouch.AddRemoverId(idCount);

        _contexts.game.ReplaceRemoverIdCount(idCount + 1);
    }
}