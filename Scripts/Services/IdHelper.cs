public static class IdHelper
{
    public static int GetNewRemoverId()
    {
        var contexts = Contexts.sharedInstance;
        contexts.game.removerIdCount.Value++;
        return contexts.game.removerIdCount.Value;
    }

    public static int GetNewId()
    {
        var contexts = Contexts.sharedInstance;
        contexts.game.idCounter.Value++;
        return contexts.game.idCounter.Value;
    }

    public static int GetNewReservationId()
    {
        var contexts = Contexts.sharedInstance;
        contexts.game.reservationIdCounter.Value++;
        return contexts.game.reservationIdCounter.Value;
    }
}