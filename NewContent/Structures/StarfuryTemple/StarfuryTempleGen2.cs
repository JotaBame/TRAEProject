using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;

namespace TRAEProject.NewContent.StarfuryTemple
{
    public static partial class StarfuryTempleGen
    {
		static void CloudIsland(int i, int j)
        {
            UnifiedRandom genRand = WorldGen.genRand;
            double width = 370;
            double num2 = width;
            double height = 15;//ISLAND HEIGHT
            int num4 = i;
            int num5 = i;
            int num6 = i;
            int num7 = j;
            Vector2D val = default(Vector2D);
            val.X = i;
            val.Y = j;
            Vector2D val2 = default(Vector2D);
            val2.X = genRand.Next(-20, 21) * 0.2;
            while (val2.X > -2.0 && val2.X < 2.0)
            {
                val2.X = genRand.Next(-20, 21) * 0.2;
            }
            val2.Y = genRand.Next(-20, -10) * 0.02;
            GenerateFloatingIslandBase(genRand, ref width, ref num2, ref height, ref num4, ref num5, ref num6, ref num7, ref val, ref val2);
            GenerateCloudyBumpsOnTheIslandBottomProbably(genRand, num4, num5, num6, num7);
            width = genRand.Next(80, 95);
            num2 = width;
            height = genRand.Next(10, 15);
            val.X = i;
            val.Y = num6;
            val2.X = genRand.Next(-20, 21) * 0.2;
            while (val2.X > -2.0 && val2.X < 2.0)
            {
                val2.X = genRand.Next(-20, 21) * 0.2;
            }
            val2.Y = genRand.Next(-20, -10) * 0.02;
            DirtPassIdkWhatToCallIt(genRand, ref width, ref num2, ref height, num6, ref val, val2);
            SomePassRelatedToCloudBlocksAndCheckingNotDirst(genRand, num4, num5, num6, num7);
            PlaceWallsBehindCloudBlocks(num4, num5, num6, num7);
            //PutWaterOnStuff(genRand, num4, num5, num6, num7);
            GenerateMiniIslands(genRand, num4, num5, num6);
        }

        private static void GenerateCloudyBumpsOnTheIslandBottomProbably(UnifiedRandom genRand, int num4, int num5, int num6, int num7)
        {
            int o = num4;
            int num17;
            for (o += genRand.Next(5); o < num5; o += genRand.Next(num17, (int)(num17 * 1.5)))
            {
                int num16 = num7;
                while (!Main.tile[o, num16].HasTile)
                {
                    num16--;
                }
                num16 += genRand.Next(-3, 4);
                num17 = genRand.Next(4, 8);
                int num18 = TileID.Cloud;
                if (genRand.NextBool(4))
                {
                    num18 = TileID.RainCloud;
                }
                for (int m = o - num17; m <= o + num17; m++)
                {
                    for (int n = num16 - num17; n <= num16 + num17; n++)
                    {
                        if (n > num6)
                        {
                            double num19 = Math.Abs(m - o);
                            double num20 = Math.Abs(n - num16) * 2;
                            if (Math.Sqrt(num19 * num19 + num20 * num20) < (num17 + genRand.Next(2)))
                            {
                                Tile tile = Main.tile[m, n];
                                tile.HasTile = true;
                                tile.TileType = (ushort)num18;
                                WorldGen.SquareTileFrame(m, n);
                            }
                        }
                    }
                }
            }
        }

        private static void SomePassRelatedToCloudBlocksAndCheckingNotDirst(UnifiedRandom genRand, int num4, int num5, int num6, int num7)
        {
            int num26 = num4;
            num26 += genRand.Next(5);
            while (num26 < num5)
            {
                int num27 = num7;
                while ((!Main.tile[num26, num27].HasTile || Main.tile[num26, num27].TileType != 0) && num26 < num5)
                {
                    num27--;
                    if (num27 < num6)
                    {
                        num27 = num7;
                        num26 += genRand.Next(1, 4);
                    }
                }
                if (num26 >= num5)
                {
                    continue;
                }
                num27 += genRand.Next(0, 4);
                int someHalfSizeThingProbably = genRand.Next(2, 5);
                for (int i = num26 - someHalfSizeThingProbably; i <= num26 + someHalfSizeThingProbably; i++)
                {
                    for (int j = num27 - someHalfSizeThingProbably; j <= num27 + someHalfSizeThingProbably; j++)
                    {
                        if (j > num6)
                        {
                            double num32 = Math.Abs(i - num26);
                            double num33 = Math.Abs(j - num27) * 2;
                            if (Math.Sqrt(num32 * num32 + num33 * num33) < someHalfSizeThingProbably)
                            {
                                Main.tile[i, j].TileType = TileID.Cloud;
                                WorldGen.SquareTileFrame(i, j);
                            }
                        }
                    }
                }
                num26 += genRand.Next(someHalfSizeThingProbably, (int)(someHalfSizeThingProbably * 1.5));
            }
        }

        private static void PlaceWallsBehindCloudBlocks(int num4, int num5, int num6, int num7)
        {
            for (int num34 = num4 - 20; num34 <= num5 + 20; num34++)
            {
                for (int num35 = num6 - 20; num35 <= num7 + 20; num35++)
                {
                    bool flag = true;
                    for (int num36 = num34 - 1; num36 <= num34 + 1; num36++)
                    {
                        for (int num37 = num35 - 1; num37 <= num35 + 1; num37++)
                        {
                            if (!Main.tile[num36, num37].HasTile)
                            {
                                flag = false;
                            }
                        }
                    }
                    if (flag)
                    {
                        Main.tile[num34, num35].WallType = 73;
                        WorldGen.SquareWallFrame(num34, num35);
                    }
                }
            }
        }

        private static void DirtPassIdkWhatToCallIt(UnifiedRandom genRand, ref double width, ref double num2, ref double height, int num6, ref Vector2D val, Vector2D val2)
        {
            height *= 3.5f;
            width *= 3.5f;
            while (width > 0.0 && height > 0.0)
            {
                width -= genRand.Next(4);
                height -= 1.0;
                int num8 = (int)(val.X - width * 4);
                int num9 = (int)(val.X + width * 4);
                int num10 = num6 - 1;
                int num11 = (int)(val.Y + width * 4);
                ClampValuesToWorldBounds(ref num8, ref num9, ref num10, ref num11);
                num2 = width/* * genRand.Next(80, 120) * 0.01*/;
                double num21 = val.Y + 1.0;
                for (int i = num8; i < num9; i++)
                {
                    if (genRand.NextBool(2))
                    {
                        num21 += genRand.Next(-1, 2);
                    }
                    if (num21 < val.Y)
                    {
                        num21 = val.Y;
                    }
                    if (num21 > val.Y + 2.0)
                    {
                        num21 = val.Y + 2.0;
                    }
                    for (int j = num10; j < num11; j++)
                    {
                        if (j > num21)
                        {
                            double num24 = Math.Abs(i - val.X);
                            double num25 = Math.Abs(j - val.Y) * 3.0;
                            if (Math.Sqrt(num24 * num24 + num25 * num25) < num2 * 0.4 && Main.tile[i, j].TileType == 189)
                            {
                                Main.tile[i, j].TileType = TileID.Dirt;
                                WorldGen.SquareTileFrame(i, j);
                            }
                        }
                    }
                }
                val += val2;
                val2.X += genRand.Next(-20, 21) * 0.05;
                if (val2.X > 1.0)
                {
                    val2.X = 1.0;
                }
                if (val2.X < -1.0)
                {
                    val2.X = -1.0;
                }
                if (val2.Y > 0.2)
                {
                    val2.Y = -0.2;
                }
                if (val2.Y < -0.2)
                {
                    val2.Y = -0.2;
                }
            }
        }

        private static void ClampValuesToWorldBounds(ref int num8, ref int num9, ref int num10, ref int num11)
        {
            if (num8 < 0)
            {
                num8 = 0;
            }
            if (num9 > Main.maxTilesX)
            {
                num9 = Main.maxTilesX;
            }
            if (num10 < 0)
            {
                num10 = 0;
            }
            if (num11 > Main.maxTilesY)
            {
                num11 = Main.maxTilesY;
            }
        }

        private static void GenerateMiniIslands(UnifiedRandom genRand, int num4, int num5, int num6)
        {
            int numberOfMiniClouds = genRand.Next(10);//default value 4
            for (int i = 0; i <= numberOfMiniClouds; i++)
            {
                int num44 = genRand.Next(num4 - 5, num5 + 5);
                int num45 = num6 - genRand.Next(20, 40);
                int num46 = genRand.Next(4, 8);
                int num47 = TileID.Cloud;
                if (genRand.NextBool(2))
                {
                    num47 = TileID.RainCloud;
                }
                for (int j = num44 - num46; j <= num44 + num46; j++)
                {
                    for (int k = num45 - num46; k <= num45 + num46; k++)
                    {
                        double num50 = Math.Abs(j - num44);
                        double num51 = Math.Abs(k - num45) * 2;
                        if (Math.Sqrt(num50 * num50 + num51 * num51) < (num46 + genRand.Next(-1, 2)))
                        {
                            Tile tile = Main.tile[j, k];
                            tile.HasTile = true;
                            Main.tile[j, k].TileType = (ushort)num47;
                            WorldGen.SquareTileFrame(j, k);
                        }
                    }
                }
                for (int j = num44 - num46 + 2; j <= num44 + num46 - 2; j++)
                {
                    int num53;
                    for (num53 = num45 - num46; !Main.tile[j, num53].HasTile; num53++)//decompilation wackness probably
                    {
                    }
                    if (WorldGen.WillWaterPlacedHereStayPut(j, num53))
                    {
                        Tile tile = Main.tile[j, num53];
                        tile.HasTile = false;
                        Main.tile[j, num53].LiquidAmount = byte.MaxValue;
                        WorldGen.SquareTileFrame(j, num53);
                    }
                }
            }
        }

        private static void PutWaterOnStuff(UnifiedRandom genRand, int num4, int num5, int num6, int num7)
        {
            for (int l = num4; l <= num5; l++)
            {
                int num39;
                for (num39 = num6 - 10; !Main.tile[l, num39 + 1].HasTile; num39++)//decomp wackness probably
                {
                }
                if (num39 >= num7 || Main.tile[l, num39 + 1].TileType != 189)
                {
                    continue;
                }
                if (genRand.NextBool(10))
                {
                    int num40 = genRand.Next(1, 3);
                    for (int k = l - num40; k <= l + num40; k++)
                    {
                        if (Main.tile[k, num39].TileType == 189 && WorldGen.WillWaterPlacedHereStayPut(k, num39))
                        {

                            Tile tile = Main.tile[k, num39];
                            tile.HasTile = false;
                            Main.tile[k, num39].LiquidAmount = byte.MaxValue;
                            //Main.tile[num41, num39].LiquidType = /* tModPorter Suggestion: LiquidType = ... */(lava: false);
                            WorldGen.SquareTileFrame(l, num39);
                        }
                        if (Main.tile[k, num39 + 1].TileType == 189 && WorldGen.WillWaterPlacedHereStayPut(k, num39 + 1))
                        {
                            Tile tile = Main.tile[k, num39 + 1];
                            tile.HasTile = false;
                            Main.tile[k, num39 + 1].LiquidAmount = byte.MaxValue;
                            //Main.tile[num41, num39 + 1].lava/* tModPorter Suggestion: LiquidType = ... */(lava: false);
                            WorldGen.SquareTileFrame(l, num39 + 1);
                        }
                        if (k > l - num40 && k < l + 2 && Main.tile[k, num39 + 2].TileType == 189 && WorldGen.WillWaterPlacedHereStayPut(k, num39 + 2))
                        {
                            Tile tile = Main.tile[k, num39 + 2];
                            tile.HasTile = false;
                            Main.tile[k, num39 + 2].LiquidAmount = byte.MaxValue;
                            //Main.tile[num41, num39 + 2].lava/* tModPorter Suggestion: LiquidType = ... */(lava: false);
                            WorldGen.SquareTileFrame(l, num39 + 2);
                        }
                    }
                }
                if (genRand.NextBool(5) && WorldGen.WillWaterPlacedHereStayPut(l, num39))
                {
                    Main.tile[l, num39].LiquidAmount = byte.MaxValue;
                }
                //Tile tile2 = Main.tile[l, num39];
                //tile2/* tModPorter Suggestion: LiquidType = ... */(lava: false);
                WorldGen.SquareTileFrame(l, num39);
            }
        }

        private static void GenerateFloatingIslandBase(UnifiedRandom genRand, ref double seed100_150, ref double num2, ref double seed20_30, ref int num4, ref int num5, ref int num6, ref int num7, ref Vector2D val, ref Vector2D val2)
        {
            while (seed100_150 > 0.0 && seed20_30 > 0.0)
            {
                seed100_150 -= genRand.Next(4);
                seed20_30 -= 0.5f;
                int startX = (int)(val.X - seed100_150 * 0.5);
                int endX = (int)(val.X + seed100_150 * 0.5);
                int startY = (int)(val.Y - seed100_150 * 0.5);
                int endY = (int)(val.Y + seed100_150 * 0.5);
                if (startX < 0)
                {
                    startX = 0;
                }
                if (endX > Main.maxTilesX)
                {
                    endX = Main.maxTilesX;
                }
                if (startY < 0)
                {
                    startY = 0;
                }
                if (endY > Main.maxTilesY)
                {
                    endY = Main.maxTilesY;
                }
                num2 = seed100_150 * genRand.Next(80, 120) * 0.01;
                double num12 = val.Y + 1.0;
                for (int k = startX; k < endX; k++)
                {
                    if (genRand.NextBool(2))
                    {
                        num12 += genRand.Next(-1, 2);
                    }
                    if (num12 < val.Y)
                    {
                        num12 = val.Y;
                    }
                    if (num12 > val.Y + 2.0)
                    {
                        num12 = val.Y + 2.0;
                    }
                    for (int l = startY; l < endY; l++)
                    {
                        if (!(l > num12))
                        {
                            continue;
                        }
                        double num13 = Math.Abs(k - val.X);
                        double num14 = Math.Abs(l - val.Y) * 3.0;
                        if (Math.Sqrt(num13 * num13 + num14 * num14) < num2 * 0.4)
                        {
                            if (k < num4)
                            {
                                num4 = k;
                            }
                            if (k > num5)
                            {
                                num5 = k;
                            }
                            if (l < num6)
                            {
                                num6 = l;
                            }
                            if (l > num7)
                            {
                                num7 = l;
                            }
                            Tile tile = Main.tile[k, l];
                            tile.HasTile = true;
                            tile.TileType = 189;
                            WorldGen.SquareTileFrame(k, l);
                        }
                    }
                }
                //at this point it has generated part of an ellipse made out of clouds with a rough cut
                val += val2;
                val2.X += genRand.Next(-20, 21) * 0.05;
                val2.X = MathHelper.Clamp((float)val2.X, -1, 1);
                val2.Y = MathHelper.Clamp((float)val2.Y, -2, 2);
            }
        }
    }
}
