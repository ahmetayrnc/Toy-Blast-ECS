public abstract partial class ItemTypeInBoard
{
    private class BeehiveType : ItemTypeInBoard
    {
        public BeehiveType() : base(117, "Beehive")
        {
        }

        public override ItemType ItemType => ItemType.Beehive;
        public override GoalType GoalType => GoalType.Bee;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCantBeDestroyed = true;
            entity.isCollectableGenerator = true;
            entity.isGoalDependent = true;
            entity.isCollectedAtLayer = true;
        }
    }
}