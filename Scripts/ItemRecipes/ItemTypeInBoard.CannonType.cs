public abstract partial class ItemTypeInBoard
{
    private abstract class CannonType : ItemTypeInBoard
    {
        public static readonly CannonType MRightCannon = new RightCannon();
        public static readonly CannonType MLeftCannon = new LeftCannon();

        private CannonType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Cannon;
        public override GoalType GoalType => GoalType.Cannon;

        private void CreateCannon(GameEntity entity, CannonDirection direction)
        {
            entity.ReplaceLayer(2);
            entity.AddLayerResetting(2);
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCantHaveBubbleOnTop = true;
            entity.isBottomCellBlocking = true;
            entity.AddGenerator(GeneratorType.Item);
            entity.AddItemToGenerate(BeachBall);
            entity.AddGoalToGenerate(GoalType.BeachBall);
            entity.AddGenerationAmount(3);
            entity.AddGeneratorRadius(GeneratorRadius.All);
            entity.AddGoalEffectType(GoalEffectType.Decrease);
            entity.AddCannonDirection(direction);
        }

        private class RightCannon : CannonType
        {
            protected internal RightCannon() : base(107, "RightCannon")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCannon(entity, CannonDirection.Right);
            }
        }

        private class LeftCannon : CannonType
        {
            protected internal LeftCannon() : base(87, "LeftCannon")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCannon(entity, CannonDirection.Left);
            }
        }
    }
}