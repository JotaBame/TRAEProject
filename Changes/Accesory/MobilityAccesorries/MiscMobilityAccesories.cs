using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;


namespace TRAEProject.Changes.Accesory
{
    public class MiscMobilityAccesories : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            switch(item.type)
            {
                case ItemID.GravityGlobe:
                item.expert = false;

                item.rare = ItemRarityID.Orange;
                break;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.ObsidianHorseshoe)
            {
                player.fireWalk = false;
                player.GetModPlayer<AccesoryEffects>().FastFall = true;
                player.gravControl = true;
                player.GetModPlayer<GravitationPlayer>().noFlipGravity = true;
            }
            if (GetInstance<TRAEConfig>().MobilityRework)
            {

                switch (item.type)
                {
                    //aglet anklet
                    case ItemID.Aglet:
                        player.moveSpeed += 0.05f;
                        break;
                    case ItemID.AnkletoftheWind:
                        player.GetModPlayer<Mobility>().ankletAcc = true;
                        player.moveSpeed -= 0.1f;
                        break;
                    case ItemID.GravityGlobe:
                        player.noFallDmg = true;
                        player.GetModPlayer<GravitationPlayer>().noFlipGravity = true;
                        break;
;
              
                    case ItemID.EmpressFlightBooster:
                        player.jumpSpeedBoost -= 1.8f;
                        break;
                    case ItemID.Magiluminescence:
                        player.hasMagiluminescence = false;
                        player.GetModPlayer<Mobility>().TRAEMagi = true;
                        break;
                    case ItemID.ShinyStone:
                        if (player.velocity.Y == 0)
                        {
                            player.lifeRegen += 4;
                        }
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
                    case ItemID.Magiluminescence:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "All horizontal movement has 15% more top speed";
                            }
                        }
                        break;
                    case ItemID.EmpressFlightBooster:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Restores double jumps every 6 seconds while airborne";
                            }
                        }
                        break;
                    case ItemID.Aglet:
                        foreach (TooltipLine line in tooltips)
                        {

                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "10% increased movement speed";
                            }
                        }
                        break;
                    case ItemID.AnkletoftheWind:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Greatly increases acceleration";
                            }
                        }
                        break;
                    case ItemID.ObsidianHorseshoe:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Hold DOWN to increase falling speed";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Prevents fall damage and allows control over gravity";
                            }

                        }
                        break;
           
                    case ItemID.HiveBackpack:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "Stores up to 16 bees while grounded, releases them while in mid-air\nIncreases jump height by 4.5% for every bee stored\nDoubles strength, recharge delay, and release rate of the bees when honeyed";
                            }
                        }
                        break;
                }
            }
        }
    }
}