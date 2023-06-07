using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;

namespace TRAEProject.Changes.Weapon.Summon
{
    public class WhipChanges : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.BlandWhip:
                    item.knockBack = 1.5f; // up from 0.5
                    break;
                case ItemID.MaceWhip:
                    item.damage = 160; // down from 165
                    item.useTime = 35;
                    item.useAnimation = 35;
                    break;
                case ItemID.RainbowWhip:
                    item.damage = 250; // up from 180
                    item.autoReuse = true;
                    break;
                case ItemID.SwordWhip:
                    item.damage = 75; // up from 55
                    break;

            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.RainbowWhip:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "50 summon tag damage";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "30% summon tag critical strike chance\nColorful destruction comes out of enemies hit by summons";
                        }
                    }
                    break;
            }
        }
    }
    public class WhipChangesP : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (ProjectileID.Sets.IsAWhip[projectile.type])
            {
                projectile.GetGlobalProjectile<ProjectileStats>().maxHits = 5;
            }
        }

    }
}
