using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int LevelNo
    {
        private get { return _levelNo; }
        set
        {
            _levelNo = value;
            levelText.text = value.ToString();
        }
    }

    public Button button;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelDescription;

    private LevelSelectionMenuUi _levelSelectionMenuUi;
    private int _levelNo;

    private void Awake()
    {
        _levelSelectionMenuUi = GetComponentInParent<LevelSelectionMenuUi>();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        _levelSelectionMenuUi.StartLevel(LevelNo);
    }
}