public class ItemChangeSystems : Feature
{
    public ItemChangeSystems(Contexts contexts) : base("Item Change Systems")
    {
        Add(new FallStateChangeSystem(contexts));
        Add(new LayerTypeChangeSystem(contexts));
        Add(new RemoverSensitivityChangeSystem(contexts));
        Add(new ConsecutionSensitiveChangeSystem(contexts));
        Add(new SubItemSystem(contexts));
//        Add(new SoapBubbleSystem(contexts));
    }
}