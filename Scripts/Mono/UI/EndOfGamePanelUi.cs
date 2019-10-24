using UnityEngine;

public class EndOfGamePanelUi : GenericUi, IAnyGameplayStateListener
{
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject shuffleLose;

    protected override void AddListeners()
    {
        Contexts.sharedInstance.game.CreateEntity().AddAnyGameplayStateListener(this);
    }

    protected override void Refresh()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        shuffleLose.SetActive(false);
    }

    public void OnAnyGameplayState(GameEntity entity, GameplayState value)
    {
        switch (value)
        {
            case GameplayState.Lose:
                losePanel.SetActive(true);
                break;
            case GameplayState.Win:
                winPanel.SetActive(true);
                break;
            case GameplayState.ShuffleLose:
                shuffleLose.SetActive(true);
                break;
            default:
                winPanel.SetActive(false);
                losePanel.SetActive(false);
                shuffleLose.SetActive(false);
                break;
        }
    }
}