using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.ExtraJumps;

namespace TRAEProject.Changes.NPCs.Miniboss.Santa
{
    public class Santa : GlobalNPC
    {
		Vector2 savedShootSpot = Vector2.Zero;
		public override bool InstancePerEntity => true;
		public override bool PreAI(NPC npc)
        {
			if (npc.type == NPCID.SantaNK1)
			{
				float num957 = 2f;
				npc.noGravity = true;
				npc.noTileCollide = true;
				if (!Main.dayTime)
				{
					npc.TargetClosest(npc.ai[0] != 1f);
				}
				bool flag52 = false;
				if ((double)npc.life < (double)npc.lifeMax * 0.75)
				{
					num957 = 3f;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5)
				{
					num957 = 4f;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.25)
				{
					num957 = 5f;
				}
				Vector2 center4 = npc.Center;
				Point point5 = center4.ToTileCoordinates();
				if (WorldGen.InWorld(point5.X, point5.Y) && !WorldGen.SolidTile(point5.X, point5.Y))
				{
					Lighting.AddLight(center4, 0.3f, 0.26f, 0.05f);
				}
				if (Main.dayTime)
				{
					npc.EncourageDespawn(10);
					num957 = 8f;
					if (npc.velocity.X == 0f)
					{
						npc.velocity.X = 0.1f;
					}
				}
				else if (npc.ai[0] == 0f)
				{
					npc.ai[1] += 1f;
					if (npc.ai[1] >= 300f && Main.netMode != 1)
					{
						npc.TargetClosest(npc.ai[0] != 1f);
						npc.ai[1] = 0f;
						npc.ai[0] = 1f;
						npc.netUpdate = true;
					}
				}
				else if (npc.ai[0] == 1f)
				{
					npc.ai[1] += 1f;
					flag52 = true;
					int num958 = 4;
					float delay = 30f;
					if (npc.ai[1] < delay)
                    {
						savedShootSpot = Main.player[npc.target].Center;
						Projectile.NewProjectile(npc.GetSource_FromAI(), savedShootSpot, Vector2.UnitX.RotatedBy((savedShootSpot - (new Vector2(npc.Center.X + (float)(npc.direction * 50), npc.Center.Y + 25))).ToRotation()), ModContent.ProjectileType<Target>(), 0, 0);
					}
					
					if (npc.ai[1] > delay && npc.ai[1] % num958 == 0f && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int shoot = ProjectileID.BulletDeadeye;
						if(npc.ai[1] <= delay)
                        {
							shoot = ModContent.ProjectileType<Scanline>();
                        }
						Vector2 vector116 = new Vector2(npc.Center.X + (npc.direction * 50), npc.Center.Y + Main.rand.Next(35, 90));
						float num959 = savedShootSpot.X - vector116.X + Main.rand.Next(-40, 41);
						float num960 = savedShootSpot.Y - vector116.Y + Main.rand.Next(-40, 41);
						num959 += Main.rand.Next(-40, 41);
						num960 += Main.rand.Next(-40, 41);
						float num961 = MathF.Sqrt(num959 * num959 + num960 * num960);
						float num962 = 15f;
						num961 = num962 / num961;
						num959 *= num961;
						num960 *= num961;
						//num959 *= 1f + (float)Main.rand.Next(-20, 21) * 0.015f;
						//num960 *= 1f + (float)Main.rand.Next(-20, 21) * 0.015f;
						Projectile.NewProjectile(npc.GetSource_FromAI(), vector116.X, vector116.Y, num959, num960, shoot, 36, 0f, Main.myPlayer);
					}
					if (npc.ai[1] > 90f + delay)
					{
						npc.ai[0] = 0f;
						npc.ai[1] = 0f;
					}
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int num964 = 600;
					int num965 = 1200;
					int num966 = 2700;
					if ((double)npc.life < (double)npc.lifeMax * 0.25)
					{
						num964 = (int)((double)num964 * 0.5);
						num965 = (int)((double)num965 * 0.5);
						num966 = (int)((double)num966 * 0.5);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.5)
					{
						num964 = (int)((double)num964 * 0.75);
						num965 = (int)((double)num965 * 0.75);
						num966 = (int)((double)num966 * 0.75);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.75)
					{
						num964 = (int)((double)num964 * 0.9);
						num965 = (int)((double)num965 * 0.9);
						num966 = (int)((double)num966 * 0.9);
					}
					/*
					if (Main.rand.Next(num964) == 0)
					{
						Vector2 vector117 = new Vector2(npc.Center.X - (float)(npc.direction * 24), npc.Center.Y - 64f);
						float num967 = Main.rand.Next(1, 100) * npc.direction;
						float num968 = 1f;
						float num969 = MathF.Sqrt(num967 * num967 + num968 * num968);
						float num970 = 1f;
						num969 = num970 / num969;
						num967 *= num969;
						num968 *= num969;
						int num971 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector117.X, vector117.Y, num967, num968, ProjectileID.Spike, 80, 0f, Main.myPlayer);
					}
					*/
					if (Main.rand.Next(num965) == 0)
					{
						npc.localAI[1] = 1f;
					}
					if (npc.localAI[1] >= 1f)
					{
						npc.localAI[1] += 1f;
						int num972 = 12;
						if (npc.localAI[1] % (float)num972 == 0f)
						{
							Vector2 vector118 = new Vector2(npc.Center.X - (float)(npc.direction * 24), npc.Center.Y - 64f);
							float num973 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector118.X;
							float num974 = Main.player[npc.target].Center.Y - vector118.Y;
							num973 += (float)Main.rand.Next(-50, 51);
							num974 += (float)Main.rand.Next(-50, 51);
							float num975 = MathF.Sqrt(num973 * num973 + num974 * num974);
							float num976 = 12.5f;
							num975 = num976 / num975;
							num973 *= num975;
							num974 *= num975;
							num973 *= 1f + (float)Main.rand.Next(-20, 21) * 0.015f;
							num974 *= 1f + (float)Main.rand.Next(-20, 21) * 0.015f;
							int num977 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector118.X, vector118.Y, num973, num974, 350, 42, 0f, Main.myPlayer);
						}
						if (npc.localAI[1] >= 100f)
						{
							npc.localAI[1] = 0f;
						}
					}
					if (Main.rand.Next(num966) == 0)
					{
						npc.localAI[2] = 2f;
					}
					if (npc.localAI[2] > 0f)
					{
						npc.localAI[2] += 1f;
						int num978 = 9;
						if (npc.localAI[2] % (float)num978 == 0f)
						{
							Vector2 vector119 = new Vector2(npc.Center.X - (float)(npc.direction * 24), npc.Center.Y - 64f);
							float num979 = Main.rand.Next(-100, 101);
							float num980 = -300f;
							float num981 = MathF.Sqrt(num979 * num979 + num980 * num980);
							float num982 = 11f;
							num981 = num982 / num981;
							num979 *= num981;
							num980 *= num981;
							num979 *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
							num980 *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
							int num983 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector119.X, vector119.Y, num979, num980, 351, 50, 0f, Main.myPlayer);
						}
						if (npc.localAI[2] >= 100f)
						{
							npc.localAI[2] = 0f;
						}
					}
				}
				if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < 50f)
				{
					flag52 = true;
				}
				if (flag52)
				{
					npc.velocity.X *= 0.9f;
					if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
					{
						npc.velocity.X = 0f;
					}
				}
				else
				{
					if (npc.direction > 0)
					{
						npc.velocity.X = (npc.velocity.X * 20f + num957) / 21f;
					}
					if (npc.direction < 0)
					{
						npc.velocity.X = (npc.velocity.X * 20f - num957) / 21f;
					}
				}
				int num984 = 80;
				int num985 = 20;
				Vector2 position6 = new Vector2(npc.Center.X - (float)(num984 / 2), npc.position.Y + (float)npc.height - (float)num985);
				bool flag53 = false;
				if (npc.position.X < Main.player[npc.target].position.X && npc.position.X + (float)npc.width > Main.player[npc.target].position.X + (float)Main.player[npc.target].width && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y + (float)Main.player[npc.target].height - 16f)
				{
					flag53 = true;
				}
				if (flag53)
				{
					npc.velocity.Y += 0.5f;
				}
				else if (Collision.SolidCollision(position6, num984, num985))
				{
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = 0f;
					}
					if ((double)npc.velocity.Y > -0.2)
					{
						npc.velocity.Y -= 0.025f;
					}
					else
					{
						npc.velocity.Y -= 0.2f;
					}
					if (npc.velocity.Y < -4f)
					{
						npc.velocity.Y = -4f;
					}
				}
				else
				{
					if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y = 0f;
					}
					if ((double)npc.velocity.Y < 0.1)
					{
						npc.velocity.Y += 0.025f;
					}
					else
					{
						npc.velocity.Y += 0.5f;
					}
				}
				if (npc.velocity.Y > 10f)
				{
					npc.velocity.Y = 10f;
				}

				return false;
			}
            return base.PreAI(npc);
        }

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.SantaNK1)
			{
				//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Booster>(), 10, 1, 1));
			}
			base.ModifyNPCLoot(npc, npcLoot);
		}
	}
}
