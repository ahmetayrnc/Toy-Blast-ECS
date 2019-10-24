public abstract partial class ItemTypeInBoard
{
    private class BeachBallType : ItemTypeInBoard
    {
        public BeachBallType() : base(34, "BeachBall")
        {
        }

        public override ItemType ItemType => ItemType.BeachBall;
        public override GoalType GoalType => GoalType.BeachBall;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.isCanFall = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;
        }
    }
}