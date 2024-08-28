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
    class SoTC : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 113f;//76f;
            stabStart = spearLength - 43.5f;
            stabEnd = -10;
            swingAmount = MathF.PI / 24;
        }
        bool spawnedEater = false;
        public override void SpearHitNPCMelee(NPC target, NPC.HitInfo hit)
        {
            if (!spawnedEater)
            {
                spawnedEater = true;
                int num701 = 1;
                for (int num702 = 0; num702 < num701; num702++)
                {
                    float num703 = (float)Main.rand.Next(-35, 36) * 0.02f;
                    float num704 = (float)Main.rand.Next(-35, 36) * 0.02f;
                    num703 *= 10f;
                    num704 *= 10f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, num703, num704, 307, (int)((double)Projectile.damage * 0.75), (int)((double)Projectile.knockBack * 0.35), Projectile.owner);
                }
            }
        }
    
    }
    class SoTCThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 113f;
            holdAt = 60f;//25.4f;
            floatTime = -1;
            DustOnDeath = DustID.ScourgeOfTheCorruptor;

        }
        public override void ThrownUpdate()
        {
            Projectile.extraUpdates = 2;
        }
        /*
        public override void OnThrow(float chargeAmt)
        {
            float dmgMult = chargeAmt;
            if(dmgMult == 1f)
            {
                dmgMult = 2f;
            }
            Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, 306, (int)(Projectile.damage * dmgMult), Projectile.knockBack, Projectile.owner)];
            p.scale = Projectile.scale;
            p.width = (int)(p.width * Projectile.scale);
            p.height = (int)(p.height * Projectile.scale);
            Projectile.Kill();
        }
        */
        public override void SpearHitNPC(bool atMaxCharge, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int amount = 2 + Main.rand.Next(2);
            if (Main.rand.NextBool(100))
                amount = 10;
            for (int num702 = 0; num702 < amount; num702++)
            {
                float num703 = (float)Main.rand.Next(-35, 36) * 0.02f;
                float num704 = (float)Main.rand.Next(-35, 36) * 0.02f;
                num703 *= 10f;
                num704 *= 10f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, num703, num704, 307, (int)((double)Projectile.damage * 0.75f * (atMaxCharge ? 2 : 1)), (int)((double)Projectile.knockBack * 0.35), Projectile.owner);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int num701 = 2;
            if (Main.rand.NextBool(3))
            {
                num701++;
            }
            if (Main.rand.NextBool(3))
            {
                num701++;
            }
            for (int num702 = 0; num702 < num701; num702++)
            {
                float num703 = (float)Main.rand.Next(-35, 36) * 0.02f;
                float num704 = (float)Main.rand.Next(-35, 36) * 0.02f;
                num703 *= 10f;
                num704 *= 10f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, num703, num704, 307, (int)((double)Projectile.damage * 0.75f * (chargeAmt == 1 ? 2 : 1)), (int)((double)Projectile.knockBack * 0.35), Projectile.owner);
            }
            return true;
        }
    }
}
