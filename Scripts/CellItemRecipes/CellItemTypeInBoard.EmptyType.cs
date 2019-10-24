public abstract partial class CellItemTypeInBoard
{
    private class EmptyType : CellItemTypeInBoard
    {
        public EmptyType() : base(-1, "Empty")
        {
        }

        public override CellItemType CellItemType => CellItemType.Bubble;
        public override GoalType GoalType => GoalType.Bubble;

        public override void Spawn(GameEntity entity)
        {
            entity.isWillBeDestroyed = true;
        }
    }
}