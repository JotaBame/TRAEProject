using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    class DarkLance : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 141f;
            stabStart = 99f;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
            
        }
        public override void SpearActive()
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 150, default, 1.4f);
            }
            int num18 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
            Main.dust[num18].noGravity = true;
            Main.dust[num18].velocity /= 2f;
            num18 = Dust.NewDust(Projectile.position - Projectile.velocity * 2f, Projectile.width, Projectile.height, 27, 0f, 0f, 150, default, 1.4f);
            Main.dust[num18].velocity /= 5f;
        }
    }
    public class DarkLanceThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 141f;
            holdAt = 59f;
            floatTime = -1;
            stickingDps = 5;
            maxSticks = 4;
            DustOnDeath = DustID.Demonite;
        }

        public override void SpearActive()
        {
            if (Projectile.friendly)
            {
                if (Main.rand.Next(5) == 0)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 14, 0f, 0f, 150, default, 1.4f);
                }
                int num18 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
                Main.dust[num18].noGravity = true;
                Main.dust[num18].velocity /= 2f;
                num18 = Dust.NewDust(Projectile.position - Projectile.velocity * 2f, Projectile.width, Projectile.height, 27, 0f, 0f, 150, default, 1.4f);
                Main.dust[num18].velocity /= 5f;
            }
        }
    }
}
