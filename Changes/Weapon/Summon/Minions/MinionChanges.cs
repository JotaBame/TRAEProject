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
                item.mana = 5;
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
                case ItemID.StardustCellStaff:
                    item.damage = 50; // down from 60
                    break;
                case ItemID.StardustDragonStaff:
                    item.damage = 60; // up from 40
                    item.SetNameOverride("Lunar Dragon Staff");
                    break;
                case ItemID.SanguineStaff:
                    item.damage = 40; // up from 35
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
                    projectile.localNPCHitCooldown = 50; // up from 10, static 
                    break;
                case ProjectileID.RainbowCrystalExplosion:
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    break;
                case ProjectileID.DD2FlameBurstTowerT3Shot:
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 240;
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

            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
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
                case ProjectileID.VampireFrog:
                    projectile.localNPCImmunity[target.whoAmI] = (int)projectile.ai[1];
                    break;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type])
            {
                modifiers.FlatBonusDamage += target.GetGlobalNPC<Tag>().Damage;
                if (Main.rand.Next(100) < target.GetGlobalNPC<Tag>().Crit)
                {
                    modifiers.SetCrit();
                }
                if (Main.rand.Next(100) < Main.player[projectile.owner].GetModPlayer<SummonStats>().minionCritChance)
                {
                    modifiers.SetCrit();
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
                case ProjectileID.VampireFrog:
                    //Main.NewText(projectile.ai[0] + ", " + projectile.ai[1] + ", " + projectile.localAI[0] + ", " + projectile.localAI[1]);
                    if(projectile.ai[1] == 20)
                    {
                        projectile.localAI[0] = 45;
                    }
                    if(projectile.localAI[0] > 0)
                    {
                        projectile.localAI[0]--;
                        if(projectile.ai[1] == 0)
                        {
                            projectile.ai[0] = 4;
                            projectile.velocity.X = 0;
                            projectile.velocity.Y += 0.4f;
                            if (projectile.velocity.Y > 10f)
                            {
                                projectile.velocity.Y = 10f;
                            }
                        }
                    }
                    else if(projectile.ai[0] == 4)
                    {
                        projectile.ai[0] = 0;
                    }
                    break;
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
        public override void PostAI(Projectile projectile)
        {
            base.PostAI(projectile);
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if(projectile.type == ProjectileID.VampireFrog)
            {
                if(projectile.ai[0] != 2 || projectile.ai[1] <= 0)
                {
                    return false;
                }
            }
            return base.CanHitNPC(projectile, target);
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