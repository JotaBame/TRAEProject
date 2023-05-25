using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TRAEProject.NewContent.StarfuryTemple
{
	public static class StarfuryTempleGen
    {

		public  static void MainGeneration()
		{
			Point center = Main.MouseWorld.ToTileCoordinates();
			Vector2 start = Main.MouseWorld.ToTileCoordinates().ToWorldCoordinates();
			PlaceRectangle(start - new Vector2(16, -16), TileID.Sunplate, 3, 10);
			PlaceRectangle(start - new Vector2(9 * 16, -64), TileID.Sunplate, 19, 7);
			WorldGen.PlaceTile(center.X, center.Y, 187, true, true, -1, 17);//ENCHANTED SWORD
			for (int i = -1; i < 2; i += 1)
			{
				for (int j = -4; j < 2; j++)
				{
					if (j < -2 && i != 0)
						continue;
					WorldGen.PlaceWall(center.X + i, center.Y + j, WallID.DiscWall, true);
				}
				if (i != 0)
				{
					//these are the stairs near the enchanted sword tile
					PlaceRightTriangle(start + new Vector2(i * 2, 1) * 16, 3, 1, 2 * i, TileID.Sunplate, false, new BlockType?[] { BlockType.Solid, BlockType.HalfBlock });
					PlaceLamp(10, center + new Point(i * 9, 3));//this places the lamps near the enchanted sword
				}
			}
			WorldGen.PlaceTile(center.X, center.Y - 4, TileID.Torches, true, true, -1, 14);//Rainbow torch
			WorldGen.PlaceTile(center.X, center.Y - 22, TileID.Platforms, style: 22);
			WorldGen.PlaceObject(center.X, center.Y - 21, TileID.SoulBottles, true, style: 3);
			WorldGen.PlaceTile(center.X, center.Y - 23, TileID.PeaceCandle);

			return;
			#region RightRoofAndPillarsAndFloor
			GetHammerPattern(2, new int[] { 7, 5, 1 }, new BlockType?[] { BlockType.Solid, BlockType.HalfBlock, BlockType.Solid }, out BlockType?[] hammerPattern);
			start += new Vector2(20, -25) * 16;
			PlaceLineWithTileUnits(start, 47, 0, 1, TileID.Sunplate, hammerPattern);//roof
			PlaceRectangle(start + new Vector2(0, 16), TileID.Sunplate, 47, 6);//roof//23,29
			PlaceStructureMiniManaCrystal(start.ToTileCoordinates() + new Point(23, 28));
			PlaceHorizontalLine(start.ToTileCoordinates() + new Point(-7, 28), 53, TileID.Grass);//grass floor
			PlaceLineWithTileUnits(start + new Vector2(-7 * 16, 29 * 16), 53, 0, 1, TileID.Dirt);//floor
			PlaceLineWithTileUnits(start + new Vector2(-8 * 16, 30 * 16), 54, 0, 1, TileID.GoldBrick);//gold line below it
			PlaceLineWithTileUnits(start + new Vector2(-8 * 16, 30 * 16), 3, -1, 0, TileID.GoldBrick);//gold line that goes up
			GetHammerPattern(2, new int[] { 1, 1 }, new BlockType?[] { BlockType.Solid, BlockType.HalfBlock }, out hammerPattern);
			Point lastTileCoord = PlaceRightTriangle(start, 7, 1, -2, TileID.Sunplate, false, hammerPattern);
			Tile lastTile = Main.tile[lastTileCoord.X - 2, lastTileCoord.Y];//it didn't actually give the last tile coord for some reason so this fixes that
			lastTile.HasTile = false;
			WorldGen.PlaceTile(lastTileCoord.X - 2, lastTileCoord.Y, TileID.Platforms);
			lastTile.TileFrameY = GetPlatformFrame(22);
			for (int i = -1; i < 4; i++)
			{
				if (i >= 0)
					PlaceStarryPillar(start.ToTileCoordinates() + new Point(13 * i, 7));				
				if (i < 3)
					PlaceStructureStarfuryTempleChandelier(new Point(9 + 13 * i, 7) + start.ToTileCoordinates());
			}
			#endregion
		}
		public  static void PlaceStructureMiniManaCrystal(Point center)
        {
			PlaceRectangle((center + new Point(-2, -1)).ToWorldCoordinates(), TileID.StarRoyaleBrick, 5, 2);
			for (int i = -2; i < 3; i += 4)			
				HammerTile(BlockType.HalfBlock, center + new Point(i, -1));		
			PlaceHorizontalLine(center + new Point(-1, 1), 3, TileID.StarRoyaleBrick);
			WorldGen.PlaceTile(center.X, center.Y + 2, TileID.StarRoyaleBrick);
            PlaceManaCrystal(new Point(center.X - 1, center.Y - 3));
            PlaceLamp(10, new Point(center.X + 1, center.Y - 2));
        }

        public static void PlaceManaCrystal(Point manaCrystalTopLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
					Tile tile = Main.tile[manaCrystalTopLeft.X + i, manaCrystalTopLeft.Y + j];
					tile.HasTile = true;
					tile.TileType = TileID.ManaCrystal;
					tile.TileFrameX = (short)(i * 18);
					tile.TileFrameY = (short)(j * 18);
                }
            }
        }

        public  static void PlaceLamp(int lampIndex, Point lampBottom)
        {
			WorldGen.PlaceTile(lampBottom.X, lampBottom.Y, TileID.Lamps);
            for (int i = 0; i < 3; i++)
            {
                Tile tile = Main.tile[lampBottom.X, lampBottom.Y - 2 + i];
                tile.TileFrameY = (short)(((lampIndex * 3 - 3) * 18) + (18 * i));
            }
        }

        public static void PlaceStarryPillar(Point topLeft)
        {
			WorldGen.PlaceTile(topLeft.X + 2, topLeft.Y + 20, TileID.Sunflower);
			int[] wallTypes = new int[] { WallID.DiscWall, WallID.GoldStarryGlassWall, WallID.BlueStarryGlassWall, WallID.BlueStarryGlassWall, WallID.GoldStarryGlassWall, WallID.DiscWall };
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 21; j++)
                {
					WorldGen.PlaceWall(i + topLeft.X, j + topLeft.Y, wallTypes[i]);
                }
			}
		}
		public static void PlaceStructureStarfuryTempleChandelier(Point topMiddle)
        {
            Point bannerFrame = Main.rand.NextBool(4) ? GravBannerFrame : Main.rand.NextBool(3) ? SunBannerFrame : Main.rand.NextBool() ? WyvernBannerFrame : FlyingIslandBannerFrame;
            for (int i = -2; i < 3; i += 4)
            {
                int x = topMiddle.X + i;
                int y = topMiddle.Y;
				WorldGen.PlaceTile(x, y, TileID.Banners);
                FrameBanner(bannerFrame, x, y);
            }
			PlaceRectangle((topMiddle + new Point(-1, 0)).ToWorldCoordinates(), TileID.Sunplate, 3, 2);
            for (int i = -1; i < 2; i += 2)
            {
                HammerTile(i < 0 ? BlockType.SlopeUpRight : BlockType.SlopeUpLeft, topMiddle + new Point(i, 1));
            }
            PlaceChandelier(15, topMiddle.X, topMiddle.Y + 2);
        }
        public static void PlaceChandelier(int chandelierType, int chandelierX, int chandelierY)
        {
			WorldGen.PlaceTile(chandelierX, chandelierY, TileID.Chandeliers);
            for (int i = -1; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Tile tile = Main.tile[chandelierX + i, chandelierY + j];
					tile.TileFrameY = (short)(54 * chandelierType % 1998 + 18 * j);
                }
            }
        }
        public static void FrameBanner(Point bannerFrame, int x, int y)
        {
            for (short j = 0; j < 3; j++)
            {
                Tile tile = Main.tile[x, y + j];
				tile.TileFrameX = (short)bannerFrame.X;
				tile.TileFrameY = (short)(bannerFrame.Y + j * 18);
            }
        }
        public static void GetHammerPattern(int amountOfSequences, int[] sequenceLengths, BlockType?[] sequenceShapes, out BlockType?[] hammerPattern)
        {
			List<BlockType?> hammerPatternList = new();
			for (int i = 0; i < amountOfSequences; i++)
            {
                for (int j = 0; j < sequenceLengths[i]; j++)
                {
					hammerPatternList.Add(sequenceShapes[i].HasValue ? sequenceShapes[i] : BlockType.Solid);
                }
            }
			hammerPattern = hammerPatternList.ToArray();
        }
		public static Point PlaceRightTriangle(Vector2 start, int steps, int yPerStep, int xPerStep, int tileType, bool invert, BlockType?[] hammerPattern = null)
		{
			Point lastTileCoord = new Point();
			start.Y -= 16 * MathF.Sign(yPerStep);
			PlaceLineWithTileUnits(start, steps, yPerStep, xPerStep, tileType, hammerPattern);
			int length = (int)MathHelper.Clamp(MathF.Abs(steps * yPerStep), 1, float.MaxValue);
			int maxWidth = (int)MathF.Abs(steps * xPerStep);
			if (yPerStep < 0)
				start.Y -= invert ? 0 : 32;
			if (xPerStep < 0 && invert)
				start.X += 32;
			for (int i = 0; i < length; i++)
			{
				float verticalProgress = (float)i / length;
				for (int j = 0; j < (int)(maxWidth * verticalProgress); j++)
				{
					Point coords = start.ToTileCoordinates();
					int x = coords.X + (invert ? (maxWidth - j) : j) * MathF.Sign(xPerStep);
					int y = coords.Y + (invert ? (length - i) : i) * MathF.Sign(yPerStep);
					x += invert ? -1 : 0;
					y += invert ? 0 : 1;
					WorldGen.PlaceTile(x, y, tileType);
					lastTileCoord = new Point(x, y);
				}
			}
			return lastTileCoord;
		}
		public static void PlaceHorizontalLine(Point start, int width, int type, BlockType?[] hammerPattern = null)
        {
			int tilesPlaced = 0;
			for (int i = 0; i < width; i++)
			{
				WorldGen.PlaceTile(start.X, start.Y, TileID.Dirt);
				if (Main.tile[start].TileType != TileID.StarRoyaleBrick)
				{
					Tile tile = Main.tile[start];
					tile.TileType = (ushort)type;
					HammerTiles(hammerPattern, tilesPlaced, start);
				}
				tilesPlaced++;
				start.X += MathF.Sign(width);		
			}
		}
		public static void PlaceLineWithTileUnits(Vector2 start, int steps, int yPerStep, int xPerStep, int tileType, BlockType?[] hammerPattern = null)//ADD COMMENT TO THIS EXPLAINING WHAT THE ANGLE FUCKERY IS FOR
		{
			start = start.ToTileCoordinates().ToWorldCoordinates();
			int tilesPlaced = 0;
			Point coordToPlace = start.ToTileCoordinates();
			float tileAngle = new Vector2(xPerStep, yPerStep).ToRotation();
			bool placeYTiles = (tileAngle > MathHelper.PiOver4 && tileAngle < 3 * MathHelper.PiOver4) || (tileAngle < -MathHelper.PiOver4 && tileAngle > 3 * -MathHelper.PiOver4);//
			for (int i = 0; i < steps; i++)
			{
				for (int j = 0; j < MathF.Abs(yPerStep); j++)
				{
					if (placeYTiles)
					{
						WorldGen.PlaceTile(coordToPlace.X, coordToPlace.Y, tileType);
						HammerTiles(hammerPattern, tilesPlaced, coordToPlace);
						tilesPlaced++;
					}
					coordToPlace.Y += MathF.Sign(yPerStep);
				}
				for (int j = 0; j < MathF.Abs(xPerStep); j++)
				{
					if (!placeYTiles)
					{
						WorldGen.PlaceTile(coordToPlace.X, coordToPlace.Y, tileType);
						HammerTiles(hammerPattern, tilesPlaced, coordToPlace);
						tilesPlaced++;
					}
					coordToPlace.X += MathF.Sign(xPerStep);
				}
			}
		}
		public static void HammerTile(BlockType hammerState, Point tileCoord)
        {
			Tile tile = Main.tile[tileCoord];
			tile.BlockType = hammerState;
		}
		public static void HammerTiles(BlockType?[] hammerPattern, int tilesPlaced, Point tileCoord)
		{

			if (hammerPattern == null)
				return;
			Tile tile = Main.tile[tileCoord];
			tile.BlockType = hammerPattern[tilesPlaced % hammerPattern.Length].GetValueOrDefault();	
		}
		public static void PlaceRectangle(Vector2 start, int tileType, int length = 1, int height = 1)
		{
			for (int i = 0; i < MathF.Abs(length); i++)
			{
				for (int j = 0; j < MathF.Abs(height); j++)
				{
					Point pos = start.ToTileCoordinates() + new Point(i * MathF.Sign(length), j * MathF.Sign(height));
					WorldGen.PlaceTile(pos.X, pos.Y, tileType);
				}
			}
		}
		public static void PlaceHorizontalBridge(Vector2 position, int length)//REWORK THIS
		{
			(int, short)?[] heightOffsetToTileAndFrame = new (int, short)?[] { (TileID.Platforms, GetPlatformFrame(22)), (TileID.Sunplate, -1), null, null, (TileID.Platforms, GetPlatformFrame(22)), (TileID.Torches, GetTorchFrame(5)) };
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < heightOffsetToTileAndFrame.Length; j++)
				{
					Point tilePos = (position + new Vector2(i * 16, j * -16)).ToTileCoordinates();
					if (heightOffsetToTileAndFrame[j].HasValue)
					{
						WorldGen.PlaceTile(tilePos.X, tilePos.Y, heightOffsetToTileAndFrame[j].Value.Item1);
						Tile tile = Main.tile[tilePos];
						tile.TileFrameY = heightOffsetToTileAndFrame[j].Value.Item2;
					}
				}
			}
		}
		public static Point SunBannerFrame { get => new(144, 0); }
		public static Point GravBannerFrame { get => new(162, 0); }
		public static Point WyvernBannerFrame { get => new(1926, 0); }
		public static Point FlyingIslandBannerFrame { get => new(126, 0 ); }
		public static Point GetBannerFrame(short bannerIndex) => new(bannerIndex * 18 % 1998, bannerIndex / 111 * 54);
		public static short GetPlatformFrame(short platformIndex) => (short)(platformIndex * 18);
		public static short GetTorchFrame(short torchIndex) => (short)(torchIndex * 22);
	}
}
