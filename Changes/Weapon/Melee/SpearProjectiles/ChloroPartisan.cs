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
    public class ChloroPartisan : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 164f;
            stabStart = 133f;
            stabEnd = -10;
            swingAmount = (float)Math.PI / 128;
        }
        public override void OnMaxReach(float direction)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, TRAEMethods.PolarVector(5 * player.GetModPlayer<MeleeStats>().meleeVelocity * (1 / player.GetAttackSpeed(DamageClass.Melee)), direction), ProjectileID.SporeCloud, Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
    public class ChloroPartisanThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 164f;
            holdAt = 85f;
            maxSticks = 1;
            stickingDps = 0;
            floatTime = 10;
        }
        public override void ThrownUpdate()
        {
            Projectile.extraUpdates = 1;
        }
        public override void SpearHitNPC(bool atMaxCharge, NPC target, int damage, float knockback, bool crit)
        {
            if(atMaxCharge)
            {
                SporeBurst();
            }
        }
        int timer = 1;
        public override void StuckEffects(NPC victim)
        {
            timer++;
            if(timer % 180 == 0)
            {
                SporeBurst();
            }
        }
        void SporeBurst()
        {
            for (int i =0; i <6; i++)
            {
                float direction = (float)Math.PI * 2f * ((float)i / 6f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, TRAEMethods.PolarVector(4f, direction), ProjectileID.SporeCloud, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
    /*
    public class Spore : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.type == ProjectileID.SporeCloud)
            {
                projectile.usesIDStaticNPCImmunity = true;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.type == ProjectileID.SporeCloud)
            {
                Projectile.perIDStaticNPCImmunity[projectile.type][target.whoAmI] = (uint)(Main.GameUpdateCount + 10);
            }
        }
    }
    */
}
