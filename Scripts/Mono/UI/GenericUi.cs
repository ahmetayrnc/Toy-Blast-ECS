using System.Collections.Generic;
using Entitas.Unity;
using UnityEngine;

public abstract class GenericUi : MonoBehaviour
{
    private List<GameEntity> eventEntities = new List<GameEntity>();

    protected virtual void Start()
    {
        SubscribeToUiResetEvent();
        AddListeners();
    }

    private void SubscribeToUiResetEvent()
    {
        GameController.Instance.ResetEvent += BaseRefresh;
    }

    protected abstract void AddListeners();

    private void BaseRefresh()
    {
//        gameObject.Unlink();
        Refresh();
        AddListeners();
    }

    protected abstract void Refresh();
}