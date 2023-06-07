using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Misc.Potions;
using System.IO;
using Terraria.ModLoader.IO;
using TRAEProject.Common;
using TRAEProject.NewContent.NPCs.Underworld.ObsidianBasilisk;
using TRAEProject.NewContent.NPCs.Underworld.Phoenix;
using TRAEProject.NewContent.NPCs.Underworld.Salalava;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using Mono.CompilerServices.SymbolWriter;

namespace TRAEProject.Changes
{

    class NPCShops : GlobalNPC
    {
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            for (int i = 0; i < shop.Length; i++)
            {
                if (shop[i] == ItemID.CelestialMagnet)
                {
                    for (int j = i + 1; j < shop.Length; j++)
                    {
                        shop[j - 1] = shop[j];
                    }
                    shop[shop.Length - 1] = 0;
                    nextSlot--;
                }    
				if (shop[i] == ItemID.PulseBow)
                {
                    for (int j = i + 1; j < shop.Length; j++)
                    {
                        shop[j - 1] = shop[j];
                    }
                    shop[shop.Length - 1] = 0;
                    nextSlot--;
                }
                if (shop[i] == ItemID.Gatligator)
                {
                    for (int j = i + 1; j < shop.Length; j++)
                    {
                        shop[j - 1] = shop[j];
                    }
                    shop[shop.Length - 1] = 0;
                    nextSlot--;
                }
            }
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Wizard)
                shop.Add(ItemID.FastClock);
            if (shop.NpcType == NPCID.ArmsDealer)
            {
                shop.Add(ItemID.Gatligator, /*Condition.InGraveyard,*/ Condition.Hardmode);
                shop.Add(ItemID.Uzi, Condition.DownedPlantera/*, Condition.NotInGraveyard*/);

                //shop.Add(ItemID.Uzi, Condition.Hardmode, Condition.NotInGraveyard);
            }
            if (shop.NpcType == NPCID.SkeletonMerchant)
            {
                //int[] items = new int[] { ItemID.WoodenBoomerang, ItemID.Umbrella, ItemID.WandofSparking, ItemID.PortableStool, ItemID.Aglet, ItemID.ClimbingClaws, ItemID.CordageGuide, ItemID.Radar, ItemID.MagicDagger };


                shop.InsertAfter(ItemID.StrangeBrew, ItemType<PowerBrew>(), Condition.Hardmode);

                shop.Add(ItemID.Rally, Condition.MoonPhases04);
                shop.Add(ItemID.ChainKnife, Condition.MoonPhases15);
                shop.Add(ItemID.BoneSword, Condition.MoonPhases26);
                shop.Add(ItemID.BookofSkulls, Condition.MoonPhases37);


            }
            if (shop.NpcType == NPCID.Truffle && !NPC.downedPlantBoss)
            {
                shop.TryGetEntry(ItemID.MushroomSpear, out var entry);
                entry.Disable();
            }
            if (shop.NpcType == NPCID.WitchDoctor)
            {
                shop.TryGetEntry(ItemID.HerculesBeetle, out var entry);
                entry.Disable();
                shop.InsertBefore(ItemID.PureWaterFountain, ItemID.HerculesBeetle, Condition.TimeDay);
            }
            if (shop.NpcType == NPCID.Pirate)
            {
                shop.Add(ItemID.TrifoldMap);
                shop.Add(ItemID.ThePlan);
            }
            var mechDowned = new Condition("DownedAMech", () => TRAEWorld.downedAMech);
            if (shop.NpcType == NPCID.Steampunker)
            {

                    shop.TryGetEntry(ItemID.StaticHook, out var entry);
                    entry.Disable();
                    shop.TryGetEntry(ItemID.Jetpack, out entry);
                    entry.Disable();
                    shop.TryGetEntry(ItemID.Cog, out entry);
                    entry.Disable();
                shop.InsertAfter(ItemID.Clentaminator, ItemID.Jetpack, mechDowned);
                shop.InsertAfter(ItemID.SteampunkMinecart, ItemID.Cog, mechDowned);
                shop.InsertAfter(ItemID.LogicGateLamp_Faulty, ItemID.StaticHook, mechDowned);

            }
        }

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            //if (shopName == NPCShopDatabase.GetShopName(NPCID.Steampunker) && TRAEWorld.downedAMech)
            //{

            //    int slot = 0;
            //    do
            //    {
            //        if (++slot >= items.Length)
            //        {
            //            Main.NewText(slot);
            //            return;

            //        }
            //    }
            //    while (!items[slot].IsAir);
            //    Main.NewText(slot);
            //    //    items[slot].SetDefaults(ItemID.Cog);
            //    //    items[slot + 1].SetDefaults(ItemID.StaticHook);
            //    //    items[slot + 2].SetDefaults(ItemID.Jetpack);

            //}
            if (shopName == NPCShopDatabase.GetShopName(NPCID.DD2Bartender))
            {
                
                if (!TRAEWorld.downedOgre)
                {
                    items[4].SetDefaults(ItemID.None); 
                    items[5].SetDefaults(ItemID.None);
                    items[6].SetDefaults(ItemID.None);

                    items[14].SetDefaults(ItemID.None);
                    items[15].SetDefaults(ItemID.None);
                    items[16].SetDefaults(ItemID.None);

                    items[24].SetDefaults(ItemID.None);
                    items[25].SetDefaults(ItemID.None);
                    items[26].SetDefaults(ItemID.None);

                    items[34].SetDefaults(ItemID.None);
                    items[35].SetDefaults(ItemID.None);
                    items[36].SetDefaults(ItemID.None);
                }
                if (!TRAEWorld.downedBetsy)
                {
                    items[7].SetDefaults(ItemID.None);
                    items[8].SetDefaults(ItemID.None);
                    items[9].SetDefaults(ItemID.None);

                    items[17].SetDefaults(ItemID.None);
                    items[18].SetDefaults(ItemID.None);
                    items[19].SetDefaults(ItemID.None);

                    items[27].SetDefaults(ItemID.None);
                    items[28].SetDefaults(ItemID.None);
                    items[29].SetDefaults(ItemID.None);

                    items[37].SetDefaults(ItemID.None);
                    items[38].SetDefaults(ItemID.None);
                    items[39].SetDefaults(ItemID.None);
                }
            }
        }
    }
}
