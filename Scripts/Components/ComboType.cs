public class ComboType : Enumeration
{
    public static readonly ComboType RotorRotor = new ComboType(1, "RotorRotor", 2);
    public static readonly ComboType TntTnt = new ComboType(2, "TntTnt", 2);
    public static readonly ComboType TntRotor = new ComboType(3, "TntRotor", 2);
    public static readonly ComboType PuzzleCombo = new ComboType(4, "PuzzleCombo", 2);
    public static readonly ComboType PuzzlePuzzle = new ComboType(5, "PuzzlePuzzle", 2);

    public readonly int InitialPoolAmount;

    private ComboType(int id, string name, int initialPoolAmount)
        : base(id, name)
    {
        InitialPoolAmount = initialPoolAmount;
    }
}