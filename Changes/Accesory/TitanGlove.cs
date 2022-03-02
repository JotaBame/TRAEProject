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
    class TitanGlove : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if(item.type == ItemID.TitanGlove || item.type == ItemID.PowerGlove || item.type == ItemID.FireGauntlet)
            {
                player.kbGlove = false;
                player.meleeScaleGlove = false;
                player.GetModPlayer<MeleeStats>().weaponSize += 0.25f;
                player.GetModPlayer<MeleeStats>().meleeVelocity += 0.5f;
            }
            if(item.type == ItemID.FireGauntlet)
            {
                player.GetDamage<MeleeDamageClass>() -= 0.12f;
                player.meleeSpeed -= 0.12f;           
				player.autoReuseGlove = false;

                player.GetModPlayer<MeleeStats>().inflictHeavyBurn += 60;
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
                if (item.type == ItemID.TitanGlove || item.type == ItemID.PowerGlove || item.type == ItemID.FireGauntlet)
                {

                    if (line.mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.text = "25% increased melee weapon size and 50% increased melee velocity";
                    }
<<<<<<< Updated upstream

=======
                    if (line.mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.text = "";
                    }
                    if (line.mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.text = "";
                    }
                    if (line.mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        line.text = "";
                    }
>>>>>>> Stashed changes
                }
                if (item.type == ItemID.FireGauntlet)
                {
                    if (line.mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.text = "Melee weapons inflict heavy fire damage";
                    }
                }
            }
        }
    }
}
