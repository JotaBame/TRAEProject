using System;
using System.Collections.Generic;
using System.Threading.Channels;
using Terraria;
using Terraria.Utilities;

namespace TRAEProject.Common.LootTableGenerator
{
    public static class LootGenerator
    {
        /// <summary>
        /// if you're in a loop of any sort, you must create the primaryLoot array OUTSIDE it.
        /// </summary>
        /// <returns>The Item[] that the Chest.item should be set to</returns>
        public static Item[] GetLootFromLootTable(ref (int id, int min, int max)[] primaryLoot, (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools, int chestCreatedSoFar, UnifiedRandom rnd = null)
        {
            rnd ??= WorldGen.genRand;
            if (chestCreatedSoFar % primaryLoot.Length == 0)
            {
                ShufflePrimaryLoot(ref primaryLoot);
            }
            List<Item> result = new(secondaryLootPools.Length + 1)
            {
                GetItemFromLootData(primaryLoot[chestCreatedSoFar % primaryLoot.Length], rnd)
            };
            for (int i = 0; i < secondaryLootPools.Length; i++)
            {
                PossiblyAddToLootPool(result, secondaryLootPools[i], rnd);
            }
            return result.ToArray();
        }
        public static void PopulateChests(Chest[] chests, (int id, int min, int max)[] primaryLoot, (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools, UnifiedRandom rnd = null)
        {
            int chestsCreatedSoFar = 0;
            rnd ??= WorldGen.genRand;
            for (int i = 0; i < chests.Length; i++)
            {
                Item[] loot = GetLootFromLootTable(ref primaryLoot, secondaryLootPools, chestsCreatedSoFar, rnd);
                InsertLootOnChest(loot, chests[i]);
                chestsCreatedSoFar++;
            }
        }
        public static void InsertLootOnChest(Item[] loot, Chest chest)
        {
            if(chest == null)
            {
                TRAEProj.Instance.Logger.Warn("Attempted to fill a null chest with items!");
                return;
            }
            int lootAmount = loot.Length;
            for (int i = chest.item.Length - 1; i >= lootAmount; i--)
            {
                chest.item[i] = chest.item[i - lootAmount];
            }
            for (int i = 0; i < lootAmount; i++)
            {
                chest.item[i] = loot[i];
            }
        }
        public static Item GetItemFromLootData((int id, int min, int max) lootData, UnifiedRandom rnd = null)
        {
            rnd ??= WorldGen.genRand;
            return new Item(lootData.id, rnd.Next(lootData.min, lootData.max + 1));
        }
        static (int id, int min, int max)[] PutFormattedLootDataOnDataStructure(params int[] idMinMax)
        {
            if (idMinMax.Length % 3 == 0)
            {
                throw new Exception("Incorrectly formatted loot table");
            }
            (int id, int min, int max)[] result = new (int id, int min, int max)[idMinMax.Length / 3];
            for (int i = 0; i < result.Length; i += 3)
            {
                result[i / 3] = (idMinMax[i], idMinMax[i + 1], idMinMax[i + 2]);
            }
            return result;
        }
        static (int id, int min, int max)[] PutFormattedLootOfItemStack1DataOnDataStructure(params int[] ids)
        {
            if (ids.Length % 3 == 0)
            {
                throw new Exception("Incorrectly formatted loot table");
            }
            (int id, int min, int max)[] result = new (int id, int min, int max)[ids.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (ids[i], 1, 1);
            }
            return result;
        }
        public static void AddNewLootPoolOfSingleItemsToSecondaryLootPoolArray(ref (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools, params int[] ids)
        {
            secondaryLootPools = ExtendArrayAndInitializeIfNeeded(secondaryLootPools);
            (int id, int min, int max)[] newLoot = PutFormattedLootOfItemStack1DataOnDataStructure(ids);
            secondaryLootPools[^1] = (1f, newLoot);
        }
        public static void AddNewLootPoolOfSingleItemsToSecondaryLootPoolArray(ref (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools, float chance, params int[] ids)
        {
            secondaryLootPools = ExtendArrayAndInitializeIfNeeded(secondaryLootPools);
            (int id, int min, int max)[] newLoot = PutFormattedLootOfItemStack1DataOnDataStructure(ids);
            secondaryLootPools[^1] = (chance, newLoot);
        }
        public static void AddNewLootPoolToSecondaryPoolArray(ref (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools, float chance, params int[] idMinMax)
        {
            secondaryLootPools = ExtendArrayAndInitializeIfNeeded(secondaryLootPools);
            (int id, int min, int max)[] newLoot = PutFormattedLootDataOnDataStructure(idMinMax);
            secondaryLootPools[^1] = (chance, newLoot);
        }
        public static void AddNewLootPoolToSecondaryPoolArray(ref (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools, int id)
        {
            secondaryLootPools = ExtendArrayAndInitializeIfNeeded(secondaryLootPools);
            (int id, int min, int max)[] newLoot = PutFormattedLootDataOnDataStructure(id, 1, 1);
            secondaryLootPools[^1] = (1f, newLoot);
        }
        public static void AddNewLootPoolToSecondaryPoolArray(ref (float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools, float chance, int id)
        {
            secondaryLootPools = ExtendArrayAndInitializeIfNeeded(secondaryLootPools);
            (int id, int min, int max)[] newLoot = PutFormattedLootDataOnDataStructure(id, 1, 1);
            secondaryLootPools[^1] = (chance, newLoot);
        }
        private static (float chance, (int id, int min, int max)[] itemData)[] ExtendArrayAndInitializeIfNeeded((float chance, (int id, int min, int max)[] itemData)[] secondaryLootPools)
        {
            if (secondaryLootPools == null || secondaryLootPools.Length <= 0)
            {
                secondaryLootPools = new (float chance, (int id, int min, int max)[] itemData)[1];
            }
            Array.Resize(ref secondaryLootPools, secondaryLootPools.Length + 1);
            return secondaryLootPools;
        }

        static void PossiblyAddToLootPool(List<Item> chestLoot, (float chance, (int id, int min, int max)[] itemData) secondaryLootPool, UnifiedRandom rnd = null)
        {
            rnd ??= WorldGen.genRand;
            if (rnd.NextFloat() < secondaryLootPool.chance)
            {
                (int id, int min, int max) itemData = secondaryLootPool.itemData[rnd.Next(secondaryLootPool.itemData.Length)];
                chestLoot.Add(GetItemFromLootData(itemData, rnd));
            }
        }
        static int GetFromSecondaryLootPool((float chance, int[] items) secondaryLootPool, UnifiedRandom rnd = null)
        {
            rnd ??= WorldGen.genRand;
            if (rnd.NextFloat() < secondaryLootPool.chance)
            {
                return secondaryLootPool.items[rnd.Next(secondaryLootPool.items.Length)];
            }
            return 0;
        }
        static void ShufflePrimaryLoot(ref (int id, int min, int max)[] primaryLoot)
        {
            for (int i = primaryLoot.Length - 1; i > 0; i--)
            {
                int j = Main.rand.Next(i + 1);
                (int id, int min, int max) temp = primaryLoot[i];
                primaryLoot[i] = primaryLoot[j];
                primaryLoot[j] = temp;
            }
        }
    }
}
