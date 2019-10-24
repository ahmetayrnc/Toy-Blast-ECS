public abstract partial class ItemTypeInBoard
{
    private class TntType : ItemTypeInBoard
    {
        public TntType() : base(85, "Tnt")
        {
        }

        public override ItemType ItemType => ItemType.Tnt;
        public override GoalType GoalType => GoalType.Tnt;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.AddMatchType(MatchType.Positive);
            entity.AddHint(HintType.None);
            entity.AddMatchGroup(-1, 0);
            entity.isCanBeActivatedByTouch = true;
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isInnerMatchItem = true;
            entity.isPositiveItem = true;
        }
    }
}