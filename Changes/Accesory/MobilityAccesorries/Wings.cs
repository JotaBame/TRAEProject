using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace TRAEProject.Changes.Accesory
{
    public class WingChanges : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            int PreHardmode = 45;
            int EarlyHardmode = 150;
            int EarlyHardmode2 = 200;
            int PostPlant = 230;
            int PostPlant2 = 240;
            
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.CreativeWings].FlyTime = PreHardmode;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.LongTrailRainbowWings].FlyTime = 360;

            
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.LeafWings].FlyTime = 360;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.DemonWings].FlyTime = EarlyHardmode;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.AngelWings].FlyTime = EarlyHardmode;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FairyWings].FlyTime = 100;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.HarpyWings].FlyTime = 100;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FrozenWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.ButterflyWings].FlyTime = EarlyHardmode;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.BeeWings].FlyTime = EarlyHardmode;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FinWings].FlyTime = EarlyHardmode;

            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.Jetpack].FlyTime = EarlyHardmode;



            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FlameWings].FlyTime = EarlyHardmode;

            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.SpectreWings].FlyTime = PostPlant;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.BoneWings].FlyTime = EarlyHardmode;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.BeetleWings].FlyTime = EarlyHardmode;

            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.Hoverboard].FlyTime = PostPlant;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.SteampunkWings].FlyTime = PostPlant;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.BatWings].FlyTime = PostPlant;

            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.MothronWings].FlyTime = PostPlant;


            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FishronWings].FlyTime = PostPlant2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.SpookyWings].FlyTime = PostPlant;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.TatteredFairyWings].FlyTime = PostPlant2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.RainbowWings].FlyTime = PostPlant;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FestiveWings].FlyTime = PostPlant;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.BetsyWings].FlyTime = PostPlant;

            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.SolarWings].FlyTime = PostPlant2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.StardustWings].FlyTime = PostPlant2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.NebulaMantle].FlyTime = PostPlant2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.VortexBooster].FlyTime = PostPlant2;

            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.CenxsWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.RedsWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.JimsWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.DTownsWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.Yoraiz0rsSpell].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.LazuresBarrierPlatform].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FoodBarbarianWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.SafemanWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.LeinforsWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.CrownosWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.GhostarsWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.GroxTheGreatWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.LokisWings].FlyTime = EarlyHardmode2;
            ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.SkiphssPaws].FlyTime = EarlyHardmode2;

            
        }
        public override void SetDefaults(Item item)
        {
            if(item.type == ItemID.BeetleWings)
            {
                item.defense = 12;
            }
        }
        
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.CenxsWings:
                case ItemID.RedsWings:
                case ItemID.JimsWings:
                case ItemID.DTownsWings:
                case ItemID.Yoraiz0rWings:
                case ItemID.FoodBarbarianWings:
                case ItemID.SafemanWings:
                case ItemID.LeinforsWings:
                case ItemID.CrownosWings:
                case ItemID.GhostarsWings:
                case ItemID.GroxTheGreatWings:
                case ItemID.BejeweledValkyrieWing:
                case ItemID.LokisWings:
                case ItemID.SkiphsWings:
                    player.moveSpeed += 0.1f;
                    player.jumpSpeedBoost += Mobility.JSV(0.1f);
                break;
                case ItemID.FinWings:
                    player.moveSpeed += 0;
                    player.ignoreWater = true;
                    break;
                case ItemID.AngelWings:
                    player.jumpSpeedBoost += Mobility.JSV(0.1f);
                    break;
                case ItemID.DemonWings:
                    player.moveSpeed += 0.1f;
                    break;
                case ItemID.HarpyWings:
                    player.jumpSpeedBoost += Mobility.JSV(0.25f);
                    break;
                case ItemID.FairyWings:
                    player.moveSpeed += 0.2f;
                    break;
                case ItemID.ButterflyWings:
                    player.jumpSpeedBoost += Mobility.JSV(0.25f);
                    break;
                case ItemID.BeeWings:
                    player.moveSpeed += 0.2f;
                    break;
                //case ItemID.FrozenWings:
                case ItemID.Jetpack:
                    player.jumpSpeedBoost += Mobility.JSV(0.2f);
                    break;
                case ItemID.FlameWings:
                    player.moveSpeed += 0.12f;
                    player.jumpSpeedBoost += Mobility.JSV(0.12f);
                    player.GetDamage(DamageClass.Generic) += 0.12f;
                    break;
                case ItemID.SteampunkWings:
                    player.moveSpeed += 0.12f;
                    player.jumpSpeedBoost += Mobility.JSV(0.12f);
                    player.GetModPlayer<TRAEProject.NewContent.Items.Accesories.MobilityJumps.TRAEJumps>().advFlight = true;
                    break;
                case ItemID.BatWings:
                    player.moveSpeed += 0.33f;
                    break;
                case ItemID.BoneWings:
                    player.moveSpeed += 0.25f;
                    player.jumpSpeedBoost += Mobility.JSV(0.25f);
                    break;
                case ItemID.MothronWings:
                    player.moveSpeed += 0.18f;
                    player.jumpSpeedBoost += Mobility.JSV(0.18f);
                    player.GetModPlayer<Mobility>().ankletAcc = true;
                    break;
                case ItemID.GhostWings:
                    player.moveSpeed += 0.18f;
                    player.jumpSpeedBoost += Mobility.JSV(0.18f);
                    break;

                case ItemID.BeetleWings:
                    player.moveSpeed += 0.12f;
                    player.jumpSpeedBoost += Mobility.JSV(0.12f);
                    break;
    

                case ItemID.TatteredFairyWings:
                    player.wingTime = player.wingTimeMax;
                    player.moveSpeed += 0.15f;
                    player.jumpSpeedBoost += Mobility.JSV(0.15f);
                    break;
                case ItemID.SpookyWings:
                    player.moveSpeed += 0.36f;
                    player.jumpSpeedBoost += Mobility.JSV(0.18f);
                    break;
                case ItemID.FestiveWings:
                    player.moveSpeed += 0.3f;
                    player.jumpSpeedBoost += Mobility.JSV(0.3f);
                    break;
                case ItemID.FishronWings:
                    player.GetModPlayer<AccesoryEffects>().FastFall = true;
                    player.moveSpeed += 0.25f;
                    player.jumpSpeedBoost += Mobility.JSV(0.25f);
                    player.GetModPlayer<Mobility>().ankletAcc = true;
                    break;
                case ItemID.RainbowWings:
                    player.moveSpeed += 0.25f;
                    player.jumpSpeedBoost += Mobility.JSV(0.4f);
                    break;
                case ItemID.BetsyWings:
                    player.moveSpeed += 0.25f;
                    player.jumpSpeedBoost += Mobility.JSV(0.15f);
                    break;
                case ItemID.WingsSolar:
                    player.moveSpeed += 0.4f;
                    player.jumpSpeedBoost += Mobility.JSV(0.4f);
                    break;
                case ItemID.WingsStardust:
                    player.moveSpeed += 0.25f;
                    player.jumpSpeedBoost += Mobility.JSV(0.6f);
                    break;
                case ItemID.WingsNebula:
                    player.wingTime = player.wingTimeMax;
                    player.moveSpeed += 0.15f;
                    player.jumpSpeedBoost += Mobility.JSV(0.15f);
                    break;
                case ItemID.WingsVortex:
                    player.moveSpeed += 0.25f;
                    player.jumpSpeedBoost += Mobility.JSV(0.25f);
                    break;
                case ItemID.LongRainbowTrailWings:
                    player.moveSpeed += 0.5f;
                    player.jumpSpeedBoost += Mobility.JSV(0.5f);
                    break;
            }
            if(ModLoader.TryGetMod("ModLoader", out Mod tModLoader))
            {
                if(item.ModItem != null && item.ModItem.Mod == tModLoader && item.wingSlot > 0)
                {
                    player.moveSpeed += 0.12f;
                    player.jumpSpeedBoost += Mobility.JSV(0.12f);
                }
            }
        }
        public static void PostProcessChanges(Player player)
        {
            player.accRunSpeed = 3;

            if(player.wingsLogic == 4)
            {
                if(player.gravDir == 1)
                {
                    if(player.velocity.Y < -Player.jumpSpeed)
                    {
                        player.velocity.Y = -Player.jumpSpeed;
                    }
                }
                else
                {
                    if(player.velocity.Y > Player.jumpSpeed)
                    {
                        player.velocity.Y = Player.jumpSpeed;
                    }
                }
            }
            //player.wingTimeMax += 60;
            //player.wingTimeMax *= 2;
            //Main.NewText("WingMax: " + player.wingTimeMax);
            //Main.NewText("WingTime: " + player.wingTime + "/" +  player.wingTimeMax);
        }
        public override bool WingUpdate(int wings, Player player, bool inUse)
        {
            return base.WingUpdate(wings, player, inUse);
        }
        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            //Main.NewText(speed);
            //speed = 6.75f;
        }
        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            //Main.NewText(maxAscentMultiplier);
            //Main.NewText(constantAscend);
            if (!player.TryingToHoverDown)
            {
                player.velocity.Y -= 0.2f * player.gravDir;
            }
            maxAscentMultiplier = 1f;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.wingSlot >= 0)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    float flightTime = MathF.Round((float)ArmorIDs.Wing.Sets.Stats[item.wingSlot].FlyTime / 60, 2);
                    if (tooltips[i].Mod == "Terraria" && tooltips[i].Name == "Tooltip0")
                    {



                        string text = "Flight Time: " + flightTime + " seconds";
                        if (item.wingSlot == ArmorIDs.Wing.TatteredFairyWings || item.wingSlot == ArmorIDs.Wing.NebulaMantle)
                        {
                            text = "Flight Time: infinite";
                        }
                        tooltips.Insert(i - 1, new TooltipLine(Mod, "WingTime", text));
                        break;
                    }
                }
            }
            switch (item.type)
            {
                case ItemID.FinWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nImproves movement in liquids";
                        }
                    }
                    break;

                case ItemID.AngelWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n10% increased jump speed";
                        }
                    }
                    break;
                case ItemID.DemonWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n10% increased movement speed";
                        }
                    }
                    break;
                case ItemID.HarpyWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n25% increased jump speed";
                        }
                    }
                    break;
                case ItemID.FairyWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n20% increased movement speed";
                        }
                    }
                    break;
                case ItemID.ButterflyWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n25% increased jump speed";
                        }
                    }
                    break;
                case ItemID.BeeWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n20% increased movement speed";
                        }
                    }
                    break;
                case ItemID.Jetpack:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n20% increased jump speed";
                        }
                    }
                    break;
                case ItemID.GhostWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n18% increased movement and jump speed";
                        }
                    }
                    break;
                case ItemID.BeetleWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n12% increased movement and jump speed";
                        }
                    }
                    break;
                case ItemID.BoneWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n25% increased movement and jump speed";
                        }
                    }
                    break;
                case ItemID.Hoverboard:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nHold down to hover for 45% more movement speed";
                        }
                    }
                    break;
                case ItemID.FlameWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n12% increased movement and jump speed\n12% increased damage";
                        }
                    }
                    break;
                case ItemID.BatWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n36% increased movement speed";
                        }
                    }
                    break;
                case ItemID.MothronWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n18% increased movement and jump speed" + "\nGrants horizontal acceleration";
                        }
                    }
                    break;
                case ItemID.SteampunkWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n12% increased movement and jump speed" + "\nGrants advanced flight";
                        }
                    }
                    break;

                case ItemID.SpookyWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n36% increased movement speed\n18% increased jump speed";
                        }
                    }
                    break;
                case ItemID.TatteredFairyWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n15% increased movement and jump speed";
                        }
                    }
                    break;
                case ItemID.FestiveWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n30% increased movement and jump speed" + "\nDrops ornaments while flying";
                        }
                    }
                    break;
                case ItemID.FishronWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n25% increased movement and jump speed" + "\nHold down to fall faster\nGreatly increases acceleration";
                        }
                    }
                    break;
                case ItemID.BetsyWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n25% increased movement speed\n15% increased jump speed" + "\nHold down to hover for 45% more movement speed";
                        }
                    }
                    break;
                case ItemID.RainbowWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n20% increased movement speed and 40% increased jump speed";
                        }
                    }
                    break;

                case ItemID.WingsNebula:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n15% increased movement and jump speed\nHold down to hover for 45% more movement speed";
                        }
                    }
                    break;
                case ItemID.WingsSolar:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n40% increased movement and jump speed";
                        }
                    }
                    break;
                case ItemID.WingsStardust:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n25% increased movement speed\n60% increased jump speed";
                        }
                    }
                    break;
                case ItemID.WingsVortex:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n25% increased movement and jump speed" + "\nHold down to hover for 55% more movement speed";
                        }
                    }
                    break;
                case ItemID.LongRainbowTrailWings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n50% increased movement and jump speed" + "\nHold down to hover for 100% more movement speed";
                        }
                    }
                    break;
                case ItemID.CenxsWings:
                case ItemID.RedsWings:
                case ItemID.JimsWings:
                case ItemID.DTownsWings:
                case ItemID.Yoraiz0rWings:
                case ItemID.FoodBarbarianWings:
                case ItemID.SafemanWings:
                case ItemID.LeinforsWings:
                case ItemID.WillsWings:
                case ItemID.ArkhalisWings:
                case ItemID.CrownosWings:
                case ItemID.GhostarsWings:
                case ItemID.GroxTheGreatWings:
                case ItemID.BejeweledValkyrieWing:
                case ItemID.LokisWings:
                case ItemID.SkiphsWings:
                    tooltips.Add(new TooltipLine(Mod, "dev wing speed", "Increases movement and jump speed by 10%"));
                    break;
            }
            if(ModLoader.TryGetMod("ModLoader", out Mod tModLoader))
            {
                if(item.ModItem != null && item.ModItem.Mod == tModLoader && item.wingSlot > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "tMod wing speed", "Increases movement and jump speed by 12%"));
                }
            }
        }
    }
    
}