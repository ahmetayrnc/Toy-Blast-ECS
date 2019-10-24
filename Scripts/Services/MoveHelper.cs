public static class MoveHelper
{
    public static void ReduceMove()
    {
        var moves = Contexts.sharedInstance.game.remainingMoves.Amount;

        if (moves == 0) return;

        Contexts.sharedInstance.game.ReplaceRemainingMoves(moves - 1);
    }
}