using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Miniboss.IceQueen
{
	public class IceQueen : GlobalNPC
	{
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.IceQueen)
            {
				npc.lifeMax = 30000; // down from 34k
            }				
        }
        public override bool PreAI(NPC npc)
		{
			if (npc.type == NPCID.IceQueen)
			{

				if (Main.dayTime)
				{
					if (npc.velocity.X > 0f)
					{
						npc.velocity.X += 0.25f;
					}
					else
					{
						npc.velocity.X -= 0.25f;
					}
					npc.velocity.Y -= 0.1f;
					npc.rotation = npc.velocity.X * 0.05f;
				}
				else if (npc.ai[0] == 0f)
				{
					if (npc.ai[2] == 0f)
					{
						npc.TargetClosest();
						if (npc.Center.X < Main.player[npc.target].Center.X)
						{
							npc.ai[2] = 1f;
						}
						else
						{
							npc.ai[2] = -1f;
						}
					}
					npc.TargetClosest();
					int num933 = 800;
					float num934 = Math.Abs(npc.Center.X - Main.player[npc.target].Center.X);
					if (npc.Center.X < Main.player[npc.target].Center.X && npc.ai[2] < 0f && num934 > (float)num933)
					{
						npc.ai[2] = 0f;
					}
					if (npc.Center.X > Main.player[npc.target].Center.X && npc.ai[2] > 0f && num934 > (float)num933)
					{
						npc.ai[2] = 0f;
					}
					float num935 = 0.45f;
					float num936 = 7f;
					if ((double)npc.life < (double)npc.lifeMax * 0.75)
					{
						num935 = 0.55f;
						num936 = 8f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.5)
					{
						num935 = 0.7f;
						num936 = 10f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.25)
					{
						num935 = 0.8f;
						num936 = 11f;
					}
					npc.velocity.X += npc.ai[2] * num935;
					if (npc.velocity.X > num936)
					{
						npc.velocity.X = num936;
					}
					if (npc.velocity.X < 0f - num936)
					{
						npc.velocity.X = 0f - num936;
					}
					float num937 = Main.player[npc.target].position.Y - (npc.position.Y + (float)npc.height);
					if (num937 < 150f)
					{
						npc.velocity.Y -= 0.2f;
					}
					if (num937 > 200f)
					{
						npc.velocity.Y += 0.2f;
					}
					if (npc.velocity.Y > 8f)
					{
						npc.velocity.Y = 8f;
					}
					if (npc.velocity.Y < -8f)
					{
						npc.velocity.Y = -8f;
					}
					npc.rotation = npc.velocity.X * 0.05f;
					if ((num934 < 500f || npc.ai[3] < 0f) && npc.position.Y < Main.player[npc.target].position.Y)
					{
						npc.ai[3] += 1f;
						int num938 = 13;
						if ((double)npc.life < (double)npc.lifeMax * 0.75)
						{
							num938 = 12;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.5)
						{
							num938 = 11;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.25)
						{
							num938 = 10;
						}
						num938++;
						if (npc.ai[3] > (float)num938)
						{
							npc.ai[3] = -num938;
						}
						if (npc.ai[3] == 0f && Main.netMode != 1)
						{
							Vector2 vector113 = new Vector2(npc.Center.X, npc.Center.Y);
							vector113.X += npc.velocity.X * 7f;
							float num939 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector113.X;
							float num940 = Main.player[npc.target].Center.Y - vector113.Y;
							float num941 = MathF.Sqrt(num939 * num939 + num940 * num940);
							float num942 = 6f;
							if ((double)npc.life < (double)npc.lifeMax * 0.75)
							{
								num942 = 7f;
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.5)
							{
								num942 = 8f;
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.25)
							{
								num942 = 9f;
							}
							num941 = num942 / num941;
							num939 *= num941;
							num940 *= num941;
							int num943 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector113.X, vector113.Y, num939, num940, 348, 42, 0f, Main.myPlayer);
						}
					}
					else if (npc.ai[3] < 0f)
					{
						npc.ai[3] += 1f;
					}
					if (Main.netMode != 1)
					{
						npc.ai[1] += Main.rand.Next(1, 4);
						if (npc.ai[1] > 800f && num934 < 600f)
						{
							npc.ai[0] = -1f;
						}
					}
				}
				else if (npc.ai[0] == 1f)
				{
					npc.TargetClosest();
					float num944 = 0.15f;
					float num945 = 7f;
					if ((double)npc.life < (double)npc.lifeMax * 0.75)
					{
						num944 = 0.17f;
						num945 = 8f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.5)
					{
						num944 = 0.2f;
						num945 = 9f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.25)
					{
						num944 = 0.25f;
						num945 = 10f;
					}
					num944 -= 0.05f;
					num945 -= 1f;
					if (npc.Center.X < Main.player[npc.target].Center.X)
					{
						npc.velocity.X += num944;
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X *= 0.98f;
						}
					}
					if (npc.Center.X > Main.player[npc.target].Center.X)
					{
						npc.velocity.X -= num944;
						if (npc.velocity.X > 0f)
						{
							npc.velocity.X *= 0.98f;
						}
					}
					if (npc.velocity.X > num945 || npc.velocity.X < 0f - num945)
					{
						npc.velocity.X *= 0.95f;
					}
					float num946 = Main.player[npc.target].position.Y - (npc.position.Y + (float)npc.height);
					if (num946 < 180f)
					{
						npc.velocity.Y -= 0.1f;
					}
					if (num946 > 200f)
					{
						npc.velocity.Y += 0.1f;
					}
					if (npc.velocity.Y > 6f)
					{
						npc.velocity.Y = 6f;
					}
					if (npc.velocity.Y < -6f)
					{
						npc.velocity.Y = -6f;
					}
					npc.rotation = npc.velocity.X * 0.01f;
					if (Main.netMode != 1)
					{
						npc.ai[3] += 1f;
						int num947 = 15;
						if ((double)npc.life < (double)npc.lifeMax * 0.75)
						{
							num947 = 14;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.5)
						{
							num947 = 12;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.25)
						{
							num947 = 10;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.1)
						{
							num947 = 8;
						}
						num947 += 3;
						if (npc.ai[3] >= (float)num947)
						{
							npc.ai[3] = 0f;
							Vector2 vector114 = new Vector2(npc.Center.X, npc.position.Y + (float)npc.height - 14f);
							int i2 = (int)(vector114.X / 16f);
							int j2 = (int)(vector114.Y / 16f);
							if (!WorldGen.SolidTile(i2, j2))
							{
								float num948 = npc.velocity.Y;
								if (num948 < 0f)
								{
									num948 = 0f;
								}
								num948 += 3f;
								float speedX2 = npc.velocity.X * 0.25f;
								int num949 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector114.X, vector114.Y, speedX2, num948, 349, 37, 0f, Main.myPlayer, Main.rand.Next(5));
							}
						}
					}
					if (Main.netMode != 1)
					{
						npc.ai[1] += Main.rand.Next(1, 4);
						if (npc.ai[1] > 600f)
						{
							npc.ai[0] = -1f;
						}
					}
				}
				else if (npc.ai[0] == 2f)
				{
					npc.TargetClosest();
					Vector2 vector115 = new Vector2(npc.Center.X, npc.Center.Y - 20f);
					Vector2 shootOut = TRAEMethods.PolarVector(500, npc.ai[1] * (MathF.PI / 6f + (0.05f)));
					float num950 = shootOut.X;
					float num951 = shootOut.Y;
					float num952 = MathF.Sqrt(num950 * num950 + num951 * num951);
					float num953 = 15f;
					npc.velocity *= 0.95f;
					num952 = num953 / num952;
					num950 *= num952;
					num951 *= num952;
					npc.rotation += 0.2f;
					vector115.X += num950 * 4f;
					vector115.Y += num951 * 4f;
					npc.ai[3] += 1f;
					int num954 = 1;
					if (npc.ai[3] > (float)num954)
					{
						npc.ai[3] = 0f;
						int num955 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector115.X, vector115.Y, num950, num951, ProjectileID.FrostShard, 35, 0f, Main.myPlayer);
					}
					if (Main.netMode != 1)
					{
						npc.ai[1]++;
						if (npc.ai[1] > 250f)
						{
							npc.ai[0] = -1f;
						}
					}
				}
				if (npc.ai[0] == -1f)
				{
					int num956 = Main.rand.Next(3);
					npc.TargetClosest();
					if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 1000f)
					{
						num956 = 0;
					}
					npc.ai[0] = num956;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
				}
				return false;
			}
			return base.PreAI(npc);
		}
	}
}
