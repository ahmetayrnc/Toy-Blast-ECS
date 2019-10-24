using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract partial class ItemTypeInBoard
{
    private class Group1Type : ItemTypeInBoard
    {
        public Group1Type() : base(89, "Group1")
        {
        }

        public override ItemType ItemType => null;
        public override GoalType GoalType => GoalType.Bee;

        public override void Spawn(GameEntity entity)
        {
            var items = Contexts.sharedInstance.game.itemGroups.Groups[0].Items;
            items[Random.Range(0, items.Count)].Spawn(entity);
        }
    }

    private class Group2Type : ItemTypeInBoard
    {
        public Group2Type() : base(90, "Group2")
        {
        }

        public override ItemType ItemType => null;
        public override GoalType GoalType => GoalType.Bee;

        public override void Spawn(GameEntity entity)
        {
            var items = Contexts.sharedInstance.game.itemGroups.Groups[1].Items;
            items[Random.Range(0, items.Count)].Spawn(entity);
        }
    }

    private class Group3Type : ItemTypeInBoard
    {
        public Group3Type() : base(91, "Group3")
        {
        }

        public override ItemType ItemType => null;
        public override GoalType GoalType => GoalType.Bee;

        public override void Spawn(GameEntity entity)
        {
            var items = Contexts.sharedInstance.game.itemGroups.Groups[2].Items;
            items[Random.Range(0, items.Count)].Spawn(entity);
        }
    }
}

public abstract partial class ItemTypeInBoard : Enumeration
{
    public static readonly ItemTypeInBoard Invalid = new InvalidType();
    public static readonly ItemTypeInBoard Empty = new EmptyType();

    public static readonly ItemTypeInBoard BlueCube = ColorCubeType.MBlueCube;
    public static readonly ItemTypeInBoard GreenCube = ColorCubeType.MGreenCube;
    public static readonly ItemTypeInBoard RedCube = ColorCubeType.MRedCube;
    public static readonly ItemTypeInBoard YellowCube = ColorCubeType.MYellowCube;

    public static readonly ItemTypeInBoard RotorVertical = RotorType.VerticalRotorType;
    public static readonly ItemTypeInBoard RotorHorizontal = RotorType.HorizontalRotorType;

    public static readonly ItemTypeInBoard Tnt = new TntType();

    public static readonly ItemTypeInBoard BluePuzzle = PuzzleType.MBluePuzzle;
    public static readonly ItemTypeInBoard GreenPuzzle = PuzzleType.MGreenPuzzle;
    public static readonly ItemTypeInBoard OrangePuzzle = PuzzleType.MOrangePuzzle;
    public static readonly ItemTypeInBoard PurplePuzzle = PuzzleType.MPurplePuzzle;
    public static readonly ItemTypeInBoard RedPuzzle = PuzzleType.MRedPuzzle;
    public static readonly ItemTypeInBoard YellowPuzzle = PuzzleType.MYellowPuzzle;

    public static readonly ItemTypeInBoard BeachBall = new BeachBallType();

    public static readonly ItemTypeInBoard Lego0 = LegoType.MLego0;
    public static readonly ItemTypeInBoard Lego1 = LegoType.MLego1;
    public static readonly ItemTypeInBoard Lego2 = LegoType.MLego2;
    public static readonly ItemTypeInBoard Lego3 = LegoType.MLego3;
    public static readonly ItemTypeInBoard Lego4 = LegoType.MLego4;

    public static readonly ItemTypeInBoard Pumpkin = new PumpkinType();
    public static readonly ItemTypeInBoard MetalLego = new MetalLegoType();
    public static readonly ItemTypeInBoard BlueLego = ColorLegoType.MBlueLego;
    public static readonly ItemTypeInBoard GreenLego = ColorLegoType.MGreenLego;
    public static readonly ItemTypeInBoard OrangeLego = ColorLegoType.MOrangeLego;
    public static readonly ItemTypeInBoard PurpleLego = ColorLegoType.MPurpleLego;
    public static readonly ItemTypeInBoard RedLego = ColorLegoType.MRedLego;
    public static readonly ItemTypeInBoard YellowLego = ColorLegoType.MYellowLego;
    public static readonly ItemTypeInBoard ActiveDuck = DuckType.MActiveDuck;
    public static readonly ItemTypeInBoard InactiveDuck = DuckType.MInActiveDuck;
    public static readonly ItemTypeInBoard PinWheel = new PinWheelType();
    public static readonly ItemTypeInBoard Piggy0 = PiggyType.MPiggy0;
    public static readonly ItemTypeInBoard Piggy1 = PiggyType.MPiggy1;
    public static readonly ItemTypeInBoard Egg0 = EggType.MEgg0;
    public static readonly ItemTypeInBoard Egg1 = EggType.MEgg1;
    public static readonly ItemTypeInBoard Toy0 = ToyType.MToy0;
    public static readonly ItemTypeInBoard Toy1 = ToyType.MToy1;
    public static readonly ItemTypeInBoard Toy2 = ToyType.MToy2;
    public static readonly ItemTypeInBoard Toy3 = ToyType.MToy3;
    public static readonly ItemTypeInBoard Toy4 = ToyType.MToy4;
    public static readonly ItemTypeInBoard Toy5 = ToyType.MToy5;
    public static readonly ItemTypeInBoard Toy6 = ToyType.MToy6;
    public static readonly ItemTypeInBoard Toy7 = ToyType.MToy7;
    public static readonly ItemTypeInBoard ActiveUfo = UfoType.MActiveUfo;
    public static readonly ItemTypeInBoard InactiveUfo = UfoType.MInActiveUfo;
    public static readonly ItemTypeInBoard ActiveCuckoo = CuckooType.MActiveCuckoo;
    public static readonly ItemTypeInBoard InactiveCuckoo = CuckooType.MInActiveCuckoo;
    public static readonly ItemTypeInBoard BlueOrb = OrbType.MBlueOrb;
    public static readonly ItemTypeInBoard GreenOrb = OrbType.MGreenOrb;
    public static readonly ItemTypeInBoard RedOrb = OrbType.MRedOrb;
    public static readonly ItemTypeInBoard YellowOrb = OrbType.MYellowOrb;
    public static readonly ItemTypeInBoard Potion = new PotionType();
    public static readonly ItemTypeInBoard Gum = new GumType();
    public static readonly ItemTypeInBoard Locker = new LockerType();
    public static readonly ItemTypeInBoard Stone = new StoneType();
    public static readonly ItemTypeInBoard Safe = new SafeType();
    public static readonly ItemTypeInBoard BlueCandyJar = CandyJarType.MBlueCandyJar;
    public static readonly ItemTypeInBoard GreenCandyJar = CandyJarType.MGreenCandyJar;
    public static readonly ItemTypeInBoard RedCandyJar = CandyJarType.MRedCandyJar;
    public static readonly ItemTypeInBoard YellowCandyJar = CandyJarType.MYellowCandyJar;
    public static readonly ItemTypeInBoard PurpleCandyJar = CandyJarType.MPurpleCandyJar;
    public static readonly ItemTypeInBoard OrangeCandyJar = CandyJarType.MOrangeCandyJar;
    public static readonly ItemTypeInBoard Bowling = new BowlingType();
    public static readonly ItemTypeInBoard Dragon = new DragonType();
    public static readonly ItemTypeInBoard Statue = new StatueType();
    public static readonly ItemTypeInBoard Dishwasher = new DishwasherType();
    public static readonly ItemTypeInBoard Vase = new VaseType();
    public static readonly ItemTypeInBoard Chick = new ChickType();
    public static readonly ItemTypeInBoard Snowman = new SnowmanType();
    public static readonly ItemTypeInBoard Toaster = new ToasterType();
    public static readonly ItemTypeInBoard Firework = new FireworkType();
    public static readonly ItemTypeInBoard Soap = new SoapType();
    public static readonly ItemTypeInBoard BlueCocktail = CocktailType.MBlueCocktail;
    public static readonly ItemTypeInBoard GreenCocktail = CocktailType.MGreenCocktail;
    public static readonly ItemTypeInBoard RedCocktail = CocktailType.MRedCocktail;
    public static readonly ItemTypeInBoard YellowCocktail = CocktailType.MYellowCocktail;
    public static readonly ItemTypeInBoard Beehive = new BeehiveType();
    public static readonly ItemTypeInBoard Washer = new WasherType();
    public static readonly ItemTypeInBoard CannonRight = CannonType.MRightCannon;
    public static readonly ItemTypeInBoard CannonLeft = CannonType.MLeftCannon;
    public static readonly ItemTypeInBoard LaunchPad = new LaunchPadType();
    public static readonly ItemTypeInBoard CandyMonsterHead = CandyMonsterType.MCandyMonsterHead;
    public static readonly ItemTypeInBoard CandyMonsterRightCandy = CandyMonsterType.MCandyMonsterCandyRight;
    public static readonly ItemTypeInBoard CandyMonsterDownCandy = CandyMonsterType.MCandyMonsterCandyDown;
    public static readonly ItemTypeInBoard CandyMonsterLeftCandy = CandyMonsterType.MCandyMonsterCandyLeft;
    public static readonly ItemTypeInBoard CandyMonsterUpCandy = CandyMonsterType.MCandyMonsterCandyUp;
    public static readonly ItemTypeInBoard Group1 = new Group1Type();
    public static readonly ItemTypeInBoard Group2 = new Group2Type();
    public static readonly ItemTypeInBoard Group3 = new Group3Type();

    private ItemTypeInBoard(int id, string name)
        : base(id, name)
    {
    }

    public bool HasItemType()
    {
        return ItemType != null;
    }

    public abstract ItemType ItemType { get; }

    public abstract GoalType GoalType { get; }

    public abstract void Spawn(GameEntity entity);

    private void AddGeneralComponents(GameEntity entity, ItemType itemType, GoalType goalType)
    {
        var spawnInfo = entity.willSpawnItem;
        var pos = new Vector2(spawnInfo.GridPosition.x, spawnInfo.WorldY);
        entity.AddPosition(pos);
        entity.AddItemType(itemType);
        entity.AddGoalType(goalType);
        entity.AddGridPosition(spawnInfo.GridPosition);
        entity.AddLayer(0);
        entity.AddRemovers(new HashSet<int>());
        entity.isItem = true;

        if (entity.hasWillFill)
        {
            entity.AddFalling(new Vector2(spawnInfo.GridPosition.x, spawnInfo.WorldY),
                (Vector2) spawnInfo.GridPosition, entity.willFill.Speed);
        }
    }

    private void AddFakeItems(GameEntity entity, Vector2Int gridPos, MultiBlockType type)
    {
        var x = gridPos.x;
        var y = gridPos.y;
        var positions2X2 = new[]
        {
            new Vector2Int(x + 1, y),
            new Vector2Int(x + 1, y + 1),
            new Vector2Int(x, y + 1)
        };

        var positions3X2 = new[]
        {
            new Vector2Int(x + 1, y),
            new Vector2Int(x + 1, y + 1),
            new Vector2Int(x, y + 1),
            new Vector2Int(x + 2, y),
            new Vector2Int(x + 2, y + 1)
        };

        var positions = type == MultiBlockType.M2X2 ? positions2X2 : positions3X2;

        var fakeItemIds = new List<int>();

        foreach (var pos in positions)
        {
            var fake = Contexts.sharedInstance.game.CreateEntity();
            fake.AddFakeItem(entity.id.Value);
            fake.AddGridPosition(pos);
            fake.AddId(IdHelper.GetNewId());
            fakeItemIds.Add(fake.id.Value);
        }

        entity.AddFakeItems(fakeItemIds);
    }

    private enum MultiBlockType
    {
        M2X2,
        M3X2
    }
}