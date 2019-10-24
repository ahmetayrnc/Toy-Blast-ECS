public class ActivationSystems : Feature
{
    public ActivationSystems(Contexts contexts) : base("Activation Systems")
    {
        Add(new ActivateGodTouchSystem(contexts));
        Add(new ActivateTouchSystem(contexts));
        Add(new ActivateItemSystem(contexts));
        Add(new ActivatePositiveItemComboSystem(contexts));
        Add(new ActivatePuzzleComboSystem(contexts));
        Add(new ActivatePuzzleSystem(contexts));
        Add(new ActivateColorMatchSystem(contexts));
        Add(new ActivateColorCubeSystem(contexts));
        Add(new ActivateRotorSystem(contexts));
        Add(new ActivateRotorRotorSystem(contexts));
        Add(new RotorRotorSpawnEnderSystem(contexts));
        Add(new ActivateTntRotorSystem(contexts));
        Add(new TntRotorSpawnEnderSystem(contexts));
        Add(new ActivateTntSystem(contexts));
        Add(new TntExplosionEnderSystem(contexts));
        Add(new ActivateTntTntSystem(contexts));
        Add(new TntTntExplosionEnderSystem(contexts));
        Add(new ActivatePuzzlePuzzleSystem(contexts));
        Add(new CandyExplosionStarterSystem(contexts));
        Add(new CandyExplosionEnderSystem(contexts));
        Add(new CollectionAtLayerSystem(contexts));
        Add(new ExplodeItemSystem(contexts));
        Add(new ActiveRotorProcessor(contexts));
    }
}