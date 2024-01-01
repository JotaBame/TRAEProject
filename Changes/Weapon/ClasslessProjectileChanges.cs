using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Armor;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon
{
    public class ClasslessProjectileChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            switch (projectile.type)
            {

                case ProjectileID.Bee:
					projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 30;
                    projectile.penetrate = 2;
                    projectile.ArmorPenetration = 6;
                    break;
                case ProjectileID.GiantBee:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 30;
                    projectile.penetrate = 3;
                    projectile.ArmorPenetration = 6;
                    break;
                case ProjectileID.EyeFire:
                    if (Main.expertMode)
                    {
                        projectile.extraUpdates = 1; // down from 3(?)
                    }
                    break;
      
                case ProjectileID.FlowerPetal: // what the fuck is this projectile, why can't i remember
                    projectile.usesLocalNPCImmunity = true;
			projectile.ArmorPenetration = 50;  
projectile.penetrate = 3;			
					 projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
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
                case ProjectileID.InsanityShadowHostile:
                    projectile.alpha = 150;
                    break;
            }
            //
        }
        int timer = 0;
        public override void AI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.InsanityShadowHostile)
            {
                
                if (timer < 30)
                {
                    timer++;
                }
                    

            }
           return;
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.TitaniumStormShard)
            {
                Player player = Main.player[projectile.owner];
                if (player.GetModPlayer<SetBonuses>().TitaniumArmorOn && player.HeldItem.type == ItemID.TitaniumSword)
                {

                    modifiers.FinalDamage *= 1.5f;
                }
            }
        }

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            if (projectile.type == ProjectileID.InsanityShadowHostile)
            {

                if (timer < 30)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
