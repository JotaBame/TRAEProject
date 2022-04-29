using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using TRAEProject.NewContent.Buffs;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Weapon.Summon.Minions
{
	public class OpticStaff : GlobalProjectile
	{
		NPC target;
		public override bool InstancePerEntity => true;
		public override bool PreAI(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			if (projectile.type == 387)
			{
				float num540 = 0f;
				float num541 = 0f;
				float num542 = 0f;
				float num543 = 0f;
				if (projectile.type == 387)
				{
					num540 = 2000f;
					num541 = 800f;
					num542 = 1200f;
					num543 = 150f;
					if (player.dead)
					{
						player.twinsMinion = false;
					}
					if (player.twinsMinion)
					{
						projectile.timeLeft = 2;
					}
				}

				float num544 = 0.05f;
				for (int num545 = 0; num545 < 1000; num545++)
				{
					bool flag22 = (Main.projectile[num545].type == 387) && (projectile.type == 387);
					if (!flag22)
					{
						flag22 = projectile.type == 533 && Main.projectile[num545].type == 533;
					}
					if (num545 != projectile.whoAmI && Main.projectile[num545].active && Main.projectile[num545].owner == projectile.owner && flag22 && Math.Abs(projectile.position.X - Main.projectile[num545].position.X) + Math.Abs(projectile.position.Y - Main.projectile[num545].position.Y) < (float)projectile.width)
					{
						if (projectile.position.X < Main.projectile[num545].position.X)
						{
							projectile.velocity.X -= num544;
						}
						else
						{
							projectile.velocity.X += num544;
						}
						if (projectile.position.Y < Main.projectile[num545].position.Y)
						{
							projectile.velocity.Y -= num544;
						}
						else
						{
							projectile.velocity.Y += num544;
						}
					}
				}
				bool flag23 = false;
				
				if (flag23)
				{
					return false;
				}
				Vector2 vector30 = projectile.position;
				bool flag24 = false;
				if (projectile.ai[0] != 1f && (projectile.type == 387))
				{
					projectile.tileCollide = true;
				}
				if (projectile.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16)))
				{
					projectile.tileCollide = false;
				}
				NPC ownerMinionAttackTargetNPC3 = projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC3 != null && ownerMinionAttackTargetNPC3.CanBeChasedBy(projectile))
				{
					float num552 = Vector2.Distance(ownerMinionAttackTargetNPC3.Center, projectile.Center);
					float num553 = num540 * 3f;
					if (num552 < num553 && !flag24 && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC3.position, ownerMinionAttackTargetNPC3.width, ownerMinionAttackTargetNPC3.height))
					{
						num540 = num552;
						vector30 = ownerMinionAttackTargetNPC3.Center;
						flag24 = true;
					}
				}
				if (!flag24)
				{
					for (int num554 = 0; num554 < 200; num554++)
					{
						NPC nPC2 = Main.npc[num554];
						if (nPC2.CanBeChasedBy(projectile))
						{
							float num555 = Vector2.Distance(nPC2.Center, projectile.Center);
							if (!(num555 >= num540) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC2.position, nPC2.width, nPC2.height))
							{
								num540 = num555;
								vector30 = nPC2.Center;
								flag24 = true;
							}
						}
					}
				}
				float num556 = num541;
				if (flag24)
				{
					num556 = num542;
				}
				Player player4 = Main.player[projectile.owner];
				if (Vector2.Distance(player4.Center, projectile.Center) > num556)
				{
					if (projectile.type == 387)
					{
						projectile.ai[0] = 1f;
					}
					projectile.tileCollide = false;
					projectile.netUpdate = true;
				}
				if ((projectile.type == 387) && flag24 && projectile.ai[0] == 0f)
				{
					Vector2 value14 = vector30 - projectile.Center;
					float num557 = value14.Length();
					value14.Normalize();
					if (num557 > 200f)
					{
						float num558 = 6f;
						value14 *= num558;
						projectile.velocity = (projectile.velocity * 40f + value14) / 41f;
					}
					else
					{
						float num559 = 4f;
						value14 *= 0f - num559;
						projectile.velocity = (projectile.velocity * 40f + value14) / 41f;
					}
				}
				else
				{
					bool flag25 = false;
					if (!flag25)
					{
						flag25 = projectile.ai[0] == 1f && (projectile.type == 387);
					}
					float num560 = 6f;
					if (flag25)
					{
						num560 = 15f;
					}
					Vector2 center3 = projectile.Center;
					Vector2 vector31 = player4.Center - center3 + new Vector2(0f, -60f);
					float num561 = vector31.Length();
					float num562 = num561;
					if (num561 > 200f && num560 < 8f)
					{
						num560 = 8f;
					}
					if (num561 < num543 && flag25 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
					{
						if (projectile.type == 387)
						{
							projectile.ai[0] = 0f;
						}
						projectile.netUpdate = true;
					}
					if (num561 > 2000f)
					{
						projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
						projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.height / 2);
						projectile.netUpdate = true;
					}
					if (num561 > 70f)
					{
						Vector2 vector32 = vector31;
						vector31.Normalize();
						vector31 *= num560;
						projectile.velocity = (projectile.velocity * 40f + vector31) / 41f;
					}
					else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
					{
						projectile.velocity.X = -0.15f;
						projectile.velocity.Y = -0.05f;
					}
				}

				if (projectile.type == 387)
				{
					if (flag24)
					{
						projectile.rotation = (vector30 - projectile.Center).ToRotation() + (float)Math.PI;
					}
					else
					{
						projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI;
					}
				}
				if (projectile.type == 387)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 3)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 2)
					{
						projectile.frame = 0;
					}
				}

				if (projectile.ai[1] > 0f && (projectile.type == 387))
				{
					projectile.ai[1] += Main.rand.Next(1, 4);
				}
				if (projectile.ai[1] > 105f && projectile.type == 387)
				{
					projectile.ai[1] = 1f;
					projectile.netUpdate = true;
					if (flag24)
					{
						if (TRAEMethods.ClosestNPC(ref target, 500 * 16, player.Center, false, player.MinionAttackTargetNPC))
						{
							if (Main.myPlayer == projectile.owner)
							{
								float shootSpeed = 20;
								float calculatedShootAngle = TRAEMethods.PredictiveAim(projectile.Center, shootSpeed, target.Center, target.velocity, out _);
								if (!float.IsNaN(calculatedShootAngle))
								{
									Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, TRAEMethods.PolarVector(shootSpeed, calculatedShootAngle), ProjectileID.MiniRetinaLaser, (int)(projectile.damage * 0.8f), projectile.knockBack, projectile.owner);
								}
							}
							projectile.netUpdate = true;
						}
					}


				}
				else
				{
					int num571 = 0;
					switch ((int)projectile.ai[0])
					{
						case 0:
						case 3:
						case 6:
							num571 = 400;
							break;
						case 1:
						case 4:
						case 7:
							num571 = 400;
							break;
						case 2:
						case 5:
						case 8:
							num571 = 600;
							break;
					}
					if (!(projectile.ai[1] == 0f && flag24) || !(num540 < (float)num571))
					{
						return false;
					}
					projectile.ai[1]++;
					if (Main.myPlayer != projectile.owner)
					{
						return false;
					}
					if (projectile.localAI[0] >= 3f)
					{
						projectile.ai[0] += 4f;
						if (projectile.ai[0] == 6f)
						{
							projectile.ai[0] = 3f;
						}
						projectile.localAI[0] = 0f;
					}
					else
					{
						projectile.ai[0] += 6f;
						Vector2 value17 = vector30 - projectile.Center;
						value17.Normalize();
						float scaleFactor = ((projectile.ai[0] == 8f) ? 12f : 10f);
						projectile.velocity = value17 * scaleFactor;
						projectile.netUpdate = true;
					}
				}
				return false;
			}
			return true;
		}
	}
}