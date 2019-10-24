public class ActiveRotorType : Enumeration
{
    public static readonly ActiveRotorType ActiveRotor = new ActiveRotorType(1, "ActiveRotor", 162);
    
    public readonly int InitialPoolAmount;

    private ActiveRotorType(int id, string name, int initialPoolAmount)
        : base(id, name)
    {
        InitialPoolAmount = initialPoolAmount;
    }
}