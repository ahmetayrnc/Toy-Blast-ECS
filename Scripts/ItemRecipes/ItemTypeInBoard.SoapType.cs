public abstract partial class ItemTypeInBoard
{
    private class SoapType : ItemTypeInBoard
    {
        public SoapType() : base(110, "Soap")
        {
        }

        public override ItemType ItemType => ItemType.Soap;
        public override GoalType GoalType => GoalType.Soap;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCanFall = true;
            entity.isCollectedAtEnd = true;
            entity.AddGenerator(GeneratorType.CellItem);
            entity.AddCellItemToGenerate(CellItemTypeInBoard.Bubble);
            entity.AddGoalToGenerate(GoalType.Bubble);
            entity.AddGenerationAmount(9);
            entity.AddGeneratorRadius(GeneratorRadius.Radius1);
            entity.AddGoalEffectType(GoalEffectType.Increase);
        }
    }
}