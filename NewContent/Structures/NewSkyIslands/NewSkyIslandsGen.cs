using Microsoft.Xna.Framework;
using StructureHelper.API;
using StructureHelper.Models;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;
using TRAEProject.Common.LootTableGenerator;
using TRAEProject.NewContent.Structures.Echosphere;

namespace TRAEProject.NewContent.Structures.NewSkyIslands
{
    public static class NewSkyIslandsGen
    {
        static int IslandApproxBoundingBoxWidth => 250;
        static int IslandApproxBoundingBoxHeight => 150;//PLACEHOLDER VALUES
        public static void GenerateIslands()
        {
            EchosphereGeneratorSystem.stopVanillaSkyIslandGen = false;
            for (int i = 0; i < GenVars.floatingIslandHouseX.Length; i++)
            {
                int x = GenVars.floatingIslandHouseX[i];
                int y = GenVars.floatingIslandHouseY[i];
                if(x== 0  || y == 0)
                {
                    continue;
                }
                if(IntersectsEchosphere(x,y, out int jIntersectAmount))
                {
                    y += jIntersectAmount;
                    GenVars.floatingIslandHouseY[i] = y;
                }
                if (GenVars.skyLake[i])
                {
                    WorldGen.CloudLake(x,y);
                }
                else
                {
                    WorldGen.CloudIsland(x, y + 4);
                }
            }
            EchosphereGeneratorSystem.stopVanillaSkyIslandGen = true;
        }
        public static void GenerateHouses()
        {
            List<int> indicesInGenVarsFloatingIslandHouseXYToGenHousesAt = new List<int>();
            for (int i = 0; i < GenVars.floatingIslandHouseX.Length; i++)
            {
                int x = GenVars.floatingIslandHouseX[i];
                int y = GenVars.floatingIslandHouseY[i];
                if (x == 0 || y == 0 || GenVars.skyLake[i])
                {
                    continue;
                }
                indicesInGenVarsFloatingIslandHouseXYToGenHousesAt.Add(i);
            }
            StructureData[] houseStrData = GetHouseStructureData(out Point16[] offsets, out int[] chestToFloorOffsets, out int swordIslandIndex);

            int[] houseTypes = GetHouseTypes(indicesInGenVarsFloatingIslandHouseXYToGenHousesAt.Count, swordIslandIndex, houseStrData.Length);
            Chest[] generatedChests = new Chest[indicesInGenVarsFloatingIslandHouseXYToGenHousesAt.Count];
            for (int i = 0; i < indicesInGenVarsFloatingIslandHouseXYToGenHousesAt.Count; i++)
            {
                int indexToGenAt = indicesInGenVarsFloatingIslandHouseXYToGenHousesAt[i];
                int x = GenVars.floatingIslandHouseX[indexToGenAt];
                int y = GenVars.floatingIslandHouseY[indexToGenAt];
                int houseTypeToGen = houseTypes[i];
                StructureData houseData = houseStrData[houseTypeToGen];
                Point16 genPoint = new Point16(x, y - chestToFloorOffsets[houseTypeToGen]) - offsets[houseTypeToGen];
                Generator.GenerateFromData(houseData, genPoint);
                int chestIndex = Chest.CreateChest(x - 2, y - chestToFloorOffsets[houseTypeToGen] - 2);
                Chest createdChest = Main.chest[chestIndex];
         
                generatedChests[i] = createdChest;
            }

            //ItemID.SunOrnament is the painting-like decoration "Eye of the Sun" 
            //ItemID.CreativeWings is fledgling wings

            (int id, int min, int max)[] primaryLoot =
                [
                (ItemID.ShinyRedBalloon, 1, 1),
                (ItemID.CreativeWings, 1, 1),
                (ItemID.CelestialMagnet, 1,1)
                ];

            (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools =
                [
                (1f/3, [(ItemID.SkyMill, 1,1)]),

                (1f, [(ItemID.HighPitch,1,1), (ItemID.BlessingfromTheHeavens,1,1), (ItemID.Constellation,1,1),
                    (ItemID.SeeTheWorldForWhatItIs,1,1), (ItemID.LoveisintheTrashSlot,1,1), (ItemID.SunOrnament,1,1)]),

                (1f, [(ItemID.Cloud,50,100)]),

                (1/6f, [(ItemID.Glowstick, 40, 75)]),

                (1/6f, [(ItemID.ThrowingKnife, 150, 300)]),

                (1/6f, [(ItemID.HerbBag, 1, 4)]),

                (1/6f, [(ItemID.CanOfWorms, 1, 4)]),

                (1/3f, [(ItemID.Grenade, 3, 5)]),

                (.5f, [(GenVars.copperBar, 3, 10), (GenVars.ironBar, 3, 10)]),

                (.5f, [(ItemID.Rope, 50, 100)]),

                (2f/3, [(ItemID.Shuriken, 25, 50), (ItemID.WoodenArrow, 25,50)]),
                
                (.5f, [(ItemID.LesserHealingPotion, 3, 5)]),

                (2f/3, [(ItemID.RecallPotion, 3, 5)]),

                (2f/3, [(ItemID.IronskinPotion, 1, 2), (ItemID.ShinePotion, 1, 2), (ItemID.NightOwlPotion, 1, 2), (ItemID.SwiftnessPotion, 1, 2), (ItemID.MiningPotion, 1 , 2), (ItemID.BuilderPotion, 1, 2)]),

                (.5f, [(ItemID.Torch, 10, 20), (ItemID.Bottle, 10, 20)]),

                (.5f, [(ItemID.SilverCoin, 10, 29)]),

                (.5f, [(ItemID.Wood, 50, 99)])
                ];

            LootGenerator.PopulateChests(generatedChests, primaryLoot, secondaryLootPools, WorldGen.genRand);
        }




        static int[] GetHouseTypes(int amountToGet, int swordIslandIndex, int houseTypeAmount)
        {
            int[] result = new int[amountToGet];
            result[0] = swordIslandIndex;
            if (amountToGet < 2)
            {
                return result;
            }
            List<int> shuffledHouseTypes = GetNonSwordIslandHouseTypeIndexes(swordIslandIndex, houseTypeAmount);
            TRAEMethods.Shuffle(ref shuffledHouseTypes, WorldGen.genRand);
            for (int i = 1; i < result.Length; i++)
            {
                //in case we run out
                if (shuffledHouseTypes.Count <= 0)
                {
                    shuffledHouseTypes = GetNonSwordIslandHouseTypeIndexes(swordIslandIndex, houseTypeAmount);
                    TRAEMethods.Shuffle(ref shuffledHouseTypes, WorldGen.genRand);
                }
                int randIndex = WorldGen.genRand.Next(0, shuffledHouseTypes.Count);
                result[i] = shuffledHouseTypes[randIndex];
                shuffledHouseTypes.RemoveAt(randIndex);
            }
            TRAEMethods.Shuffle(ref result);//again so sword island isn't always the first
            return result;
        }

        private static List<int> GetNonSwordIslandHouseTypeIndexes(int swordIslandIndex, int houseTypeAmount)
        {
            List<int> possibleHouseTypes = new List<int>();
            for (int i = 0; i < houseTypeAmount; i++)
            {
                if (i != swordIslandIndex)
                {
                    possibleHouseTypes.Add(i);
                }

            }

            return possibleHouseTypes;
        }

        public static bool IntersectsEchosphere(int i, int j, out int jIntersectAmount)
        {
            Rectangle echosphereBounds = EchosphereGeneratorSystem.GetTileCoordsBoundingBox();
            Rectangle structureBounds = new Rectangle(
                i - IslandApproxBoundingBoxWidth / 2,
                j - IslandApproxBoundingBoxHeight / 2,
                IslandApproxBoundingBoxWidth,
                IslandApproxBoundingBoxHeight);

            Rectangle intersection = Rectangle.Intersect(echosphereBounds, structureBounds);

            if (intersection.Width > 0 && intersection.Height > 0)
            {
                jIntersectAmount = echosphereBounds.Bottom - structureBounds.Top;
                return true;
            }

            jIntersectAmount = 0;
            return false;
        }
        public static bool IntersectsEchosphere(int i, int j, out int iIntersectAmount, out int jIntersectAmount)
        {
            Rectangle echosphereBounds = EchosphereGeneratorSystem.GetTileCoordsBoundingBox();
            Rectangle structureBounds = new Rectangle(i - IslandApproxBoundingBoxWidth / 2, j - IslandApproxBoundingBoxHeight / 2, IslandApproxBoundingBoxWidth, IslandApproxBoundingBoxHeight);

            Rectangle intersection = Rectangle.Intersect(echosphereBounds, structureBounds);

            if (intersection.Width > 0 && intersection.Height > 0)
            {
                if (structureBounds.Left < echosphereBounds.Left)
                {
                    iIntersectAmount = -intersection.Width;
                }
                else
                {
                    iIntersectAmount = intersection.Width;
                }
                if (structureBounds.Top < echosphereBounds.Top)
                {
                    jIntersectAmount = -intersection.Height;
                }
                else
                {
                    jIntersectAmount = intersection.Height;
                }

                return true;
            }
            iIntersectAmount = 0;
            jIntersectAmount = 0;
            return false;
        }
        //not the best way of doing this but eh it works
        public static bool IntersectsEchosphere(StructureData structure, int i, int j, out int iIntersectAmount, out int jIntersectAmount)
        {
            Rectangle echosphereBounds = EchosphereGeneratorSystem.GetTileCoordsBoundingBox();
            Rectangle structureBounds = new Rectangle(i, j, structure.width, structure.height);

            Rectangle intersection = Rectangle.Intersect(echosphereBounds, structureBounds);

            if (intersection.Width > 0 && intersection.Height > 0)
            {
                if (structureBounds.Left < echosphereBounds.Left)
                {
                    iIntersectAmount = -intersection.Width;
                }
                else
                {
                    iIntersectAmount = intersection.Width;
                }
                if (structureBounds.Top < echosphereBounds.Top)
                {
                    jIntersectAmount = -intersection.Height;
                }
                else
                {
                    jIntersectAmount = intersection.Height;  
                }

                return true;
            }
            iIntersectAmount = 0;
            jIntersectAmount = 0;
            return false;
        }
        public static StructureData[] GetHouseStructureData(out Point16[] offsets, out int[] chestToFloorOffsets, out int swordIslandIndex)
        {
            offsets = [new Point16(19, 12), new Point16(12, 24), new Point16(17, 18), new Point16(21, 21), new Point16(22, 20), new Point16(23, 13), new Point16(33, 18)];
            chestToFloorOffsets = [18, 0, 0, 0, 8, 9, 0];
            StructureData[] result = new StructureData[offsets.Length];
            for (int i = 0; i < result.Length; i++)
            {
                string extra = offsets[i].X.ToString() + offsets[i].Y.ToString() + chestToFloorOffsets[i].ToString();
                result[i] = StructureHelper.API.Generator.GetStructureData("NewContent/Structures/NewSkyIslands/StructureData/NewIslandHouse" + extra, TRAEProj.Instance);
            }
            swordIslandIndex = 6;
            result[swordIslandIndex] = Generator.GetStructureData("NewContent/Structures/NewSkyIslands/StructureData/NewFloatingIslandSwordShrine", TRAEProj.Instance);
            offsets[swordIslandIndex] = result[swordIslandIndex].HalfSize();
            return result;
        }
        public static StructureData[] GetIslandStructureDataAndOffsets(out Point16[] offsets, out int swordIslandIndex)
        {
            offsets = [new Point16(64, 42), new Point16(68, 23), new Point16(50, 38), new Point16(56, 30), new Point16(59, 29), new Point16(59, 37), new Point16(63, 32)];
            StructureData[] result = new StructureData[offsets.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = StructureHelper.API.Generator.GetStructureData("NewContent/Structures/NewSkyIslands/StructureData/NewFloatingIsland" + offsets[i].X.ToString() + offsets[i].Y.ToString(), TRAEProj.Instance);
            }
            swordIslandIndex = 5;
            return result;
        }
        static void Generate_Old()
        {
            StructureData[] houseStrData = GetHouseStructureData(out Point16[] offsets, out int[] chestToFloorOffsets, out int swordIslandIndex);
            List<int> nonLakeIndexes = new List<int>();
            for (int i = 0; i < GenVars.floatingIslandHouseX.Length; i++)
            {
                int x = GenVars.floatingIslandHouseX[i];
                int y = GenVars.floatingIslandHouseY[i];
                if (x == 0 || y == 0 || GenVars.skyLake[i])
                {
                    continue;
                }
                nonLakeIndexes.Add(i);
            }
            int[] islandTypesToGenerate = new int[nonLakeIndexes.Count];
            islandTypesToGenerate[0] = swordIslandIndex;
            List<int> nonSwordIslandIndexes = new List<int>();
            for (int i = 0; i < offsets.Length; i++)
            {
                if (i != swordIslandIndex)
                {
                    nonSwordIslandIndexes.Add(i);
                }
            }
            for (int i = 1; i < islandTypesToGenerate.Length; i++)
            {
                int randIndex = Main.rand.Next(nonSwordIslandIndexes.Count);
                int chosenIslandType = nonSwordIslandIndexes[randIndex];
                nonSwordIslandIndexes.RemoveAt(randIndex);
                islandTypesToGenerate[i] = chosenIslandType;
            }
            TRAEMethods.Shuffle(ref islandTypesToGenerate, WorldGen.genRand);


            for (int i = 0; i < nonLakeIndexes.Count; i++)
            {
                int nonLakeIndex = nonLakeIndexes[i];
                int islandTypeToGen = islandTypesToGenerate[i];

                int x = GenVars.floatingIslandHouseX[nonLakeIndex];
                int y = GenVars.floatingIslandHouseY[nonLakeIndex];
                if (IntersectsEchosphere(x, y, out int iIntersectAmount, out int jIntersectAmount))
                {
                    y += jIntersectAmount;
                    x += iIntersectAmount;
                    GenVars.floatingIslandHouseX[nonLakeIndex] = x;
                    GenVars.floatingIslandHouseY[nonLakeIndex] = y;
                }
                WorldGen.CloudIsland(x, y + 4);
                StructureData strData = houseStrData[islandTypeToGen];
                Point16 genPoint = new Point16(x, y - chestToFloorOffsets[islandTypeToGen]) - offsets[islandTypeToGen];
                StructureHelper.API.Generator.GenerateFromData(strData, genPoint);
            }
            for (int i = 0; i < GenVars.floatingIslandHouseX.Length; i++)
            {
                int x = GenVars.floatingIslandHouseX[i];
                int y = GenVars.floatingIslandHouseY[i];
                if (x == 0 || y == 0 || !GenVars.skyLake[i])
                {
                    continue;
                }
                if (IntersectsEchosphere(x, y, out int iIntersectAmount, out int jIntersectAmount))
                {
                    y += jIntersectAmount;
                    y += IslandApproxBoundingBoxHeight;
                    x += iIntersectAmount;
                    GenVars.floatingIslandHouseX[i] = x;
                    GenVars.floatingIslandHouseY[i] = y;
                }
                WorldGen.CloudLake(x, y);
            }
        }
    }
    public class NewIskyIslandsHousesGenPass : GenPass
    {
        public NewIskyIslandsHousesGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            NewSkyIslandsGen.GenerateHouses();
        }
    }
    public class NewSkyIslandsGenPass : GenPass
    {
        public NewSkyIslandsGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            NewSkyIslandsGen.GenerateIslands();
        }
    }
}
