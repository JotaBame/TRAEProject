using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace TRAEProject.NewContent.Structures.EchosphereGen
{
    public class EchosphereGeneratorSystem : ModSystem
    {
        public override void Load()
        {
            On_WorldGen.CloudIsland += DontGenVanillaCloudIsland;
            On_WorldGen.CloudLake += DontGenVanillaCloudLake;
        }

        private void DontGenVanillaCloudLake(On_WorldGen.orig_CloudLake orig, int i, int j)
        {
        }

        private void DontGenVanillaCloudIsland(On_WorldGen.orig_CloudIsland orig, int i, int j)
        {

        }
        public static Vector2 echosphereTopLeft;
        public static Vector2 echosphereBottomRight;
        public static Vector2 echosphereCenter;
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            //for (int i = 0; i < tasks.Count; i++)
            //{
            //    GenPass task = tasks[i];
            //    if(task != null && task.Name == "Floating Islands")
            //    {
            //        task.Disable();
            //        tasks.RemoveAt(i);
            //        i--;
            //    }
            //}
        }

        public override void PostWorldGen()
        {
            Console.WriteLine("post world gen called");
            int upperPadding = 40;
            int structureSpacing = 30;
            StructureData[] asteroids = EchosphereGenHelper.GetAsteroidStructureData();
            StructureData[] temples = EchosphereGenHelper.GetTempleStructureData();
            Main.NewText(Main.LocalPlayer.Center);
            int side = WorldGen.genRand.Next(2) * 2 - 1;

            float minX = Main.maxTilesX * 0.666f;
            float maxX = Main.maxTilesX * 0.9f;//due to algorithm reasons, these are not the actual bound box of the biome, but rather are used to calculate the centering point
            if (side == -1)
            {
                minX = Main.maxTilesX * 0.1f;
                maxX = Main.maxTilesX * 0.333f;
            }
            float middleX = (minX + maxX) * .5f;
            int minY = upperPadding + Main.offLimitBorderTiles;
            int maxY = (int)(Main.worldSurface * 0.25f);
            Main.NewText(Main.maxTilesY);
            Main.NewText($"echosphere bounds Y: {minY}, {maxY}");
            Main.NewText(Main.LocalPlayer.Center.ToTileCoordinates16());
            int middleY = (maxY + minY) / 2;
            Point16 echosphereCenter = new((int)middleX, middleY);
            Point16 currentGenPoint = new(0, WorldGen.genRand.Next(minY, maxY));
            List<StructureData> templesLeftToAdd = temples.ToList();
            int asteroidsToAdd = Main.rand.Next(3);//left most
            bool initialAsteroidsIs0 = asteroidsToAdd == 0;
            //populate this list, then calculate a bounding box of everything, and move all the points to place so that the overall bounding box is on the center
            List<(Point16 pointToPlace, StructureData structure)> placingData = new();
            while (templesLeftToAdd.Count > 0)
            {
                if (asteroidsToAdd > 0 || initialAsteroidsIs0)//initial value can be 0
                {
                    if (asteroidsToAdd > 0)
                    {
                        for (int i = 0; i < asteroidsToAdd; i++)
                        {
                            StructureData asteroidToGenerate = asteroids[WorldGen.genRand.Next(asteroids.Length)];
                            placingData.Add((currentGenPoint, asteroidToGenerate));
                            currentGenPoint = EchosphereGenHelper.GenerateNextPosition(currentGenPoint, structureSpacing, minY, maxY);
                        }
                    }
                    asteroidsToAdd = Main.rand.Next(2, 4);//how many asteroids  between the current and next temple
                }
                int templeIndexToAdd = WorldGen.genRand.Next(templesLeftToAdd.Count);
                StructureData templeToGenerate = templesLeftToAdd[templeIndexToAdd];
                placingData.Add((currentGenPoint, templeToGenerate));
                currentGenPoint = EchosphereGenHelper.GenerateNextPosition(currentGenPoint, structureSpacing, minY, maxY);
                templesLeftToAdd.RemoveAt(templeIndexToAdd);
            }


            //calculating overall bounding box here
            int boundBoxMinX = 99999999;
            int boundBoxMaxX = -1;
            int boundBoxMaxY = -1;
            int boundBoxMinY = 99999999;
            for (int i = 0; i < placingData.Count; i++)
            {
                boundBoxMinX = (int)MathF.Min(boundBoxMinX, placingData[i].pointToPlace.X);
                boundBoxMinY = (int)MathF.Min(boundBoxMinY, placingData[i].pointToPlace.Y);
                boundBoxMaxX = (int)MathF.Max(boundBoxMaxX, placingData[i].pointToPlace.X + placingData[i].structure.width);
                boundBoxMaxY = (int)MathF.Max(boundBoxMaxY, placingData[i].pointToPlace.Y + placingData[i].structure.height);
            }
            float centerPointX = (boundBoxMinX + boundBoxMaxX) * .5f;
            float centerPointY = (boundBoxMinY + boundBoxMaxY) * .5f;
            int offsetForCenteringX = (int)(echosphereCenter.X - centerPointX);
            int offsetForCenteringY = (int)(echosphereCenter.Y - centerPointY);

            int widthOfPlacedStructures = 0;
            if (placingData.Count > 0)
            {
                for (int i = 0; i < placingData.Count; i++)
                {
                    (Point16 pos, StructureData strData) = placingData[i];
                    pos = new Point16(pos.X + offsetForCenteringX + widthOfPlacedStructures, pos.Y + offsetForCenteringY);
                    Generator.GenerateFromData(strData, pos);
                    widthOfPlacedStructures += strData.width;//compensate so the generation of the next asteroid starts on
                    if (i < placingData.Count - 1)
                    {
                        boundBoxMaxX += strData.width;
                    }
                }
            }
            boundBoxMaxX += offsetForCenteringX;
            boundBoxMaxY += offsetForCenteringY;
            boundBoxMinX += offsetForCenteringX;
            boundBoxMinY += offsetForCenteringY;
            centerPointY = (boundBoxMinY + boundBoxMaxY) * .5f;
            centerPointX = (boundBoxMinX + boundBoxMaxX) * .5f;
            //TODO: STORE BOTTOM RIGHT AND TOP LEFT IN WORLD FILE DATA
            EchosphereGeneratorSystem.echosphereCenter = new Vector2(centerPointX * 16 + 8, centerPointY * 16 + 8);
            EchosphereGeneratorSystem.echosphereBottomRight = new Vector2(boundBoxMaxX * 16 + 8, boundBoxMaxY * 16 + 8);
            EchosphereGeneratorSystem.echosphereTopLeft = new Vector2(boundBoxMinX * 16 + 8, boundBoxMinY * 16 + 8);
            
            PlacePaintingsStatuesChestsAndOres((short)boundBoxMaxX, (short)boundBoxMaxY, (short)boundBoxMinY, (short)boundBoxMinX);
            //PlacePaintingsStatuesChestsAndOres((short)Main.maxTilesX, (short)Main.maxTilesY, (short)0, (short)0);
        }
        private static void PlacePaintingsStatuesChestsAndOres(short boundBoxMaxX, short boundBoxMaxY, short boundBoxMinY, short boundBoxMinX)
        {
            int oreTileID = ModContent.TileType<EchosphereGenDummyOreTile>();
            int painting3x2TileID = ModContent.TileType<EchosphereGenDummyPaintingTile3x2>();
            int painting3x3TileID = ModContent.TileType<EchosphereGenDummyPaintingTile3x3>();
            int painting6x4TileID = ModContent.TileType<EchosphereGenDummyPaintingTile6x4>();
            int statueTileID = ModContent.TileType<EchosphereGenDummyStatueTile>();
            int chestTileID = ModContent.TileType<EchosphereGenDummyChestTile>();
            int chestsCreatedSoFar = 0;
            int[] shuffledPrimaryLootTable = EchosphereLootTableGenerator.GetShuffledPrimaryLootTable();
            for (short i = boundBoxMinX; i <= boundBoxMaxX; i++)
            {
                for (short j = boundBoxMinY; j <= boundBoxMaxY; j++)
                {
                    Tile tile = Main.tile[i, j];
                    int type = tile.TileType;
                    if (type == painting3x2TileID)
                    {
                        tile.HasTile = false;
                        EchosphereGenHelper.PlaceRandomPainting3x2(i, j);
                        continue;
                    }
                    if (type == painting3x3TileID)
                    {
                        tile.HasTile = false;
                        EchosphereGenHelper.PlaceRandomPainting3x3(i, j);
                        continue;
                    }
                    if (type == painting6x4TileID)
                    {
                        tile.HasTile = false;
                        EchosphereGenHelper.PlaceRandomPainting6x4(i, j);
                        continue;
                    }
                    if(type == statueTileID)
                    {
                        tile.HasTile = false;
                        EchosphereGenHelper.PlaceRandomStatue(i, j);
                        continue;
                    }
                    if(type == chestTileID)
                    {
                        tile.HasTile = false;
                        EchosphereGenHelper.PlaceChest(i, j, ref chestsCreatedSoFar, ref shuffledPrimaryLootTable);
                        if(chestsCreatedSoFar % shuffledPrimaryLootTable.Length == 0)
                        {
                            shuffledPrimaryLootTable = EchosphereLootTableGenerator.GetShuffledPrimaryLootTable();
                        }
                        continue;
                    }
                    if (type == oreTileID)
                    {
                        EchosphereGenHelper.FloodFillOreVein(i, j, out _, out _);
                        continue;
                    }
                }
            }
        }
        public const string echosphereTopLeftKey = "echosphereTopLeft";
        public const string echosphereBottomRightKey = "echosphereBottomRight";
        public override void LoadWorldData(TagCompound tag)
        {
            Vector2 topLeft = tag.Get<Vector2>(echosphereTopLeftKey);
            Vector2 bottomRight = tag.Get<Vector2>(echosphereBottomRightKey);
            Console.WriteLine("load world data called");
            Console.WriteLine("echosphere top left: " + topLeft);
            Console.WriteLine("echosphere bottom right: " + bottomRight);
            EchosphereGeneratorSystem.echosphereTopLeft = topLeft;
            EchosphereGeneratorSystem.echosphereBottomRight = bottomRight;
            EchosphereGeneratorSystem.echosphereCenter = (topLeft + bottomRight) * .5f;
        }
        public static void GetCorners(out Vector2 topLeft, out Vector2 topRight, out Vector2 bottomLeft, out Vector2 bottomRight)
        {
            topLeft = echosphereTopLeft;
            bottomRight = echosphereBottomRight;
            bottomLeft = new Vector2(topLeft.X, bottomRight.Y);
            topRight = new Vector2(bottomRight.X, topLeft.Y);
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add(echosphereTopLeftKey, EchosphereGeneratorSystem.echosphereTopLeft);
            tag.Add(echosphereBottomRightKey, EchosphereGeneratorSystem.echosphereBottomRight);
        }
        public override void ClearWorld()
        {
            Console.WriteLine("clear world called");
        }
    }
}
