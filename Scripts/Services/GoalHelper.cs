using UnityEngine;

public static class GoalHelper
{
    public static bool IsInGoal(GoalType goalType)
    {
        var itemGoals = Contexts.sharedInstance.game.goal.Goals;

        if (!itemGoals.ContainsKey(goalType))
        {
            return false;
        }

        var itemProgress = Contexts.sharedInstance.game.goalProgress.Progress;

        if (itemProgress[goalType] >= itemGoals[goalType])
        {
            return false;
        }

        return true;
    }

    public static void AddToProgress(GoalType goalType, int amount)
    {
        var goal = Contexts.sharedInstance.game.goal.Goals;
        var progress = Contexts.sharedInstance.game.goalProgress.Progress;

        var itemGoal = goal[goalType];
        var itemProgress = progress[goalType];
        itemProgress = Mathf.Min(itemProgress + amount, itemGoal);
        progress[goalType] = itemProgress;

        Contexts.sharedInstance.game.ReplaceGoalProgress(progress);
    }

    public static void AddToGoal(GoalType goalType, int amount)
    {
        var goal = Contexts.sharedInstance.game.goal.Goals;
        var itemGoal = goal[goalType];
        itemGoal += amount;
        goal[goalType] = itemGoal;

        Contexts.sharedInstance.game.ReplaceGoal(goal);
    }

    public static bool IsGoalComplete()
    {
        var remainingAmount = 0;
        var goals = Contexts.sharedInstance.game.goal.Goals;
        var progress = Contexts.sharedInstance.game.goalProgress.Progress;

        foreach (var itemProgressEntry in progress)
        {
            var itemGoal = goals[itemProgressEntry.Key];
            var itemProgress = itemProgressEntry.Value;

            remainingAmount += itemGoal - itemProgress;
        }

        return remainingAmount == 0;
    }
}