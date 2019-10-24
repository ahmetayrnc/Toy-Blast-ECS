using System.Threading;
using DG.Tweening;
using UnityEngine;

public abstract class ThrowItemView : MonoBehaviour
{
    private CancellationTokenSource _tokenSource;
    private Transform _transform;

    protected const string ThrowLayer = "throw";

    protected virtual void Awake()
    {
        _transform = transform;
    }

    protected CancellationToken GetNewCancellationToken()
    {
        _tokenSource = new CancellationTokenSource();
        return _tokenSource.Token;
    }

    public void StopTimedThings()
    {
        StopAllTweens();
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

    public abstract void Refresh();

    protected abstract void UpdateSortingLayer(string layerName);
//    protected abstract void GetThrown();
}