using System.Threading;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public abstract class GenericItemView : MonoBehaviour, IView, IPositionListener, IWillBeDestroyedListener
{
    private CancellationTokenSource _cancellationTokenSource;

    protected int BaseSortingOrder;
    protected int ReverseBaseSortingOrder;
    private const int OrderDifference = 100;

    protected const string ItemLayer = "items";
    protected const string RotorLayer = "rotors";
    protected const string AnimLayer = "anim";
    protected const string BubbleLayer = "bubble";
    protected const string MergeLayer = "merge";
    protected const string HintLayer = "hint";
    protected const string PuzzleInUseLayer = "puzzleInUse";
    protected const string AboveItemLayer = "aboveItems";

    public void Link(IEntity entity)
    {
        gameObject.Link(entity);
        Vector2 pos = CalculatePosition(entity);
        SetPosition(pos);
        CalculateBaseSortingOrder((int) pos.y);
        AddListeners((GameEntity) entity);
        Refresh(entity);
    }

    private void Refresh(IEntity entity)
    {
        gameObject.SetActive(true);
        ARefresh((GameEntity) entity);
        StopAllCoroutines();
    }

    protected abstract void ARefresh(GameEntity entity);

    private void AddListeners(GameEntity entity)
    {
        entity.AddPositionListener(this);
        entity.AddWillBeDestroyedListener(this);
        AAddListeners(entity);
    }

    protected abstract void AAddListeners(GameEntity entity);

    public void OnPosition(GameEntity entity, Vector2 value)
    {
        transform.localPosition = new Vector3(value.x, value.y);
        CalculateBaseSortingOrder((int) value.y);
        UpdateSortingOrder();
    }

    protected abstract void UpdateSortingOrder();

    public abstract void OnWillBeDestroyed(GameEntity entity);

    private void CalculateBaseSortingOrder(int y)
    {
        BaseSortingOrder = y * OrderDifference;
        ReverseBaseSortingOrder = (Contexts.sharedInstance.game.board.Size.y - y) * OrderDifference;
    }

    protected int BaseSortingOrderOf(int y)
    {
        return y * OrderDifference;
        ;
    }

    private static Vector2 CalculatePosition(IEntity entity)
    {
        var ge = (GameEntity) entity;

        if (ge.hasGridPosition)
        {
            return ge.gridPosition.value;
        }

        if (ge.hasPosition)
        {
            return ge.position.value;
        }

        return Vector2.zero;
    }

    private void SetPosition(Vector2 pos)
    {
        transform.localPosition = new Vector3(pos.x, pos.y);
    }

    public void SetName(string newName)
    {
        gameObject.name = newName;
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void UnlinkItem()
    {
        var link = gameObject.GetEntityLink();
        if (link != null && link.entity != null)
        {
            gameObject.Unlink();
        }
    }

    public abstract void UpdateSortingLayer(string layerName);
}