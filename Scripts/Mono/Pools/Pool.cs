using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Pool<TItems, TType, TView> : MonoBehaviour where TView : Object where TType : Enumeration
{
    public static Pool<TItems, TType, TView> Instance;

    public Transform poolParent;
    public TItems items;

    protected Dictionary<TType, TView> Pairs;
    private Dictionary<TType, Queue<TView>> _poolQueue;
    private Dictionary<TType, List<TView>> _allItems;
    protected abstract int GetInitialPoolCount(TType type);

    private void Awake()
    {
        Instance = this;
        _poolQueue = new Dictionary<TType, Queue<TView>>();
        _allItems = new Dictionary<TType, List<TView>>();
        Pairs = new Dictionary<TType, TView>();

        FillUpPairs();

        foreach (var entry in Pairs)
        {
            _poolQueue[entry.Key] = new Queue<TView>();
            _allItems[entry.Key] = new List<TView>();
        }
    }

    public void SpawnAllInitialItems()
    {
        foreach (var entry in Pairs)
        {
            for (var i = 0; i < GetInitialPoolCount(entry.Key); i++)
            {
                SpawnFresh(entry.Key);
            }
        }
    }

    public void SpawnInitialItems(TType type)
    {
        for (var i = 0; i < GetInitialPoolCount(type); i++)
        {
            SpawnFresh(type);
        }
    }

    public void SoftRestart()
    {
        foreach (var queue in _poolQueue)
        {
            queue.Value.Clear();
        }

        foreach (var entry in _allItems)
        {
            var itemList = entry.Value;
            foreach (var item in itemList)
            {
                DeActivateItem(item);
                _poolQueue[entry.Key].Enqueue(item);
            }
        }
    }

    private void SpawnFresh(TType type)
    {
        var collectItem = Pairs[type];
        var itemName = type.ToString();
        var temp = Instantiate(collectItem, poolParent);

        RenameItem(temp, itemName);
        DeActivateItem(temp);

        _poolQueue[type].Enqueue(temp);
        _allItems[type].Add(temp);
    }

    public TView Spawn(TType type, Vector3 pos)
    {
        if (_poolQueue[type].Count == 0)
        {
            SpawnFresh(type);
        }

        var collectItem = _poolQueue[type].Dequeue();

        RefreshItem(collectItem, pos);

        return collectItem;
    }

    public void Destroy(TView view)
    {
        if (!Enumeration.TryParse(NameOf(view), out TType collectItemType))
        {
            Debug.LogError("Pool Cant Destroy Unidentified object");
            return;
        }

        DeActivateItem(view);
        _poolQueue[collectItemType].Enqueue(view);
    }

    protected abstract void FillUpPairs();
    protected abstract void DeActivateItem(TView view);
    protected abstract void RefreshItem(TView view, Vector2 pos);
    protected abstract void RenameItem(TView view, string newName);
    protected abstract string NameOf(TView view);
}