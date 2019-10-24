public abstract partial class ItemTypeInBoard
{
    private class ToasterType : ItemTypeInBoard
    {
        public ToasterType() : base(39, "Toaster")
        {
        }

        public override ItemType ItemType => ItemType.Toaster;
        public override GoalType GoalType => GoalType.Toast;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCantBeDestroyed = true;
            entity.isCollectableGenerator = true;
            entity.isCantHaveBubbleOnTop = true;
            entity.isBottomCellBlocking = true;
            entity.isCollectedAtLayer = true;
        }
    }
}