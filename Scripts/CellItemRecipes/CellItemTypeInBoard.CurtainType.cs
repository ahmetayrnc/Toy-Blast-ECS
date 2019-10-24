public abstract partial class CellItemTypeInBoard
{
    private class CurtainType : CellItemTypeInBoard
    {
        public CurtainType() : base(56, "Curtain")
        {
        }
        public override CellItemType CellItemType => CellItemType.Curtain;
        public override GoalType GoalType => GoalType.Curtain;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, CellItemType, GoalType);
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCanStopItemActivation = true;
        }
    }
}