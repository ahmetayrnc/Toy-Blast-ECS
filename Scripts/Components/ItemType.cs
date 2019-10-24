public class ItemType : Enumeration
{
    public static readonly ItemType ColorCube = new ItemType(1, "ColorCube", 162);
    public static readonly ItemType Rotor = new ItemType(2, "Rotor", 81);
    public static readonly ItemType Tnt = new ItemType(3, "Tnt", 81);
    public static readonly ItemType Puzzle = new ItemType(4, "Puzzle", 81);
    public static readonly ItemType BeachBall = new ItemType(5, "BeachBall", 81);
    public static readonly ItemType Lego = new ItemType(6, "Lego", 81);
    public static readonly ItemType Pumpkin = new ItemType(7, "Pumpkin", 81);
    public static readonly ItemType MetalLego = new ItemType(8, "MetalLego", 81);
    public static readonly ItemType ColorLego = new ItemType(9, "ColorLego", 81);
    public static readonly ItemType Duck = new ItemType(10, "Duck", 81);
    public static readonly ItemType PinWheel = new ItemType(11, "PinWheel", 81);
    public static readonly ItemType Piggy = new ItemType(12, "Piggy", 81);
    public static readonly ItemType Egg = new ItemType(13, "Egg", 81);
    public static readonly ItemType Toy = new ItemType(14, "Toy", 81);
    public static readonly ItemType Ufo = new ItemType(15, "Ufo", 81);
    public static readonly ItemType Cuckoo = new ItemType(16, "Cuckoo", 81);
    public static readonly ItemType Orb = new ItemType(17, "Orb", 81);
    public static readonly ItemType Potion = new ItemType(18, "Potion", 81);
    public static readonly ItemType Gum = new ItemType(19, "Gum", 81);
    public static readonly ItemType Locker = new ItemType(20, "Locker", 81);
    public static readonly ItemType Stone = new ItemType(21, "Stone", 81);
    public static readonly ItemType Safe = new ItemType(22, "Safe", 81);
    public static readonly ItemType CandyJar = new ItemType(23, "CandyJar", 81);
    public static readonly ItemType Bowling = new ItemType(24, "Bowling", 21);
    public static readonly ItemType Dragon = new ItemType(25, "Dragon", 21);
    public static readonly ItemType Statue = new ItemType(26, "Statue", 21);
    public static readonly ItemType Dishwasher = new ItemType(27, "Dishwasher", 21);
    public static readonly ItemType Vase = new ItemType(28, "Vase", 81);
    public static readonly ItemType Chick = new ItemType(29, "Chick", 81);
    public static readonly ItemType Snowman = new ItemType(30, "Snowman", 81);
    public static readonly ItemType Soap = new ItemType(31, "Soap", 81);
    public static readonly ItemType Toaster = new ItemType(32, "Toaster", 81);
    public static readonly ItemType Firework = new ItemType(33, "Firework", 21);
    public static readonly ItemType Cocktail = new ItemType(34, "Cocktail", 21);
    public static readonly ItemType Beehive = new ItemType(35, "Beehive", 81);
    public static readonly ItemType Washer = new ItemType(36, "Washer", 81);
    public static readonly ItemType Cannon = new ItemType(37, "Cannon", 81);
    public static readonly ItemType LaunchPad = new ItemType(38, "LaunchPad", 81);
    public static readonly ItemType CandyMonster = new ItemType(39, "CandyMonster", 41);

    public readonly int InitialPoolAmount;

    private ItemType(int id, string name, int initialPoolAmount)
        : base(id, name)
    {
        InitialPoolAmount = initialPoolAmount;
    }
}