using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Buffs;
using System.Collections.Generic;
using TRAEProject.Common;

using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.Changes.Weapon.Summon.Minions
{
    public class SummonStaffs : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            if(item.CountsAsClass(DamageClass.Summon) && item.mana > 0) //give all minions and sentries quick resummon
            {
                item.mana = 20;
                item.useTime = 12;
                item.useAnimation = 12;
                item.autoReuse = true;
            }
            switch (item.type)
            {
                case ItemID.FlinxStaff:
                    item.damage = 5; // down from 8
                    break;
                case ItemID.HornetStaff:
                    item.damage = 12;
                    break;
                case ItemID.OpticStaff:
                    item.damage = 25; // down from 30
                    break;
                case ItemID.QueenSpiderStaff:
                    item.damage = 19; // down from 26
                    break;
                case ItemID.PygmyStaff:
                    item.damage = 45; // up from 34
                    break;
                case ItemID.StormTigerStaff:
                    item.damage = 34; // down from 41
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.TempestStaff:
                    item.damage = 42; //down from 50
                    break;
                case ItemID.DeadlySphereStaff:
                    item.damage = 15; // down from 50
                    break;
                case ItemID.StardustDragonStaff:
                    item.damage = 60; // up from 40
                    item.SetNameOverride("Lunar Dragon Staff");
                    break;
                
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.StardustDragonStaff:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Summons a lunar dragon to fight for you";
                        }
                    }
                    break;
            }
        }
    }
    public class MinionChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.FlinxMinion:
                    break;
                case ProjectileID.BabySlime:
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 20;
                    break;
                case ProjectileID.Tempest:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 15;
                    break;
                case ProjectileID.Retanimini:
                case ProjectileID.Spazmamini:
                    projectile.tileCollide = false;
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 30;
                    break;
                case ProjectileID.MiniRetinaLaser:
				    projectile.penetrate = -1;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homingRange = 200f;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = -1;
                    break;
                    break;
                case ProjectileID.DeadlySphere:
                    projectile.extraUpdates = 1;
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 30;
                    break;
                case ProjectileID.DangerousSpider:
                case ProjectileID.VenomSpider:
                case ProjectileID.JumperSpider:
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 45;
                    break;
                case ProjectileID.HornetStinger:
                    projectile.extraUpdates = 2;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homingRange = 150f;
                    break;
                case ProjectileID.ImpFireball:
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 60;
                    break;
                case ProjectileID.VampireFrog:
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 60; // up from 10, static 
                    break;
            }

        }
        public override void AI(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.FlyingImp:
                    if (projectile.ai[1] > 0f)
                    {
                        projectile.ai[1] -= 0.33f;
                    }
                    break;
                case ProjectileID.DeadlySphere:
                    projectile.ai[1] += 2f;
                    break;
                case ProjectileID.BatOfLight:
                    projectile.localNPCHitCooldown = (int)projectile.ai[0];
                    break;
                case ProjectileID.Spazmamini:
                    if(projectile.ai[1] > 1)
                    {
                        projectile.ai[1] -= 0.5f;
                    }
                    break;

            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            switch (projectile.type)
            {
                case ProjectileID.JumperSpider:
                case ProjectileID.VenomSpider:
                case ProjectileID.DangerousSpider:
                    {
                        projectile.localAI[1] = 45f; // up from 20
                        int findbuffIndex = target.FindBuffIndex(BuffID.Venom);
                        if (findbuffIndex != -1)
                        {
                            target.DelBuff(findbuffIndex);
                        };
                        break;
                    }
                case ProjectileID.Tempest:
                    target.immune[projectile.owner] = 0;
                    projectile.localNPCImmunity[target.whoAmI] = 20;
                    break;
                case ProjectileID.MiniRetinaLaser:
                    projectile.Kill();
                    break;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(projectile.type == ProjectileID.Smolstar)
            {
                knockback = 0;
            }
            if (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type])
            {
                damage += target.GetGlobalNPC<Tag>().Damage;
                if (Main.rand.Next(100) < target.GetGlobalNPC<Tag>().Crit)
                {
                    crit = true;
                }
                if (Main.rand.Next(100) < Main.player[projectile.owner].GetModPlayer<SummonStats>().minionCritChance)
                {
                    crit = true;
                }
            }
        }
        public NPC target;
        public Projectile hook;
        public int pygmyCharge = 0;
        public override bool PreAI(Projectile projectile)
        {
            switch(projectile.type)
            {
                case ProjectileID.Pygmy:
                case ProjectileID.Pygmy2:
                case ProjectileID.Pygmy3:
                case ProjectileID.Pygmy4:
                    Pygmy.AI(projectile);
                    return false;
                case ProjectileID.PirateCaptain:
                case ProjectileID.OneEyedPirate:
                case ProjectileID.SoulscourgePirate:
                    return Pirate.AI(projectile);
                case ProjectileID.Tempest:
                    Tempest.AI(projectile);
                    return false;
            }
            if(projectile.type == ProjectileID.PygmySpear)
            {
                Pygmy.CreateChargeDust(projectile, projectile.extraUpdates);
                projectile.ai[0] -= 0.5f;
            }
            if (projectile.type == ProjectileID.MiniSharkron)
            {
                projectile.ai[0] = projectile.timeLeft < (3600 - (16 * 5000) / 10) ? 45 : 0;
            }
            return true;
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (projectile.type == ProjectileID.Pygmy || projectile.type == ProjectileID.Pygmy2 || projectile.type == ProjectileID.Pygmy3 || projectile.type == ProjectileID.Pygmy4)
            {
                if(projectile.localAI[0] > 0)
                {
                    fallThrough = false;
                }
            }
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileID.PirateCaptain || projectile.type == ProjectileID.OneEyedPirate || projectile.type == ProjectileID.SoulscourgePirate)
            {
                Pirate.DrawHook(projectile, ref lightColor);
            }
            return base.PreDraw(projectile, ref lightColor);
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.type == ProjectileID.PirateCaptain || projectile.type == ProjectileID.OneEyedPirate || projectile.type == ProjectileID.SoulscourgePirate)
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
    }

    public class Tag : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int Damage = 0;
        public int Crit = 0;
        public override void ResetEffects(NPC npc)
        {
            Damage = 0;
            Crit = 0;
        }
    }
    
}