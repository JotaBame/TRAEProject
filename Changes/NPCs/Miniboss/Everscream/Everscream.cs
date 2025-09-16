using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Miniboss.Everscream
{
    public class Everscream : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.Everscream)
            {
				float num868 = 2f;
				npc.noGravity = true;
				npc.noTileCollide = true;
				if (!Main.dayTime)
				{
					npc.TargetClosest();
				}
				bool flag50 = false;
				if ((double)npc.life < (double)npc.lifeMax * 0.75)
				{
					num868 = 3f;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5)
				{
					num868 = 4f;
				}
				if (npc.type == 344)
				{
					Lighting.AddLight(npc.Bottom + new Vector2(0f, -30f), 0.3f, 0.16f, 0.125f);
				}
				if (npc.type == 325)
				{
					Lighting.AddLight(npc.Bottom + new Vector2(0f, -30f), 0.3f, 0.125f, 0.06f);
				}
				if (Main.dayTime)
				{
					npc.EncourageDespawn(10);
					num868 = 8f;
				}
				else if (npc.ai[0] == 0f)
				{
					npc.ai[1] += 1f;
					if ((double)npc.life < (double)npc.lifeMax * 0.5)
					{
						npc.ai[1] += 1f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.25)
					{
						npc.ai[1] += 1f;
					}
					if (npc.ai[1] >= 300f && Main.netMode != 1)
					{
						npc.ai[1] = 0f;

						npc.ai[0] = Main.rand.Next(1, 3);

						npc.netUpdate = true;
					}
				}
				else if (npc.ai[0] == 1f)
				{
					if (Main.rand.Next(5) == 0)
					{
						int num869 = Dust.NewDust(npc.position + Main.rand.NextVector2Square(0f, 1f) * npc.Size - new Vector2(1f, 2f), 10, 14, DustID.SilverCoin, 0f, 0f, 254, Color.Transparent, 0.25f);
						Dust dust = Main.dust[num869];
						dust.velocity *= 0.2f;
					}
					flag50 = true;
					npc.ai[1] += 1f;
					if (npc.ai[1] % 5f == 0f)
					{
						Vector2 vector100 = new Vector2(npc.position.X + 20f + (float)Main.rand.Next(npc.width - 40), npc.position.Y + 20f + (float)Main.rand.Next(npc.height - 40));
						float num870 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector100.X;
						float num871 = Main.player[npc.target].position.Y - vector100.Y;
						num870 += (float)Main.rand.Next(-50, 51);
						num871 += (float)Main.rand.Next(-50, 51);
						num871 -= Math.Abs(num870) * ((float)Main.rand.Next(0, 21) * 0.01f);
						float num872 = MathF.Sqrt(num870 * num870 + num871 * num871);
						float num873 = 12.5f;
						num872 = num873 / num872;
						num870 *= num872;
						num871 *= num872;
						num870 *= 1f + (float)Main.rand.Next(-20, 21) * 0.02f;
						num871 *= 1f + (float)Main.rand.Next(-20, 21) * 0.02f;
						int num874 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector100.X, vector100.Y, num870, num871, ProjectileID.PineNeedleHostile, 43, 0f, Main.myPlayer, Main.rand.Next(0, 31));
					}
					if (npc.ai[1] >= 180f)
					{
						npc.ai[1] = 0f;
						npc.ai[0] = 0f;
					}
					if (npc.ai[1] >= 120f)
					{
						npc.ai[1] = 0f;
						npc.ai[0] = 0f;
					}
				}
				else if (npc.ai[0] == 2f)
				{
					flag50 = true;
					npc.ai[1] += 1f;
					if (npc.ai[1] >= 60f)
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Top, new (0, -6), ModContent.ProjectileType<Everstar>(), 50, 0);
                        npc.ai[1] = 0f;
                        npc.ai[0] = 0f;

                    }
				}
				if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < 50f)
				{
					flag50 = true;
				}
				if (flag50)
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
						npc.velocity.X = (npc.velocity.X * 20f + num868) / 21f;
					}
					if (npc.direction < 0)
					{
						npc.velocity.X = (npc.velocity.X * 20f - num868) / 21f;
					}
				}
				int num900 = 80;
				int num901 = 20;
				Vector2 position5 = new Vector2(npc.Center.X - (float)(num900 / 2), npc.position.Y + (float)npc.height - (float)num901);
				bool flag51 = false;
				if (npc.position.X < Main.player[npc.target].position.X && npc.position.X + (float)npc.width > Main.player[npc.target].position.X + (float)Main.player[npc.target].width && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y + (float)Main.player[npc.target].height - 16f)
				{
					flag51 = true;
				}
				if (flag51)
				{
					npc.velocity.Y += 0.5f;
				}
				else if (Collision.SolidCollision(position5, num900, num901))
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
    }
}
