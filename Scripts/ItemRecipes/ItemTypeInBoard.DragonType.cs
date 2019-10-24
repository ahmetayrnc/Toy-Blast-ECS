public abstract partial class ItemTypeInBoard
{
    private class DragonType : ItemTypeInBoard
    {
        public DragonType() : base(158, "Dragon")
        {
        }

        public override ItemType ItemType => ItemType.Dragon;
        public override GoalType GoalType => GoalType.Dragon;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.isCanBeActivatedByBottom = true;
            entity.isCantBeActivated = true;
            entity.isCanFall = true;
            entity.isMultiBlock = true;

            AddFakeItems(entity, entity.willSpawnItem.GridPosition, MultiBlockType.M2X2);
        }
    }
}