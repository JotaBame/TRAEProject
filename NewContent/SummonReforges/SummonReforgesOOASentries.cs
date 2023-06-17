using System;
using System.Collections.Generic;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.WorldBuilding;

namespace TRAEProject.NewContent.SummonReforges
{
    public partial class TestForSummonReforgesMinionChanges : GlobalProjectile//TODO: DETOUR LIGHTNING AURA CAN HIT FOR RANGE DETECTION
    {
        private static Conditions.IsSolid _cachedConditions_solid = new Conditions.IsSolid();
		private static Conditions.NotNull _cachedConditions_notNull = new Conditions.NotNull();//needed for lightning aura idk why
		private void AI_134_Ballista(Projectile projectile)
		{
			float shot_range = 900f * GetAggroRangeBoost(projectile.owner);
			float deadBottomAngle = 0.75f;
			Vector2 visualCenter = projectile.Center;
			int num2;
			float shotVelLength = 16f;
			int startingFrame = 1;
			int num5 = 5;
			int animSpeed = 5;
			if (Main.player[projectile.owner].setSquireT2)
			{
				shotVelLength = 21f;
			}
			int ballistaShotDelay = Projectile.GetBallistraShotDelay(Main.player[projectile.owner]);
			num2 = animSpeed;
			if (projectile.type == 677)// if it's the smallest one, lower the shoot offset by a bit
			{
				visualCenter.Y -= 4f;
			}
			if (projectile.ai[0] == 0f)
			{
				projectile.direction = (projectile.spriteDirection = Main.player[projectile.owner].direction);
				projectile.ai[0] = 1f;
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
				if (projectile.direction == -1)
				{
					projectile.rotation = MathF.PI;
				}
			}
			if (projectile.ai[0] == 1f)
			{
				projectile.frame = 0;
				bool flag = projectile.ai[1] <= 0f;
				if (Main.player[projectile.owner].ballistaPanic && projectile.ai[1] > 60f)
				{
					projectile.ai[1] = 60f;
				}
				if (Main.player[projectile.owner].ballistaPanic && Main.player[projectile.owner].setSquireT3 && projectile.ai[1] > 30f)
				{
					projectile.ai[1] = 30f;
				}
				if (projectile.ai[1] > 0f)
				{
					projectile.ai[1] -= 1f * GetAttackRateAsTimerIncrease(projectile.owner);
				}
				int targetIndex = AI_134_Ballista_FindTarget(shot_range, deadBottomAngle, visualCenter, projectile);
				if (targetIndex != -1)
				{
					Vector2 dirToTargetVec = (Main.npc[targetIndex].Center - visualCenter).SafeNormalize(Vector2.UnitY);
					projectile.rotation = projectile.rotation.AngleLerp(dirToTargetVec.ToRotation(), 0.08f);
					projectile.direction = (projectile.rotation > MathF.PI / 2f || projectile.rotation < -MathF.PI / 2f) ? -1 : 1;
					if (flag && projectile.owner == Main.myPlayer)
					{
						projectile.direction = MathF.Sign(dirToTargetVec.X);
						projectile.ai[0] = 2f;
						projectile.ai[1] = 0f;
						projectile.netUpdate = true;
					}
				}
				else
				{
					float targetAngle = 0f;
					if (projectile.direction == -1)
					{
						targetAngle = MathF.PI;
					}
					projectile.rotation = projectile.rotation.AngleLerp(targetAngle, 0.05f);
				}
			}
			else if (projectile.ai[0] == 2f)
			{
				projectile.frame = startingFrame + (int)(projectile.ai[1] / (float)animSpeed);
				if (projectile.ai[1] == num2)
				{
					SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, projectile.Center);
					Vector2 dirToTargetVector = new(projectile.direction, 0f);//makes it face forwards if no target
					int targetIndex = AI_134_Ballista_FindTarget(shot_range, deadBottomAngle, visualCenter, projectile);
					if (targetIndex != -1)
					{
						dirToTargetVector = (Main.npc[targetIndex].Center - visualCenter).SafeNormalize(Vector2.UnitX * projectile.direction);
					}
					projectile.rotation = dirToTargetVector.ToRotation();
					projectile.direction = (projectile.rotation > MathF.PI / 2f || projectile.rotation < -MathF.PI / 2f) ? -1 : 1;
					Vector2 shotVel = dirToTargetVector * shotVelLength * GetAttackVelocity(projectile.owner);
					if (projectile.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(projectile.GetSource_FromThis(), visualCenter, shotVel, ProjectileID.DD2BallistraProj, projectile.damage, projectile.knockBack, projectile.owner);
					}
				}
				if ((projectile.ai[1] += 1f) >= (num5 * animSpeed))
				{
					projectile.ai[0] = 1f;
					projectile.ai[1] = ballistaShotDelay;
				}
			}
			projectile.spriteDirection = projectile.direction;
			projectile.tileCollide = true;
			projectile.velocity.Y += 0.2f;
		}
		private int AI_134_Ballista_FindTarget(float shot_range, float deadBottomAngle, Vector2 shootingSpot, Projectile projectile)
		{
			int targetIndex = -1;
			NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this))
			{
				for (int i = 0; i < 1; i++)
				{
					if (!ownerMinionAttackTargetNPC.CanBeChasedBy(this))
					{
						continue;
					}
					float num2 = Vector2.Distance(shootingSpot, ownerMinionAttackTargetNPC.Center);
					if (!(num2 > shot_range))
					{
						Vector2 vector = (ownerMinionAttackTargetNPC.Center - shootingSpot).SafeNormalize(Vector2.UnitY);
						if ((!(Math.Abs(vector.X) < Math.Abs(vector.Y) * deadBottomAngle) || !(vector.Y > 0f)) && (targetIndex == -1 || num2 < Vector2.Distance(shootingSpot, Main.npc[targetIndex].Center)) && Collision.CanHitLine(shootingSpot, 0, 0, ownerMinionAttackTargetNPC.Center, 0, 0))
						{
							targetIndex = ownerMinionAttackTargetNPC.whoAmI;
						}
					}
				}
				if (targetIndex != -1)
				{
					return targetIndex;
				}
			}
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (!nPC.CanBeChasedBy(this))
				{
					continue;
				}
				float num3 = Vector2.Distance(shootingSpot, nPC.Center);
				if (!(num3 > shot_range))
				{
					Vector2 vector2 = (nPC.Center - shootingSpot).SafeNormalize(Vector2.UnitY);
					if ((!(Math.Abs(vector2.X) < Math.Abs(vector2.Y) * deadBottomAngle) || !(vector2.Y > 0f)) && (targetIndex == -1 || num3 < Vector2.Distance(shootingSpot, Main.npc[targetIndex].Center)) && Collision.CanHitLine(shootingSpot, 0, 0, nPC.Center, 0, 0))
					{
						targetIndex = j;
					}
				}
			}
			return targetIndex;
		}
		private void AI_130_FlameBurstTower(Projectile projectile)
		{
			float aggroRange = 900f * GetAggroRangeBoost(projectile.owner);
			float angleRatioMax = 1f;
			Vector2 projShootPos = projectile.Center;
			int projTypeToShoot = 664;
			int num3 = 12;
			float shotVelLength = 12f;
			int firstFrame = 1;
			int num6 = 6;
			int num7 = 4;
			int num8 = 80;
			switch (projectile.type)
			{
				case ProjectileID.DD2FlameBurstTowerT1:
					{
						Lighting.AddLight(projectile.Center, new Vector3(0.4f, 0.2f, 0.1f));
						Lighting.AddLight(projectile.Bottom + new Vector2(0f, -10f), new Vector3(0.4f, 0.2f, 0.1f));
						projShootPos = projectile.Bottom + new Vector2(projectile.direction * 6, -40f);
						if ((projectile.localAI[0] += 1f) >= 300f)
						{
							projectile.localAI[0] = 0f;
						}
						Rectangle r3 = new Rectangle((int)projectile.position.X + projectile.width / 4, (int)projectile.position.Y + projectile.height - 16, projectile.width / 4 * 3, 6);
						if (projectile.direction == 1)
						{
							r3.X -= projectile.width / 4;
						}
						for (int i = 0; i < 1; i++)
						{
							if (Main.rand.NextBool(2))
							{
								Dust dust5 = Dust.NewDustDirect(r3.TopLeft() + new Vector2(-2f, -2f), r3.Width + 4, r3.Height + 4, 270, -projectile.direction * 2, -2f, 200, new Color(255, 255, 255, 0));
								dust5.fadeIn = 0.6f + Main.rand.NextFloat() * 0.6f;
								dust5.scale = 0.4f;
								dust5.noGravity = true;
								dust5.noLight = true;
								dust5.velocity = Vector2.Zero;
								dust5.velocity.X = (float)(-projectile.direction) * Main.rand.NextFloat() * dust5.fadeIn;
							}
						}
						r3 = new Rectangle((int)projectile.Center.X, (int)projectile.Bottom.Y, projectile.width / 4, 10);
						if (projectile.direction == -1)
						{
							r3.X -= r3.Width;
						}
						r3.X += projectile.direction * 4;
						r3.Y -= projectile.height - 10;
						for (int n = 0; n < 1; n++)
						{
							if (Main.rand.NextBool(5))
							{
								Dust dust6 = Dust.NewDustDirect(r3.TopLeft(), r3.Width, r3.Height, 6);
								dust6.fadeIn = 1f;
								dust6.scale = 1f;
								dust6.noGravity = true;
								dust6.noLight = true;
								dust6.velocity *= 2f;
							}
						}
						break;
					}
				case ProjectileID.DD2FlameBurstTowerT2:
					{
						Lighting.AddLight(projectile.Center, new Vector3(0.4f, 0.2f, 0.1f) * 1.2f);
						Lighting.AddLight(projectile.Bottom + new Vector2(0f, -10f), new Vector3(0.4f, 0.2f, 0.1f) * 1.2f);
						num8 = 70;
						shotVelLength += 3f;
						num6 = 8;
						projTypeToShoot = 666;
						projShootPos = projectile.Bottom + new Vector2(projectile.direction * 6, -44f);
						if ((projectile.localAI[0] += 1f) >= 300f)
						{
							projectile.localAI[0] = 0f;
						}
						Rectangle r2 = new Rectangle((int)projectile.position.X + projectile.width / 4, (int)projectile.position.Y + projectile.height - 16, projectile.width / 4 * 2, 6);
						if (projectile.direction == 1)
						{
							r2.X -= projectile.width / 4;
						}
						for (int i = 0; i < 1; i++)
						{
							if (Main.rand.NextBool(2))
							{
								Dust dust3 = Dust.NewDustDirect(r2.TopLeft() + new Vector2(-2f, -2f), r2.Width + 4, r2.Height + 4, 270, -projectile.direction * 2, -2f, 200, new Color(255, 255, 255, 0));
								dust3.fadeIn = 0.6f + Main.rand.NextFloat() * 0.6f;
								dust3.scale = 0.4f;
								dust3.noGravity = true;
								dust3.noLight = true;
								dust3.velocity = Vector2.Zero;
								dust3.velocity.X = (float)(-projectile.direction) * Main.rand.NextFloat() * dust3.fadeIn;
							}
						}
						r2 = new Rectangle((int)projectile.Center.X, (int)projectile.Bottom.Y, projectile.width / 4, 10);
						if (projectile.direction == -1)
						{
							r2.X -= r2.Width;
						}
						r2.X += projectile.direction * 4;
						r2.Y -= projectile.height - 10;
						for (int l = 0; l < 2; l++)
						{
							if (Main.rand.NextBool(5))
							{
								Dust dust4 = Dust.NewDustDirect(r2.TopLeft(), r2.Width, r2.Height, 6);
								dust4.fadeIn = 1f;
								dust4.scale = 1f;
								dust4.noGravity = true;
								dust4.noLight = true;
								dust4.velocity *= 2f;
							}
						}
						break;
					}
				case ProjectileID.DD2FlameBurstTowerT3:
					{
						Lighting.AddLight(projectile.Center, new Vector3(0.4f, 0.2f, 0.1f) * 1.5f);
						Lighting.AddLight(projectile.Bottom + new Vector2(0f, -10f), new Vector3(0.4f, 0.2f, 0.1f) * 1.5f);
						num8 = 60;
						shotVelLength += 6f;
						num6 = 8;
						projTypeToShoot = 668;
						projShootPos = projectile.Bottom + new Vector2(projectile.direction * 6, -46f);
						if ((projectile.localAI[0] += 1f) >= 300f)
						{
							projectile.localAI[0] = 0f;
						}
						Rectangle r = new Rectangle((int)projectile.position.X + projectile.width / 4, (int)projectile.position.Y + projectile.height - 16, projectile.width / 4 * 2, 6);
						if (projectile.direction == 1)
						{
							r.X -= projectile.width / 4;
						}
						for (int i = 0; i < 1; i++)
						{
							if (Main.rand.NextBool(2))
							{
								Dust dust = Dust.NewDustDirect(r.TopLeft() + new Vector2(-2f, -2f), r.Width + 4, r.Height + 4, DustID.FlameBurst, -projectile.direction * 2, -2f, 200, new Color(255, 255, 255, 0));
								dust.fadeIn = 0.6f + Main.rand.NextFloat() * 0.6f;
								dust.scale = 0.4f;
								dust.noGravity = true;
								dust.noLight = true;
								dust.velocity = Vector2.Zero;
								dust.velocity.X = (float)(-projectile.direction) * Main.rand.NextFloat() * dust.fadeIn;
							}
						}
						r = new Rectangle((int)projectile.Center.X, (int)projectile.Bottom.Y, projectile.width / 4, 10);
						if (projectile.direction == -1)
						{
							r.X -= r.Width;
						}
						r.X += projectile.direction * 4;
						r.Y -= projectile.height - 10;
						for (int i = 0; i < 3; i++)
						{
							if (Main.rand.NextBool(5))
							{
								Dust dust2 = Dust.NewDustDirect(r.TopLeft(), r.Width, r.Height, 6);
								dust2.fadeIn = 1.1f;
								dust2.scale = 1f;
								dust2.noGravity = true;
								dust2.noLight = true;
								dust2.velocity *= 2.4f;
							}
						}
						break;
					}
			}
			if (Main.player[projectile.owner].setApprenticeT2)
			{
				angleRatioMax = 0.1f;
				aggroRange *= 1.5f;
				shotVelLength *= 1.4f;
			}
			if (projectile.ai[0] == 0f)
			{
				projectile.direction = (projectile.spriteDirection = Main.player[projectile.owner].direction);
				projectile.ai[0] = 1f;
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}
			if (projectile.ai[0] == 1f)
			{
				projectile.frame = 0;
				bool flag = false;
				if (projectile.ai[1] > 0f)
				{
					projectile.ai[1] -= 1f * GetAttackRateAsTimerIncrease(projectile.owner);
				}
				else
				{
					flag = true;
				}
				if (flag && projectile.owner == Main.myPlayer)
				{
					int targetIndex = AI_130_FlameBurstTower_FindTarget(aggroRange, angleRatioMax, projShootPos, true, projectile);
					if (targetIndex != -1)
					{
						projectile.direction = Math.Sign(projectile.DirectionTo(Main.npc[targetIndex].Center).X);
						projectile.ai[0] = 2f;
						projectile.ai[1] = 0f;
						projectile.netUpdate = true;
					}
				}
			}
			else if (projectile.ai[0] == 2f)//ai0 = 2 means that they are attacking I think
			{
				projectile.frame = firstFrame + (int)(projectile.ai[1] / (float)num7);
				if (projectile.ai[1] == (float)num3)
				{
					Vector2 dirToTargetNormalized = new Vector2(projectile.direction, 0f);
					int num10 = AI_130_FlameBurstTower_FindTarget(aggroRange, angleRatioMax, projShootPos, canChangeDirection: false, projectile);
					if (num10 != -1)
					{
						dirToTargetNormalized = (Main.npc[num10].Center - projShootPos).SafeNormalize(Vector2.UnitX * projectile.direction);
					}
					Vector2 shotVel = dirToTargetNormalized * shotVelLength * GetAttackVelocity(projectile.owner);
					if (projectile.owner == Main.myPlayer)
					{

						Projectile.NewProjectile(projectile.GetSource_FromThis(), projShootPos, shotVel, projTypeToShoot, projectile.damage, projectile.knockBack, projectile.owner);
					}
				}
				if ((projectile.ai[1] += 1f) >= (float)(num6 * num7))
				{
					projectile.ai[0] = 1f;
					projectile.ai[1] = num8;
				}
			}
			//Main.NewText(projectile.ai[1]);

			projectile.spriteDirection = projectile.direction;
			projectile.tileCollide = true;
			projectile.velocity.Y += 0.2f;
		}
		private int AI_130_FlameBurstTower_FindTarget(float shot_range, float angleRatioMax, Vector2 shootingSpot, bool canChangeDirection, Projectile projectile)
		{
			int num = -1;
			NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this))
			{
				for (int i = 0; i < 1; i++)
				{
					if (!ownerMinionAttackTargetNPC.CanBeChasedBy(this))
					{
						continue;
					}
					float num2 = Vector2.Distance(shootingSpot, ownerMinionAttackTargetNPC.Center);
					if (!(num2 > shot_range))
					{
						Vector2 vector = (ownerMinionAttackTargetNPC.Center - shootingSpot).SafeNormalize(Vector2.UnitY);
						if (!(Math.Abs(vector.X) < Math.Abs(vector.Y) * angleRatioMax) && (canChangeDirection || !((float)projectile.direction * vector.X < 0f)) && (num == -1 || num2 < Vector2.Distance(shootingSpot, Main.npc[num].Center)) && Collision.CanHitLine(shootingSpot, 0, 0, ownerMinionAttackTargetNPC.Center, 0, 0))
						{
							num = ownerMinionAttackTargetNPC.whoAmI;
						}
					}
				}
				if (num != -1)
				{
					return num;
				}
			}
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (!nPC.CanBeChasedBy(this))
				{
					continue;
				}
				float num3 = Vector2.Distance(shootingSpot, nPC.Center);
				if (!(num3 > shot_range))
				{
					Vector2 vector2 = (nPC.Center - shootingSpot).SafeNormalize(Vector2.UnitY);
					if (!(Math.Abs(vector2.X) < Math.Abs(vector2.Y) * angleRatioMax) && (canChangeDirection || !((float)projectile.direction * vector2.X < 0f)) && (num == -1 || num3 < Vector2.Distance(shootingSpot, Main.npc[num].Center)) && Collision.CanHitLine(shootingSpot, 0, 0, nPC.Center, 0, 0))
					{
						num = j;
					}
				}
			}
			return num;
		}
		private void AI_138_ExplosiveTrap(Projectile projectile)
		{
			projectile.spriteDirection = projectile.direction = 1;
			int explosionProjID = ProjectileID.DD2ExplosiveTrapT1Explosion;
			int verticalExplosionOffset = 48;
			int explosiveTrapCooldown = Projectile.GetExplosiveTrapCooldown(Main.player[projectile.owner]);
			switch (projectile.type)
			{
				case ProjectileID.DD2ExplosiveTrapT2:
					explosionProjID = ProjectileID.DD2ExplosiveTrapT2Explosion;
					break;
				case ProjectileID.DD2ExplosiveTrapT3:
					explosionProjID = ProjectileID.DD2ExplosiveTrapT3Explosion;
					break;
			}
			Lighting.AddLight(projectile.Center, 0.6f, 0.5f, 0.3f);
			if (++projectile.frameCounter >= 12)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}
			if (projectile.localAI[0] > 0f)
			{
				projectile.localAI[0] -= 1f * GetAttackRateAsTimerIncrease(projectile.owner);
			}
			if (projectile.localAI[0] <= 0f && projectile.owner == Main.myPlayer)
			{
				projectile.localAI[0] = 3f;
				bool hasTarget = false;
				Rectangle rectangle = Utils.CenteredRectangle(projectile.Center + new Vector2(0f, -verticalExplosionOffset * GetAggroRangeBoost(projectile.owner)), new Vector2(verticalExplosionOffset * 3 * GetAggroRangeBoost(projectile.owner)));
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC nPC = Main.npc[i];
					if (nPC.CanBeChasedBy(this) && rectangle.Intersects(nPC.Hitbox))
					{
						hasTarget = true;
						break;
					}
				}
				if (hasTarget)
				{
					SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);
					projectile.localAI[0] = explosiveTrapCooldown;
					Projectile spawnedExplosion = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + new Vector2(0f, -verticalExplosionOffset), Vector2.Zero, explosionProjID, projectile.damage, projectile.knockBack, projectile.owner);
					spawnedExplosion.Center -= new Vector2(0.5f * (GetAggroRangeBoost(projectile.owner) * spawnedExplosion.Size.X - spawnedExplosion.Size.X), GetAggroRangeBoost(projectile.owner) * spawnedExplosion.Size.Y - spawnedExplosion.Size.Y);
					spawnedExplosion.Size *= GetAggroRangeBoost(projectile.owner);
					spawnedExplosion.scale *= GetAggroRangeBoost(projectile.owner);//mght cause multiplayer badness but I have no clue I am just paranoid
				}
			}
			projectile.tileCollide = true;
			projectile.velocity.Y += 0.2f;
		}
		private void AI_137_LightningAura(Projectile projectile)
        {
            int whatEvenIsTHisAAAAAA = 10;
            int num2 = 999;
            int hitDelay = 30;
            int num4 = (int)(40/** GetAggroRangeBoost()*/);
            int reachInTilesMinusTwoWeirdlyCodedToHave8MinSize = 4;
            projectile.knockBack = 0f;

			int likeActualRangeInTilesThisIsStupid = 8;
			if (Main.player[projectile.owner].setMonkT2)
            {
                hitDelay -= 5;
            }
            if (Main.player[projectile.owner].setMonkT3)
            {
                whatEvenIsTHisAAAAAA = 14;
				likeActualRangeInTilesThisIsStupid = 12;
            }
			likeActualRangeInTilesThisIsStupid = (int)(likeActualRangeInTilesThisIsStupid * GetAggroRangeBoost(projectile.owner));
			reachInTilesMinusTwoWeirdlyCodedToHave8MinSize = likeActualRangeInTilesThisIsStupid + 2;
            projectile.ai[0] += 1f * GetAttackRateAsTimerIncrease(projectile.owner);
            if (projectile.ai[0] >= hitDelay)
            {
                projectile.ai[0] = 0f;
            }
            if (projectile.ai[0] == 0f)//I think this is for sound effects only? after testing: yes sound effect only
            {
                bool justHitAnEnemy = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(this) && npc.Hitbox.Distance(projectile.Center) < (float)(projectile.width / 2) && projectile.Colliding(projectile.Hitbox, npc.Hitbox))
                    {
                        justHitAnEnemy = true;
                        break;
                    }
                }
                if (justHitAnEnemy)
                {
                    SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap with { MaxInstances = 0 }, projectile.Center);
                }
            }
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;//oh wait this oly triggers upon summoning
                projectile.velocity = Vector2.Zero;
                Point origin = projectile.Center.ToTileCoordinates();
                bool whatIsThis = true;
                if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down((int)(500)), _cachedConditions_notNull, _cachedConditions_solid), out Point result))
                {
                    whatIsThis = false;
                    projectile.position.Y += 16f;
                    return;
                }
                if (!WorldUtils.Find(new Point(result.X, result.Y - 1), Searches.Chain(new Searches.Up(whatEvenIsTHisAAAAAA), _cachedConditions_notNull, _cachedConditions_solid), out Point result2))
                {
                    result2 = new Point(origin.X, origin.Y - whatEvenIsTHisAAAAAA - 1);
                }
                int num6 = 0;
                if (whatIsThis && Main.tile[result.X, result.Y] != null && Main.tile[result.X, result.Y].BlockType == BlockType.HalfBlock)
                {
                    num6 += 8;
                }
                Vector2 center = result.ToWorldCoordinates(8f, num6);
                Vector2 ceiling = result2.ToWorldCoordinates(8f, 0f);
                projectile.Size = new Vector2(1f, center.Y - ceiling.Y);
                if (projectile.height > whatEvenIsTHisAAAAAA * 16)
                {
                    projectile.height = whatEvenIsTHisAAAAAA * 16;
                }
                if (projectile.height < reachInTilesMinusTwoWeirdlyCodedToHave8MinSize * 16)
                {
                    projectile.height = reachInTilesMinusTwoWeirdlyCodedToHave8MinSize * 16;
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
            LightningAura_DustEffect(projectile, num4);
            projectile.tileCollide = true;
            projectile.velocity.Y += 0.2f;
        }

        private static void LightningAura_DustEffect(Projectile projectile, int num4)
        {
			Vector2 vector2 = new Vector2(projectile.Top.X, projectile.position.Y + (float)num4);
			for (int i = 0; i < 7; i++)
            {
                if (!Main.rand.NextBool(6))
                {
                    continue;
                }
                Vector2 vector3 = Main.rand.NextVector2Unit();
                if (!(Math.Abs(vector3.X) < 0.12f))
                {
                    Vector2 targetPosition = projectile.Center + vector3 * new Vector2((projectile.height - num4) / 2);
                    if (!WorldGen.SolidTile((int)targetPosition.X / 16, (int)targetPosition.Y / 16) && projectile.AI_137_CanHit(targetPosition))
                    {
                        Dust dust = Dust.NewDustDirect(targetPosition, 0, 0, DustID.Electric, 0f, 0f, 100);
                        dust.position = targetPosition;
                        dust.velocity = (vector2 - dust.position).SafeNormalize(Vector2.Zero);
                        dust.scale = 0.7f;
                        dust.fadeIn = 1f;
                        dust.noGravity = true;
                        dust.noLight = true;
                    }
                }
            }
            for (int i = 0; i < 1; i++)
            {
                if (!Main.rand.NextBool(10))
                {
                    continue;
                }
                Vector2 vector4 = Main.rand.NextVector2Unit();
                if (!(Math.Abs(vector4.X) < 0.12f))
                {
                    Vector2 targetPosition2 = projectile.Center + vector4 * new Vector2((projectile.height - num4) / 2) * Main.rand.NextFloat();
                    if (!WorldGen.SolidTile((int)targetPosition2.X / 16, (int)targetPosition2.Y / 16) && projectile.AI_137_CanHit(targetPosition2))
                    {
                        Dust dust2 = Dust.NewDustDirect(targetPosition2, 0, 0, DustID.Electric, 0f, 0f, 100);
                        dust2.velocity *= 0.6f;
                        dust2.velocity += Vector2.UnitY * -2f;
                        dust2.noGravity = true;
                        dust2.noLight = true;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust dust3 = Dust.NewDustDirect(vector2 - new Vector2(8f, 0f), 16, projectile.height / 2 - 40, DustID.Electric, 0f, 0f, 100);
                    dust3.velocity *= 0.6f;
                    dust3.velocity += Vector2.UnitY * -2f;
                    dust3.scale = 0.7f;
                    dust3.noGravity = true;
                    dust3.noLight = true;
                }
            }
        }
    }
}
