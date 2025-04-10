using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Weapons.Summoner.Whip;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;
using TRAEProject.NewContent.TRAEDebuffs;
using TRAEProject.NewContent.NPCs;
using Terraria.Net;
using Terraria.Chat;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.Localization;

namespace TRAEProject.Changes.Weapon.Melee
{
    public class MeleeProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            // Yoyo Defaults (1f = 1 second;  16f = 1 tile)
            // Default: -1f lifetime, 200f Range, 10f Top Speed
            // Wooden Yoyo: 3f lifetime, 130f Range, 9f Top Speed
            // Rally: 5f lifetime, 170f Range, 11f Top Speed
            // Malaise: 7f lifetime, 195f Range, 12.5f Top Speed
            // Artery: 6f lifetime, 207f Range, 12f Top Speed
            // Amazon: 8f lifetime, 215f Range, 13f Top Speed
            // Code1: 9f lifetime, 220f Range, 13f Top Speed
            // Valor: 11f lifetime, 225f Range, 14f Top Speed
            // Cascade: 13f lifetime, 235f Range, 14f Top Speed
            // Format C: 8f lifetime, 235f Range, 15f Top Speed
            // Gradient: 10f lifetime, 250f Range, 12f Top Speed
            // Chik: 16f lifetime, 275f range, 17f Top Speed
            // Amarok: 15f lifetime, 270f range, 14f Top Speed
            // Hel-fire: 12f lifetime, 275f range, 15f Top Speed
            // Code 2: -1f (infinite) lifetime, 280 range, 17f Top Speed
            // Yelets: 14f lifetime, 290f range, 16f Top Speed
            // Kraken: -1f lifetime, 340f range, 16f Top Speed
            // Red's Throw: -1f lifetime, 370f range, 16f Top Speed
            // Valkyrie Yoyo: -1f lifetime, 370f range, 16f Top Speed
            // Eye Of Cthulhu: -1f lifetime, 360f range, 16.5f Top Speed
            // Terrarian: -4f lifetime, 400f range, 17.5f top speed
            // 
            //
            //ProjectileID.Sets.YoyosTopSpeed[ProjectileID.Chik] = 20f;
            //ProjectileID.Sets.YoyosMaximumRange[ProjectileID.Chik] = 300f;


             ProjectileID.Sets.YoyosLifeTimeMultiplier[ProjectileID.HelFire] = -1f;

            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.TheEyeOfCthulhu] = 500f; // 

            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.Kraken] = 300f; //
            ProjectileID.Sets.YoyosTopSpeed[ProjectileID.Kraken] = 28f;

            ProjectileID.Sets.YoyosLifeTimeMultiplier[ProjectileID.Code1] = -1f;

            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.HelFire] = 330f; // up from 275f
            //
            if (projectile.aiStyle == 99)
            {
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 10;
            }
            if (projectile.type == ProjectileID.Kraken)
            {
                projectile.idStaticNPCHitCooldown = 6;
            }
            switch (projectile.type)
            {

                case ProjectileID.BlackCounterweight:
                case ProjectileID.YellowCounterweight:
                case ProjectileID.RedCounterweight:
                case ProjectileID.BlueCounterweight:
                case ProjectileID.PurpleCounterweight:
                case ProjectileID.GreenCounterweight:
                    projectile.extraUpdates = 2; // up from 0
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.FormatC:
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.4f;
                    break;
                        case ProjectileID.VampireKnife:
                    projectile.ArmorPenetration = 20;
                    break;
                case ProjectileID.Shroomerang:
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case 131:
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.ButchersChainsaw:
                    projectile.penetrate = -1;
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.Spark:
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.SporeCloud:
                    projectile.penetrate = 4;
                    projectile.DamageType = DamageClass.Melee;
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.ChlorophyteOrb: // Revisit
                    projectile.penetrate = 5;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 30;
                    projectile.timeLeft = 4 * 60;
                    //projectile.aiStyle = 32;

                    break;
                case ProjectileID.PaladinsHammerFriendly:
                    projectile.tileCollide = false;
                    projectile.GetGlobalProjectile<ProjectileStats>().maxHits = 7;
                    break;
                case ProjectileID.TerraBlade2Shot:
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.7f;
                    projectile.penetrate = 3;
                    break;

                case ProjectileID.CorruptYoyo:
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Poisoned;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 180;
                    break;
                case ProjectileID.HelFire:
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 180;
                    break;
                case ProjectileID.StarWrath:
                    projectile.penetrate = 1;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 300;
                    break;
                case ProjectileID.DaybreakExplosion:
                    projectile.penetrate = 2;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    break;
                case ProjectileID.GolemFist:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    break;
                case ProjectileID.FlyingKnife:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    break;

            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.type == ProjectileID.ChlorophyteOrb)
            {
                /*
                if (projectile.ai[1] > 10f) {
                    NetworkText networkText = NetworkText.FromKey("Game.BallBounceResult", NetworkText.FromKey(Lang.GetProjectileName(projectile.type).Key), projectile.ai[1]);
                    if (Main.netMode == 0)
                        Main.NewText(networkText.ToString(), byte.MaxValue, 240, 20);
                    else if (Main.netMode == 2)
                        ChatHelper.BroadcastChatMessage(networkText, new Color(255, 240, 20));
                }
                */

                projectile.ai[1] = 0f;
                if (projectile.velocity.X != oldVelocity.X)
                    projectile.velocity.X = oldVelocity.X * -0.6f;

                if (projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 2f)
                    projectile.velocity.Y = oldVelocity.Y * -0.6f;
                return false;
            }
            /*
            if(projectile.type == ProjectileID.ChlorophyteOrb)
            {
                if(projectile.velocity.Length() < 2)
                {
                    return true;
                }
                NPC target = null;
                if(TRAEMethods.ClosestNPC(ref target, 300, projectile.Center, false, -1, delegate (NPC possibleTarget) { return projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
                {
                    projectile.velocity = (target.Center - projectile.Center).SafeNormalize(-Vector2.UnitY) * projectile.velocity.Length();
                }
                else
                {
                    if (projectile.velocity.X != oldVelocity.X)
                    {
                        projectile.velocity.X = -oldVelocity.X;
                    }
                    if (projectile.velocity.Y != oldVelocity.Y)
                    {
                        projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                    }
                }
                return false;
            }
            */
            return true;
        }
        int timer = 0; bool justSpawned = true;
        Vector2 spawnCenter = new Vector2(0, 0);
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.TrueNightsEdge)
            {
                float num = 50f;
                float num2 = 15f;
                float num3 = projectile.ai[1] + num;
                float num4 = num3 + num2;
                float num5 = 99f;
 
                if (projectile.localAI[0] == 0f && projectile.type == 973)
                {
                    SoundEngine.PlaySound(SoundID.Item8, projectile.position);
                }
                projectile.localAI[0] += 1f;

                if (projectile.type == 973 && projectile.damage == 0 && projectile.localAI[0] < MathHelper.Lerp(num3, num4, 0.5f))
                {
                    projectile.localAI[0] += 6f;
                }
                 projectile.Opacity = Utils.Remap(projectile.localAI[0], 0f, projectile.ai[1], 0f, 1f) * Utils.Remap(projectile.localAI[0], num3, num4, 1f, 0f);
                if (projectile.localAI[0] >= num4)
                {
                    projectile.localAI[1] = 1f;
                    projectile.Kill();
                    return false;
                }
                Player player = Main.player[projectile.owner];
                float fromValue = projectile.localAI[0] / projectile.ai[1];
                float num6 = Utils.Remap(projectile.localAI[0], projectile.ai[1] * 0.4f, num4, 0f, 1f);
                projectile.direction = (projectile.spriteDirection = (int)projectile.ai[0]);

                int num7 = 3;
                if (projectile.damage != 0 && projectile.localAI[0] >= num5 + (float)num7)
                {
                    projectile.damage = 0;
                }
                if (projectile.damage != 0)
                {
                    int num8 = 80;
                    bool flag = false;
                    float num9 = projectile.velocity.ToRotation();
                    for (float num10 = -1f; num10 <= 1f; num10 += 0.5f)
                    {
                        Vector2 position = projectile.Center + (num9 + num10 * ((float)Math.PI / 4f) * 0.25f).ToRotationVector2() * num8 * 0.5f * projectile.scale;
                        Vector2 position2 = projectile.Center + (num9 + num10 * ((float)Math.PI / 4f) * 0.25f).ToRotationVector2() * num8 * projectile.scale;
                        if (!Collision.SolidTiles(projectile.Center, 0, 0) && Collision.CanHit(position, 0, 0, position2, 0, 0))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        projectile.damage = 0;
                    }
                }
                fromValue = projectile.localAI[0] / projectile.ai[1];
                projectile.localAI[1] += 1f;
                num6 = Utils.Remap(projectile.localAI[1], projectile.ai[1] * 0.4f, num4, 0f, 1f);
                if (justSpawned)
                {
                    justSpawned = false;
                    spawnCenter = new Vector2(projectile.Center.X, projectile.Center.Y);

                }
                projectile.Center = spawnCenter - projectile.velocity + projectile.velocity * num6 * num6 * num5 * player.GetModPlayer<MeleeStats>().meleeVelocity;
                projectile.rotation += projectile.ai[0] * ((float)Math.PI * 2f) * (4f + projectile.Opacity * 4f) / 90f;
                projectile.scale = Utils.Remap(projectile.localAI[0], projectile.ai[1] + 2f, num4, 1.12f, 1f) * projectile.ai[2];
                float f = projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
                Vector2 vector = projectile.Center + f.ToRotationVector2() * 84f * projectile.scale;
                if (Main.rand.Next(5) == 0)
                {
                    Dust dust = Dust.NewDustPerfect(vector, 14, null, 150, default(Color), 1.4f);
                    dust.noLight = (dust.noLightEmittence = true);
                }
                for (int i = 0; (float)i < 3f * projectile.Opacity; i++)
                {
                    Vector2 vector2 = projectile.velocity.SafeNormalize(Vector2.UnitX);
                    int num11 = ((Main.rand.NextFloat() < projectile.Opacity) ? 75 : 27);
                    Dust dust2 = Dust.NewDustPerfect(vector, num11, projectile.velocity * 0.2f + vector2 * 3f, 100, default(Color), 1.4f);
                    dust2.noGravity = true;
                    dust2.customData = projectile.Opacity * 0.2f;
                }


                projectile.ownerHitCheck = projectile.localAI[0] <= 6f;
                if (projectile.localAI[0] >= MathHelper.Lerp(num3, num4, 0.65f))
                {
                    projectile.damage = 0;
                }

                float num14 = projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.9f;
                Vector2 vector3 = projectile.Center + num14.ToRotationVector2() * 85f * projectile.scale;
                (num14 + projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();
                Color value = new Color(64, 220, 96);
                Color value2 = new Color(15, 84, 125);
                Lighting.AddLight(projectile.Center + projectile.rotation.ToRotationVector2() * 85f * projectile.scale, value.ToVector3());
                for (int j = 0; j < 3; j++)
                {
                    if (Main.rand.NextFloat() < projectile.Opacity + 0.1f)
                    {
                        Color.Lerp(Color.Lerp(Color.Lerp(value2, value, Utils.Remap(fromValue, 0f, 0.6f, 0f, 1f)), Color.White, Utils.Remap(fromValue, 0.6f, 0.8f, 0f, 0.5f)), Color.White, Main.rand.NextFloat() * 0.3f);
                        Dust dust3 = Dust.NewDustPerfect(vector3, 107, projectile.velocity * 0.7f, 100, default(Color) * projectile.Opacity, 0.8f * projectile.Opacity);
                        dust3.scale *= 0.7f;
                        dust3.velocity += player.velocity * 0.1f;
                        dust3.position -= dust3.velocity * 6f;
                    }
                }
                if (projectile.damage == 0)
                {
                    projectile.localAI[0] += 3f;
                    projectile.velocity *= 0.76f;
                }
                if (projectile.localAI[0] < 10f && (projectile.localAI[1] == 1f || projectile.damage == 0))
                {
                    projectile.localAI[0] += 1f;
                    projectile.velocity *= 0.85f;
                    for (int k = 0; k < 4; k++)
                    {
                        float num15 = Main.rand.NextFloatDirection();
                        float num16 = 1f - Math.Abs(num15);
                        num14 = projectile.rotation + num15 * ((float)Math.PI / 2f) * 0.9f;
                        vector3 = projectile.Center + num14.ToRotationVector2() * 85f * projectile.scale;
                        Color.Lerp(Color.Lerp(Color.Lerp(value2, value, Utils.Remap(fromValue, 0f, 0.6f, 0f, 1f)), Color.White, Utils.Remap(fromValue, 0.6f, 0.8f, 0f, 0.5f)), Color.White, Main.rand.NextFloat() * 0.3f);
                        Dust dust4 = Dust.NewDustPerfect(vector3, 107, projectile.velocity.RotatedBy(num15 * ((float)Math.PI / 4f)) * 0.2f * Main.rand.NextFloat(), 100, default(Color), 1.4f * num16);
                        dust4.velocity += player.velocity * 0.1f;
                        dust4.position -= dust4.velocity * Main.rand.NextFloat() * 3f;
                    }
                }
                return false;
            }


            if (projectile.type == ProjectileID.ChlorophyteOrb)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter % 10 == 0)
                {
                    projectile.frame++;
                    if (projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
                projectile.penetrate = 5;
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 30;
                projectile.ai[0] += 1f;
                if (projectile.ai[0] >= 20f)
                {
                    projectile.ai[0] = 18f;
                    Rectangle rectangle3 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                    for (int num264 = 0; num264 < 255; num264++) {
                        Entity entity = Main.player[num264];
                        if (entity.active && rectangle3.Intersects(entity.Hitbox)) {
                            projectile.ai[0] = 0f;
                            projectile.velocity.Y = -4.5f;
                            if (projectile.velocity.X > 2f)
                                projectile.velocity.X = 2f;

                            if (projectile.velocity.X < -2f)
                                projectile.velocity.X = -2f;

                            projectile.velocity.X = (projectile.velocity.X + (float)entity.direction * 1.75f) / 2f;
                            projectile.velocity.X += entity.velocity.X * 3f;
                            projectile.velocity.Y += entity.velocity.Y;
                            if (projectile.velocity.X > 6f)
                                projectile.velocity.X = 6f;

                            if (projectile.velocity.X < -6f)
                                projectile.velocity.X = -6f;

                            if (projectile.velocity.Length() > 16f)
                                projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * 16f;

                            projectile.netUpdate = true;
                            projectile.ai[1] += 1f;
                        }
                    }

                    for (int num265 = 0; num265 < 1000; num265++)
                    {
                        if (num265 == projectile.whoAmI)
                            continue;

                        Entity entity = Main.projectile[num265];
                        if (entity.active && rectangle3.Intersects(entity.Hitbox)) {
                            projectile.ai[0] = 0f;
                            projectile.velocity.Y = -4.5f;
                            if (projectile.velocity.X > 2f)
                                projectile.velocity.X = 2f;

                            if (projectile.velocity.X < -2f)
                                projectile.velocity.X = -2f;

                            projectile.velocity.X = (projectile.velocity.X + (float)entity.direction * 1.75f) / 2f;
                            projectile.velocity.X += entity.velocity.X * 3f;
                            projectile.velocity.Y += entity.velocity.Y;
                            if (projectile.velocity.X > 6f)
                                projectile.velocity.X = 6f;

                            if (projectile.velocity.X < -6f)
                                projectile.velocity.X = -6f;

                            if (projectile.velocity.Length() > 16f)
                                projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * 16f;

                            projectile.netUpdate = true;
                            projectile.ai[1] += 1f;
                        }
                    }
                }

                if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                    projectile.Kill();

                projectile.rotation += 0.02f * projectile.velocity.X;
                if (projectile.velocity.Y == 0f)
                    projectile.velocity.X *= 0.98f;
                else if (projectile.wet)
                    projectile.velocity.X *= 0.99f;
                else
                    projectile.velocity.X *= 0.995f;

                if ((double)projectile.velocity.X > -0.03 && (double)projectile.velocity.X < 0.03)
                    projectile.velocity.X = 0f;

                if (projectile.wet) {
                    projectile.ai[1] = 0f;
                    if (projectile.velocity.Y > 0f)
                        projectile.velocity.Y *= 0.95f;

                    projectile.velocity.Y -= 0.1f;
                    if (projectile.velocity.Y < -4f)
                        projectile.velocity.Y = -4f;

                    if (projectile.velocity.X == 0f)
                        projectile.Kill();
                }
                else {
                    projectile.velocity.Y += 0.1f;
                }

                if (projectile.velocity.Y > 10f)
                    projectile.velocity.Y = 10f;
                return false;
            }
            //if (projectile.type == ProjectileID.SolarWhipSword)
            //{
            //    Player player = Main.player[projectile.owner];
            //    Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
            //    float num = (float)(Math.PI / 2f);
            //    if (Main.netMode != 2 && projectile.localAI[0] == 0f)
            //    {
            //        projectile.ai[1] = (Main.rand.NextFloat() - 0.5f) * (MathF.PI / 3f);
            //        SoundEngine.PlaySound(SoundID.Item116, projectile.Center);
            //    }
            //    if (projectile.localAI[1] > 0f)
            //    {
            //        projectile.localAI[1] -= 1f;
            //    }
            //    projectile.alpha -= 42;
            //    if (projectile.alpha < 0)
            //    {
            //        projectile.alpha = 0;
            //    }
            //    if (projectile.localAI[0] == 0f)
            //    {
            //        projectile.localAI[0] = projectile.velocity.ToRotation();
            //    }
            //    float num50 = ((projectile.localAI[0].ToRotationVector2().X >= 0f) ? 1 : (-1));
            //    if (projectile.ai[1] <= 0f)
            //    {
            //        num50 *= -1f;
            //    }
            //    Vector2 spinningpoint5 = (num50 * (projectile.ai[0] / 30f * (MathF.PI * 2f) - MathF.PI / 2f)).ToRotationVector2();
            //    spinningpoint5.Y *= MathF.Sin(projectile.ai[1]);
            //    if (projectile.ai[1] <= 0f)
            //    {
            //        spinningpoint5.Y *= -1f;
            //    }
            //    spinningpoint5 = spinningpoint5.RotatedBy(projectile.localAI[0]);
            //    projectile.ai[0] += 1f;
            //    if (projectile.ai[0] < 30f)
            //    {
            //        projectile.velocity += 48f * (player.GetModPlayer<MeleeStats>().weaponSize + player.GetAttackSpeed(DamageClass.Melee) * 0.25f) * spinningpoint5;
            //    }
            //    else
            //    {
            //        projectile.Kill();
            //    }
            //    projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - projectile.Size * 2f;
            //    projectile.rotation = projectile.velocity.ToRotation() + num;
            //    projectile.spriteDirection = projectile.direction;
            //    projectile.timeLeft = 2; 
            //    int num2 = 2;
            //    float num3 = 0f;
            //    player.ChangeDir(projectile.direction);
            //    player.heldProj = projectile.whoAmI;
            //    player.SetDummyItemTime(num2);
            //    player.itemRotation = MathHelper.WrapAngle(MathF.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction) + num3);
            //    Vector2 vector38 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            //    if (player.direction != 1)
            //    {
            //        vector38.X = (float)player.bodyFrame.Width - vector38.X;
            //    }
            //    if (player.gravDir != 1f)
            //    {
            //        vector38.Y = (float)player.bodyFrame.Height - vector38.Y;
            //    }
            //    vector38 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            //    projectile.Center = player.RotatedRelativePoint(player.MountedCenter - new Vector2(20f, 42f) / 2f + vector38, reverseRotation: false, addGfxOffY: false) - projectile.velocity;
            //    for (int num78 = 0; num78 < 2; num78++)
            //    {
            //        Dust obj = Main.dust[Dust.NewDust(projectile.position + projectile.velocity * 2f, projectile.width, projectile.height, 6, 0f, 0f, 100, Color.Transparent, 2f)];
            //        obj.noGravity = true;
            //        obj.velocity *= 2f;
            //        obj.velocity += projectile.localAI[0].ToRotationVector2();
            //        obj.fadeIn = 1.5f;
            //    }
            //    float num79 = 18f;
            //    for (int num80 = 0; (float)num80 < num79; num80++)
            //    {
            //        if (Main.rand.Next(4) == 0)
            //        {
            //            Vector2 vector39 = projectile.position + projectile.velocity + projectile.velocity * ((float)num80 / num79);
            //            Dust obj2 = Main.dust[Dust.NewDust(vector39, projectile.width, projectile.height, 6, 0f, 0f, 100, Color.Transparent)];
            //            obj2.noGravity = true;
            //            obj2.fadeIn = 0.5f;
            //            obj2.velocity += projectile.localAI[0].ToRotationVector2();
            //            obj2.noLight = true;
            //        }
            //    }
            //    return false;
            //}
            return true;
        }

 
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
  
 
                if ((projectile.type == ProjectileID.HelFire || projectile.type == ProjectileID.Sunfury) && projectile.ai[2] == 0)
            {
                projectile.ai[2] = 1;
                int damage = projectile.damage / 2;
                if (projectile.type == ProjectileID.Sunfury)
                    damage *= 2; // flail projectile's base damage is half of what's stated in the tooltip
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<HelAura>(), damage, projectile.knockBack / 2, projectile.owner, projectile.whoAmI);
            }
            if (projectile.counterweight)
            {
                projectile.damage = player.HeldItem.damage;
                return;
            }
            switch (projectile.type)
            {
                case ProjectileID.Shroomerang:
                    ++timer;
					if (timer > 20)
                    {
                        timer -= 20;
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity, 131, projectile.damage / 4, 0f, projectile.owner);
                    }
                    return;
                case ProjectileID.Gradient:
                    {
                        float PosX = projectile.position.X;
                        float PosY = projectile.position.Y;
                        float Float1 = 600f;
                        bool flag4 = false;
                        if (projectile.owner == Main.myPlayer)
                        {
                            projectile.localAI[1] += 1f + (1 * player.GetAttackSpeed(DamageClass.Melee));
                            if (projectile.localAI[1] > 90f)
                            {
                                projectile.localAI[1] = 90f;
                                for (int o = 0; o < 200; ++o)
                                {
                                    if (Main.npc[o].CanBeChasedBy(this, false))
                                    {
                                        float posX = Main.npc[o].position.X + (Main.npc[o].width / 2);
                                        float posY = Main.npc[o].position.Y + (Main.npc[o].height / 2);
                                        float num228 = Math.Abs(projectile.position.X + (projectile.width / 2) - posX) + Math.Abs(projectile.position.Y + (projectile.height / 2) - posY);
                                        if (num228 < Float1 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[o].position, Main.npc[o].width, Main.npc[o].height))
                                        {
                                            Float1 = num228;
                                            PosX = posX;
                                            PosY = posY;
                                            flag4 = true;
                                        }
                                    }
                                }
                            }

                        }
                        if (flag4)
                        {
                            projectile.localAI[1] = 0f;
                            float num229 = 14f;
                            Vector2 vector19 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                            float velX = PosX - vector19.X;
                            float velY = PosY - vector19.Y;
                            float sqrRoot = MathF.Sqrt(velX * velX + velY * velY);
                            sqrRoot = num229 / sqrRoot;
                            velX *= sqrRoot;
                            velY *= sqrRoot;
                            int bone = Projectile.NewProjectile(projectile.GetSource_FromThis(), vector19.X, vector19.Y, velX, velY, ProjectileID.BoneGloveProj, projectile.damage * 2, projectile.knockBack, Main.myPlayer, 0f, 0f);
                            Main.projectile[bone].DamageType = DamageClass.Melee;
                        }
                        return;
                    }
                case ProjectileID.Kraken:
                    {
                        /*
                        float PosX = projectile.position.X;
                        float PosY = projectile.position.Y;
                        float Range = 500f;
                        bool flag4 = false;
                        if (projectile.owner == Main.myPlayer)
                        {
                            projectile.localAI[1] += 1f;
                            if (projectile.localAI[1] >= 15f)
                            {
                                projectile.localAI[1] = 15f;
                                for (int o = 0; o < 200; ++o)
                                {
                                    if (Main.npc[o].CanBeChasedBy(this, false))
                                    {
                                        float posX = Main.npc[o].position.X + (Main.npc[o].width / 2);
                                        float posY = Main.npc[o].position.Y + (Main.npc[o].height / 2);
                                        float Distance = Math.Abs(projectile.position.X + (projectile.width / 2) - posX) + Math.Abs(projectile.position.Y + (projectile.height / 2) - posY);
                                        if (Distance < Range && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[o].position, Main.npc[o].width, Main.npc[o].height))
                                        {
                                            Range = Distance;
                                            PosX = posX;
                                            PosY = posY;
                                            flag4 = true;
                                        }
                                    }
                                }
                            }

                        }
                        if (flag4)
                        {
                            projectile.localAI[1] = 0f;
                            float num229 = 14f;
                            Vector2 vector19 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                            float velX = PosX - vector19.X;
                            float velY = PosY - vector19.Y;
                            float sqrRoot = MathF.Sqrt(velX * velX + velY * velY);
                            sqrRoot = num229 / sqrRoot;
                            velX *= sqrRoot;
                            velY *= sqrRoot;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), vector19.X, vector19.Y, velX, velY, ProjectileType<PhantomTentacle>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                        }
                        */
                        return;
                    }
                case ProjectileID.FormatC:
                    {
                        bool flag4 = false;
                        int mult = 1;
                        projectile.scale = 1f + (float)projectile.damage / 1000;
                        projectile.localAI[1] += 1f;
                        if (projectile.localAI[1] >= 36f && projectile.damage <= 300)
                        {
                            mult += 1;
                            projectile.damage += projectile.damage / mult;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with { MaxInstances = 0 }, projectile.Center);
                            for (int i = 0; i < 25; i++)
                            {
                                // Create a new dust
                                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 64, 10f, 10f, 0, default, 2f);
                                dust.velocity *= Main.rand.NextFloat(-1.5f, 1.5f);
                                dust.noGravity = true;
                            }
                            projectile.localAI[1] = 45f;
                            flag4 = true;
                        }
                        if (flag4)
                        {
                            projectile.localAI[1] = 0f;
                        }
                        return;
                    }
            }                    
        }

        public int HitCount = 0;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(projectile.type == ProjectileID.ChlorophyteOrb)
            {
                projectile.localNPCImmunity[target.whoAmI] = 30;
                projectile.ai[1] = 0f;
                Vector2 oldVelocity = projectile.velocity;
                    projectile.velocity.X = oldVelocity.X * -0.6f;

                if (oldVelocity.Y > 2f)
                    projectile.velocity.Y = oldVelocity.Y * -0.6f;
            }
            if(projectile.type == ProjectileID.FlamingMace)
            {
                target.AddBuff(BuffID.OnFire, 4 * 60);
            }
            Player player = Main.player[projectile.owner];             
            if (projectile.type == ProjectileID.TinyEater)
            { 
                TRAEDebuff.Apply<Corrupted>(target, 181, 1);
            }

            if (player.HasBuff(BuffID.WeaponImbueNanites) && (projectile.DamageType == DamageClass.Melee || projectile.aiStyle == 165 || projectile.type == ProjectileType<WhipProjectile>()))
            {
                player.AddBuff(BuffType<NanoHealing>(), 60, false);
            }
            switch (projectile.type)
            {
                case ProjectileID.Cascade:
                    {
                        ++HitCount;
                        if (HitCount >= 5)
                            projectile.Kill();
                        break;
                    }
                case ProjectileID.CrimsonYoyo:
                    {
                        player.lifeRegenCount += 30;
                        break;
                    }
              
            }
        }
        public override void PostAI(Projectile projectile)
        {
            //if (projectile.aiStyle == 3 && projectile.ai[0] == 1)
            //{
            //    Player player = Main.player[projectile.owner];
            //    float maxSpeed = 0f;
            //    switch (projectile.type)
            //    {
            //        case ProjectileID.Flamarang:
            //        case ProjectileID.Bananarang:
            //            maxSpeed = 30f;
            //            break;
            //        case ProjectileID.EnchantedBoomerang:
            //        case ProjectileID.IceBoomerang:          
            //        case ProjectileID.Shroomerang:
            //            maxSpeed = 9f;
            //            break;
            //        case ProjectileID.Trimarang:
            //            maxSpeed = 9.5f;
            //            break;
            //        case ProjectileID.ThornChakram:
            //            maxSpeed = 21.6f;
            //            break;
            //        case ProjectileID.CombatWrench:
            //            maxSpeed = 20f ;
            //            break;
            //        case ProjectileID.LightDisc:
 
            //        case ProjectileID.BouncingShield:
            //            maxSpeed = 19.2f;
            //            break;
            //        case ProjectileID.BloodyMachete:
            //            maxSpeed = 45f;
            //            break;
            //        case ProjectileID.FruitcakeChakram:
            //            maxSpeed = 7.2f;
            //            break;
            //    }
            //    if (maxSpeed != 0 && projectile.velocity.Length() < maxSpeed * player.GetAttackSpeed<MeleeDamageClass>())
            //    {
            //        Main.NewText(player.direction);
            //        Main.NewText(projectile.velocity);

            //     }
                 
            //}
            if (projectile.type == ProjectileID.TheHorsemansBlade)
            {
                projectile.scale *= 1.2f;
            }
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        { 
            switch (projectile.type)
            {
                case ProjectileID.StarWrath:
                    {
                        for (int num899 = 0; num899 < 4; num899++)
                        {
                            int num900 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default, 1.5f);
                            Main.dust[num900].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
                        }
                        for (int num901 = 0; num901 < 8; num901++)
                        {
                            int num902 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 200, default, 2.7f);
                            Main.dust[num902].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
                            Main.dust[num902].noGravity = true;
                            Dust dust2 = Main.dust[num902];
                            dust2.velocity *= 3f;
                            num902 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default, 1.5f);
                            Main.dust[num902].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
                            dust2 = Main.dust[num902];
                            dust2.velocity *= 2f;
                            Main.dust[num902].noGravity = true;
                            Main.dust[num902].fadeIn = 2.5f;
                        }
                        return false;
                    }
             
                
                case ProjectileID.Cascade:
                    {
                        projectile.position.X += projectile.width / 2;
                        projectile.position.Y += projectile.height / 2;
                        projectile.width = projectile.height = 125;
                        projectile.position.X -= projectile.width / 2;
                        projectile.position.Y -= projectile.height / 2;
                        projectile.position.X += projectile.width / 2;
                        projectile.position.Y += projectile.height / 2;
                        projectile.width = projectile.height = 125;
                        projectile.position.X -= (projectile.width / 2);
                        projectile.position.Y -= (projectile.height / 2);
                        for (int k = 0; k < 200; k++)
                        {
                            NPC nPC = Main.npc[k];
                            if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(projectile.Center, nPC.Center) <= 125)
                            {
                                Main.player[projectile.owner].ApplyDamageToNPC(nPC, (int)(projectile.damage * 2), 0f, 0, crit: false);
                                if (nPC.FindBuffIndex(BuffID.OnFire) == -1)
                                {
                                    nPC.AddBuff(BuffID.OnFire, 120);
                                }
                            }
                        }
                        TRAEMethods.DefaultExplosion(projectile);
                        return false;
                    }
                               
            }
            return true;
        }     
    }
}