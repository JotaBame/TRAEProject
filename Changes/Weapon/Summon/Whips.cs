using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
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
                case ItemID.ThornWhip:
                    item.damage = 19; // up from 18
                    break;
                case ItemID.BoneWhip:
                    item.damage = 29; // down from 29
                    break;
                case ItemID.SwordWhip:
                    item.damage = 70; //up from 55
                    break;
                case ItemID.ScytheWhip:
                    item.damage = 111; // up from 100
                    break;
                case ItemID.RainbowWhip:
                    item.damage = 250; // up from 180
                    item.autoReuse = true;
                    break;

            }
        }
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            switch (item.type)
            {
                case ProjectileID.CoolWhip:
                    TRAEDebuff.Apply<CoolWhipTag>(target, 240, 1);
                    break;

            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.CoolWhip:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n8 summon tag damage";
                        }
                    }
                    break;
                case ItemID.MaceWhip:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "15% summon tag critical strike chance";
                        }
                    }
                    break;
                case ItemID.ScytheWhip:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n10 summon tag damage";
                        }
                    }
                    break;
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
        
    }
}
