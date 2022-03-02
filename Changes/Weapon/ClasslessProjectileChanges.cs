using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon
{
    public class ClasslessProjectileChanges : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            switch (projectile.type)
            {

                case ProjectileID.Bee:
                case ProjectileID.GiantBee:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 30;
                    projectile.penetrate = 2;
                    projectile.GetGlobalProjectile<ProjectileStats>().armorPenetration = 6;
                    break;
                case ProjectileID.CrystalLeafShot:
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    break;
                case ProjectileID.EyeFire:
                    if (Main.expertMode)
                    {
                        projectile.extraUpdates = 1; // down from 3(?)
                    }
                    break;
      
                case ProjectileID.FlowerPetal: // what the fuck is this projectile, why can't i remember
                    projectile.usesLocalNPCImmunity = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
                    break;
                case ProjectileID.StarCloakStar:
                    projectile.penetrate = -1;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    projectile.tileCollide = false; 
                    
                    projectile.penetrate = 5;
                    projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 80;
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.25f;
                    break;

            }
            //
        }
    }
}
