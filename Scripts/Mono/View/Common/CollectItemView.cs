using System.Threading;
using DG.Tweening;
using UnityEngine;

public abstract class CollectItemView : MonoBehaviour
{
    private CancellationTokenSource _tokenSource;
    private Transform _transform;
    private GoalPanelUi _goalPanelUi;

    protected const string CollectLayer = "collect";

    protected virtual void Awake()
    {
        _transform = transform;
        _goalPanelUi = GoalPanelUi.Instance;
    }

    public void StopTimedThings()
    {
        StopAllTweens();
        StopAllCoroutines();
    }

    private void StopAllTweens()
    {
        transform.DOKill();
    }

    public void SetParent(Transform parent)
    {
        _transform.SetParent(parent);
    }

    public void SetParentToRoot()
    {
        _transform.SetParent(null);
    }

    public void SetName(string newName)
    {
        gameObject.name = newName;
    }

    public void SetPosition(Vector3 pos)
    {
        _transform.position = pos;
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    protected Vector2 WorldPositionOf(GoalType goalType)
    {
        var pos = _goalPanelUi.PositionOf(goalType);
        pos.x -= 0.5f;
        pos.y -= 0.5f;
        return pos;
    }

    public abstract void GetCollected(GoalType goalType);

    public void Refresh()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        RefreshExtra();
    }

    protected abstract void RefreshExtra();
    protected abstract void UpdateSortingLayer(string layerName);
    protected abstract void OnCollectionEnded(GoalType goalType);
}