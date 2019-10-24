using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuUi : MonoBehaviour
{
    public Transform levelButtonParent;
    public IntReference levelNo;
    public GameObject levelButtonPrefab;
    private const string GameplayScene = "GameplayScene";

    public void StartLevel(int no)
    {
        levelNo.variable.RunTimeValue = no;
        SceneManager.LoadScene(GameplayScene);
    }

    private void Awake()
    {
        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        var levelDir = new DirectoryInfo($"{Application.streamingAssetsPath}/Levels");
        var levelFiles = levelDir.GetFiles().Where(f => f.Extension.Equals(".tmx")).ToArray();
        for (var i = 0; i < levelFiles.Length; i++)
        {
            var levelButton = Instantiate(levelButtonPrefab, levelButtonParent).GetComponent<LevelButton>();
            levelButton.LevelNo = i + 1;
        }
    }
}