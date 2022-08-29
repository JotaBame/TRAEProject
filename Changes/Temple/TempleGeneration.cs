using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Terraria.UI;

namespace TRAEProject.Changes
{
    public class TempleGeneration : ModSystem
    {
        public override void PreUpdateWorld()
        {
            //string inText = JungleRooms.test;
            //string[] tD = inText.Split(",".ToCharArray());
            //Main.NewText(tD[0]);
        }
        const int subRoomWidth = 70;
        const int subRoomHeight = 70;
        const int subRoomSpacing = 3;
        const int subRoomGap = 6;
        const int altarRoomHeight = 50;
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle Temple"));
            if (ShiniesIndex != -1)
            {
                tasks.RemoveAt(ShiniesIndex);
            }
            ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Hives"));
            if (ShiniesIndex != -1)
            {
                tasks.Add(new PassLegacy("Building a better temple!", delegate (GenerationProgress progress, GameConfiguration configuration)
                {
                    
                    //makeTemple(1000, 1000);
                    
                    int lastAttemptsCounter = 0;
                    long attemptCounter = 0L;
                    double distanceRate = 0.25;
                    while (true)
                    {
                        int j = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 500);
                        int i = (int)(((WorldGen.genRand.NextDouble() * distanceRate + 0.1) * (double)(-WorldGen.dungeonSide) + 0.5) * (double)Main.maxTilesX);
                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == TileID.JungleGrass)
                        {
                            makeTemple(i, j);
                            break;
                        }
                        if (attemptCounter++ > 2000000)
                        {
                            if (distanceRate == 0.35)
                            {
                                lastAttemptsCounter++;
                                if (lastAttemptsCounter > 10)
                                {
                                    break;
                                }
                            }
                            distanceRate = Math.Min(0.35, distanceRate + 0.05);
                            attemptCounter = 0L;
                        }
                    }
                    
                }));
            }
        }
        static void makeTemple(int x, int y)
		{
			int templeWidth =  Main.maxTilesX / 18;
            templeWidth = (templeWidth - (templeWidth % (subRoomWidth + subRoomSpacing))) + subRoomSpacing;
            int roomCountX = templeWidth / (subRoomWidth + subRoomSpacing);
			int templeHeight = (int)(templeWidth * 0.6f);
            templeHeight = (templeHeight - (templeHeight % (subRoomHeight + subRoomSpacing))) + subRoomSpacing;
            int roomCountY = templeHeight / (subRoomHeight + subRoomSpacing);

            Microsoft.Xna.Framework.Point TopCorner = new Microsoft.Xna.Framework.Point(x - templeWidth / 2, y - templeHeight / 2);
            for(int i =0; i < templeWidth; i++)
            {
                for(int j = 0; j < templeHeight; j++)
                {
                    WorldGen.KillTile(TopCorner.X + i, TopCorner.Y + j);
                    WorldGen.PlaceTile(TopCorner.X + i, TopCorner.Y + j, TileID.ChlorophyteBrick, forced: true);
                    WorldGen.KillWall(TopCorner.X + i, TopCorner.Y + j);
                    Main.tile[TopCorner.X + i, TopCorner.Y + j].LiquidAmount = 0;
                    //WorldGen.PlaceWall(TopCorner.X + i, TopCorner.Y + j, WallID.LihzahrdBrickUnsafe);
                }
            }

            for(int g =0; g < roomCountX; g++)
            {
                for(int h = 0; h < roomCountY; h++)
                {
                    for(int i =0; i < subRoomWidth; i++)
                    {
                        for(int j = 0; j < subRoomHeight; j++)
                        {
                            int posX = TopCorner.X + g * (subRoomWidth + subRoomSpacing) + subRoomSpacing + i;
                            int posY = TopCorner.Y + h * (subRoomHeight + subRoomSpacing) + subRoomSpacing + j;
                            WorldGen.KillTile(posX, posY);
                            WorldGen.KillWall(posX, posY);
                            WorldGen.PlaceWall(posX, posY, WallID.LihzahrdBrickUnsafe);
                        }
                    }
                    
                    if((g == 0 && h % 2 == 0) || (g == roomCountX - 1 && h % 2 == 1))
                    {
                        int posX = TopCorner.X + g * (subRoomWidth + subRoomSpacing) + subRoomSpacing + subRoomWidth / 2 - subRoomGap / 2;
                        int posY = TopCorner.Y + h * (subRoomHeight + subRoomSpacing);
                        for(int i =0; i < subRoomGap; i++)
                        {
                            for(int j = 0; j < subRoomSpacing; j++)
                            {
                                WorldGen.KillTile(posX + i, posY + j);
                            }
                        }
                        //PlaceRoom(TopCorner.X + g * (subRoomWidth + subRoomSpacing) + subRoomSpacing, TopCorner.Y + h * (subRoomHeight + subRoomSpacing) + subRoomSpacing, RoomType.FromAbove, h % 2 == 0);
                    }
                    else
                    {
                        int posX = TopCorner.X + g * (subRoomWidth + subRoomSpacing);
                        if(h % 2 == 1)
                        {
                            posX += (subRoomWidth + subRoomSpacing);
                        }
                        int height = subRoomGap;
                        if(h == roomCountY - 1 && ((h % 2 == 1 && g == 0)||(h % 2 == 0 && g == roomCountX - 1)))
                        {
                            height = altarRoomHeight;
                        }
                        int posY = TopCorner.Y + h * (subRoomHeight + subRoomSpacing) + subRoomSpacing + subRoomHeight / 2 - height / 2;
                        for(int i =0; i < subRoomSpacing; i++)
                        {
                            for(int j = 0; j < height; j++)
                            {
                                WorldGen.KillTile(posX + i, posY + j);
                            }
                        }
                        RoomType rT = RoomType.Straight;
                        if((g == 0 && h % 2 == 1) || (g == roomCountX - 1 && h % 2 == 0))
                        {
                            rT = RoomType.Down;
                        }
                        if(h == roomCountY - 1 && ((h % 2 == 1 && g == 0)||(h % 2 == 0 && g == roomCountX - 1)))
                        {
                            rT = RoomType.AltarB;
                        }
                        if(h == roomCountY - 1 && ((h % 2 == 1 && g == 1)||(h % 2 == 0 && g == roomCountX - 2)))
                        {
                            rT = RoomType.AltarA;
                        }
                        //PlaceRoom(TopCorner.X + g * (subRoomWidth + subRoomSpacing) + subRoomSpacing, TopCorner.Y + h * (subRoomHeight + subRoomSpacing) + subRoomSpacing, rT, h % 2 == 0);
                    }
                }
            }
		}
        public static void PlaceRoom(int x, int y, RoomType roomType, bool flipped = false)
        {
            
            //TextReader tr = new StreamReader("TempleRooms/testRoom_Straight_room.txt");
            //string inText = tr.ReadLine();
            string inText = StraightRooms.test;
            switch(roomType)
            {
                case RoomType.Straight:
                inText = StraightRooms.test;
                break;
                case RoomType.Down:
                inText = StraightRooms.test;
                break;
                case RoomType.FromAbove:
                inText = StraightRooms.test;
                break;
                case RoomType.AltarA:
                inText = StraightRooms.test;
                break;
                case RoomType.AltarB:
                inText = StraightRooms.test;
                break;
            }
            string[] tD = inText.Split(",".ToCharArray());
            for(int j = subRoomHeight - 1; j >= 0; j--)
            {
                for(int i = 0; i < subRoomWidth; i++)
                {
                    int location = i + j * subRoomWidth;
                    if(flipped)
                    {
                        location = (subRoomWidth - 1 - i) + j * subRoomWidth;
                    }
                    for(int dataType = 0; dataType < 7; dataType++)
                    {
                        int k = location * 7 + dataType;
                        switch(dataType)
                        {
                            case 0:
                                if(tD[k] == "n")
                                {
                                }
                                else
                                {
                                    WorldGen.PlaceTile(x + i, y + j, int.Parse(tD[k]));
                                }
                                break;
                            case 1:
                                int slope = int.Parse(tD[k]);
                                if(flipped)
                                {
                                    switch(slope)
                                    {
                                        case (int)SlopeType.SlopeDownLeft:
                                        slope = (int)SlopeType.SlopeDownRight;
                                        break;
                                        case (int)SlopeType.SlopeDownRight:
                                        slope = (int)SlopeType.SlopeDownLeft;
                                        break;
                                        case (int)SlopeType.SlopeUpLeft:
                                        slope = (int)SlopeType.SlopeUpRight;
                                        break;
                                        case (int)SlopeType.SlopeUpRight:
                                        slope = (int)SlopeType.SlopeUpLeft;
                                        break;
                                    }
                                }
                                WorldGen.SlopeTile(x + i, y + j, slope);
                                break;
                            case 2:
                                int wire = int.Parse(tD[k]);
                                if (wire % 10 == 1)
                                {
                                    WorldGen.PlaceWire(i + x, j + y);
                                }
                                if (wire % 100 >= 10)
                                {
                                    WorldGen.PlaceWire2(i + x, j + y);
                                }
                                if (wire % 1000 >= 100)
                                {
                                    WorldGen.PlaceWire3(i + x, j + y);
                                }
                                if (wire % 10000 >= 1000)
                                {
                                    WorldGen.PlaceWire4(i + x, j + y);
                                }
                                if (wire % 100000 >= 10000)
                                {
                                    WorldGen.PlaceActuator(i + x, j + y);
                                }
                                if (wire % 1000000 >= 100000)
                                {
                                    //Main.tile[x + i, y + j].IsActuated = true;
                                }
                                break;
                            case 3:
                                int fX = int.Parse(tD[k]);
                                Main.tile[i + x, j + y].TileFrameX = (short)fX;
                                break;
                            case 4:
                                int fY = int.Parse(tD[k]);
                                Main.tile[i + x, j + y].TileFrameY = (short)fY;
                                break;
                            case 5:
                                WorldGen.PlaceLiquid(i + x, j + y, byte.Parse(tD[k+1]), byte.Parse(tD[k]));
                                break;
                            case 6:
                                break;
                        }
                    }
                }
            }
        }
        public static void ExportRoom(int x, int y, string name, RoomType roomType)
        {
            //BuildTinyRoom((int)position.X, (int)position.Y, roomType);

            // create a writer and open the file
            string typeName = "striaght";
            switch(roomType)
            {
                case RoomType.Straight:
                    typeName = "straight";
                    break;
                case RoomType.Down:
                    typeName = "down";
                    break;
                case RoomType.FromAbove:
                    typeName = "from";
                    break;
                case RoomType.AltarA:
                    typeName = "altarA";
                    break;
                case RoomType.AltarB:
                    typeName = "altarB";
                    break;
            }
            TextWriter tw = new StreamWriter(name + "_" + typeName + "_room.txt");

            // write a line of text to the file
            String tileStructure = "";

            for(int j = 0; j < subRoomHeight; j++)
            {
                for(int i = 0; i < subRoomWidth; i++)
                {
                    if(Main.tile[x + i, y + j].HasTile)
                    {
                        tileStructure += Main.tile[x + i, y + j].TileType;
                    }
                    else
                    {
                        tileStructure += "n";
                    }
                    tileStructure += ",";
                    tileStructure += (byte)Main.tile[x + i, y + j].Slope;
                    tileStructure += ",";
                    int wires = 0;
                    if (Main.tile[x + i, y + j].RedWire)
                    {
                        wires += 1;
                    }
                    if (Main.tile[x + i, y + j].BlueWire)
                    {
                        wires += 10;
                    }
                    if (Main.tile[x + i, y + j].GreenWire)
                    {
                        wires += 100;
                    }
                    if (Main.tile[x + i, y + j].YellowWire)
                    {
                        wires += 1000;
                    }
                    if (Main.tile[x + i, y + j].HasActuator)
                    {
                        wires += 10000;
                    }
                    if (Main.tile[x + i, y + j].IsActuated)
                    {
                        wires += 100000;
                    }
                    tileStructure += wires;
                    tileStructure += ",";
                    tileStructure += Main.tile[x + i, y + j].TileFrameX;
                    tileStructure += ",";
                    tileStructure += Main.tile[x + i, y + j].TileFrameY;
                    tileStructure += ",";
                    tileStructure += Main.tile[x + i, y + j].LiquidAmount;
                    tileStructure += ",";
                    tileStructure += Main.tile[x + i, y + j].LiquidType;
                    tileStructure += ",";
                }
            }
            tw.WriteLine(tileStructure);
            // close the stream
            tw.Close();
        }
    }
    public enum RoomType : byte
    {
        Straight,
        Down,
        FromAbove,
        AltarA,
        AltarB
    }
    internal class ExportRoom : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "exportRoom"; }
        }


        public override string Usage
        {
            get { return "/exportRoom type name"; }
        }

        public override string Description
        {
            get { return "puts tile data into a .txt"; }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length != 2)
            {
                Main.NewText("Invalid arguments");
            }
            else
            {
                RoomType roomType = RoomType.Straight;
                string rType = args[0];
                switch(rType)
                {
                    case "Straight":
                    roomType = RoomType.Straight;
                    break;
                    case "Down":
                    roomType = RoomType.Down;
                    break;
                    case "FromAbove":
                    roomType = RoomType.FromAbove;
                    break;
                    case "AltarA":
                    roomType = RoomType.AltarA;
                    break;
                    case "AltarB":
                    roomType = RoomType.AltarA;
                    break;
                }
                string name = args[1];
                Microsoft.Xna.Framework.Point pos = Main.MouseWorld.ToTileCoordinates();
                TempleGeneration.ExportRoom(pos.X, pos.Y, name, roomType);

            }
        }
    }
}
