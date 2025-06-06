﻿using Microsoft.Xna.Framework;
using TRAEProject.NewContent.Items.Accesories.MechanicalEye;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common.ModPlayers;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using TRAEProject.Common;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace TRAEProject.Changes.Accesory
{
    public class AccessoryStats : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.MagicQuiver)
            {
                player.magicQuiver = false;
                player.GetModPlayer<RangedStats>().Magicquiver += 1;

                if (player.HeldItem.useAmmo == AmmoID.Arrow || player.HeldItem.useAmmo == AmmoID.Stake)
                    player.GetDamage<RangedDamageClass>() -= 0.1f; // 1.4.4 magic quiver uses a new bool and guess what it is not supported by tmod (at least at the time i'm writing this note)
            }
             if (item.type == ItemID.StalkersQuiver)
            {
                player.magicQuiver = false;
                player.GetModPlayer<RangedStats>().Magicquiver += 1;

                if (player.HeldItem.useAmmo == AmmoID.Arrow || player.HeldItem.useAmmo == AmmoID.Stake)
                    player.GetDamage<RangedDamageClass>() -= 0.1f; // 1.4.4 magic quiver uses a new bool and guess what it is not supported by tmod (at least at the time i'm writing this note)                player.GetModPlayer<RangedStats>().Magicquiver += 1;
                player.GetDamage<RangedDamageClass>() += 0.04f;
                player.GetCritChance<RangedDamageClass>() += 4;
            }
            if (item.type == ItemID.RifleScope)
            {
                player.GetModPlayer<RangedStats>().GunScope = true;
                player.GetModPlayer<RangedStats>().spreadModifier /= 3;
             }
            if (item.type == ItemID.SniperScope)
            {
                player.GetModPlayer<RangedStats>().GunScope = true;
                player.GetDamage<RangedDamageClass>() -= 0.1f; 
                player.GetModPlayer<RangedStats>().spreadModifier /= 3;

            }

            if (item.type == ItemID.ReconScope)
            {
                player.GetModPlayer<RangedStats>().ReconScope += 1;
                player.GetDamage<RangedDamageClass>() -= 0.1f;
                player.GetModPlayer<RangedStats>().spreadModifier /= 3;

                player.GetCritChance<RangedDamageClass>() -= 10;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {                case ItemID.MagicQuiver:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Your arrows will bounce towards nearby enemies, losing 33% damage in the process";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.StalkersQuiver:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Your arrows will bounce towards nearby enemies, losing 33% damage in the process";

                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "4% increased ranged damage and critical strike chance";
                        }
                    }
                    break;
                case ItemID.RifleScope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases view range for guns (right click to zoom out!)";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Increases ranged velocity and tightens gun spread";
                        }
                    }
                    break;
                case ItemID.SniperScope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Increases ranged velocity and tightens gun spread\n10% increased ranged critical strike chance";
                        }
                    }
                    break;
                case ItemID.ReconScope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases view range for guns (right click to zoom out!)";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Increases ranged velocity and tightens gun spread";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Your ranged attacks will bounce towards their nearby enemy";
                        }
                    }
                    break;
            }
        }
    }
    public class ScopeAndQuiver : GlobalProjectile
    {

        public override bool InstancePerEntity => true;

        public int smartbounces = 0;
        public bool hasBounced = false;
        public bool stunOnCrit = false;
        public bool otherAmmo = false;
        public bool AffectedByAlphaScope = false;
        public bool AffectedByReconScope = false;
        public float timer = 0;
        public override void SetDefaults(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.PulseBolt:
                case ProjectileID.Stake:
                case ProjectileID.Hellwing:
                    AffectedByReconScope = true;
                    AffectedByAlphaScope = true;
                    projectile.arrow = true;
                    return;
                case ProjectileID.Bullet:
                case ProjectileID.MeteorShot:
                case ProjectileID.CrystalBullet:
                case ProjectileID.CrystalShard:
                case ProjectileID.CursedBullet:
                case ProjectileID.IchorBullet:
                case ProjectileID.BulletHighVelocity:
                case ProjectileID.ExplosiveBullet:
                case ProjectileID.GoldenBullet:
                case ProjectileID.PartyBullet:
                case ProjectileID.VenomBullet:
                case ProjectileID.ChlorophyteBullet:
                case ProjectileID.NanoBullet:

                case ProjectileID.MoonlordBullet:
                    AffectedByReconScope = true; 
                    AffectedByAlphaScope = false; // wait why is this set to false, im too afraid of breaking things to find out
                    break;
            }
            if (projectile.arrow)
            {
                AffectedByReconScope = true; 
                AffectedByAlphaScope = true;
            }
            if (AffectedByReconScope)
            {
                AffectedByAlphaScope = true;
            }
 
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];
            if (!projectile.GetGlobalProjectile<NewRockets>().IsARocket)
            {
                if (projectile.arrow && smartbounces < player.GetModPlayer<RangedStats>().Magicquiver + player.GetModPlayer<RangedStats>().ReconScope && !hasBounced)
                {
                    smartbounces += player.GetModPlayer<RangedStats>().Magicquiver;
                    smartbounces += player.GetModPlayer<RangedStats>().ReconScope;
                }
                if (projectile.CountsAsClass(DamageClass.Ranged) && AffectedByReconScope && smartbounces < player.GetModPlayer<RangedStats>().ReconScope && !hasBounced)
                {
                    smartbounces += player.GetModPlayer<RangedStats>().ReconScope;
                }
                if (projectile.CountsAsClass(DamageClass.Ranged) && AffectedByAlphaScope && smartbounces < player.GetModPlayer<RangedStats>().AlphaScope && !hasBounced)
                {
                    smartbounces += player.GetModPlayer<RangedStats>().AlphaScope;
                }
                if (projectile.CountsAsClass(DamageClass.Ranged) && player.GetModPlayer<RangedStats>().AlphaScope > 0)
                {
                    projectile.GetGlobalProjectile<ProjectileStats>().FirstHitDamage += 0.05f * player.GetModPlayer<RangedStats>().AlphaScope;
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (smartbounces > 0 && projectile.penetrate > 1)
            {
                QuiverBounce(projectile, target.whoAmI);
            }
        }        
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
            if (smartbounces > 0)
            {
                int maxRange = 600;
                NPC closest = null;


                // specialCondition: delegate (NPC possibleTarget) { return projectile.Center.Distance(projectile.Center) > minimumRange; }
                if (TRAEMethods.ClosestNPC(ref closest, maxRange, projectile.Center) && closest.Distance(projectile.Center) > 50)
                {
                    Vector2 value2 = closest.Center - projectile.Center;
                    float scaleFactor2 = projectile.velocity.Length();
                    value2.Normalize();
                    projectile.velocity = value2 * scaleFactor2;
                    projectile.netUpdate = true;
                    projectile.damage = (int)(projectile.damage * 0.67);
                    --smartbounces;
                    hasBounced = true;
                    for (int i = 0; i < 10; ++i)
                    {
                        Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.UndergroundHallowedEnemies, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default, 1.5f);
                        dust.noGravity = true;
                    }
                    return false;
                }                   
            }
            if (projectile.type == ProjectileID.ChlorophyteArrow || projectile.type == ProjectileID.NanoBullet)
            {
                projectile.Kill();
            }
			return true;
        }
        void QuiverBounce(Projectile projectile, int idTargethit = -1 )
        {
             int maxRange = 600;
             NPC closest = null;

            TRAEMethods.ClosestNPC(ref closest, maxRange, projectile.Center, IgnoreThisOne: idTargethit);
            // specialCondition: delegate (NPC possibleTarget) { return projectile.Center.Distance(projectile.Center) > minimumRange; }
             if (closest != null && closest.Distance(projectile.Center) > 50)
            {
                Vector2 value2 = closest.Center - projectile.Center;
                float scaleFactor2 = projectile.velocity.Length();
                value2.Normalize();
                projectile.velocity = value2 * scaleFactor2;
                projectile.netUpdate = true;
                projectile.damage = (int)(projectile.damage * 0.67);
                --smartbounces;
                hasBounced = true;
                for (int i = 0; i < 10; ++i)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.UndergroundHallowedEnemies, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default, 1.5f);
                    dust.noGravity = true;
                }
            }
          
        }
    }
}
