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
            ProjectileID.Sets.YoyosTopSpeed[ProjectileID.Chik] = 20f;
            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.Chik] = 300f;


            ProjectileID.Sets.YoyosTopSpeed[ProjectileID.HelFire] = 20f;
            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.HelFire] = 370f;
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
            if(projectile.type == ProjectileID.Kraken)
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
                case ProjectileID.CrimsonYoyo:
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.4f;
                    break;

                case ProjectileID.VampireKnife:
                    projectile.ArmorPenetration = 20;
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
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.6f;
					projectile.penetrate = 3;
                    break;

                case ProjectileID.CorruptYoyo:
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Poisoned;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 180;
                    break;
                case ProjectileID.StarWrath:
                    projectile.penetrate = 1;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 300;
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

            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if(projectile.type == ProjectileID.ChlorophyteOrb)
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
            return base.OnTileCollide(projectile, oldVelocity);
        }
        int timer = 0;
        public override bool PreAI(Projectile projectile)
        {
            if(projectile.type == ProjectileID.ChlorophyteOrb)
            {
                projectile.frameCounter++;
                if(projectile.frameCounter % 10 == 0)
                {
                    projectile.frame++;
                    if(projectile.frame >= Main.projFrames[projectile.type])
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
            if(projectile.type == ProjectileID.HelFire && projectile.ai[2] == 0)
            {
                projectile.ai[2] = 1;
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<HelAura>(), projectile.damage / 2, 0, projectile.owner, projectile.whoAmI);
            }
            Player player = Main.player[projectile.owner];
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
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity, 131, projectile.damage / 2, 0f, projectile.owner);
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
                case ProjectileID.Sunfury:

                    TRAEDebuff.Apply<HeavyBurn>(target, 120, 1);
                    break;
            }
        }
        public override void PostAI(Projectile projectile)
        {
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