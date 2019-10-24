using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : Pool<ParticlePoolItems, ParticleType, ParticleHandler>
{
    protected override int GetInitialPoolCount(ParticleType type)
    {
        return type.InitialPoolAmount;
    }

    protected override void FillUpPairs()
    {
        Pairs = new Dictionary<ParticleType, ParticleHandler>
        {
            {ParticleType.BlueCubeExplode, items.blueCubeExplodeParticle},
            {ParticleType.GreenCubeExplode, items.greenCubeExplodeParticle},
            {ParticleType.RedCubeExplode, items.redCubeExplodeParticle},
            {ParticleType.YellowCubeExplode, items.yellowCubeExplodeParticle},
            {ParticleType.OrangeCubeExplode, items.orangeCubeExplodeParticle},
            {ParticleType.PurpleCubeExplode, items.purpleCubeExplodeParticle},
            {ParticleType.CubeSmoke, items.cubeSmokeParticle},
            {ParticleType.SpecialItemCreation, items.specialItemCreationParticle},
            {ParticleType.TntExplode, items.tntExplodeParticle},
            {ParticleType.BigTntExplode, items.bigTntExplodeParticle},
            {ParticleType.PuzzleRay, items.puzzleRayParticle},
            {ParticleType.PuzzleInUse, items.puzzleInUseParticle},
            {ParticleType.PuzzleExplode, items.puzzleExplodeParticle},
            {ParticleType.PuzzleInUseCombo, items.puzzleInUseComboParticle},
            {ParticleType.PuzzlePuzzleExplode, items.puzzlePuzzleExplodeParticle},
            {ParticleType.Collect, items.collectParticle},
            {ParticleType.BowlingExplode, items.bowlingExplodeParticle},
            {ParticleType.Crumb, items.crumbParticle},
            {ParticleType.BeehiveCollect, items.beehiveCollectParticle},
            {ParticleType.BeehiveExplode, items.beehiveExplodeParticle},
            {ParticleType.BubbleExplode, items.bubbleExplodeParticle},
            {ParticleType.SoapExplode, items.soapExplodeParticle},
            {ParticleType.WasherBubble1, items.washerBubbleParticle1},
            {ParticleType.WasherBubble2, items.washerBubbleParticle2},
            {ParticleType.BeachBallExplode, items.beachBallExplodeParticle},
            {ParticleType.LegoExplode, items.legoExplodeParticle},
            {ParticleType.LegoPieceExplode, items.legoPieceExplodeParticle},
            {ParticleType.CageExplode, items.cageExplodeParticle},
            {ParticleType.ColorLegoExplode, items.colorLegoExplodeParticle},
            {ParticleType.PiggyExplode0, items.piggyExplodeParticle0},
            {ParticleType.PiggyExplode1, items.piggyExplodeParticle1},
            {ParticleType.PinWheelExplode, items.pinWheelExplodeParticle},
            {ParticleType.EggExplode0, items.eggExplodeParticle0},
            {ParticleType.EggExplode1, items.eggExplodeParticle1},
            {ParticleType.BlueCandyJarExplode, items.blueCandyJarExplodeParticle},
            {ParticleType.RedCandyJarExplode, items.redCandyJarExplodeParticle},
            {ParticleType.GreenCandyJarExplode, items.greenCandyJarExplodeParticle},
            {ParticleType.YellowCandyJarExplode, items.yellowCandyJarExplodeParticle},
            {ParticleType.PumpkinExplode0, items.pumpkinExplodeParticle0},
            {ParticleType.PumpkinExplode1, items.pumpkinExplodeParticle1},
            {ParticleType.PumpkinExplode2, items.pumpkinExplodeParticle2},
            {ParticleType.MetalLegoExplode, items.metalLegoExplodeParticle},
            {ParticleType.PotionExplode, items.potionExplode},
            {ParticleType.SnowmanExplode, items.snowmanExplode},
            {ParticleType.SnowmanExplodeFinal, items.snowmanExplodeFinal},
            {ParticleType.GumExplode, items.gumExplode},
            {ParticleType.GumExplodeFinal, items.gumExplodeFinal},
            {ParticleType.StatueExplode, items.statueExplode},
            {ParticleType.StatueExplodeFinal, items.statueExplodeFinal},
            {ParticleType.DuckExplode, items.duckExplode},
        };
    }

    protected override void DeActivateItem(ParticleHandler view)
    {
        view.DeActivate();
        view.SetParent(poolParent);
    }

    protected override void RefreshItem(ParticleHandler view, Vector2 pos)
    {
        view.Refresh();
        view.SetParentToRoot();
        view.SetPosition(pos);
    }

    protected override void RenameItem(ParticleHandler view, string newName)
    {
        view.SetName(newName);
    }

    protected override string NameOf(ParticleHandler view)
    {
        return view.gameObject.name;
    }
}

public class ParticleType : Enumeration
{
    public static readonly ParticleType BlueCubeExplode = new ParticleType(1, "BlueCubeExplode", 81);
    public static readonly ParticleType GreenCubeExplode = new ParticleType(2, "GreenCubeExplode", 81);
    public static readonly ParticleType RedCubeExplode = new ParticleType(3, "RedCubeExplode", 81);
    public static readonly ParticleType YellowCubeExplode = new ParticleType(4, "YellowCubeExplode", 81);
    public static readonly ParticleType OrangeCubeExplode = new ParticleType(5, "OrangeCubeExplode", 81);
    public static readonly ParticleType PurpleCubeExplode = new ParticleType(6, "PurpleCubeExplode", 81);
    public static readonly ParticleType CubeSmoke = new ParticleType(7, "CubeSmoke", 81);
    public static readonly ParticleType SpecialItemCreation = new ParticleType(8, "SpecialItemCreation", 81);
    public static readonly ParticleType TntExplode = new ParticleType(9, "TntExplode", 81);
    public static readonly ParticleType BigTntExplode = new ParticleType(10, "BigTntExplode", 2);
    public static readonly ParticleType PuzzleRay = new ParticleType(11, "PuzzleRay", 81);
    public static readonly ParticleType PuzzleInUse = new ParticleType(12, "PuzzleInUse", 2);
    public static readonly ParticleType PuzzleExplode = new ParticleType(13, "PuzzleExplode", 12);
    public static readonly ParticleType PuzzleInUseCombo = new ParticleType(14, "PuzzleInUseCombo", 2);
    public static readonly ParticleType PuzzlePuzzleExplode = new ParticleType(15, "PuzzlePuzzleExplode", 2);
    public static readonly ParticleType Collect = new ParticleType(16, "Collect", 2);
    public static readonly ParticleType BowlingExplode = new ParticleType(17, "BowlingExplode", 21);
    public static readonly ParticleType Crumb = new ParticleType(18, "Crumb", 21);
    public static readonly ParticleType BeehiveCollect = new ParticleType(19, "BeehiveCollect", 21);
    public static readonly ParticleType BeehiveExplode = new ParticleType(20, "BeehiveExplode", 81);
    public static readonly ParticleType BubbleExplode = new ParticleType(21, "BubbleExplode", 81);
    public static readonly ParticleType SoapExplode = new ParticleType(22, "SoapExplode", 81);
    public static readonly ParticleType WasherBubble1 = new ParticleType(23, "WasherBubble1", 81);
    public static readonly ParticleType WasherBubble2 = new ParticleType(24, "WasherBubble2", 81);
    public static readonly ParticleType BeachBallExplode = new ParticleType(25, "BeachBallExplode", 81);
    public static readonly ParticleType LegoExplode = new ParticleType(26, "LegoExplode", 81);
    public static readonly ParticleType LegoPieceExplode = new ParticleType(27, "LegoPieceExplode", 81);
    public static readonly ParticleType CageExplode = new ParticleType(28, "CageExplode", 81);
    public static readonly ParticleType ColorLegoExplode = new ParticleType(29, "ColorLegoExplode", 81);
    public static readonly ParticleType PiggyExplode0 = new ParticleType(30, "PiggyExplode0", 81);
    public static readonly ParticleType PiggyExplode1 = new ParticleType(31, "PiggyExplode1", 81);
    public static readonly ParticleType PinWheelExplode = new ParticleType(32, "PinWheelExplode", 81);
    public static readonly ParticleType EggExplode0 = new ParticleType(33, "EggExplode0", 81);
    public static readonly ParticleType EggExplode1 = new ParticleType(34, "EggExplode1", 81);
    public static readonly ParticleType BlueCandyJarExplode = new ParticleType(35, "BlueCandyJarExplode", 81);
    public static readonly ParticleType RedCandyJarExplode = new ParticleType(36, "RedCandyJarExplode", 81);
    public static readonly ParticleType GreenCandyJarExplode = new ParticleType(37, "GreenCandyJarExplode", 81);
    public static readonly ParticleType YellowCandyJarExplode = new ParticleType(38, "YellowCandyJarExplode", 81);
    public static readonly ParticleType PumpkinExplode0 = new ParticleType(39, "PumpkinExplode0", 81);
    public static readonly ParticleType PumpkinExplode1 = new ParticleType(40, "PumpkinExplode1", 81);
    public static readonly ParticleType PumpkinExplode2 = new ParticleType(41, "PumpkinExplode2", 81);
    public static readonly ParticleType MetalLegoExplode = new ParticleType(42, "MetalLegoExplode", 81);
    public static readonly ParticleType PotionExplode = new ParticleType(43, "PotionExplode", 81);
    public static readonly ParticleType SnowmanExplode = new ParticleType(44, "SnowmanExplode", 81);
    public static readonly ParticleType SnowmanExplodeFinal = new ParticleType(45, "SnowmanExplodeFinal", 81);
    public static readonly ParticleType GumExplode = new ParticleType(46, "GumExplode", 81);
    public static readonly ParticleType GumExplodeFinal = new ParticleType(47, "GumExplodeFinal", 81);
    public static readonly ParticleType StatueExplode = new ParticleType(48, "StatueExplode", 81);
    public static readonly ParticleType StatueExplodeFinal = new ParticleType(49, "StatueExplodeFinal", 81);
    public static readonly ParticleType DuckExplode = new ParticleType(50, "DuckExplode", 81);

    public readonly int InitialPoolAmount;

    private ParticleType(int id, string name, int initialPoolAmount)
        : base(id, name)
    {
        InitialPoolAmount = initialPoolAmount;
    }
}

namespace ParticlePoolExtensions
{
    public static class MyExtensions
    {
        public static ParticleHandler Spawn(this ParticleType particleType, Vector2 pos)
        {
            return ParticlePool.Instance.Spawn(particleType, pos);
        }

        public static void Destroy(this ParticleHandler particle)
        {
            ParticlePool.Instance.Destroy(particle);
        }
    }
}