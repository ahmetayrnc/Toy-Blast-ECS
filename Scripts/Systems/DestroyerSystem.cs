using Entitas;

public class DestroyerSystem : ICleanupSystem
{
    private readonly IGroup<GameEntity> _willDestroys;

    public DestroyerSystem(Contexts contexts)
    {
        _willDestroys = contexts.game.GetGroup(GameMatcher.WillBeDestroyed);
    }

    public void Cleanup()
    {
        foreach (var e in _willDestroys.GetEntities())
        {
            e.Destroy();
        }
    }
}