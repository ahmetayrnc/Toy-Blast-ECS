public class CellItemType : Enumeration
 {
     public static readonly CellItemType Bubble = new CellItemType(1, "Bubble", 81);
     public static readonly CellItemType Cage = new CellItemType(2, "Cage", 81);
     public static readonly CellItemType Curtain = new CellItemType(3, "Curtain", 81);
 
     public readonly int InitialPoolAmount;

     private CellItemType(int id, string name, int initialPoolAmount)
         : base(id, name)
     {
         InitialPoolAmount = initialPoolAmount;
     }
 }