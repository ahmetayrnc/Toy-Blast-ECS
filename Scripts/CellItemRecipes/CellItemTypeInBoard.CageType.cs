public abstract partial class CellItemTypeInBoard
{
    private class CageType : CellItemTypeInBoard
    {
        public CageType() : base(32, "Cage")
        {
        }

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, CellItemType, GoalType);
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCanStopItemFall = true;
            entity.isCanStopItemActivation = true;
            entity.isCollectedAtEnd = true;
        }

        public override CellItemType CellItemType => CellItemType.Cage;
        public override GoalType GoalType => GoalType.Cage;
    }
}