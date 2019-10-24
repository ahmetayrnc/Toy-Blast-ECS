public abstract partial class ItemTypeInBoard
{
    private class WasherType : ItemTypeInBoard
    {
        public WasherType() : base(108, "Washer")
        {
        }

        public override ItemType ItemType => ItemType.Washer;
        public override GoalType GoalType => GoalType.Washer;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(2);
            entity.AddLayerResetting(2);
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCantHaveBubbleOnTop = true;
            entity.isBottomCellBlocking = true;
            entity.AddGenerator(GeneratorType.CellItem);
            entity.AddCellItemToGenerate(CellItemTypeInBoard.Bubble);
            entity.AddGoalToGenerate(GoalType.Bubble);
            entity.AddGenerationAmount(3);
            entity.AddGeneratorRadius(GeneratorRadius.All);
            entity.AddGoalEffectType(GoalEffectType.Decrease);
        }
    }
}