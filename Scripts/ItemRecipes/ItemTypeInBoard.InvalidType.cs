public abstract partial class ItemTypeInBoard
{
    private class InvalidType : ItemTypeInBoard
    {
        public InvalidType() : base(-1, "Invalid")
        {
        }

        public override ItemType ItemType { get; }
        public override GoalType GoalType { get; }

        public override void Spawn(GameEntity entity)
        {
            entity.isWillBeDestroyed = true;
        }
    }
}