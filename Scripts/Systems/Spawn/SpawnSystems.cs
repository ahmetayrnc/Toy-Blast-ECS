public class SpawnSystems : Feature
{
    public SpawnSystems(Contexts contexts) : base("Spawn Systems")
    {
        Add(new ItemSpawnSystem(contexts));
        Add(new CellItemSpawnSystem(contexts));
    }
}