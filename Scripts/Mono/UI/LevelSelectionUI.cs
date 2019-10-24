using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectionUI : GenericUi
{
    public IntVariable currentLevelNo;

    private const string LevelScene = "LevelScene";
    
    protected override void AddListeners()
    {
    }

    public void ReturnToLevelMenu()
    {
        GameController.Instance.RemoveGame();
        SceneManager.LoadScene(LevelScene);
    }

    public void RestartLevel()
    {
        GameController.Instance.RestartGame(currentLevelNo);
    }

    protected override void Refresh()
    {
    }
}