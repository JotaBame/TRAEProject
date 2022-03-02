using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace TRAEProject.Changes.Weapons.Rockets
{
    public class RocketStatChanges : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
	    public virtual Vector2? HoldoutOffset(int type) {
			if (type == ItemID.RocketLauncher)
			{
				return new Vector2(-16f, -5f);
			}
			return null;
		}
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.RocketI:
                    item.damage = 25; // down from 50
                    item.SetNameOverride("Rocket");
                    break;
                case ItemID.RocketII:
                    item.damage = 20; // down from 50
                    item.SetNameOverride("Destructive Rocket");
                    break;
                case ItemID.RocketIV:
                    item.damage = 33; // down from 65
                    item.SetNameOverride("Direct Rocket");
                    break;
                case ItemID.RocketIII:
                    item.damage = 50; // down from 65
                    item.SetNameOverride("Super Rocket");
                    break;
                case ItemID.ClusterRocketI:
                    item.damage = 40; // down from 65
                    item.SetNameOverride("Cluster Rocket");
                    break;
                case ItemID.ClusterRocketII:
                    item.damage = 50; // down from 65
                    item.SetNameOverride("Heavy Rocket");
                    break;
                case ItemID.MiniNukeI:
                    item.SetNameOverride("Mini Nuke");
                    break;
                case ItemID.MiniNukeII:
                    item.damage = 20; // down from 75
                    item.SetNameOverride("Destructive Mini Nuke");
                    break;
                case ItemID.GrenadeLauncher:
                    item.damage = 25; // down from 60
                    item.useTime = 33; // up from 20
                    item.useAnimation = 33;
                    break;
                case ItemID.RocketLauncher:
                    item.damage = 120; // up from 45
					break;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.RocketLauncher:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "";
                        }
                    }
                    return;
                case ItemID.RocketI:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Normal explosion.";
                        }
                    }
                    return;
                case ItemID.RocketII:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Normal explosion. Will destroy tiles.";
                        }
                    }
                    return;
                case ItemID.RocketIII:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Big explosion.";
                        }
                    }
                    return;
                case ItemID.RocketIV:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Small explosion. Direct hits deal 50% more damage.";
                        }
                    }
                    return;
                case ItemID.MiniNukeI:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Giant Explosion.";
                        }
                    }
                    return;
                case ItemID.MiniNukeII:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Giant explosion. Will destroy tiles.";
                        }
                    }
                    return;
                case ItemID.ClusterRocketII:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Normal explosion. Stuns enemies.";
                        }
                    }
                    return;
            }
        }
    }
}