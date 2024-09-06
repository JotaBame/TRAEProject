using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Accesory
{
    public class Boots : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!GetInstance<TRAEConfig>().MobilityRework && (item.type == ItemID.TerrasparkBoots || item.type == ItemID.LavaWaders || item.type == ItemID.LavaCharm))
            {
                player.GetModPlayer<Mobility>().TRAELavaMax += 420;
            }
            if (GetInstance<TRAEConfig>().MobilityRework)
            {
                switch (item.type)
                {
                    //running boots
                    case ItemID.HermesBoots:
                    case ItemID.FlurryBoots:
                    case ItemID.SailfishBoots:
                        player.moveSpeed += Mobility.bootSpeed * 0.01f;
                        player.accRunSpeed = 4.8f; // makes your movement speed 25 mph if it isn't already
                        break;
                    case ItemID.SandBoots:
                        player.moveSpeed += Mobility.bootSpeed * 0.01f;
                        player.desertBoots = false;
                        player.GetModPlayer<AccesoryEffects>().sandRunning = true;
                        player.accRunSpeed = 4.8f;
                        break;
                    //frog leg and tinkers
                    case ItemID.FrogLeg:
                    case ItemID.FrogWebbing:
                    case ItemID.FrogFlipper:
                        player.frogLegJumpBoost = false;
                        player.extraFall += 15;
                        player.jumpSpeedBoost -= Mobility.JSV(0.08f);

                        break;
                    case ItemID.FrogGear:
                        player.jumpSpeedBoost -= Mobility.JSV(0.08f);

                        player.frogLegJumpBoost = false;
                        player.accFlipper = true;
                        player.dashType = 1;
                        player.spikedBoots = 0;
                        player.extraFall += 15;
                        break;
                    case ItemID.AmphibianBoots:
                        player.frogLegJumpBoost = false;
                        player.extraFall += 15;
                        player.jumpSpeedBoost -= Mobility.JSV(0.08f);

                        player.moveSpeed += Mobility.amphibootSpeed * 0.01f;
                        player.accRunSpeed = 4.8f;
                        break;

                    //ice skates
                    case ItemID.IceSkates:
                        player.dashType = 99;
                        break;
                    case ItemID.FrostsparkBoots:

                        player.accRunSpeed = 4.8f;
                        player.moveSpeed -= 0.08f; // get rid of the 8% move speed buff separately to not mess up future calcs 
                        player.moveSpeed += Mobility.bootSpeed * 0.01f;
                        player.dashType = 99;

                        if (!GetInstance<TRAEConfig>().TerrasparkLineRework)
                        { player.rocketTimeMax += 5; }
                        break;
                    //rocket boots line
                    case ItemID.RocketBoots:
                        player.rocketTimeMax += 5;
                        break;
                    case ItemID.ObsidianWaterWalkingBoots:
                        player.waterWalk2 = false;
                        player.rocketBoots = 1;
                        player.rocketTimeMax += 10;
                        player.rocketBoots = player.vanityRocketBoots = 1;

                        player.GetModPlayer<AccesoryEffects>().FastFall = true;
                        player.noFallDmg = true;
                        player.fireWalk = false;
                        break;
                    case ItemID.FairyBoots:
                        player.accRunSpeed = 4.8f;
                        player.moveSpeed += Mobility.bootSpeed * 0.01f;
                        player.rocketTimeMax += 10;
                        break;
                    case ItemID.HellfireTreads:
                    case ItemID.SpectreBoots:
                        player.accRunSpeed = 4.8f;
                        player.moveSpeed += Mobility.bootSpeed * 0.01f;
                        player.rocketTimeMax += 5;
                        break;
                    //lighting terraspark
                    case ItemID.LightningBoots:

                        player.moveSpeed -= 0.08f; // get rid of the 8% move speed buff separately to not mess up future calcs 
                        player.accRunSpeed = 6f;
                        player.moveSpeed += Mobility.bootSpeed * 0.01f;
                        player.GetModPlayer<Mobility>().ankletAcc = true;
                        player.rocketTimeMax += 5;

                        break;
                    case ItemID.TerrasparkBoots:
                        player.moveSpeed -= 0.08f; // get rid of the 8% move speed buff separately to not mess up future calcs 
                        player.moveSpeed += Mobility.bootSpeed * 0.01f;
                        player.GetModPlayer<Mobility>().ankletAcc = true;
                        player.rocketTimeMax += 5;
                        if (GetInstance<TRAEConfig>().TerrasparkLineRework)
                        {
                            player.iceSkate = false;
                            player.lavaMax -= 42;


                            player.lavaRose = false;
                            player.fireWalk = false;
                            player.waterWalk = false;
                            player.accRunSpeed = 6f;
                            player.dashType = 1;

                        }
                        if (!GetInstance<TRAEConfig>().TerrasparkLineRework)
                        {
                            player.dashType = 99;
                            player.GetModPlayer<Mobility>().TRAELavaMax += 720;
                            player.GetModPlayer<AccesoryEffects>().waterRunning = true;
                            player.GetModPlayer<AccesoryEffects>().LavaShield = true;
                            player.GetModPlayer<Mobility>().TRAEwaterwalk = true;

                        }
                        break;
                    //utility boots
                    case ItemID.WaterWalkingBoots:
                        player.GetModPlayer<AccesoryEffects>().waterRunning = true;
                        player.GetModPlayer<Mobility>().TRAEwaterwalk = true;
                        player.waterWalk = true;
                        break;
                    case ItemID.LavaCharm:
                        player.lavaRose = true;
                        player.buffImmune[BuffID.Burning] = true;

                        player.GetModPlayer<AccesoryEffects>().LavaShield = true;
                        player.GetModPlayer<Mobility>().TRAELavaMax += 720;
                        break;
                    case ItemID.LavaWaders:

                        player.GetModPlayer<AccesoryEffects>().waterRunning = true;
                        player.GetModPlayer<AccesoryEffects>().LavaShield = true;
                        player.GetModPlayer<Mobility>().TRAEwaterwalk = true;
                        player.buffImmune[BuffID.Burning] = true;
                        player.GetModPlayer<Mobility>().TRAELavaMax += 720;

                        player.lavaRose = true;
                        break;
                }
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            if (GetInstance<TRAEConfig>().MobilityRework)
            {
                switch (item.type)
                {
                    case ItemID.FrostsparkBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = Mobility.bootSpeed + "% increased movement speed";
                                if (!GetInstance<TRAEConfig>().TerrasparkLineRework)
                                {
                                    line.Text += "\nAllows flight and extra mobility on ice";
                                }
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Allows skating by double tapping on the ground";
                            }

                        }
                        break;
                    case ItemID.LightningBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = Mobility.bootSpeed + "% increased movement speed" + "\nGreatly increases acceleration";

                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Provides rocket boot flight";
                            }

                        }
                        break;
                    case ItemID.TerrasparkBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (GetInstance<TRAEConfig>().TerrasparkLineRework)
                            {
                                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                                {
                                    line.Text = Mobility.bootSpeed + "% increased movement speed" + "\nGreatly increases acceleration";
                                }
                                if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                                {
                                    line.Text = "Provides rocket boot flight";
                                }
                                if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                                {
                                    line.Text = "Provides a dash";
                                }
                                if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                                {
                                    line.Text = "";
                                }
                                if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                                {
                                    line.Text = "";
                                }
                            }
                            else
                            {
                                if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                                {
                                    line.Text = "20% increased movement speed and greatly increased acceleration";
                                }
                                if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                                {
                                    line.Text = "Grants immunity to fire blocks and 12 seconds of immunity to lava\nIncreases movement speed and shields the wearer when entering liquids\nAllows skating when double tapping on the ground";
                                }
                            }

                        }
                        break;

                    case ItemID.HermesBoots:
                    case ItemID.SailfishBoots:
                    case ItemID.FlurryBoots:
                        foreach (TooltipLine line in tooltips)
                        {

                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = Mobility.bootSpeed + "% increased movement speed";
                            }
                        }
                        break;
                    case ItemID.HellfireTreads:
                        foreach (TooltipLine line in tooltips)
                        {

                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = Mobility.bootSpeed + "% increased movement speed";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Provides rocket boot flight";
                            }
                        }
                        break;
                    case ItemID.SandBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = Mobility.bootSpeed + "% increased movement speed" + "\n20% increased movement speed on sand";
                            }
                        }
                        break;
                    case ItemID.FairyBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = Mobility.bootSpeed + "% increased movement speed";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Provides extended rocket boot flight";
                            }
                        }
                        break;
                    case ItemID.SpectreBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = Mobility.bootSpeed + "% increased movement speed";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Provides rocket boot flight";
                            }
                        }
                        break;

                    case ItemID.AmphibianBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {

                                line.Text = Mobility.amphibootSpeed + "% increased movement speed\n" + Mobility.flegSpeed + "% increased jump speed";

                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Allows auto-jump";
                            }
                        }
                        break;

                    case ItemID.WaterWalkingBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "The wearer can walk on water\nIncreases running speed by 33% when walking on a liquid";
                            }
                        }
                        break;
                    case ItemID.FrogGear:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Allows the wearer to perform a short dash";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                            {
                                line.Text = "Increases jump speed by 24% and allows auto jump";
                            }
                        }
                        break;
                    case ItemID.FrogFlipper:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Increases jump speed by 24% and allows auto jump";
                            }
                        }
                        break;
                    case ItemID.FrogLeg:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Increases jump speed by 24% and allows auto jump";
                            }
                        }
                        break;
                    case ItemID.IceSkates:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Allows skating by double tapping on the ground";
                            }
                        }
                        break;
                    case ItemID.RocketBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Provides rocket boot flight";
                            }
                        }
                        break;
                    case ItemID.ObsidianWaterWalkingBoots:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Provides extended rocket boot flight";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Hold DOWN to increase falling speed\nGrants immunity to fall damage";
                            }
                        }
                        break;
                    case ItemID.LavaCharm:
                        foreach (TooltipLine line in tooltips)
                        {
                            {
                                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                                {
                                    line.Text = "Grants 12 seconds of lava immunity, and reduces damage when touching lava\nShields the wearer when entering lava";
                                }
                            }
                        }
                        break;
                    case ItemID.LavaWaders:
                        foreach (TooltipLine line in tooltips)
                        {

                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Increases movement speed and shields the wearer when entering liquids";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Allows walking on water and grants 12 seconds of immunity to lava";
                            }

                        }
                        break;
                }
            }
        }
    }
}