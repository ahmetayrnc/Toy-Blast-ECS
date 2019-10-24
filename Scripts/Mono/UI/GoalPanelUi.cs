using System.Collections.Generic;
using UnityEngine;

public class GoalPanelUi : GenericUi, IAnyGoalListener, IAnyGoalProgressListener
{
    private static GoalPanelUi _inst;

    public static GoalPanelUi Instance
    {
        get
        {
            if (_inst) return _inst;

            _inst = FindObjectOfType<GoalPanelUi>();
            Debug.Assert(_inst != null, "No GoalPanelUi in scene");

            return _inst;
        }
    }

    public GoalUi goal1;
    public GoalUi goal2;
    public GoalUi goal3;
    public GoalUi goal4;
    public GoalUiItems uiItems;

    private GoalUi[] _goals;

    private Dictionary<GoalType, GoalUi> _goalPairs;
    private Dictionary<GoalType, Sprite> _goalSpritePairs;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    protected override void Start()
    {
        base.Start();
        _goals = new[] {goal1, goal2, goal3, goal4};
        _goalPairs = new Dictionary<GoalType, GoalUi>();
        _goalSpritePairs = new Dictionary<GoalType, Sprite>
        {
            {GoalType.BlueCube, uiItems.blueCubeGoalSprite},
            {GoalType.RedCube, uiItems.redCubeGoalSprite},
            {GoalType.GreenCube, uiItems.blueCubeGoalSprite},
            {GoalType.YellowCube, uiItems.blueCubeGoalSprite},
            {GoalType.Bee, uiItems.beeGoalSprite},
            {GoalType.Toast, uiItems.toastGoalSprite},
            {GoalType.BowlingPin, uiItems.bowlingGoalSprite},
            {GoalType.Bubble, uiItems.bubbleGoalSprite},
            {GoalType.Toy0, uiItems.toy0GoalSprite},
            {GoalType.Toy1, uiItems.toy1GoalSprite},
            {GoalType.Toy2, uiItems.toy2GoalSprite},
            {GoalType.Toy3, uiItems.toy3GoalSprite},
            {GoalType.Toy4, uiItems.toy4GoalSprite},
            {GoalType.Toy5, uiItems.toy5GoalSprite},
            {GoalType.Toy6, uiItems.toy6GoalSprite},
            {GoalType.Toy7, uiItems.toy7GoalSprite},
            {GoalType.BeachBall, uiItems.beachBallSprite},
            {GoalType.CandyMonster, uiItems.candyMonsterSprite},
            {GoalType.Lego, uiItems.legoSprite},
            {GoalType.Cage, uiItems.cageSprite},
            {GoalType.ColorLego, uiItems.colorLego},
            {GoalType.Piggy, uiItems.piggy},
            {GoalType.PinWheel, uiItems.pinWheel},
            {GoalType.Egg, uiItems.egg},
            {GoalType.Soap, uiItems.soap},
            {GoalType.CandyJar, uiItems.candyJar},
            {GoalType.Pumpkin, uiItems.pumpkin},
            {GoalType.MetalLego, uiItems.metalLego},
            {GoalType.Ufo, uiItems.ufo},
            {GoalType.Potion, uiItems.potion},
            {GoalType.Snowman, uiItems.snowman},
            {GoalType.Gum, uiItems.gum},
            {GoalType.Statue, uiItems.statue},
            {GoalType.Duck, uiItems.duck},
        };
    }

    protected override void AddListeners()
    {
        Contexts.sharedInstance.game.CreateEntity().AddAnyGoalListener(this);
        Contexts.sharedInstance.game.CreateEntity().AddAnyGoalProgressListener(this);
    }

    protected override void Refresh()
    {
        _goalPairs.Clear();
    }

    public void OnAnyGoal(GameEntity entity, Dictionary<GoalType, int> goals)
    {
        var goalIndex = 0;
        foreach (var itemGoalEntry in goals)
        {
            var goalUi = _goals[goalIndex];
            goalUi.Activate();
            goalUi.UpdateGoal(_goalSpritePairs[itemGoalEntry.Key]);
            goalUi.UpdateAmount(itemGoalEntry.Value);

            _goalPairs[itemGoalEntry.Key] = goalUi;
            goalIndex++;
        }

        for (int i = goalIndex; i < _goals.Length; i++)
        {
            var goalUi = _goals[i];
            goalUi.DeActivate();
        }
    }

    public void OnAnyGoalProgress(GameEntity entity, Dictionary<GoalType, int> progress)
    {
        var itemGoals = Contexts.sharedInstance.game.goal.Goals;

        foreach (var itemProgressEntry in progress)
        {
            var goalUi = _goalPairs[itemProgressEntry.Key];
            var itemGoal = itemGoals[itemProgressEntry.Key];
            var itemProgress = itemProgressEntry.Value;
            goalUi.UpdateAmount(itemGoal - itemProgress);
        }
    }

    public Vector2 PositionOf(GoalType goalType)
    {
        var camPos = _cam.transform.position;
        var goalUiScreenPos = _goalPairs[goalType].GetPosition();
        var goalUiWorldPos = new Vector3(goalUiScreenPos.x, goalUiScreenPos.y, camPos.y - goalUiScreenPos.y);
        return goalUiWorldPos;
    }
}