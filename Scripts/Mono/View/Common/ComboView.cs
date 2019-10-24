using ComboPoolExtensions;
using UnityEngine;

public abstract class ComboView : GenericItemView, ICreatedFromMatchListener
{
    protected override void ARefresh(GameEntity entity)
    {
        ARefreshForComboItems(entity);
    }

    protected abstract void AAddListenersForComboItems(GameEntity entity);
    protected abstract void ARefreshForComboItems(GameEntity entity);

    protected override void AAddListeners(GameEntity entity)
    {
        entity.AddCreatedFromMatchListener(this);
        AAddListenersForComboItems(entity);
    }

    public virtual void OnCreatedFromMatch(GameEntity entity)
    {
        var creationParticle = ParticlePool.Instance.Spawn(ParticleType.SpecialItemCreation,
            (Vector2) entity.gridPosition.value);
        creationParticle.PlayAndDie();
        RemoveCreatedFromMatch(entity);
    }

    private void RemoveCreatedFromMatch(GameEntity entity)
    {
        DoWait.WaitSeconds(0.35f, () => entity.isCreatedFromMatch = false);
    }

    public override void OnWillBeDestroyed(GameEntity entity)
    {
        this.Destroy();
    }
}