using EntitasBlast;

public abstract partial class ItemTypeInBoard
{
    private abstract class PuzzleType : ItemTypeInBoard
    {
        public static readonly PuzzleType MBluePuzzle = new BluePuzzleType();
        public static readonly PuzzleType MGreenPuzzle = new GreenPuzzleType();
        public static readonly PuzzleType MOrangePuzzle = new OrangePuzzleType();
        public static readonly PuzzleType MPurplePuzzle = new PurplePuzzleType();
        public static readonly PuzzleType MRedPuzzle = new RedPuzzleType();
        public static readonly PuzzleType MYellowPuzzle = new YellowPuzzleType();

        private PuzzleType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Puzzle;
        public override GoalType GoalType => GoalType.Puzzle;

        private void CreatePuzzle(GameEntity entity, Color color)
        {
            entity.AddColor(color);
            entity.AddMatchType(MatchType.Positive);
            entity.AddHint(HintType.None);
            entity.AddMatchGroup(-1, 0);
            entity.isCanBeActivatedByTouch = true;
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isInnerMatchItem = true;
            entity.isPositiveItem = true;
        }

        private class BluePuzzleType : PuzzleType
        {
            protected internal BluePuzzleType() : base(42, "BluePuzzle")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePuzzle(entity, Color.Blue);
            }
        }
        
        private class GreenPuzzleType : PuzzleType
        {
            protected internal GreenPuzzleType() : base(43, "GreenPuzzle")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePuzzle(entity, Color.Green);
            }
        }
        
        private class OrangePuzzleType : PuzzleType
        {
            protected internal OrangePuzzleType() : base(45, "OrangePuzzle")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePuzzle(entity, Color.Orange);
            }
        }
        
        private class PurplePuzzleType : PuzzleType
        {
            protected internal PurplePuzzleType() : base(44, "PurplePuzzle")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePuzzle(entity, Color.Purple);
            }
        }
        
        private class RedPuzzleType : PuzzleType
        {
            protected internal RedPuzzleType() : base(41, "RedPuzzle")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePuzzle(entity, Color.Red);
            }
        }
        
        private class YellowPuzzleType : PuzzleType
        {
            protected internal YellowPuzzleType() : base(40, "YellowPuzzle")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePuzzle(entity, Color.Yellow);
            }
        }
    }
}