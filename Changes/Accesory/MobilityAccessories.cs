using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TRAEProject.Changes.Accesory
{
    public class MoveSpeed : ModPlayer
    {
                public override void PreUpdate()
        {
            Player.rocketTimeMax = 7; // without this Obsidian Hover Shoes permanently set it to 14          
   
        }
		public override void PostUpdateEquips()
        {
            Player.jumpSpeedBoost += 1f;
            Player.moveSpeed *= 1.33f;
        }
    }
    public class MobilityAccessories : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.HermesBoots:
                case ItemID.FlurryBoots:
                case ItemID.SandBoots:
                case ItemID.SailfishBoots:
                    player.moveSpeed += 0.25f;
					player.accRunSpeed = 4.8f;
                    return;
                case ItemID.FrogLeg:
                case ItemID.FrogWebbing:
                case ItemID.FrogFlipper:
                    player.frogLegJumpBoost = false;
                    player.extraFall += 15;
                    player.jumpSpeedBoost += 1.4f;
                    return;
                case ItemID.FrogGear:
                    player.frogLegJumpBoost = false;
                    player.accFlipper = true;
                    player.dashType = 2;
                    player.spikedBoots = 0;
                    player.extraFall += 15;
                    player.jumpSpeedBoost += 1.4f;
                    return;
                case ItemID.AmphibianBoots:
                    player.frogLegJumpBoost = false;
                    player.extraFall += 15;
                    player.jumpSpeedBoost += 1.4f;
                    player.moveSpeed += 0.25f;
                    player.accRunSpeed = 4.8f;
                    return;     
                case ItemID.Aglet:
                    player.moveSpeed += 0.05f;
                    return;
                case ItemID.AnkletoftheWind:
                    player.moveSpeed -= 0.1f;
                    player.moveSpeed *= 1.2f;
                    return;
                case ItemID.IceSkates:
                    player.GetModPlayer<TRAEPlayer>().icceleration = true;
                    return;
                case ItemID.FrostsparkBoots:
                    player.rocketTimeMax -= 7;
                    player.accRunSpeed = 4.8f;
                    player.moveSpeed -= 0.08f; // get rid of the 8% move speed buff separately to not mess up future calcs 
                    player.GetModPlayer<TRAEPlayer>().icceleration = true; 
                    player.moveSpeed += 0.25f;
                    return;
                case ItemID.TerrasparkBoots:
                    player.iceSkate = false;
                    player.lavaMax -= 42;
                    player.fireWalk = false;
                    player.waterWalk = false;
                    player.accRunSpeed = 5.4f; 
                    player.moveSpeed -= 0.08f; // get rid of the 8% move speed buff separately to not mess up future calcs 
                    player.moveSpeed += 0.25f;
                    player.dash = 1;
                    return;
                case ItemID.LightningBoots:
                    player.moveSpeed -= 0.08f; // get rid of the 8% move speed buff separately to not mess up future calcs 
                    player.jumpSpeedBoost += 0.5f;
                    player.rocketTimeMax = 7;
                    player.accRunSpeed = 5.4f; 
                    player.moveSpeed += 0.25f;
                    player.moveSpeed *= 1.2f;
                    return;
                case ItemID.RocketBoots:
                    player.rocketTimeMax = 7;
                    return;
                case ItemID.FairyBoots:
                case ItemID.SpectreBoots:
                    player.rocketTimeMax = 7; 
                    player.accRunSpeed = 5.2f;
                    player.moveSpeed += 0.25f;
                    return;
                case ItemID.ObsidianHorseshoe:
                    player.fireWalk = false;
                    player.GetModPlayer<TRAEPlayer>().FastFall = true;
                    return;
                case ItemID.ObsidianWaterWalkingBoots:
                    player.waterWalk2 = false;
                    player.rocketBoots = 1;
                    player.rocketTimeMax = 14;
                    player.GetModPlayer<TRAEPlayer>().FastFall = true;
                    player.noFallDmg = true;
                    player.fireWalk = false;
                    player.buffImmune[BuffID.Burning] = false;
                    return;
                case ItemID.BalloonHorseshoeHoney:
                case ItemID.HoneyBalloon:
                    player.lifeRegen += 2;
                    return;
                case ItemID.BundleofBalloons:
                    player.noFallDmg = true;
                    return;
                case ItemID.WaterWalkingBoots:
                    player.GetModPlayer<TRAEPlayer>().waterRunning = true;
                    player.waterWalk = true;
                    return;
                case ItemID.LavaCharm:
                    player.GetModPlayer<TRAEPlayer>().LavaShield = true;
                    return;
                case ItemID.LavaWaders:
                    player.GetModPlayer<TRAEPlayer>().waterRunning = true;
                    player.GetModPlayer<TRAEPlayer>().LavaShield = true;
                    player.fireWalk = false;
                    player.lavaImmune = true;
                    player.lavaRose = false;
                    return;
                case ItemID.HiveBackpack:
                    player.GetModPlayer<TRAEPlayer>().Hivepack = true;
                    player.strongBees = false;
                    return;
                case ItemID.EmpressFlightBooster:
                    player.jumpSpeedBoost -= 2.4f;
                    return;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.HermesBoots:
                case ItemID.SailfishBoots:
                case ItemID.FlurryBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "25% increased movement speed";
                        }
                    }
                    return;
                case ItemID.SandBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "25% increased movement speed, run even faster on sand";
                        }
                    }
                    return;
                case ItemID.Aglet:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "10% increased movement speed";
                        }
                    }
                    return;
                case ItemID.AnkletoftheWind:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Multiplies movement speed by 1.2";
                        }
                    }
                    return;
                case ItemID.FairyBoots:
                case ItemID.SpectreBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "25% increased movement speed";
                        }
                    }
                    return;
                case ItemID.FrostsparkBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "25% increased movement speed";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "Increases acceleration and mobility on ice";
                        }
                    }
                    return;
                case ItemID.AmphibianBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "25% increased movement speed";
                        }
                    }
                    return;
                case ItemID.LightningBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Allows flight";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "25% increased movement speed, then multiplied by 20%";
                        }
                    }
                    return;
                case ItemID.TerrasparkBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "25% increased movement speed, then multiplied by 20%";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "Allows flight and the ability to dash";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.text = "";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip3")
                        {
                            line.text = "";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip4")
                        {
                            line.text = "";
                        }
                    }
                    return;
                case ItemID.WaterWalkingBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "The wearer can walk on water\nIncreases running speed by 33% when walking on a liquid";
                        }
                    }
                    return;
                case ItemID.ObsidianHorseshoe:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Negates fall damage";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "Hold DOWN to increase falling speed";
                        }
                    }
                    return;
                case ItemID.ObsidianWaterWalkingBoots:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Allows extended flight";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "Hold DOWN to increase falling speed";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.text = "Grants immunity to fall damage";
                        }
                    }
                    return;
                case ItemID.LavaCharm:
                    foreach (TooltipLine line in tooltips)
                    {
                        {
                            if (line.mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.text += "\nShields the wearer when entering lava";
                            }
                        }
                    }
                    return;
                case ItemID.LavaWaders:
                    foreach (TooltipLine line in tooltips)
                    {
                        {
                            if (line.mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.text = "Increases movement speed and shields the wearer when entering liquids";
                            }
                            if (line.mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.text = "Allows walking on water and grants immunity to lava";
                            }
                        }
                    }
                    return;
                case ItemID.HiveBackpack:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Stores up to 16 bees while grounded, releases them while in mid-air\nIncreases jump height by 4.5% for every bee stored\nDoubles strength, recharge delay, and release rate of the bees when honeyed";
                        }
                    }
                    return;
                case ItemID.FrogGear:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "Allows the wearer to perform a short dash";
                        }
                    }
                    return;
                case ItemID.HoneyBalloon:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Releases bees and douses you in honey when damaged";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "Increases jump height and life regeneration";
                        }
                    }
                    return;
                case ItemID.BalloonHorseshoeHoney:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Releases bees and douses you in honey when damaged and negates fall damage";
                        }
                        if (line.mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.text = "Increases jump height and life regeneration";
                        }
                    }
                    return;
                case ItemID.IceSkates:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Increases acceleration\nIncreases mobility in ice";
                        }
                    }
                    return;
            }
        }
   
    }
}





