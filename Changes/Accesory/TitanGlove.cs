using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.Changes.Accesory
{
    public class TitanGlove : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.FeralClaws:
                case ItemID.PowerGlove:
                case ItemID.MechanicalGlove:
                case ItemID.BerserkerGlove:
                case ItemID.TitanGlove:
                case ItemID.FireGauntlet:
                    player.GetModPlayer<MeleeStats>().TRAEAutoswing = true; // is this even needed anymore
                    break;
            }
            if (item.type == ItemID.TitanGlove || item.type == ItemID.PowerGlove || item.type == ItemID.FireGauntlet)
            {
                player.kbGlove = false;
                player.meleeScaleGlove = false;
                player.GetModPlayer<MeleeStats>().weaponSize += 0.20f;
                player.GetModPlayer<MeleeStats>().meleeVelocity += 0.2f;
            }
            if(item.type == ItemID.FireGauntlet)
            {
                player.GetDamage<MeleeDamageClass>() -= 0.12f;
                player.GetAttackSpeed(DamageClass.Melee) -= 0.12f;           
            }
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if(item.CountsAsClass(DamageClass.Melee))
            {
                velocity *= player.GetModPlayer<MeleeStats>().meleeVelocity;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (item.type == ItemID.TitanGlove)
                {

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "20% increased melee weapon size and velocity";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = "Allows autoswing for all melee weapons";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = "";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        line.Text = "";
                    }
                }
				if (item.type == ItemID.PowerGlove )
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "12% increased melee speed";
                    }  
					if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = "20% increased melee weapon size and velocity";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = "Enables autoswing for melee weapons";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        line.Text = "";
                    }
                }
				
                if (item.type == ItemID.FireGauntlet)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "20% increased melee weapon size and velocity";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = "Allows autoswing for all melee weapons";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = "Melee attacks deal fire damage";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        line.Text = "Melee critical strikes have have a very low chance to incinerate, higher on stronger hits";
                    }

                }
            }
        }
    }
}
