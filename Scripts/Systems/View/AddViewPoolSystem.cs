using System.Collections.Generic;
using ActiveRotorPoolExtensions;
using CellItemPoolExtensions;
using ComboPoolExtensions;
using Entitas;
using ItemPoolExtensions;

public class AddViewPoolSystem : ReactiveSystem<GameEntity>
{
    public AddViewPoolSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        => context.CreateCollector(GameMatcher.AnyOf(
            GameMatcher.ItemType,
            GameMatcher.CellItemType,
            GameMatcher.ComboType,
            GameMatcher.ActiveRotor)
        );

    protected override bool Filter(GameEntity entity) =>
        (entity.hasItemType || entity.hasCellItemType || entity.hasComboType || entity.hasActiveRotor) &&
        !entity.hasView;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (e.isItem)
            {
                e.AddView(InstantiateViewItem(e));
            }
            else if (e.isCellItem)
            {
                e.AddView(InstantiateViewCellItem(e));
            }
            else if (e.hasComboType)
            {
                e.AddView(InstantiateViewCombo(e));
            }
            else if (e.hasActiveRotor)
            {
                e.AddView(InstantiateViewActiveRotor(e));
            }
        }
    }

    IView InstantiateViewItem(GameEntity entity)
    {
        var gameObject = entity.itemType.Value.Spawn();
        var view = gameObject.GetComponent<IView>();
        view.Link(entity);
        return view;
    }

    IView InstantiateViewCellItem(GameEntity entity)
    {
        var gameObject = entity.cellItemType.Value.Spawn();
        var view = gameObject.GetComponent<IView>();
        view.Link(entity);
        return view;
    }

    IView InstantiateViewCombo(GameEntity entity)
    {
        var gameObject = entity.comboType.Value.Spawn();
        var view = gameObject.GetComponent<IView>();
        view.Link(entity);
        return view;
    }

    IView InstantiateViewActiveRotor(GameEntity entity)
    {
        var gameObject = ActiveRotorType.ActiveRotor.Spawn();
        var view = gameObject.GetComponent<IView>();
        view.Link(entity);
        return view;
    }
}