using CellItemPoolExtensions;

public abstract class CellItemView : GenericItemView
{
    public sealed override void OnWillBeDestroyed(GameEntity entity)
    {
        UnlinkItem();
        this.Destroy();
        OnWillBeDestroyedExtra(entity);
    }

    protected virtual void OnWillBeDestroyedExtra(GameEntity entity)
    {
    }
}