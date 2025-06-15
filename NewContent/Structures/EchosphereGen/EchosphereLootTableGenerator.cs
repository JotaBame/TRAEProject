using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.AdvFlight;

namespace TRAEProject.NewContent.Structures.EchosphereGen
{
    public static class EchosphereLootTableGenerator
    {
        
        static int[] PrimaryLoot => [ItemID.GravityGlobe, ModContent.ItemType<AdvFlightSystem>(), ItemID.StarCloak];
        //FORMAT: ID, Min, Max
        //so for example:
        //[ItemID.FloatingIslandFishingCrate, 2, 4, ItemID.GoldenCrate, 1, 1]
        //places 2 to 4 sky crates, and then 1 golden crate
        static int[] SecondaryLoot => [ItemID.FloatingIslandFishingCrate, 1, 1, ItemID.GoldenCrate, 1, 1];
        static int MeteoriteBarMin => 15; 
        static int MeteoriteBarMax => 29;
        //"normalized" meaning 100% is 1, and 0% is 0
        static float MeteoriteBarNormalizedChance => 1f;
        static int NightcrawlerMin => 11;
        static int NightcrawlerMax => 16;
        static int FallenStarMin => 22;
        static int FallenStarMax => 22;
        static int VioletMossBrickMin => 190;
        static int VioletMossBrickMax => 210;
        //"normalized" meaning 100% is 1, and 0% is 0
        static float GoldCoinNormalizedChance => 1f;
        static int GoldCoinMin => 2;
        static int GoldCoinMax => 4;

        static float PotionPoolANormalizedChance => 0.66f;
        //FORMAT: ID, Min, Max
        //for example:
        //[ItemID.LifeforcePotion, 1, 2, ItemID.InfernoPotion, 4, 5]
        //will generate either 1-2 lifeforce potions, or 4-5 inferno potions
        static int[] PotionPoolA => [ItemID.LifeforcePotion, 1, 2, ItemID.InfernoPotion, 1, 2];
        //will be used if PotionPoolA isn't used.
        static int[] PotionPoolB => [ItemID.HunterPotion, 2, 3];

        public static Item[] GetChestLoot(ref int[] shuffledPrimaryLootTable, int chestsCreatedSoFar)
        {
            List<Item> result = new();
            int primaryLootID = shuffledPrimaryLootTable[chestsCreatedSoFar % shuffledPrimaryLootTable.Length];
            
            result.Add(new Item(primaryLootID, 1, -1));

            AddSecondaryLoot(result);

            AddMeteoriteBars(result);

            AddPotions(result);

            AddNightcrawlerOrFallenStar(result);


            result.Add(new Item(ItemID.VioletMossBlock, WorldGen.genRand.Next(VioletMossBrickMin, VioletMossBrickMax)));

            if (WorldGen.genRand.NextFloat() < GoldCoinNormalizedChance)
            {
                result.Add(new Item(ItemID.GoldCoin, WorldGen.genRand.Next(GoldCoinMin, GoldCoinMax + 1)));
            }
            return result.ToArray();
        }

        private static void AddSecondaryLoot(List<Item> result)
        {
            int[] secondaryLoot = SecondaryLoot;
            for (int i = 0; i < secondaryLoot.Length; i += 3)
            {
                result.Add(new Item(secondaryLoot[i], WorldGen.genRand.Next(secondaryLoot[i + 1], secondaryLoot[i + 2] + 1)));
            }
        }

        private static void AddMeteoriteBars(List<Item> result)
        {
            if (WorldGen.genRand.NextFloat() < MeteoriteBarNormalizedChance)
            {
                result.Add(new Item(ItemID.MeteoriteBar, WorldGen.genRand.Next(MeteoriteBarMin, MeteoriteBarMax + 1)));
            }
        }

        private static void AddNightcrawlerOrFallenStar(List<Item> result)
        {
            if (WorldGen.genRand.NextBool())
            {
                result.Add(new Item(ItemID.EnchantedNightcrawler, WorldGen.genRand.Next(NightcrawlerMin, NightcrawlerMax + 1)));
            }
            else
            {
                result.Add(new Item(ItemID.FallenStar, WorldGen.genRand.Next(FallenStarMin, FallenStarMax + 1)));
            }
        }

        private static void AddPotions(List<Item> result)
        {
            if (WorldGen.genRand.NextFloat() < PotionPoolANormalizedChance)
            {
                int[] potionPoolA = PotionPoolA;
                int i = WorldGen.genRand.Next(potionPoolA.Length / 3) * 3;
                result.Add(new Item(potionPoolA[i], WorldGen.genRand.Next(potionPoolA[i + 1], potionPoolA[i + 2] + 1)));
            }
            else
            {
                int[] potionPoolB = PotionPoolB;
                int i = WorldGen.genRand.Next(potionPoolB.Length / 3) * 3;
                result.Add(new Item(potionPoolB[i], WorldGen.genRand.Next(potionPoolB[i + 1], potionPoolB[i + 2] + 1)));
            }
        }

        public static int[] GetShuffledPrimaryLootTable()
        {
            List<int> primaryLootTable = PrimaryLoot.ToList();
            int items = primaryLootTable.Count;
            List<int> shuffled = new List<int>(primaryLootTable.Count);

            for (int i = 0; i < items; i++)
            {
                int index = WorldGen.genRand.Next(primaryLootTable.Count);
                shuffled.Add(primaryLootTable[index]);
                primaryLootTable.RemoveAt(index);
            }
            return shuffled.ToArray();
        }
    }
}
