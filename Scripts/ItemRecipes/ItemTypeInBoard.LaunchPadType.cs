public abstract partial class ItemTypeInBoard
{
    private class LaunchPadType : ItemTypeInBoard
    {
        public LaunchPadType() : base(111, "LaunchPad")
        {
        }
        
        public override ItemType ItemType => ItemType.LaunchPad;
        public override GoalType GoalType => GoalType.LaunchPad;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(2);
            entity.AddLayerResetting(2);
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCantHaveBubbleOnTop = true;
            entity.isBottomCellBlocking = true;
            entity.AddGenerator(GeneratorType.Item);
            entity.AddItemToGenerate(Toy2);
            entity.AddGoalToGenerate(GoalType.Toy2);
            entity.AddGenerationAmount(1);
            entity.AddGeneratorRadius(GeneratorRadius.All);
            entity.AddGoalEffectType(GoalEffectType.Decrease);
            entity.isBottomCellConsiderate = true;
        }
    }
}