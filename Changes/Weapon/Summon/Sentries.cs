using System;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Summon
{
    public class Sentries : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.QueenSpiderStaff:
                    item.damage = 19; // down from 26
                    break;
                case ItemID.DD2LightningAuraT1Popper:
                    item.damage = 7; // up from 4
                    break;
                case ItemID.DD2LightningAuraT2Popper:
                    item.damage = 15; // up from 11
                    break;
                case ItemID.DD2LightningAuraT3Popper:
                    item.damage = 44; // up from 34
                    break;
                case ItemID.DD2FlameburstTowerT1Popper:
                    item.damage = 25; // up from 17
                    break;
                case ItemID.DD2FlameburstTowerT2Popper:
                    item.damage = 58; // up from 42
                    break;
                case ItemID.DD2FlameburstTowerT3Popper:
                    item.damage = 123; // up from 88
                    break;
                case ItemID.StaffoftheFrostHydra:
                    item.damage = 75; // down from 100
                    break;
                case ItemID.RainbowCrystalStaff:
                    item.damage = 30; // down from 150
                    break;
                case ItemID.MoonlordTurretStaff:
                    item.damage = 33; // down from 100

                    item.SetNameOverride("Stardust Portal Staff");
                    break;
                
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.MoonlordTurretStaff:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Summons a stardust portal to shoot lasers at your enemies";
                        }
                    }
                    break;
            }
        }
    }
    public class SentryChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            switch(projectile.type)
            {
                case ProjectileID.DD2LightningAuraT1:
                case ProjectileID.DD2LightningAuraT2:
                case ProjectileID.DD2LightningAuraT3:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 25;
                    break;
                case ProjectileID.FrostBlastFriendly:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.usesIDStaticNPCImmunity = false;
                    projectile.localNPCHitCooldown = -1;
                    projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
                    break;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (projectile.type)
            {
                case ProjectileID.SpiderEgg:
                case ProjectileID.BabySpider:
                {
                    int findbuffIndex = target.FindBuffIndex(BuffID.Venom);
                    if (findbuffIndex != -1)
                    {
                        target.DelBuff(findbuffIndex);
                    };
                    return;
                }
           
            }
        }
        public override bool? CanDamage(Projectile projectile)
        {
            if (projectile.aiStyle == 137 && projectile.ai[0] != 0)
            {
                return false;
            }
            return null;
        }
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.aiStyle == 137)
            {
                int num = 10;
                int num2 = 999;
                int num3 = 30;
                int num4 = 40;
                int num5 = 4;
                projectile.knockBack = 0f;
                if (Main.player[projectile.owner].setMonkT2)
                {
                    num3 -= 5; 
                    num = 12;
                    num5 = 6;
                }
                if (Main.player[projectile.owner].setMonkT3)
                {
                    num = 14;
                    num5 = 8;
                }
 
                projectile.ai[0] += 1f;
                if (projectile.ai[0] >= (float)num3)
                {
                    projectile.ai[0] = 0f;
                }
                if (projectile.ai[0] == 0f)
                {
                    bool flag = false;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC nPC = Main.npc[i];
                        if (nPC.CanBeChasedBy(this) && nPC.Hitbox.Distance(projectile.Center) < (float)(projectile.width / 2) && projectile.Colliding(projectile.Hitbox, nPC.Hitbox))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, projectile.Center);
                    }
                }
                if (projectile.localAI[0] == 0f)
                {
                    projectile.localAI[0] = 1f;
                    projectile.velocity = Vector2.Zero;
                    Point origin = projectile.Center.ToTileCoordinates();
                    bool flag2 = true;
                    if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(500), new Conditions.NotNull(), new Conditions.IsSolid()), out var result))
                    {
                        flag2 = false;
                       projectile.position.Y += 16f;
                        return false;
                    }
                    if (!WorldUtils.Find(new Point(result.X, result.Y - 1), Searches.Chain(new Searches.Up(num), new Conditions.NotNull(), new Conditions.IsSolid()), out var result2))
                    {
                        result2 = new Point(origin.X, origin.Y - num - 1);
                    }
                    int num6 = 0;
                    if (flag2 && Main.tile[result.X, result.Y] != null && Main.tile[result.X, result.Y].BlockType == BlockType.HalfBlock)
                    {
                        num6 += 8;
                    }
                    Vector2 center = result.ToWorldCoordinates(8f, num6);
                    Vector2 vector = result2.ToWorldCoordinates(8f, 0f);
                    projectile.Size = new Vector2(1f, center.Y - vector.Y);
                    if (projectile.height > num * 16)
                    {
                        projectile.height = num * 16;
                    }
                    if (projectile.height < num5 * 16)
                    {
                        projectile.height = num5 * 16;
                    }
                    projectile.height *= 2;
                    projectile.width = (int)((float)projectile.height * 1f);
                    if (projectile.width > num2)
                    {
                        projectile.width = num2;
                    }
                    projectile.Center = center;
                }
                if (++projectile.frameCounter >= 8)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
                DelegateMethods.v3_1 = new Vector3(0.2f, 0.7f, 1f);
                Utils.PlotTileLine(projectile.Center + Vector2.UnitX * -40f, projectile.Center + Vector2.UnitX * 40f, 80f, DelegateMethods.CastLightOpen);
                Vector2 vector2 = new Vector2(projectile.Top.X,projectile.position.Y + (float)num4);
                for (int j = 0; j < 4; j++)
                {
                    if (Main.rand.Next(6) != 0)
                    {
                        continue;
                    }
                    Vector2 vector3 = Main.rand.NextVector2Unit();
                    if (!(Math.Abs(vector3.X) < 0.12f))
                    {
                        Vector2 targetPosition = projectile.Center + vector3 * new Vector2((projectile.height - num4) / 2);
                        if (!WorldGen.SolidTile((int)targetPosition.X / 16, (int)targetPosition.Y / 16) && projectile.AI_137_CanHit(targetPosition))
                        {
                            Dust dust = Dust.NewDustDirect(targetPosition, 0, 0, 226, 0f, 0f, 100);
                            dust.position = targetPosition;
                            dust.velocity = (vector2 - dust.position).SafeNormalize(Vector2.Zero);
                            dust.scale = 0.7f;
                            dust.fadeIn = 1f;
                            dust.noGravity = true;
                            dust.noLight = true;
                        }
                    }
                }
                for (int k = 0; k < 0; k++)
                {
                    if (Main.rand.Next(10) != 0)
                    {
                        continue;
                    }
                    Vector2 vector4 = Main.rand.NextVector2Unit();
                    if (!(Math.Abs(vector4.X) < 0.12f))
                    {
                        Vector2 targetPosition2 = projectile.Center + vector4 * new Vector2((projectile.height - num4) / 2) * Main.rand.NextFloat();
                        if (!WorldGen.SolidTile((int)targetPosition2.X / 16, (int)targetPosition2.Y / 16) && projectile.AI_137_CanHit(targetPosition2))
                        {
                            Dust dust2 = Dust.NewDustDirect(targetPosition2, 0, 0, 226, 0f, 0f, 100);
                            dust2.velocity *= 0.6f;
                            dust2.velocity += Vector2.UnitY * -2f;
                            dust2.noGravity = true;
                            dust2.noLight = true;
                        }
                    }
                }
                for (int l = 0; l < 4; l++)
                {
                    if (Main.rand.Next(10) == 0)
                    {
                        Dust dust3 = Dust.NewDustDirect(vector2 - new Vector2(8f, 0f), 16, projectile.height / 2 - 40, 226, 0f, 0f, 100);
                        dust3.velocity *= 0.6f;
                        dust3.velocity += Vector2.UnitY * -2f;
                        dust3.scale = 0.7f;
                        dust3.noGravity = true;
                        dust3.noLight = true;
                    }
                }
                projectile.tileCollide = true;
                projectile.velocity.Y += 0.2f;
                return false;
            }
            if (projectile.aiStyle == 134 && Main.player[projectile.owner].ballistaPanic && projectile.ai[1] > 0f && projectile.ai[0] == 1f)
            {
                projectile.ai[1] += 0.33f;
                
            }
            return true;
        }

    }

}
