using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    class RottedFork : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 137f;
            stabStart = 85f;
            stabEnd = -10;
            swingAmount = MathF.PI / 12;
        }
        public override void SpearActive()
        {
            int num21 = Dust.NewDust(Projectile.position - Projectile.velocity * 3f, Projectile.width, Projectile.height, 115, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 140);
            Main.dust[num21].noGravity = true;
            Main.dust[num21].fadeIn = 1.25f;
            Main.dust[num21].velocity *= 0.25f;
        }
    }
    public class RottedForkThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 119f;
            holdAt = 59f;
            maxSticks = 2;
            stickingDps = 6;
            floatTime = 65;
            DustOnDeath = 115;
        }
        public override void SpearActive()
        {
            int num21 = Dust.NewDust(Projectile.position - Projectile.velocity * 3f, Projectile.width, Projectile.height, 115, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 140);
            Main.dust[num21].noGravity = true;
            Main.dust[num21].fadeIn = 1.25f;
            Main.dust[num21].velocity *= 0.25f;
        }
    }
}
