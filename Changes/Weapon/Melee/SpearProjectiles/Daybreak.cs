using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    public class Daybreak : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 127f;
            stabStart = 85f;
            stabEnd = 0;
            swingAmount = (float)Math.PI / 24;
        }
        public override void SpearHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, 953, Projectile.damage / 2, 10f, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
            }
        }
    }
    public class DaybreakThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 127f;
            holdAt = 60f;
            maxSticks = 8;
            stickingDps = 100;
        }
        public override void Kill(int timeLeft)
        {
            Rectangle hitbox2 = Projectile.Hitbox;
            for (int num118 = 0; num118 < 6; num118 += 3)
            {
                hitbox2.X = (int)Projectile.oldPos[num118].X;
                hitbox2.Y = (int)Projectile.oldPos[num118].Y;
                for (int num119 = 0; num119 < 5; num119++)
                {
                    int num120 = Utils.SelectRandom<int>(Main.rand, 6, 259, 158);
                    int num121 = Dust.NewDust(hitbox2.TopLeft(), Projectile.width, Projectile.height, num120, 2.5f * (float)Projectile.direction, -2.5f);
                    Main.dust[num121].alpha = 200;
                    Dust dust = Main.dust[num121];
                    dust.velocity *= 2.4f;
                    dust = Main.dust[num121];
                    dust.scale += Main.rand.NextFloat();
                }
            }
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, 953, Projectile.damage, 10f, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
            }
        }
        public override void StuckEffects(NPC victim)
        {
            if( victim.type == NPCID.MoonLordHead)
            {
                if (victim.localAI[3] == 13f && !victim.dontTakeDamage)
                {
                    Projectile.Kill();
                }
            }
            if(victim.type == NPCID.MoonLordHand)
            {
                if (victim.frameCounter == 19.0 && !victim.dontTakeDamage)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}
