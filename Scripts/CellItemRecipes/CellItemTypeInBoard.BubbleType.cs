public abstract partial class CellItemTypeInBoard
{
    private class BubbleType : CellItemTypeInBoard
    {
        public BubbleType() : base(36, "Bubble")
        {
        }
        public override CellItemType CellItemType => CellItemType.Bubble;
        public override GoalType GoalType => GoalType.Bubble;
        
        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, CellItemType, GoalType);
            entity.isCanBeActivatedByInnerMatch = true;
            entity.isCantBeActivatedByPositiveItem = true;
            entity.isCollectedAtEnd = true;
        }
    }
}