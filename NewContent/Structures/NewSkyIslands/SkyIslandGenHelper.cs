using Microsoft.Xna.Framework;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TRAEProject.Common.LootTableGenerator;
using TRAEProject.NewContent.Structures.Echosphere;
using TRAEProject.NewContent.Structures.Echosphere.Generation;
using TRAEProject.NewContent.Structures.NewSkyIslands;

namespace TRAEProject.NewContent.Structures.NewSkyIslands
{
    public static class SkyIslandGenHelper
    {
        public static (short frameX, short frameY)[] GetPaintingsFrameData3x3()
        {
            (short frameX, short frameY)[] paintingIDs = new (short frameX, short frameY)[] { (1080, 108), (648, 108) };
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
            //sheet is 36 
            (short frameX, short frameY)[] paintingIDs = new (short frameX, short frameY)[] { (0, 540), (0, 756) };
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
            (short frameX, short frameY)[] paintingIDs = new (short frameX, short frameY)[] { (216, 576), (216, 0), (0, 864) };
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
        public static (short frameX, short frameY)[] GetPaintingsFrameData2x3()
        {
            (short frameX, short frameY)[] paintingIDs = new (short frameX, short frameY)[] { (936, 0), (900, 0) };
            return paintingIDs;
        }
        public static void PlaceRandomPainting2x3(short x, short y)
        {
            (short frameX, short frameY)[] ids = GetPaintingsFrameData2x3();
            (short frameX, short frameY) paintingFraming = ids[Main.rand.Next(ids.Length - 1)];
            for (short i = 0; i < 2; i++)
            {
                for (short j = 0; j < 3; j++)
                {
                    Tile tile = Main.tile[x + i, y + j];
                    tile.ClearTile();
                    tile.ClearBlockPaintAndCoating();
                    tile.HasTile = true;
                    tile.TileType = TileID.Painting2X3;
                    tile.TileFrameX = (short)(paintingFraming.frameX + i * 18);
                    tile.TileFrameY = (short)(paintingFraming.frameY + j * 18);
                }
            }
        }
        private static void ClearDoorSpace(int i, int j, int side)
        {
            //i,j is coords to bottom tile of the door
            //side is -1 if left side scan and 1 if right side scan
            for (int k = 1; k <= 3; k++)
            {
                for (int l = -2; l <= 0; l++)
                {
                    int x = i + k * side;
                    int y = j + l;
                    Tile t = Main.tile[x, y];
                    Tile tileBelow = Main.tile[x, y + 1];
                    tileBelow.WallType = 0;//remove wall
                    t.WallType = 0;
                    if (t.HasUnactuatedTile && Main.tileSolid[t.TileType] && !Main.tileSolidTop[t.TileType] && !TileID.Sets.Platforms[t.TileType])
                    {
                        t.HasTile = false;
                    }
                }
            }
        }
        public static void CheckForOreAndPaintings(short boundBoxMaxX, short boundBoxMaxY, short boundBoxMinY, short boundBoxMinX)
        {
            // return;//DEBUGG
            int oreTileID = ModContent.TileType<EchosphereGenDummyOreTile>();
            int painting3x2TileID = ModContent.TileType<EchosphereGenDummyPaintingTile3x2>();
            int painting3x3TileID = ModContent.TileType<EchosphereGenDummyPaintingTile3x3>();
            int painting2x3TileID = ModContent.TileType<EchosphereGenDummyPaintingTile2x3>();
            int painting6x4TileID = ModContent.TileType<EchosphereGenDummyPaintingTile6x4>();
            int doorTileID = TileID.ClosedDoor;

            for (short i = boundBoxMinX; i <= boundBoxMaxX; i++)
            {
                int side = i < (boundBoxMaxX + boundBoxMinX) * .5f ? -1 : 1;
                for (short j = boundBoxMinY; j <= boundBoxMaxY; j++)
                {
                    Tile tile = Main.tile[i, j];
                    int type = tile.TileType;
                    if (!tile.HasTile)
                    {
                        continue;
                    }
                    if (type == doorTileID)
                    {
                        Tile tileBelow = Main.tile[i, j + 1];
                        if (tileBelow.TileType != doorTileID)//if the bottom of the door
                        {
                            ClearDoorSpace(i, j, side);
                        }
                    }
                    if (type == painting3x2TileID)
                    {
                        tile.HasTile = false;
                        PlaceRandomPainting3x2(i, j);
                        continue;
                    }
                    if (type == painting3x3TileID)
                    {
                        tile.HasTile = false;
                        PlaceRandomPainting3x3(i, j);
                        continue;
                    }
                    if (type == painting6x4TileID)
                    {
                        tile.HasTile = false;
                        PlaceRandomPainting6x4(i, j);
                        continue;
                    }
                    if (type == oreTileID)
                    {
                        EchosphereGenHelper.FloodFillOreVein(i, j, out _, out _);
                        continue;
                    }
                    if (type == painting2x3TileID)
                    {
                        tile.HasTile = false;
                        PlaceRandomPainting2x3(i, j);
                    }
                }
            }
        }

        internal static bool TryPlaceChest(int newX, int newY, out Chest placedChest)
        {
            //water chets is style 17
            int index = WorldGen.PlaceChest(newX, newY, 21, false, 17);
            if(index == -1)
            {
                placedChest = null;
                return false;
            }
            placedChest = Main.chest[index];
            return true;
        }

        public static bool IsSkyLakeIndex(int i, out bool chestNearby)
        {
            int x = GenVars.floatingIslandHouseX[i];
            int y = GenVars.floatingIslandHouseY[i];
            if (x == 0 || y == 0)
            {
                chestNearby = true;
                return false;
            }
            int liquidCount = 0;
            int existingWaterChestCheckHalfSize = 40;
            chestNearby = false;
            for (int k = -existingWaterChestCheckHalfSize; k <= existingWaterChestCheckHalfSize; k++)
            {
                for (int l = -existingWaterChestCheckHalfSize; l <= existingWaterChestCheckHalfSize; l++)
                {
                    Tile tileToCheck = Main.tile[x + k, y + l];
                    liquidCount += tileToCheck.LiquidAmount;
                    if (tileToCheck.HasTile && (tileToCheck.TileType == TileID.Containers))
                    {
                        chestNearby = true;
                    }
                }
            }
            return liquidCount > 255 * 50;//if over 50 tiles of liquid it's probably a sky lake
        }
    }
}
