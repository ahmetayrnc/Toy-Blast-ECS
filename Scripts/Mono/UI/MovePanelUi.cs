using TMPro;

public class MovePanelUi : GenericUi, IAnyRemainingMovesListener
{
    public TextMeshProUGUI moveText;

    protected override void AddListeners()
    {
        Contexts.sharedInstance.game.CreateEntity().AddAnyRemainingMovesListener(this);
    }

    public void OnAnyRemainingMoves(GameEntity entity, int amount)
    {
        moveText.text = amount.ToString();
    }

    protected override void Refresh()
    {
    }
}