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
using Terraria.GameContent.Bestiary;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;

namespace TRAEProject.Changes.NPCs
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
            if (shop.NpcType == NPCID.Demolitionist)
            {
                shop.TryGetEntry(ItemID.Grenade, out var entry);
                entry.Disable();
                shop.Add(ItemID.Grenade, Condition.DownedEyeOfCthulhu);
            }
            /*
            if (shop.NpcType == NPCID.BestiaryGirl)
            {
                shop.Add(ItemID.DiggingMoleMinecart, new Condition("noCart", () => Main.GetBestiaryProgressReport().CompletionPercent < 0.6f));
            }
            */
            if (shop.NpcType == NPCID.Wizard)
                shop.Add(ItemID.FastClock);
            if (shop.NpcType == NPCID.ArmsDealer)
            {
                shop.TryGetEntry(ItemID.QuadBarrelShotgun, out var entry);
                entry.Disable();
                shop.TryGetEntry(ItemID.Shotgun, out var entry1);
                entry1.Disable();
                shop.InsertAfter(ItemID.Minishark, ItemID.Shotgun, Condition.DownedEowOrBoc);
                if (WorldGen.crimson)
                {
                    shop.TryGetEntry(ItemID.UnholyArrow, out var entry2);
                    entry2.Disable();
                    shop.InsertAfter(ItemID.MusketBall, ItemType<BloodyArrow>(), Condition.Hardmode);

                }

                shop.Add(ItemID.QuadBarrelShotgun, Condition.Hardmode, Condition.InGraveyard);
                shop.Add(ItemID.Uzi, Condition.Hardmode);
                shop.InsertAfter(ItemID.AmmoBox, ItemID.RifleScope, Condition.Hardmode);

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
            if (shop.NpcType == NPCID.Truffle)
            {
                shop.TryGetEntry(ItemID.MushroomSpear, out var entry);
                entry.Disable(); 
                shop.Add(ItemID.MushroomSpear, Condition.DownedPlantera);

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
