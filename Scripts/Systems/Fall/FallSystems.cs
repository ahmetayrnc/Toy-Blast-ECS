public class FallSystems : Feature
{
    public FallSystems(Contexts contexts) : base("Fall Systems")
    {
        Add(new FallDetermination(contexts));
        Add(new FillSystem(contexts));
        Add(new FallHandler(contexts));
    }
}