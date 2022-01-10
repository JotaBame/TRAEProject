using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace TRAEProject.Changes.Projectiles
{
	public static class Pygmy
	{
		static int attackCooldown = 30;
		static int attackRecencyCooldown = 60;

		public static void AI(Projectile projectile)
		{
			//Main.NewText("AI:" + projectile.ai[0] + ", " + projectile.ai[1] + ", " + projectile.localAI[0] + ", " + projectile.localAI[1]);

			bool moveLeft = false;
			bool moveRight = false;

			ManageMinion(projectile);
			CreateChargeDust(projectile, projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge);
			IdlePositioning(projectile, ref moveLeft, ref moveRight);
			if (projectile.localAI[0] == 0f)
			{
				ReturnModeCheck(projectile);
			}
			if (projectile.ai[0] != 0f) //return mode
			{
				ReturnMode(projectile);
			}
			else
			{
				projectile.rotation = 0f;
				projectile.tileCollide = true;

				//attack recency countdown
				projectile.localAI[0] -= 1f;
				if (projectile.localAI[0] < 0f)
				{
					projectile.localAI[0] = 0f;
				}

				//attack cooldown countdown
				if (projectile.ai[1] > 0f)
				{
					projectile.ai[1] -= 1f;
				}
				else
				{
					TargetEnemies(projectile, ref moveLeft, ref moveRight);
				}

				Movement(projectile, ref moveLeft, ref moveRight);
			}
		}
		public static void CreateChargeDust(Projectile projectile, int type)
		{
			if (type > 0)
			{
				int dustType = 134;
				switch (type)
				{
					case 1:
					case 2:
					case 3:
					case 4:
						dustType = 131;
						break;
					case 5:
					case 6:
					case 7:
					case 8:
						dustType = 132;
						break;
					case 9:
					case 10:
					case 11:
					case 12:
						dustType = 133;
						break;
				}
				for (int i = 0; i < 1; i++)
				{
					Dust dust = Dust.NewDustPerfect(projectile.position + new Vector2(Main.rand.NextFloat(projectile.width), Main.rand.NextFloat(projectile.height/2)), dustType, Vector2.UnitY * -4, 0, default, Scale: 1f);
					dust.frame.Y = 0;
					dust.noGravity = true;
					dust.noLight = true;
					dust.scale = 0.5f;
				}
			}
		}
        static void EnterReturnMode(Projectile projectile)
        {
			projectile.ai[0] = 1f;
			projectile.velocity.Y = 0f;
			Poof(projectile.Center);
		}
		static void Poof(Vector2 position)
        {
			int num528 = Gore.NewGore(position, new Vector2(0f, 0f), Main.rand.Next(61, 64), 1f);
			Gore gore = Main.gore[num528];
			gore.velocity *= 0.1f;
		}
		static void ManageMinion(Projectile projectile)
        {
			Player player = Main.player[projectile.owner];
			if (!player.active)
			{
				projectile.active = false;
				return;
			}

			if (player.dead)
			{
				player.pygmy = false;
			}
			if (player.pygmy)
			{
				projectile.timeLeft = Main.rand.Next(2, 10);
			}


			if (projectile.lavaWet)
			{
				projectile.ai[0] = 1f;
				projectile.ai[1] = 0f;
			}
		}
		static void ReturnModeCheck(Projectile projectile)
        {

			Player player = Main.player[projectile.owner];
			Point origin = player.Bottom.ToTileCoordinates();
			int nearGround = 10;
			if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(nearGround), new GenCondition[]
			{
			new Conditions.IsSolid()
			}), out _))
            {
				if(!Collision.CanHit(player, projectile) || (player.Center - projectile.Center).Length() > 3000)
				{
					EnterReturnMode(projectile);
				}
				else if (projectile.localAI[0] == 0f && Math.Abs(player.Center.Y - projectile.Center.Y) > nearGround * 16)
                {
					EnterReturnMode(projectile);
				}
			}
		}
		static void IdlePositioning(Projectile projectile, ref bool moveLeft, ref bool moveRight)
        {
			Player player = Main.player[projectile.owner];
			int idleMargin = 10;
			int idleDistanceFromPlayer = (40 * (projectile.minionPos + 1)) * player.direction;
			if (player.Center.X < projectile.Center.X - (float)idleMargin + (float)idleDistanceFromPlayer)
			{
				moveLeft = true;
			}
			else if (player.Center.X > projectile.Center.X + (float)idleMargin + (float)idleDistanceFromPlayer)
			{
				moveRight = true;
			}
		}
		static void ReturnMode(Projectile projectile)
        {
			Player player = Main.player[projectile.owner];
			projectile.tileCollide = false;
			projectile.Center = player.Center + Vector2.UnitX * (8 * projectile.minionPos);
			projectile.velocity = player.velocity;
			Poof(projectile.Center);
			projectile.ai[0] = 0;

		}
		static void TargetEnemies(Projectile projectile, ref bool moveLeft, ref bool moveRight)
        {
			float minionOffset = 40 * projectile.minionPos;
			

			float maxDistance = 100000f;
			NPC target = null;
			if (TRAEMethods.ClosestNPC(ref target, maxDistance, projectile.Center, false, projectile.OwnerMinionAttackTargetNPC != null ? projectile.OwnerMinionAttackTargetNPC.whoAmI : -1))
			{
				float horizontalDist = target.Center.X - (projectile.Center.X);
				float distRequired = 180 + 16 * projectile.minionPos;
				if (horizontalDist > distRequired || horizontalDist < -distRequired)
				{
					if (horizontalDist < -50f)
					{
						moveLeft = true;
						moveRight = false;
					}
					else if (horizontalDist > 50f)
					{
						moveRight = true;
						moveLeft = false;
					}
				}
				else if (projectile.owner == Main.myPlayer)
				{
					Vector2 shootFrom = new Vector2(projectile.Center.X, projectile.Center.Y - 8f);
					Vector2 shootAt = target.Center;
					float shootSpeed = 18f;
					float shootAngle = TRAEMethods.PredictiveAim(shootFrom, shootSpeed * (1 + projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge), shootAt, target.velocity, out float travelTime);
					if (float.IsNaN(shootAngle) || travelTime > 40f / (1 + projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge))
					{
						if (projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge < 16)
						{
							projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge++;
							projectile.ai[1] = attackCooldown;
						}
						projectile.netUpdate = true;
					}
					else
					{
						projectile.localAI[0] = attackRecencyCooldown;
						projectile.ai[1] = attackCooldown;
						int spearIndex = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), shootFrom, TRAEMethods.PolarVector(shootSpeed, shootAngle), ProjectileID.PygmySpear, projectile.damage * (1 + projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge), projectile.knockBack, Main.myPlayer);
						Main.projectile[spearIndex].timeLeft = 300;
						Main.projectile[spearIndex].extraUpdates = projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge;
						projectile.GetGlobalProjectile<MinionChanges>().pygmyCharge = 0;
						if (shootAt.X - projectile.Center.X < 0f)
						{
							projectile.direction = -1;
						}
						if (shootAt.X - projectile.Center.X > 0f)
						{
							projectile.direction = 1;
						}
						projectile.netUpdate = true;
					}
				}
			}
		}

		static void Movement(Projectile projectile, ref bool moveLeft, ref bool moveRight)
		{
			Player player = Main.player[projectile.owner];


			bool playerBelow = false;
			bool aboutToHitWall = false;

			//don't move if attacking
			if (projectile.ai[1] != 0f)
			{
				moveLeft = false;
				moveRight = false;
			}
			else if (projectile.localAI[0] == 0f)
			{
				projectile.direction = player.direction;
			}


			float maxVelocityX = 12f;
			float accX = 0.8f;
			if (maxVelocityX < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				maxVelocityX = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				accX = 0.3f;
			}

			if (moveLeft)
			{
				if ((double)projectile.velocity.X > -3.5)
				{
					projectile.velocity.X -= accX;
				}
				else
				{
					projectile.velocity.X -= accX * 0.25f;
				}
			}
			else if (moveRight)
			{
				if ((double)projectile.velocity.X < 3.5)
				{
					projectile.velocity.X += accX;
				}
				else
				{
					projectile.velocity.X += accX * 0.25f;
				}
			}
			else
			{
				projectile.velocity.X *= 0.9f;
				if (projectile.velocity.X >= 0f - accX && projectile.velocity.X <= accX)
				{
					projectile.velocity.X = 0f;
				}
			}

			if (moveLeft || moveRight)
			{
				int tilePosX = (int)(projectile.Center.X) / 16;
				int tilePosY = (int)(projectile.Center.Y) / 16;
				if (moveLeft)
				{
					tilePosX--;
				}
				if (moveRight)
				{
					tilePosX++;
				}
				tilePosX += (int)projectile.velocity.X;
				if (WorldGen.SolidTile(tilePosX, tilePosY))
				{
					aboutToHitWall = true;
				}
			}
			if (player.position.Y + (float)player.height - 8f > projectile.position.Y + (float)projectile.height)
			{
				playerBelow = true;
			}

			Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
			if (projectile.velocity.Y == 0f)
			{
				/*
				if (!playerBelow && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
				{
					int num158 = (int)(projectile.Center.X) / 16;
					int j3 = (int)(projectile.Center.Y) / 16 + 1;
					if (moveLeft)
					{
						num158--;
					}
					if (moveRight)
					{
						num158++;
					}
					WorldGen.SolidTile(num158, j3);
				}
				*/
				if (aboutToHitWall)
				{
					int tilePosX = (int)(projectile.Center.X) / 16;
					int tilePosY = (int)(projectile.Bottom.Y) / 16;
					if (WorldGen.SolidTileAllowBottomSlope(tilePosX, tilePosY) || Main.tile[tilePosX, tilePosY].IsHalfBlock || Main.tile[tilePosX, tilePosY].Slope > 0)
					{

						try
						{
							tilePosX = (int)(projectile.Center.X) / 16;
							tilePosY = (int)(projectile.Center.Y) / 16;
							if (moveLeft)
							{
								tilePosX--;
							}
							if (moveRight)
							{
								tilePosX++;
							}
							tilePosX += (int)projectile.velocity.X;
							if (!WorldGen.SolidTile(tilePosX, tilePosY - 1) && !WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								projectile.velocity.Y = -5.1f;
							}
							else if (!WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								projectile.velocity.Y = -7.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 5))
							{
								projectile.velocity.Y = -11.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 4))
							{
								projectile.velocity.Y = -10.1f;
							}
							else
							{
								projectile.velocity.Y = -9.1f;
							}
						}
						catch
						{
							projectile.velocity.Y = -9.1f;
						}
					}
				}
			}
			if (projectile.velocity.X > maxVelocityX)
			{
				projectile.velocity.X = maxVelocityX;
			}
			if (projectile.velocity.X < 0f - maxVelocityX)
			{
				projectile.velocity.X = 0f - maxVelocityX;
			}
			if (projectile.velocity.X < 0f)
			{
				projectile.direction = -1;
			}
			if (projectile.velocity.X > 0f)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X > accX && moveRight)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X < 0f - accX && moveLeft)
			{
				projectile.direction = -1;
			}
			if (projectile.direction != 0)
			{
				projectile.spriteDirection = -projectile.direction;
			}

			bool notWalking = projectile.position.X - projectile.oldPosition.X == 0f;
			if (projectile.ai[1] > 0f)
			{
				if (projectile.localAI[1] == 0f)
				{
					projectile.localAI[1] = 1f;
					projectile.frame = 1;
				}
				if (projectile.frame != 0)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 4)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame >= 4)
					{
						projectile.frame = 0;
					}
				}
			}
			else if (projectile.velocity.Y == 0f)
			{
				projectile.localAI[1] = 0f;
				if (notWalking)
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
				else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
				{
					projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
					projectile.frameCounter++;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 5)
					{
						projectile.frame = 5;
					}
					if (projectile.frame >= 11)
					{
						projectile.frame = 5;
					}
				}
				else
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
			}
			else if (projectile.velocity.Y < 0f)
			{
				projectile.frameCounter = 0;
				projectile.frame = 4;
			}
			else if (projectile.velocity.Y > 0f)
			{
				projectile.frameCounter = 0;
				projectile.frame = 4;
			}
			projectile.velocity.Y += 0.4f;
			if (projectile.velocity.Y > 10f)
			{
				projectile.velocity.Y = 10f;
			}
		}



		static void LegacyManageMinion(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			if (!player.active)
			{
				projectile.active = false;
				return;
			}

			if (player.dead)
			{
				player.pygmy = false;
			}
			if (player.pygmy)
			{
				projectile.timeLeft = Main.rand.Next(2, 10);
			}


			if (projectile.lavaWet)
			{
				projectile.ai[0] = 1f;
				projectile.ai[1] = 0f;
			}
		}
		static void LegacyReturnModeCheck(Projectile projectile)
		{

			Player player = Main.player[projectile.owner];
			int returnModeDistance = 500;

			returnModeDistance += 40 * projectile.minionPos;
			if (projectile.localAI[0] > 0f)
			{
				returnModeDistance += 500;
			}

			if (player.rocketDelay2 > 0)
			{
				projectile.ai[0] = 1f;
			}
			float verticalDistToPlayer = player.Center.Y - projectile.Center.Y;
			float distToPlayer = (player.Center - projectile.Center).Length();
			if (distToPlayer > 2000f)
			{
				projectile.Center = player.Center;
			}
			else if (distToPlayer > (float)returnModeDistance || (Math.Abs(verticalDistToPlayer) > 300f && (!(projectile.localAI[0] > 0f))))
			{

				if (verticalDistToPlayer > 0f && projectile.velocity.Y < 0f)
				{
					projectile.velocity.Y = 0f;
				}
				if (verticalDistToPlayer < 0f && projectile.velocity.Y > 0f)
				{
					projectile.velocity.Y = 0f;
				}
				projectile.ai[0] = 1f;
			}
		}
		static void LegacyIdlePositioning(Projectile projectile, ref bool moveLeft, ref bool moveRight)
		{
			Player player = Main.player[projectile.owner];
			int idleMargin = 10;
			int idleDistanceFromPlayer = (40 * (projectile.minionPos + 1)) * player.direction;
			if (player.Center.X < projectile.Center.X - (float)idleMargin + (float)idleDistanceFromPlayer)
			{
				moveLeft = true;
			}
			else if (player.Center.X > projectile.Center.X + (float)idleMargin + (float)idleDistanceFromPlayer)
			{
				moveRight = true;
			}
		}
		static void LegacyReturnMode(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			projectile.tileCollide = false;
			float horizontalDistToPlayer = player.Center.X - projectile.Center.X;

			horizontalDistToPlayer -= (float)(40 * player.direction);
			float maxNpcDistance = 800f;

			bool foundTarget = false;
			int targetNpcIndex = -1;
			for (int k = 0; k < 200; k++)
			{
				if (!Main.npc[k].CanBeChasedBy(projectile))
				{
					continue;
				}
				float npcX = Main.npc[k].Center.X;
				float npcY = Main.npc[k].Center.Y;
				if (Math.Abs(player.Center.X - npcX) + Math.Abs(player.Center.Y - npcY) < maxNpcDistance)
				{
					if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
					{
						targetNpcIndex = k;
					}
					foundTarget = true;
					break;
				}
			}
			if (!foundTarget)
			{
				horizontalDistToPlayer -= (float)(40 * projectile.minionPos * player.direction);
			}
			if (foundTarget && targetNpcIndex >= 0)
			{
				projectile.ai[0] = 0f;
			}

			float verticalDistToPlayer = player.Center.Y - projectile.Center.Y;
			float distToPlayer = (player.Center - projectile.Center).Length();


			float speed = 0.4f;
			int closingDistance = 100;

			float num89 = 12f;
			if (num89 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				num89 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
			}

			if (distToPlayer < (float)closingDistance && player.velocity.Y == 0f && projectile.Bottom.Y <= player.Bottom.Y && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
			{
				projectile.ai[0] = 0f;
				if (projectile.velocity.Y < -6f)
				{
					projectile.velocity.Y = -6f;
				}
			}
			if (distToPlayer < 60f)
			{
				horizontalDistToPlayer = projectile.velocity.X;
				verticalDistToPlayer = projectile.velocity.Y;
			}
			else
			{
				distToPlayer = num89 / distToPlayer;
				horizontalDistToPlayer *= distToPlayer;
				verticalDistToPlayer *= distToPlayer;
			}


			if (projectile.velocity.X < horizontalDistToPlayer)
			{
				projectile.velocity.X += speed;
				if (projectile.velocity.X < 0f)
				{
					projectile.velocity.X += speed * 1.5f;
				}
			}
			if (projectile.velocity.X > horizontalDistToPlayer)
			{
				projectile.velocity.X -= speed;
				if (projectile.velocity.X > 0f)
				{
					projectile.velocity.X -= speed * 1.5f;
				}
			}
			if (projectile.velocity.Y < verticalDistToPlayer)
			{
				projectile.velocity.Y += speed;
				if (projectile.velocity.Y < 0f)
				{
					projectile.velocity.Y += speed * 1.5f;
				}
			}
			if (projectile.velocity.Y > verticalDistToPlayer)
			{
				projectile.velocity.Y -= speed;
				if (projectile.velocity.Y > 0f)
				{
					projectile.velocity.Y -= speed * 1.5f;
				}
			}

			if (projectile.frame < 12)
			{
				projectile.frame = Main.rand.Next(12, 18);
			}
			projectile.frameCounter = 0;

			if ((double)projectile.velocity.X > 0.5)
			{
				projectile.spriteDirection = -1;
			}
			else if ((double)projectile.velocity.X < -0.5)
			{
				projectile.spriteDirection = 1;
			}

			if (projectile.spriteDirection == -1)
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
			}
			else
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 3.14f;
			}
		}
		static void LegacyTargetEnemies(Projectile projectile, ref bool moveLeft, ref bool moveRight)
		{
			float minionOffset = 40 * projectile.minionPos;


			float posX = projectile.position.X;
			float posY = projectile.position.Y;
			float maxDistance = 100000f;
			float localMaxDistance = maxDistance;
			int priorityTargetIndex = -1;
			NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(projectile))
			{
				float targetCenterX = ownerMinionAttackTargetNPC.Center.X;
				float targetCenterY = ownerMinionAttackTargetNPC.Center.Y;
				float distanceToTarget = Math.Abs(projectile.Center.X - targetCenterX) + Math.Abs(projectile.Center.Y - targetCenterY);
				if (distanceToTarget < maxDistance)
				{
					if (priorityTargetIndex == -1 && distanceToTarget <= localMaxDistance)
					{
						localMaxDistance = distanceToTarget;
						posX = targetCenterX;
						posY = targetCenterY;
					}
					if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC.position, ownerMinionAttackTargetNPC.width, ownerMinionAttackTargetNPC.height))
					{
						maxDistance = distanceToTarget;
						posX = targetCenterX;
						posY = targetCenterY;
						priorityTargetIndex = ownerMinionAttackTargetNPC.whoAmI;
					}
				}
			}
			if (priorityTargetIndex == -1)
			{
				for (int num118 = 0; num118 < 200; num118++)
				{
					if (!Main.npc[num118].CanBeChasedBy(projectile))
					{
						continue;
					}
					float targetCenterX = Main.npc[num118].position.X + (float)(Main.npc[num118].width / 2);
					float targetCenterY = Main.npc[num118].position.Y + (float)(Main.npc[num118].height / 2);
					float distanceToTarget = Math.Abs(projectile.Center.X - targetCenterX) + Math.Abs(projectile.Center.Y - targetCenterY);
					if (distanceToTarget < maxDistance)
					{
						if (priorityTargetIndex == -1 && distanceToTarget <= localMaxDistance)
						{
							localMaxDistance = distanceToTarget;
							posX = targetCenterX;
							posY = targetCenterY;
						}
						if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num118].position, Main.npc[num118].width, Main.npc[num118].height))
						{
							maxDistance = distanceToTarget;
							posX = targetCenterX;
							posY = targetCenterY;
							priorityTargetIndex = num118;
						}
					}
				}
			}
			if (priorityTargetIndex == -1 && localMaxDistance < maxDistance)
			{
				maxDistance = localMaxDistance;
			}
			float rangeOffset = 400f;
			if ((double)projectile.position.Y > Main.worldSurface * 16.0)
			{
				rangeOffset = 200f;
			}
			if (maxDistance < rangeOffset + minionOffset && priorityTargetIndex == -1)
			{
				float horizontalDist = posX - (projectile.Center.X);
				if (horizontalDist < -5f)
				{
					moveLeft = true;
					moveRight = false;
				}
				else if (horizontalDist > 5f)
				{
					moveRight = true;
					moveLeft = false;
				}
			}
			else if (priorityTargetIndex >= 0 && maxDistance < 800f + minionOffset)
			{
				projectile.localAI[0] = attackRecencyCooldown;
				float horizontalDist = posX - (projectile.Center.X);
				if (horizontalDist > 450f || horizontalDist < -450f)
				{
					if (horizontalDist < -50f)
					{
						moveLeft = true;
						moveRight = false;
					}
					else if (horizontalDist > 50f)
					{
						moveRight = true;
						moveLeft = false;
					}
				}
				else if (projectile.owner == Main.myPlayer)
				{
					projectile.ai[1] = attackCooldown;
					Vector2 shootFrom = new Vector2(projectile.Center.X, projectile.Center.Y - 8f);
					float shootTowardX = posX - shootFrom.X + (float)Main.rand.Next(-20, 21);
					float minorYOffset = Math.Abs(shootTowardX) * 0.1f;
					minorYOffset *= (float)Main.rand.Next(0, 100) * 0.001f;
					float shootTowardY = posY - shootFrom.Y + (float)Main.rand.Next(-20, 21) - minorYOffset;
					float distance = (float)Math.Sqrt(shootTowardX * shootTowardX + shootTowardY * shootTowardY);
					distance = 18f / distance;
					shootTowardX *= distance;
					shootTowardY *= distance;
					int spearIndex = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), shootFrom, new Vector2(shootTowardX, shootTowardY), ProjectileID.PygmySpear, projectile.damage, projectile.knockBack, Main.myPlayer);
					Main.projectile[spearIndex].timeLeft = 300;
					if (shootTowardX < 0f)
					{
						projectile.direction = -1;
					}
					if (shootTowardX > 0f)
					{
						projectile.direction = 1;
					}
					projectile.netUpdate = true;
				}
			}
		}

		static void LegacyMovement(Projectile projectile, ref bool moveLeft, ref bool moveRight)
		{
			Player player = Main.player[projectile.owner];


			bool playerBelow = false;
			bool aboutToHitWall = false;

			//don't move if attacking
			if (projectile.ai[1] != 0f)
			{
				moveLeft = false;
				moveRight = false;
			}
			else if (projectile.localAI[0] == 0f)
			{
				projectile.direction = player.direction;
			}


			float maxVelocityX = 6f;
			float accX = 0.2f;
			if (maxVelocityX < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				maxVelocityX = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				accX = 0.3f;
			}

			if (moveLeft)
			{
				if ((double)projectile.velocity.X > -3.5)
				{
					projectile.velocity.X -= accX;
				}
				else
				{
					projectile.velocity.X -= accX * 0.25f;
				}
			}
			else if (moveRight)
			{
				if ((double)projectile.velocity.X < 3.5)
				{
					projectile.velocity.X += accX;
				}
				else
				{
					projectile.velocity.X += accX * 0.25f;
				}
			}
			else
			{
				projectile.velocity.X *= 0.9f;
				if (projectile.velocity.X >= 0f - accX && projectile.velocity.X <= accX)
				{
					projectile.velocity.X = 0f;
				}
			}

			if (moveLeft || moveRight)
			{
				int tilePosX = (int)(projectile.Center.X) / 16;
				int tilePosY = (int)(projectile.Center.Y) / 16;
				if (moveLeft)
				{
					tilePosX--;
				}
				if (moveRight)
				{
					tilePosX++;
				}
				tilePosX += (int)projectile.velocity.X;
				if (WorldGen.SolidTile(tilePosX, tilePosY))
				{
					aboutToHitWall = true;
				}
			}
			if (player.position.Y + (float)player.height - 8f > projectile.position.Y + (float)projectile.height)
			{
				playerBelow = true;
			}

			Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
			if (projectile.velocity.Y == 0f)
			{
				/*
				if (!playerBelow && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
				{
					int num158 = (int)(projectile.Center.X) / 16;
					int j3 = (int)(projectile.Center.Y) / 16 + 1;
					if (moveLeft)
					{
						num158--;
					}
					if (moveRight)
					{
						num158++;
					}
					WorldGen.SolidTile(num158, j3);
				}
				*/
				if (aboutToHitWall)
				{
					int tilePosX = (int)(projectile.Center.X) / 16;
					int tilePosY = (int)(projectile.Bottom.Y) / 16;
					if (WorldGen.SolidTileAllowBottomSlope(tilePosX, tilePosY) || Main.tile[tilePosX, tilePosY].IsHalfBlock || Main.tile[tilePosX, tilePosY].Slope > 0)
					{

						try
						{
							tilePosX = (int)(projectile.Center.X) / 16;
							tilePosY = (int)(projectile.Center.Y) / 16;
							if (moveLeft)
							{
								tilePosX--;
							}
							if (moveRight)
							{
								tilePosX++;
							}
							tilePosX += (int)projectile.velocity.X;
							if (!WorldGen.SolidTile(tilePosX, tilePosY - 1) && !WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								projectile.velocity.Y = -5.1f;
							}
							else if (!WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								projectile.velocity.Y = -7.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 5))
							{
								projectile.velocity.Y = -11.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 4))
							{
								projectile.velocity.Y = -10.1f;
							}
							else
							{
								projectile.velocity.Y = -9.1f;
							}
						}
						catch
						{
							projectile.velocity.Y = -9.1f;
						}
					}
				}
			}
			if (projectile.velocity.X > maxVelocityX)
			{
				projectile.velocity.X = maxVelocityX;
			}
			if (projectile.velocity.X < 0f - maxVelocityX)
			{
				projectile.velocity.X = 0f - maxVelocityX;
			}
			if (projectile.velocity.X < 0f)
			{
				projectile.direction = -1;
			}
			if (projectile.velocity.X > 0f)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X > accX && moveRight)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X < 0f - accX && moveLeft)
			{
				projectile.direction = -1;
			}
			if (projectile.direction != 0)
			{
				projectile.spriteDirection = -projectile.direction;
			}

			bool notWalking = projectile.position.X - projectile.oldPosition.X == 0f;
			if (projectile.ai[1] > 0f)
			{
				if (projectile.localAI[1] == 0f)
				{
					projectile.localAI[1] = 1f;
					projectile.frame = 1;
				}
				if (projectile.frame != 0)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 4)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame >= 4)
					{
						projectile.frame = 0;
					}
				}
			}
			else if (projectile.velocity.Y == 0f)
			{
				projectile.localAI[1] = 0f;
				if (notWalking)
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
				else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
				{
					projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
					projectile.frameCounter++;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 5)
					{
						projectile.frame = 5;
					}
					if (projectile.frame >= 11)
					{
						projectile.frame = 5;
					}
				}
				else
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
			}
			else if (projectile.velocity.Y < 0f)
			{
				projectile.frameCounter = 0;
				projectile.frame = 4;
			}
			else if (projectile.velocity.Y > 0f)
			{
				projectile.frameCounter = 0;
				projectile.frame = 4;
			}
			projectile.velocity.Y += 0.4f;
			if (projectile.velocity.Y > 10f)
			{
				projectile.velocity.Y = 10f;
			}
		}
	}
}
