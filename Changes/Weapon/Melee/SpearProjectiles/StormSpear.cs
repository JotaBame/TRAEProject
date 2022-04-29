using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    class StormSpear : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 99f;
            stabStart = 65f;
            stabEnd = -10;
            swingAmount = (float)Math.PI / 48;
        }
        public override void SpearHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            StormSpearShot.MakeBolt(Projectile, target);
        }
    }
    class StormSpearThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 99f;
            holdAt = 46f;
            floatTime = 60;
        }
        public override void SpearHitNPC(bool atMaxCharge, NPC target, int damage, float knockback, bool crit)
        {
            if(atMaxCharge)
            {
                StormSpearShot.MakeBolt(Projectile, target);
            }
        }
    }
    class StormSpearShot : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.type == ProjectileID.ThunderSpearShot)
            {
                projectile.usesLocalNPCImmunity = true;
            }
        }
        public static void MakeBolt(Projectile projectile, NPC target)
        {
            NPC npc = null;
            if (TRAEMethods.ClosestNPC(ref npc, 400, projectile.Center, false, -1, delegate(NPC possibleTarget) { return possibleTarget != target; }))
            {
                Projectile p = Main.projectile[Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, TRAEMethods.PolarVector(12, (npc.Center - projectile.Center).ToRotation()), ProjectileID.ThunderSpearShot, projectile.damage, 0, projectile.owner)];
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n] != npc)
                    {
                        p.localNPCImmunity[n] = -1;
                    }
                }
            }
        }
    }
}
