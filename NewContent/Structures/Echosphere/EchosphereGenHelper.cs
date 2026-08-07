using Microsoft.Xna.Framework;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TRAEProject.NewContent.Structures.Echosphere.Generation;

namespace TRAEProject.NewContent.Structures.Echosphere
{
    public static class EchosphereGenHelper
    {
        public static (short frameX, short frameY)[] GetPaintingsFrameData3x3()
        {
            //sheet is 36 paintings wide
            const short PaintingSize = 3 * 18;
            //const int PaintingsInSheetX = 36;
          //  const int PaintingsInSheetY = 3;
            (short frameX, short frameY)[] paintingIDs = new (short frameX, short frameY)[] {  (4,2), (6,2),(8,2), (9,2),(10,2), (18,2) };
            for (int i = 0; i < paintingIDs.Length; i++)
            {
                (short frameX, short frameY) framing = paintingIDs[i];
                framing.frameX *= PaintingSize;
                framing.frameY *= PaintingSize;
                paintingIDs[i] = framing;
            }
            return paintingIDs;
        }
        public static void PlaceRandomPainting3x3(short x, short y)
        {
            (short frameX, short frameY)[] ids = GetPaintingsFrameData3x3();
            (short frameX, short frameY) paintingFraming = ids[Main.rand.Next(ids.Length - 1)];
            for (short i = 0; i < 3; i++)
            {
                for (short j = 0; j < 3; j++)
                {
                    Tile tile = Main.tile[x + i, y + j];
                    tile.ClearTile();
                    tile.ClearBlockPaintAndCoating();
                    tile.HasTile = true;
                    tile.TileType = TileID.Painting3X3;
                    tile.TileFrameX = (short)(paintingFraming.frameX + i * 18);
                    tile.TileFrameY = (short)(paintingFraming.frameY + j * 18);
                }
            }
        }
        public static (short frameX, short frameY)[] GetPaintingsFrameData3x2()
        {
            //sheet is 36 paintings wide
            const short PaintingSizeX = 3 * 18;
            const short PaintingSizeY = 2 * 18;
            (short frameX, short frameY)[] paintingIDs = new (short frameX, short frameY)[] { (0, 6), (0, 9), (0, 15), (0, 33), (0, 36) };
            for (int i = 0; i < paintingIDs.Length; i++)
            {
                (short frameX, short frameY) framing = paintingIDs[i];
                framing.frameX *= PaintingSizeX;
                framing.frameY *= PaintingSizeY;
                paintingIDs[i] = framing;
            }
            return paintingIDs;
        }
        public static void PlaceRandomPainting3x2(short x, short y)
        {
            (short frameX, short frameY)[] ids = GetPaintingsFrameData3x2();
            (short frameX, short frameY) paintingFraming = ids[Main.rand.Next(ids.Length - 1)];
            for (short i = 0; i < 3; i++)
            {
                for (short j = 0; j < 2; j++)
                {
                    Tile tile = Main.tile[x + i, y + j];
                    tile.ClearTile();
                    tile.ClearBlockPaintAndCoating();
                    tile.HasTile = true;
                    tile.TileType = TileID.Painting3X2;
                    tile.TileFrameX = (short)(paintingFraming.frameX + i * 18);
                    tile.TileFrameY = (short)(paintingFraming.frameY + j * 18);
                }
            }
        }
        public static (short frameX, short frameY)[] GetPaintingsFrameData6x4()
        {
            const short PaintingSizeX = 6 * 18;
            const short PaintingSizeY = 4 * 18;
            (short frameX, short frameY)[] paintingIDs = new (short frameX, short frameY)[] { (2, 0), (2, 1), (2, 5), (2, 7), (0, 12), (1, 25) };
            for (int i = 0; i < paintingIDs.Length; i++)
            {
                (short frameX, short frameY) framing = paintingIDs[i];
                framing.frameX *= PaintingSizeX;
                framing.frameY *= PaintingSizeY;
                paintingIDs[i] = framing;
            }
            return paintingIDs;
        }
        public static void PlaceRandomPainting6x4(short x, short y)
        {
            (short frameX, short frameY)[] ids = GetPaintingsFrameData6x4();
            (short frameX, short frameY) paintingFraming = ids[Main.rand.Next(ids.Length - 1)];
            for (short i = 0; i < 6; i++)
            {
                for (short j = 0; j < 4; j++)
                {
                    Tile tile = Main.tile[x + i, y + j];
                    tile.ClearTile();
                    tile.ClearBlockPaintAndCoating();
                    tile.HasTile = true;
                    tile.TileType = TileID.Painting6X4;
                    tile.TileFrameX = (short)(paintingFraming.frameX + i * 18);
                    tile.TileFrameY = (short)(paintingFraming.frameY + j * 18);
                }
            }
        }
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
                result[i] = "NewContent/Structures/Echosphere/StructureData/EchosphereAsteroid" + i.ToString();
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
                result[i] = "NewContent/Structures/Echosphere/StructureData/EchosphereTemple" + i.ToString();
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

        public static void Debug_ClearTop200Tiles()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    Tile tile = Main.tile[i, j];
                    tile.ClearEverything();
                }
            }
        }
        public static void FloodFillOreVein(int i, int j, out int[] filledIs, out int[] filledJs)
        {
            int dummyOreTile = ModContent.TileType<EchosphereGenDummyOreTile>();
            ushort typeToFillWith = (ushort)(Main.rand.NextBool() ? GenVars.silver : GenVars.gold);
            const int maxFillCount = 200;//failsafe value

            List<int> isList = new List<int>();
            List<int> jsList = new List<int>();

            bool[,] visited = new bool[Main.maxTilesX, Main.maxTilesY];
            Queue<Point> queue = new Queue<Point>();

            void TryEnqueue(int x, int y)
            {
                if (x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY &&
                    !visited[x, y] && Main.tile[x, y].HasTile && Main.tile[x, y].TileType == dummyOreTile)
                {
                    queue.Enqueue(new Point(x, y));
                    visited[x, y] = true;
                }
            }

            TryEnqueue(i, j);
            int fillCount = 0;

            while (queue.Count > 0 && fillCount < maxFillCount)
            {
                Point current = queue.Dequeue();
                int x = current.X;
                int y = current.Y;

                Main.tile[x, y].TileType = typeToFillWith;

                isList.Add(x);
                jsList.Add(y);
                fillCount++;

                TryEnqueue(x + 1, y);
                TryEnqueue(x - 1, y);
                TryEnqueue(x, y + 1);
                TryEnqueue(x, y - 1);
                TryEnqueue(x + 1, y + 1);
                TryEnqueue(x + 1, y - 1);
                TryEnqueue(x - 1, y + 1);
                TryEnqueue(x - 1, y - 1);
            }

            filledIs = isList.ToArray();
            filledJs = jsList.ToArray();
        }

        public static void Debug_SpawnDustAt(int[] iPositions, int[] jPositions)
        {
            for (int index = 0; index < iPositions.Length && index < jPositions.Length; index++)
            {
                int i = iPositions[index];
                int j = jPositions[index];

                Vector2 worldPosition = new Vector2(i * 16 + 8, j * 16 + 8);

                Dust.QuickDust(worldPosition, Color.Yellow);
            }
        }

        public static void PlaceRandomStatue(short i, short j)
        {
            WorldGen.SetupStatueList();
            int statueIndex =WorldGen.genRand.Next(2, GenVars.statueList.Length);
            WorldGen.PlaceTile(i, j + 2, GenVars.statueList[statueIndex].X, mute: true, forced: true, -1, GenVars.statueList[statueIndex].Y);
        }

        public static void PlaceChest(short i, short j, ref int chestsCreatedSoFar, ref int[] shuffledPrimaryLootTable)
        {
            int index = WorldGen.PlaceChest(i, j + 1, 21, false, 4);
            if (index >= 0 && index < Main.maxChests)
            {
                FillChest(chestsCreatedSoFar, Main.chest[index],ref shuffledPrimaryLootTable);
            }
            chestsCreatedSoFar++;
        }
        public static void FillChest(int chestsCreatedSoFar, Chest chest, ref int[] shuffledPrimaryLootTable)
        {
            Item[] loot = EchosphereLootTableGenerator.GetChestLoot(ref shuffledPrimaryLootTable, chestsCreatedSoFar);
            TransferLootTableToChest(loot, chest);
        }
        public static void TransferLootTableToChest(Item[] lootTable, Chest chest)
        {
            int itemsTransferred = 0;
            for (int i = 0; i < chest.item.Length; i++)
            {
                if (chest.item[i].IsAir)
                {
                    chest.item[i] = lootTable[itemsTransferred];
                    itemsTransferred++;
                    if (itemsTransferred >= lootTable.Length)
                    {
                        break;
                    }
                }
            }
        }
    }
}