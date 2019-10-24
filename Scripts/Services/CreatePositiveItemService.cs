using UnityEngine;
using Color = EntitasBlast.Color;

public static class CreatePositiveItemService
{
    public enum PositiveItem
    {
        Rotor,
        Tnt,
        Puzzle
    }

    public static void CreateRandomPositiveItem(Vector2Int pos)
    {
        var posItem = (PositiveItem) Random.Range(0, System.Enum.GetValues(typeof(PositiveItem)).Length);
        var color = (Color) Random.Range(0, System.Enum.GetValues(typeof(Color)).Length);

        CreatePositiveItem(posItem, pos, color);
    }

    public static int CreatePositiveItem(PositiveItem itemType, Vector2Int pos, Color color = Color.Blue)
    {
        var contexts = Contexts.sharedInstance;
        var posItem = contexts.game.CreateEntity();
        var itemTypeInBoard = ItemTypeInBoard.Tnt;

        if (itemType == PositiveItem.Rotor)
        {
            var axis = Random.Range(0, 2);
            itemTypeInBoard = axis == 0 ? ItemTypeInBoard.RotorHorizontal : ItemTypeInBoard.RotorVertical;
        }

        else if (itemType == PositiveItem.Tnt)
        {
            itemTypeInBoard = ItemTypeInBoard.Tnt;
        }

        else if (itemType == PositiveItem.Puzzle)
        {
            switch (color)
            {
                case Color.Blue:
                    itemTypeInBoard = ItemTypeInBoard.BluePuzzle;
                    break;
                case Color.Green:
                    itemTypeInBoard = ItemTypeInBoard.GreenPuzzle;
                    break;
                case Color.Orange:
                    itemTypeInBoard = ItemTypeInBoard.OrangePuzzle;
                    break;
                case Color.Purple:
                    itemTypeInBoard = ItemTypeInBoard.PurplePuzzle;
                    break;
                case Color.Red:
                    itemTypeInBoard = ItemTypeInBoard.RedPuzzle;
                    break;
                case Color.Yellow:
                    itemTypeInBoard = ItemTypeInBoard.YellowPuzzle;
                    break;
                default:
                    itemTypeInBoard = ItemTypeInBoard.BluePuzzle;
                    break;
            }
        }

        posItem.AddWillSpawnItem(itemTypeInBoard, pos, pos.y);
        posItem.AddId(IdHelper.GetNewId());
        return posItem.id.Value;
    }
}