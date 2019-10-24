public abstract partial class ItemTypeInBoard
{
    private abstract class CandyMonsterType : ItemTypeInBoard
    {
        public static readonly CandyMonsterType MCandyMonsterHead = new CandyMonsterHeadType();
        public static readonly CandyMonsterType MCandyMonsterCandyRight = CandyMonsterCandy.MmCandyMonsterRightCandy;
        public static readonly CandyMonsterType MCandyMonsterCandyUp = CandyMonsterCandy.MmCandyMonsterUpCandy;
        public static readonly CandyMonsterType MCandyMonsterCandyLeft = CandyMonsterCandy.MmCandyMonsterLeftCandy;
        public static readonly CandyMonsterType MCandyMonsterCandyDown = CandyMonsterCandy.MmCandyMonsterDownCandy;

        private CandyMonsterType(int value, string name) : base(value, name)
        {
        }


        public override ItemType ItemType => ItemType.CandyMonster;
        public override GoalType GoalType => GoalType.CandyMonster;

        private class CandyMonsterHeadType : CandyMonsterType
        {
            private int _candyLength;
            private ItemDirection _itemDirection;

            protected internal CandyMonsterHeadType() : base(160, "CandyMonsterHead")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                entity.isCollectedAtEnd = true;
                entity.isCanBeActivatedByNearMatch = true;
                entity.isMultiBlock = true;
                entity.isRemoverSensitive = true;
                entity.isCantHaveBubbleOnTop = true;
                //some components are added in CandyFinderSystem
            }
        }

        private abstract class CandyMonsterCandy : CandyMonsterType
        {
            public static readonly CandyMonsterType MmCandyMonsterRightCandy = new CandyMonsterRightCandyType();
            public static readonly CandyMonsterType MmCandyMonsterUpCandy = new CandyMonsterUpCandyType();
            public static readonly CandyMonsterType MmCandyMonsterLeftCandy = new CandyMonsterLeftCandyType();
            public static readonly CandyMonsterType MmCandyMonsterDownCandy = new CandyMonsterDownCandyType();

            public abstract ItemDirection ItemDirection { get; }

            private CandyMonsterCandy(int value, string name) : base(value, name)
            {
            }

            private void CreateCandyMonsterCandy(GameEntity entity, ItemDirection direction)
            {
                var fake = Contexts.sharedInstance.game.CreateEntity();
                fake.isCandyFakeItem = true;
                fake.AddItemDirection(direction);
                fake.AddGridPosition(entity.willSpawnItem.GridPosition);
            }

            public override void Spawn(GameEntity entity)
            {
                CreateCandyMonsterCandy(entity, ItemDirection);
            }

            private class CandyMonsterRightCandyType : CandyMonsterCandy
            {
                public override ItemDirection ItemDirection => ItemDirection.Right;

                protected internal CandyMonsterRightCandyType() : base(161, "CandyMonsterRightCandy")
                {
                }
            }

            private class CandyMonsterDownCandyType : CandyMonsterCandy
            {
                public override ItemDirection ItemDirection => ItemDirection.Down;

                protected internal CandyMonsterDownCandyType() : base(162, "CandyMonsterDownCandy")
                {
                }
            }

            private class CandyMonsterLeftCandyType : CandyMonsterCandy
            {
                public override ItemDirection ItemDirection => ItemDirection.Left;

                protected internal CandyMonsterLeftCandyType() : base(163, "CandyMonsterLeftCandy")
                {
                }
            }

            private class CandyMonsterUpCandyType : CandyMonsterCandy
            {
                public override ItemDirection ItemDirection => ItemDirection.Up;

                protected internal CandyMonsterUpCandyType() : base(164, "CandyMonsterUpCandy")
                {
                }
            }
        }
    }
}