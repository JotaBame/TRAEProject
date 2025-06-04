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
using Terraria.WorldBuilding;

namespace TRAEProject.NewContent.Structures.EchosphereGen
{
    public class EchosphereGeneratorSystem : ModSystem
    {
        public static Vector2 echosphereCenter;
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {

        }

        public override void PostWorldGen()
        {
            int upperPadding = 40;
            int structureSpacing = 30;
            StructureData[] asteroids = EchosphereGenHelper.GetAsteroidStructureData();
            StructureData[] temples = EchosphereGenHelper.GetTempleStructureData();
            Main.NewText(Main.LocalPlayer.Center);
            int side = WorldGen.genRand.Next(2) * 2 - 1;

            float minX = Main.maxTilesX * 0.666f;
            float maxX = Main.maxTilesX * 0.9f;//due to algorithm reasons, these are not the actual bound box of the biome, but rather are used to calculate the centering point
            if(side == -1)
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
            Point16 currentGenPoint = new Point16(0, WorldGen.genRand.Next(minY, maxY));
            List<StructureData> templesLeftToAdd = temples.ToList();
            int asteroidsToAdd = Main.rand.Next(3);//left most

            //populate this list, then calculate a bounding box of everything, and move all the points to place so that the overall bounding box is on the center
            List<(Point16 pointToPlace, StructureData structure)> placingData = new();
            while (templesLeftToAdd.Count > 0)
            {
                if (asteroidsToAdd > 0)//initial value can be 0
                {
                    for (int i = 0; i < asteroidsToAdd; i++)
                    {
                        StructureData asteroidToGenerate = asteroids[WorldGen.genRand.Next(asteroids.Length)];
                        placingData.Add((currentGenPoint, asteroidToGenerate));
                        currentGenPoint = EchosphereGenHelper.GenerateNextPosition(currentGenPoint, structureSpacing, minY, maxY);
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

            //one extra step to generate a  List<Rectangle> of structure bounding boxes, then use those to find intersecting floating islands.
            //if found, either move the floating islands or move the echosphere structure
            //need to check if the moving wont put it out of bounds of the map (check for above first, if not enough space, move below
            if (placingData.Count > 0)
            {
                for (int i = 0; i < placingData.Count; i++)
                {
                    (Point16 pos, StructureData strData) = placingData[i];
                    pos = new Point16(pos.X + offsetForCenteringX + widthOfPlacedStructures, pos.Y + offsetForCenteringY);
                    Generator.GenerateFromData(strData, pos);
                    widthOfPlacedStructures += strData.width;//compensate so the generation of the next asteroid starts on
                    boundBoxMaxX += strData.width;
                }
            }
            centerPointX = (boundBoxMinX + boundBoxMaxX) * .5f;
            EchosphereGeneratorSystem.echosphereCenter = new Vector2(centerPointX * 16, centerPointY * 16);

            //afterwards scan bounding box for ores and bars and switch them if needed
        }
    }
    public class EchospherePass : GenPass
    {
        public EchospherePass(string name, double loadWeight) : base(name, loadWeight)
        {
            Name = name;
            Weight = loadWeight;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {

        }
    }
}
