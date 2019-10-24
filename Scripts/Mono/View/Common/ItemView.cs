using ItemPoolExtensions;

public abstract class ItemView : GenericItemView
{
    public sealed override void OnWillBeDestroyed(GameEntity entity)
    {
        BeforeOnWillBeDestroyed(entity);
        UnlinkItem();
        this.Destroy();
        AfterOnWillBeDestroyed(entity);
    }

    protected virtual void AfterOnWillBeDestroyed(GameEntity entity)
    {
    }

    protected virtual void BeforeOnWillBeDestroyed(GameEntity entity)
    {
    }
}