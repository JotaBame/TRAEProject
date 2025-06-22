using Microsoft.Xna.Framework;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TRAEProject.NewContent.Structures.Echosphere.Generation;
using TRAEProject.NewContent.Structures.NewSkyIslands;

namespace TRAEProject.NewContent.Structures.Echosphere
{
    public class EchosphereGeneratorSystem : ModSystem
    {
        public static bool stopVanillaSkyIslandGen = false;
        public override void Load()
        {
            On_WorldGen.CloudIsland += DontGenVanillaCloudIsland;
            On_WorldGen.CloudLake += DontGenVanillaCloudLake;
            On_WorldGen.IslandHouse += DontGenIslandHouse;
        }
        private void DontGenIslandHouse(On_WorldGen.orig_IslandHouse orig, int i, int j, int islandStyle)
        {
            if (!stopVanillaSkyIslandGen)
            {
                orig(i, j, islandStyle);
            }
            //  WorldGen.PlaceTile(i, j, TileID.Sunplate);
        }

        private void DontGenVanillaCloudLake(On_WorldGen.orig_CloudLake orig, int i, int j)
        {
            if (!stopVanillaSkyIslandGen)
            {
                orig(i, j);
            }
        }

        private void DontGenVanillaCloudIsland(On_WorldGen.orig_CloudIsland orig, int i, int j)
        {
            if (!stopVanillaSkyIslandGen)
            {
                orig(i, j);
            }
            // WorldGen.PlaceTile(i, j, TileID.Dirt);
        }
        public static Vector2 echosphereTopLeft;
        public static Vector2 echosphereBottomRight;
        public static Vector2 echosphereCenter;

        public static void Setup(out int boundBoxMaxX, out int boundBoxMaxY, out int boundBoxMinX, out int boundBoxMinY, out List<(Point16 pointToPlace, StructureData structure)> placingData)
        {
            int upperPadding = 40;
            int structureSpacing = 30;
            StructureData[] asteroids = EchosphereGenHelper.GetAsteroidStructureData();
            StructureData[] temples = EchosphereGenHelper.GetTempleStructureData();
            // Main.NewText(Main.LocalPlayer.Center);
            int side = WorldGen.genRand.Next(2) * 2 - 1;

            float minX = Main.maxTilesX * 0.6f;
            float maxX = Main.maxTilesX * 0.8f;//due to algorithm reasons, these are not the actual bound box of the biome, but rather are used to calculate the centering point
            if (side == -1)
            {
                minX = Main.maxTilesX * 0.2f;
                maxX = Main.maxTilesX * 0.4f;
            }
            float middleX = (minX + maxX) * .5f;
            int minY = upperPadding + Main.offLimitBorderTiles;
            int maxY = (int)(Main.worldSurface * 0.2f);
            //Main.NewText(Main.maxTilesY);
            // Main.NewText($"echosphere bounds Y: {minY}, {maxY}");
            // Main.NewText(Main.LocalPlayer.Center.ToTileCoordinates16());
            int middleY = (maxY + minY) / 2;
            Point16 echosphereCenter = new((int)middleX, middleY);
            Point16 currentGenPoint = new(0, WorldGen.genRand.Next(minY, maxY));
            List<StructureData> templesLeftToAdd = temples.ToList();
            int asteroidsToAdd = Main.rand.Next(3);//left most
            bool initialAsteroidsIs0 = asteroidsToAdd == 0;
            //populate this list, then calculate a bounding box of everything, and move all the points to place so that the overall bounding box is on the center
            placingData = new();
            while (templesLeftToAdd.Count > 0)
            {
                if (asteroidsToAdd > 0 || initialAsteroidsIs0)//initial value can be 0
                {
                    initialAsteroidsIs0 = false;
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
            boundBoxMinX = 99999999;
            boundBoxMaxX = -1;
            boundBoxMaxY = -1;
            boundBoxMinY = 99999999;
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
                    // Generator.GenerateFromData(strData, pos);
                    placingData[i] = (pos, strData);//update placingdata with modified value
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
            EchosphereGeneratorSystem.echosphereCenter = new Vector2(centerPointX * 16 + 8, centerPointY * 16 + 8);
            EchosphereGeneratorSystem.echosphereBottomRight = new Vector2(boundBoxMaxX * 16 + 8, boundBoxMaxY * 16 + 8);
            EchosphereGeneratorSystem.echosphereTopLeft = new Vector2(boundBoxMinX * 16 + 8, boundBoxMinY * 16 + 8);

            //PlacePaintingsStatuesChestsAndOres((short)Main.maxTilesX, (short)Main.maxTilesY, (short)0, (short)0);

            stopVanillaSkyIslandGen = false;
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
                    if (type == statueTileID)
                    {
                        tile.HasTile = false;
                        EchosphereGenHelper.PlaceRandomStatue(i, j);
                        continue;
                    }
                    if (type == chestTileID)
                    {
                        tile.HasTile = false;
                        EchosphereGenHelper.PlaceChest(i, j, ref chestsCreatedSoFar, ref shuffledPrimaryLootTable);
                        if (chestsCreatedSoFar % shuffledPrimaryLootTable.Length == 0)
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
            //Console.WriteLine("load world data called");
            //Console.WriteLine("echosphere top left: " + topLeft);
            //Console.WriteLine("echosphere bottom right: " + bottomRight);
            EchosphereGeneratorSystem.echosphereTopLeft = topLeft;
            EchosphereGeneratorSystem.echosphereBottomRight = bottomRight;
            EchosphereGeneratorSystem.echosphereCenter = (topLeft + bottomRight) * .5f;
        }
        public override void PreWorldGen()
        {
            stopVanillaSkyIslandGen = true;
        }
        public static void GetCorners(out Vector2 topLeft, out Vector2 topRight, out Vector2 bottomLeft, out Vector2 bottomRight)
        {
            topLeft = echosphereTopLeft;
            bottomRight = echosphereBottomRight;
            bottomLeft = new Vector2(topLeft.X, bottomRight.Y);
            topRight = new Vector2(bottomRight.X, topLeft.Y);
        }
        public static Rectangle GetTileCoordsBoundingBox()
        {
            Vector2 topLeft = echosphereTopLeft;
            Vector2 bottomRight = echosphereBottomRight;
            topLeft.X -= 8;
            topLeft.Y -= 8;
            bottomRight.X -= 8;
            bottomRight.Y -= 8;

            int width = (int)(bottomRight.X - topLeft.X) / 16;
            int height = (int)(bottomRight.Y - topLeft.Y) / 16;
            return new Rectangle((int)topLeft.X / 16, (int)topLeft.Y / 16, width, height);
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add(echosphereTopLeftKey, EchosphereGeneratorSystem.echosphereTopLeft);
            tag.Add(echosphereBottomRightKey, EchosphereGeneratorSystem.echosphereBottomRight);
        }
        public override void ClearWorld()
        {
            stopVanillaSkyIslandGen = true;
            //not sure if should reset those center values here...
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            bool insertedEchosphereSetupAndNewSkyIslandPass = false;
            bool insertedNewHousePass = false;
            bool insertedEchospherePlacementPass = false;
            for (int i = 0; i < tasks.Count; i++)
            {
                GenPass pass = tasks[i];
                if (pass.Name == "Floating Islands" && !insertedEchosphereSetupAndNewSkyIslandPass)
                {
                    insertedEchosphereSetupAndNewSkyIslandPass = true;
                    tasks.Insert(i + 1, new EchosphereSetupGenPass("Echosphere Setup", 100f));//no idea what weight is
                    tasks.Insert(i + 2, new NewSkyIslandsGenPass("Reworked Floating Islands", 100f));//no idea what weight is
                    //pass.Disable();//don't disable because it sets some importatn variables that the echospherre and reworked islands rely on
                }
                else if (pass.Name == "Floating Island Houses" && !insertedNewHousePass)
                {
                    insertedNewHousePass = true;
                    tasks.Insert(i + 1, new NewIskyIslandsHousesGenPass("Reworked Floating Island Houses", 100f));
                    pass.Disable();
                }
                else if (pass.Name == "Micro Biomes" && !insertedEchospherePlacementPass)
                {
                    insertedEchospherePlacementPass = true;
                    tasks.Insert(i + 1, new EchosphereGenPass("Echosphere", 100f));
                }
                //DEBUG THING!!!
                else if (!pass.Name.Contains("Floating") && pass.Name != "Echosphere")
                {
                    // pass.Disable();
                }
            }
        }
        public static void Generate(List<(Point16 pointToPlace, StructureData structure)> placingData)
        {
            if (placingData == null || placingData.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < placingData.Count; i++)
            {
                (Point16 pos, StructureData strData) = placingData[i];
                Generator.GenerateFromData(strData, pos);
            }
            short boundBoxMaxX = (short)(echosphereBottomRight.X / 16);
            short boundBoxMaxY = (short)(echosphereBottomRight.Y / 16);
            short boundBoxMinX = (short)(echosphereTopLeft.X / 16);
            short boundBoxMinY = (short)(echosphereTopLeft.Y / 16);
            PlacePaintingsStatuesChestsAndOres(boundBoxMaxX, boundBoxMaxY, boundBoxMinY, boundBoxMinX);
        }
    }
    public class EchosphereSetupGenPass : GenPass
    {
        public static List<(Point16 pointToPlace, StructureData structure)> placingData;
        public EchosphereSetupGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Echosphere Setup";
            EchosphereGeneratorSystem.Setup(out _, out _, out _, out _, out placingData);
        }
    }
    public class EchosphereGenPass : GenPass
    {
        public EchosphereGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Echosphere";
            EchosphereGeneratorSystem.Generate(EchosphereSetupGenPass.placingData);
            EchosphereSetupGenPass.placingData = null;
        }
    }
}
