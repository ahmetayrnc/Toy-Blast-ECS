//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Entitas;
//using UnityEngine;
//
//public class SoapBubbleSystem : ReactiveSystem<GameEntity>, IInitializeSystem
//{
//    private readonly Contexts _contexts;
//
//    private int _width;
//    private int _height;
//
//    public SoapBubbleSystem(Contexts contexts) : base(contexts.game)
//    {
//        _contexts = contexts;
//    }
//
//    public void Initialize()
//    {
//        var boardSize = _contexts.game.board.Size;
//        _width = boardSize.x;
//        _height = boardSize.y;
//    }
//
//    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
//    {
//        return context.CreateCollector(GameMatcher.WillExplode);
//    }
//
//    protected override bool Filter(GameEntity entity)
//    {
//        return entity.isItem && entity.itemType.Value == ItemType.Soap;
//    }
//
//    protected override void Execute(List<GameEntity> entities)
//    {
//        foreach (var soap in entities)
//        {
//            var cellItemIds = new List<Tuple<int, int>>();
//            for (int x = soap.gridPosition.value.x - 1; x <= soap.gridPosition.value.x + 1; x++)
//            {
//                for (int y = soap.gridPosition.value.y - 1; y <= soap.gridPosition.value.y + 1; y++)
//                {
//                    if (!InBounds(x, y)) continue;
//
//                    var cell = _contexts.game.GetEntityWithCellId(new Tuple<int, int>(x, y));
//                    if (cell.hasCellItemReservation) continue;
//
//                    var cellItem = _contexts.game.GetEntityWithCellItemId(new Tuple<int, int>(x, y));
//                    if (cellItem != null) continue;
//
//                    cellItemIds.Add(new Tuple<int, int>(x, y));
//                }
//            }
//
//            //goal min with amount
//            var reservations = new List<int>();
//            foreach (var id in cellItemIds)
//            {
//                var reservationId = BoardHelper.Instance.GetNewReservationId();
//                var cell = _contexts.game.GetEntityWithCellId(id);
//                cell.AddCellItemReservation(reservationId, CellItemType.Bubble);
//                reservations.Add(reservationId);
//            }
//
//            soap.AddReservedCells(reservations);
//        }
//    }
//
//    private bool InBounds(int x, int y)
//    {
//        return !(x < 0 || x >= _width || y < 0 || y >= _height);
//    }
//}