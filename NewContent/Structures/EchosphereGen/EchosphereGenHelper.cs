using Microsoft.Xna.Framework;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace TRAEProject.NewContent.Structures.EchosphereGen
{
    public static class EchosphereGenHelper
    {
        public static Point16 HalfSize(this StructureData structure)
        {
            return new Point16(structure.width / 2, structure.height / 2);
        }
        public static string[] GetAsteroidPaths()
        {
            int count = 8;
            string[] result = new string[count];
            for (int i = 0; i < count; i++)
            {
                //
                result[i] = "NewContent/Structures/EchosphereGen/StructureData/EchosphereAsteroid" + i.ToString();
            }
            return result;
        }
        public static StructureData[] GetAsteroidStructureData()
        {
            string[] paths = GetAsteroidPaths();
            StructureData[] result = new StructureData[paths.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Generator.GetStructureData(paths[i], TRAEProj.Instance);
            }
            return result;
        }
        public static Point16 GenerateNextPosition(Point16 currentPos, int structureSpacing, int minY, int maxY)
        {
            return new Point16(currentPos.X + structureSpacing, WorldGen.genRand.Next(minY, maxY));
        }
        public static void GenerateNextPosition_Old(Point16 currentPos, Point16 echosphereCenter, int structureSpacing, StructureData structureToCalculatePositionFor, int minY, int maxY, out Point16 generationPoint)
        {
            Vector2 currentPosVec2 = currentPos.ToVector2();
            
            Point16 next = new Point16(currentPos.X + structureSpacing, Main.rand.Next(minY, maxY));
            Vector2 toNext = next.ToVector2() - currentPosVec2;
            toNext = Vector2.Normalize(toNext) * structureSpacing;
            toNext.X += structureToCalculatePositionFor.width;
            next = (currentPosVec2 + toNext).ToPoint16();
            generationPoint = next;
        }
        public static void AppendRandomAsteroidToList(List<StructureData> structureList, StructureData[] asteroids, int count)
        {
            for (int i = 0; i < count; i++)
            {
                structureList.Add(asteroids[WorldGen.genRand.Next(asteroids.Length)]);
            }
        }  
        public static string[] GetTemplePaths()
        { 
            int count = 5;
            string[] result = new string[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = "NewContent/Structures/EchosphereGen/StructureData/EchosphereTemple" + i.ToString();
            }
            return result;
        }
        public static StructureData[] GetTempleStructureData()
        {
            string[] paths = GetTemplePaths();
            StructureData[] result = new StructureData[paths.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Generator.GetStructureData(paths[i], TRAEProj.Instance);
            }
            return result;
        }
    }
}