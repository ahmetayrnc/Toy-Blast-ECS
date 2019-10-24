using Entitas;

public class WinLoseSystem : IExecuteSystem
{
    private readonly Contexts _contexts;

    public WinLoseSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        switch (_contexts.game.gameplayState.Value)
        {
            case GameplayState.Play:
                PlayState();
                break;
            case GameplayState.Waiting:
                WaitingState();
                break;
            case GameplayState.Determination:
                DeterminationState();
                break;
            case GameplayState.Win:
            case GameplayState.Lose:
            case GameplayState.ShuffleLose:
                break;
        }
    }

    private void PlayState()
    {
        var movesLeft = _contexts.game.remainingMoves.Amount > 0;
        var goalCompleted = GoalHelper.IsGoalComplete();
        var cantBeShuffled = _contexts.game.isCantBeShuffled;

        if (!movesLeft || goalCompleted || cantBeShuffled)
        {
            _contexts.game.ReplaceGameplayState(GameplayState.Waiting);
            WaitHelper.Increase(WaitType.Input);
        }
    }

    private void WaitingState()
    {
        if (WaitHelper.Has(WaitType.FallingItem, WaitType.CriticalAnimation, WaitType.WillBeDestroyedItem,
            WaitType.WillSpawnItem, WaitType.CollectAnimation, WaitType.Activation))
        {
            return;
        }

        _contexts.game.ReplaceGameplayState(GameplayState.Determination);
    }

    private void DeterminationState()
    {
        var goalCompleted = GoalHelper.IsGoalComplete();
        var movesLeft = _contexts.game.remainingMoves.Amount > 0;
        var cantBeShuffled = _contexts.game.isCantBeShuffled;

        if (goalCompleted)
        {
            _contexts.game.ReplaceGameplayState(GameplayState.Win);
        }

        else if (!movesLeft)
        {
            _contexts.game.ReplaceGameplayState(GameplayState.Lose);
        }

        else if (cantBeShuffled)
        {
            _contexts.game.ReplaceGameplayState(GameplayState.ShuffleLose);
        }
        else
        {
            _contexts.game.ReplaceGameplayState(GameplayState.Play);
        }
    }
}