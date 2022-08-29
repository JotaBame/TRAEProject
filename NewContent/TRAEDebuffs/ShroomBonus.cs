using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Ammo;
namespace TRAEProject.NewContent.TRAEDebuffs
{
    public class ShroomBonus: TRAEDebuff
    {
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            damage = (int)(damage * 1.05);
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * 1.05);
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Main.rand.Next(10) == 0)
            {
                float num852 = ((float)Math.PI * 2f);
                float randomAngle = 5.917f * Main.rand.Next(1, 30);
                float f2 = num852 + (randomAngle / (678f * (float)Math.PI)) * ((float)Math.PI * 2f);
                Vector2 velocity = f2.ToRotationVector2();
                Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, DustID.GlowingMushroom, velocity.X, velocity.Y, Scale: 0.9f); 
            }
        }
    }
}
