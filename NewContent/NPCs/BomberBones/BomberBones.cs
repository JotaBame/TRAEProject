using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Banners;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.NPCs.BomberBones
{
    public class BomberBonesNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bomber Bones");
            Main.npcFrameCount[NPC.type] = 20;
	NPC.buffImmune[BuffID.Poisoned] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 46;
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.defense = 16;
            NPC.lifeMax = 180;
            NPC.value = 750;
            AnimationType = NPCID.SkeletonCommando;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.knockBackResist = 0.4f;
		
            NPC.rarity = 1;
            NPC.scale = 1.05f;;
            Banner = NPC.type;
            BannerItem = ItemType<BomberBonesBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("The backbone of the Dungeon's armada, especialized in making sure no intruders leave the place in one piece. Literally. Part of a military unit in their past lives.")
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.GrenadeLauncher, 12));
            npcLoot.Add(ItemDropRule.Common(ItemID.RocketI, 2, 33, 100));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDungeon)
            {
                return 0.1f;
            }
            return 0f;
        }

        public override void AI()
		{
			if (Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height == NPC.position.Y + (float)NPC.height)
			{
				NPC.directionY = -1;
			}
			bool flag = false;
			bool flag5 = false;
			bool flag6 = false;
			if (NPC.velocity.X == 0f)
			{
				flag6 = true;
			}
			if (NPC.justHit)
			{
				flag6 = false;
			}
			int num55 = 60;
			bool flag7 = false;
			//bool flag8 = true;
			//if (KEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEP5)
			//{
			bool flag8 = false;
			//}
			bool flag9 = false;
			bool flag10 = true;
			if (NPC.ai[2] > 0f)
			{
				flag10 = false;
			}
			if (!flag9 && flag10)
				{
					if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
					{
						flag7 = true;
					}
					if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num55 || flag7)
					{
						NPC.ai[3] += 1f;
					}
					else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
					{
						NPC.ai[3] -= 1f;
					}
					if (NPC.ai[3] > (float)(num55 * 10))
					{
						NPC.ai[3] = 0f;
					}
					if (NPC.justHit)
					{
						NPC.ai[3] = 0f;
					}
					if (NPC.ai[3] == (float)num55)
					{
						NPC.netUpdate = true;
					}
					if (Main.player[NPC.target].Hitbox.Intersects(NPC.Hitbox))
					{
						NPC.ai[3] = 0f;
					}
				}
			if (NPC.ai[3] < (float)num55 && NPC.DespawnEncouragement_AIStyle3_Fighters_NotDiscouraged(NPC.type, NPC.position, NPC))
			{
				if (Main.rand.NextBool(1000))
				{
					SoundEngine.PlaySound(SoundID.ZombieMoan with { MaxInstances = 0 }, NPC.Center);
				}
				NPC.TargetClosest();
				if (NPC.directionY > 0 && Main.player[NPC.target].Center.Y <= NPC.Bottom.Y)
				{
					NPC.directionY = -1;
				}
			}
			else if (!(NPC.ai[2] > 0f) || !NPC.DespawnEncouragement_AIStyle3_Fighters_CanBeBusyWithAction(NPC.type))
			{
				if (Main.dayTime && (double)(NPC.position.Y / 16f) < Main.worldSurface)
				{
					NPC.EncourageDespawn(10);
				}
				if (NPC.velocity.X == 0f)
				{
					if (NPC.velocity.Y == 0f)
					{
						NPC.ai[0] += 1f;
						if (NPC.ai[0] >= 2f)
						{
							NPC.direction *= -1;
							NPC.spriteDirection = NPC.direction;
							NPC.ai[0] = 0f;
						}
					}
				}
				else
				{
					NPC.ai[0] = 0f;
				}
				if (NPC.direction == 0)
				{
					NPC.direction = 1;
				}
			}


			bool flag16 = true;
			int num141 = -1;
			int num142 = -1;
			if (NPC.ai[1] > 0f)
			{
				NPC.ai[1] -= 1f;
			}
			if (NPC.justHit)
			{
				NPC.ai[1] -= 10f;
				NPC.ai[2] = 0f;
			}
			int AttackTimer = 60;

			int EffectiveattackTimer = AttackTimer / 2;
			if (NPC.confused)
			{
				NPC.ai[2] = 0f;
			}
			if (NPC.ai[2] > 0f)
			{
				if (flag16)
				{
					NPC.TargetClosest();
				}
				if (NPC.ai[1] == (float)EffectiveattackTimer)
				{
					NPC.velocity.X = 0;
					float num145 = 10f;

					Vector2 vector34 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);

					float num146 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector34.X;
					float num147 = Math.Abs(num146) * 0.1f;

					float num148 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector34.Y - num147;

					float num149 = (float)Math.Sqrt(num146 * num146 + num148 * num148);
					NPC.netUpdate = true;
					num149 = num145 / num149;
					num146 *= num149;
					num148 *= num149;
					int TYPE = ProjectileType<BomberBonesGrenade>();
					int Damage = 25;
					vector34.X += num146;
					vector34.Y += num148;
					if (Main.netMode != 1)
					{

						Projectile.NewProjectile(NPC.GetSource_FromAI(), vector34.X, vector34.Y, num146, num148, TYPE, Damage, 0f, Main.myPlayer);

					}
					if (Math.Abs(num148) > Math.Abs(num146) * 2f)
					{
						if (num148 > 0f)
						{
							NPC.ai[2] = 1f;
						}
						else
						{
							NPC.ai[2] = 5f;
						}
					}
					else if (Math.Abs(num146) > Math.Abs(num148) * 2f)
					{
						NPC.ai[2] = 3f;
					}
					else if (num148 > 0f)
					{
						NPC.ai[2] = 2f;
					}
					else
					{
						NPC.ai[2] = 4f;
					}
				}
				if ((NPC.velocity.Y != 0f) || NPC.ai[1] <= 0f)
				{
					NPC.ai[2] = 0f;
					NPC.ai[1] = 0f;
				}
				else if ((num141 != -1 && NPC.ai[1] >= (float)num141 && NPC.ai[1] < (float)(num141 + num142) && (NPC.velocity.Y == 0f)))
				{
					NPC.velocity.X *= 0.9f;
					NPC.spriteDirection = NPC.direction;
				}
			}

			else if ((NPC.ai[2] <= 0f) && (NPC.velocity.Y == 0f) && NPC.ai[1] <= 0f && !Main.player[NPC.target].dead)
			{
				bool flag18 = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
				if (Main.player[NPC.target].stealth == 0f && Main.player[NPC.target].itemAnimation == 0)
				{
					flag18 = false;
				}
				if (flag18)
				{
					float num155 = 10f;
					Vector2 vector35 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num156 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector35.X;
					float num157 = Math.Abs(num156) * 0.1f;
					float num158 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector35.Y - num157;
					num156 += (float)Main.rand.Next(-40, 41);
					num158 += (float)Main.rand.Next(-40, 41);
					float num159 = (float)Math.Sqrt(num156 * num156 + num158 * num158);
					float num160 = 700f;

					if (num159 < num160)
					{
						NPC.netUpdate = true;
						NPC.velocity.X *= 0.5f;
						num159 = num155 / num159;
						num156 *= num159;
						num158 *= num159;
						NPC.ai[2] = 3f;
						NPC.ai[1] = AttackTimer;
						if (Math.Abs(num158) > Math.Abs(num156) * 2f)
						{
							if (num158 > 0f)
							{
								NPC.ai[2] = 1f;
							}
							else
							{
								NPC.ai[2] = 5f;
							}
						}
						else if (Math.Abs(num156) > Math.Abs(num158) * 2f)
						{
							NPC.ai[2] = 3f;
						}
						else if (num158 > 0f)
						{
							NPC.ai[2] = 2f;
						}
						else
						{
							NPC.ai[2] = 4f;
						}
					}
				}
			}
			if (NPC.ai[2] <= 0f )
			{
				float num161 = 1f;
				float num162 = 0.07f;
				float num163 = 0.8f;
				bool flag19 = false;
				if (NPC.velocity.X < 0f - num161 || NPC.velocity.X > num161 || flag19)
				{
					if (NPC.velocity.Y == 0f)
					{
						NPC.velocity *= num163;
					}
				}
				else if (NPC.velocity.X < num161 && NPC.direction == 1)
				{
					NPC.velocity.X += num162;
					if (NPC.velocity.X > num161)
					{
						NPC.velocity.X = num161;
					}
				}
				else if (NPC.velocity.X > 0f - num161 && NPC.direction == -1)
				{
					NPC.velocity.X -= num162;
					if (NPC.velocity.X < 0f - num161)
					{
						NPC.velocity.X = 0f - num161;
					}
				}
			}
			if (NPC.velocity.Y == 0f || flag)
			{
				int num167 = (int)(NPC.position.Y + (float)NPC.height + 7f) / 16;
				int num168 = (int)(NPC.position.Y - 9f) / 16;
				int num169 = (int)NPC.position.X / 16;
				int num170 = (int)(NPC.position.X + (float)NPC.width) / 16;
				int num171 = (int)(NPC.position.X + 8f) / 16;
				int num172 = (int)(NPC.position.X + (float)NPC.width - 8f) / 16;
				bool flag20 = false;
				for (int num173 = num171; num173 <= num172; num173++)
				{
					if (num173 >= num169 && num173 <= num170 && Main.tile[num173, num167] == null)
					{
						flag20 = true;
						continue;
					}
					if (Main.tile[num173, num168] != null && Main.tile[num173, num168].HasUnactuatedTile && Main.tileSolid[Main.tile[num173, num168].TileType])
					{
						flag5 = false;
						break;
					}
					if (!flag20 && num173 >= num169 && num173 <= num170 && Main.tile[num173, num167].HasUnactuatedTile && Main.tileSolid[Main.tile[num173, num167].TileType])
					{
						flag5 = true;
					}
				}
				if (!flag5 && NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y = 0f;
				}
				if (flag20)
				{
					return;
				}
			}
				if (NPC.velocity.Y >= 0f && NPC.directionY != 1)
				{
					int num174 = 0;
					if (NPC.velocity.X < 0f)
					{
						num174 = -1;
					}
					if (NPC.velocity.X > 0f)
					{
						num174 = 1;
					}
					Vector2 vector37 = NPC.position;
					vector37.X += NPC.velocity.X;
					int num175 = (int)((vector37.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 1) * num174)) / 16f);
					int num176 = (int)((vector37.Y + (float)NPC.height - 1f) / 16f);
					if (WorldGen.InWorld(num175, num176, 4))
					{
				         Tile tile = Main.tile[num175, num176];

					if (tile == null)
                    {
						tile = new Tile();
                    }
                    if (tile == null)
                    {
                        tile = new Tile();
                    }
                    if (tile == null)
                    {
						tile = new Tile();
                    }
                    if (tile == null)
                    {
						tile = new Tile();
                    }
                    if (tile == null)
                    {
						tile = new Tile();
                    }
                    if ((num175 * 16) < vector37.X + NPC.width && (num175 * 16 + 16) > vector37.X && ((Main.tile[num175, num176].HasUnactuatedTile && Main.tile[num175, num176].Slope != SlopeType.SlopeUpLeft && Main.tile[num175, num176 - 1].Slope != SlopeType.SlopeUpRight && Main.tileSolid[Main.tile[num175, num176].TileType] && !Main.tileSolidTop[Main.tile[num175, num176].TileType]) || (Main.tile[num175, num176 - 1].IsHalfBlock && Main.tile[num175, num176 - 1].HasUnactuatedTile)) && (!Main.tile[num175, num176 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 1].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 1].TileType] || (Main.tile[num175, num176 - 1].IsHalfBlock && (!Main.tile[num175, num176 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 4].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 4].TileType]))) && (!Main.tile[num175, num176 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 2].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 2].TileType]) && (!Main.tile[num175, num176 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 3].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 3].TileType]) && (!Main.tile[num175 - num174, num176 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175 - num174, num176 - 3].TileType]))
						{
							float num177 = num176 * 16;
							if (Main.tile[num175, num176].IsHalfBlock)
							{
								num177 += 8f;
							}
							if (Main.tile[num175, num176 - 1].IsHalfBlock)
							{
								num177 -= 8f;
							}
							if (num177 < vector37.Y + (float)NPC.height)
							{
								float num178 = vector37.Y + (float)NPC.height - num177;
								float num179 = 16.1f;
								if (num178 <= num179)
								{
									NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num177;
									NPC.position.Y = num177 - (float)NPC.height;
									if (num178 < 9f)
									{
										NPC.stepSpeed = 1f;
									}
									else
									{
										NPC.stepSpeed = 2f;
									}
								}
							}
						}
					}
				}
				if (flag5)
				{
					int num180 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
					int num181 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
				Tile tile2 = Main.tile[num180, num181];

				if (tile2 == null)
                {
					tile2 = new Tile();
                }
                if (tile2 == null)
                {
					tile2 = new Tile();
                }
                if (tile2 == null)
                {
					tile2 = new Tile();
                }
                if (tile2 == null)
                {
					tile2 = new Tile();
                }
                if (tile2 == null)
                {
                  tile2 = new Tile();
                }
                if (tile2 == null)
                {
					tile2 = new Tile();
                }
                if (tile2 == null)
                {
					tile2 = new Tile();
                }
                if (tile2 == null)
                {
					tile2 = new Tile();
                }

                //Main.tile[num180, num181 + 1].IsHalfBlock; why is this part of the code, if it returns an error every time...
                if (tile2.HasUnactuatedTile && (tile2.TileType == 10 || tile2.TileType == 388) && flag8)
					{
						NPC.ai[2] += 1f;
						NPC.ai[3] = 0f;
						if (NPC.ai[2] >= 60f)
						{
							NPC.velocity.X = 0.5f * (float)(-NPC.direction);
							int num182 = 5;
							if (Main.tile[num180, num181 - 1].TileType == 388)
							{
								num182 = 2;
							}
							NPC.ai[1] += num182;
							NPC.ai[2] = 0f;
							bool flag23 = false;
							if (NPC.ai[1] >= 10f)
							{
								flag23 = true;
								NPC.ai[1] = 10f;
							}
							WorldGen.KillTile(num180, num181 - 1, fail: true);
							if ((Main.netMode != 1 || !flag23) && flag23 && Main.netMode != 1)
							{

									if (Main.tile[num180, num181 - 1].TileType == 10)
									{
										bool flag24 = WorldGen.OpenDoor(num180, num181 - 1, NPC.direction);
										if (!flag24)
										{
											NPC.ai[3] = num55;
											NPC.netUpdate = true;
										}
										if (Main.netMode == NetmodeID.Server && flag24)
										{
											NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num180, num181 - 1, NPC.direction);
										}
									}
									if (Main.tile[num180, num181 - 1].TileType == 388)
									{
										bool flag25 = WorldGen.ShiftTallGate(num180, num181 - 1, closing: false);
										if (!flag25)
										{
											NPC.ai[3] = num55;
											NPC.netUpdate = true;
										}
										if (Main.netMode == NetmodeID.Server && flag25)
										{
											NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num180, num181 - 1);
										}
									}
								
							}
						}
					}
					else
					{
						int num183 = NPC.spriteDirection;

						if ((NPC.velocity.X < 0f && num183 == -1) || (NPC.velocity.X > 0f && num183 == 1))
						{
							if (NPC.height >= 32 && Main.tile[num180, num181 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 2].TileType])
							{
								if (Main.tile[num180, num181 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 3].TileType])
								{
									NPC.velocity.Y = -8f;
									NPC.netUpdate = true;
								}
								else
								{
									NPC.velocity.Y = -7f;
									NPC.netUpdate = true;
								}
							}
							else if (Main.tile[num180, num181 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 1].TileType])
							{
				
									NPC.velocity.Y = -6f;
								
								NPC.netUpdate = true;
							}
							else if (NPC.position.Y + (float)NPC.height - (float)(num181 * 16) > 20f && Main.tile[num180, num181].HasUnactuatedTile && Main.tile[num180, num181].Slope != SlopeType.SlopeUpLeft && Main.tileSolid[Main.tile[num180, num181].TileType])
							{
								NPC.velocity.Y = -5f;
								NPC.netUpdate = true;
							}
							else if (NPC.directionY < 0 && (!Main.tile[num180, num181 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num180, num181 + 1].TileType]) && (!Main.tile[num180 + NPC.direction, num181 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num180 + NPC.direction, num181 + 1].TileType]))
							{
								NPC.velocity.Y = -8f;
								NPC.velocity.X *= 1.5f;
								NPC.netUpdate = true;
							}
							else if (flag8)
							{
								NPC.ai[1] = 0f;
								NPC.ai[2] = 0f;
							}
							if (NPC.velocity.Y == 0f && flag6 && NPC.ai[3] == 1f)
							{
								NPC.velocity.Y = -5f;
							}
							if (NPC.velocity.Y == 0f && Main.expertMode && Main.player[NPC.target].Bottom.Y < NPC.Top.Y && Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < (float)(Main.player[NPC.target].width * 3) && Collision.CanHit(NPC, Main.player[NPC.target]))
							{
								if (NPC.velocity.Y == 0f)
								{
									int num186 = 6;
									if (Main.player[NPC.target].Bottom.Y > NPC.Top.Y - (float)(num186 * 16))
									{
										NPC.velocity.Y = -7.9f;
									}
									else
									{
										int num187 = (int)(NPC.Center.X / 16f);
										int num188 = (int)(NPC.Bottom.Y / 16f) - 1;
										for (int num189 = num188; num189 > num188 - num186; num189--)
										{
											if (Main.tile[num187, num189].HasUnactuatedTile && TileID.Sets.Platforms[Main.tile[num187, num189].TileType])
											{
												NPC.velocity.Y = -7.9f;
												break;
											}
										}
									}
								}
							}
						}
					}
				}
				else if (flag8)
				{
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
				}
				if (Main.netMode == 1 || !(NPC.ai[3] >= (float)num55))
				{
					return;
				}
		}
        public override void HitEffect(NPC.HitInfo hit)
        {
			if (NPC.life > 0)
			{
				for (int num662 = 0; (double)num662 < hit.Damage / (double)NPC.lifeMax * 50.0; num662++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 26, hit.HitDirection, -1f);
				}
				return;
			}
			for (int num663 = 0; num663 < 20; num663++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 26, 2.5f * (float)hit.HitDirection, -2.5f);
			}
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 42, NPC.scale);
			Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, 43, NPC.scale);
			Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, 44, NPC.scale);
		}
	}
	public class BomberBonesGrenade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GrenadeI);
            AIType = ProjectileID.GrenadeI;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 120;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            TRAEMethods.Explode(Projectile, 80);
            TRAEMethods.DefaultExplosion(Projectile);
        }
        public override void AI()
        {
            if (Projectile.timeLeft == 5)
            {
                TRAEMethods.Explode(Projectile, 120);
                TRAEMethods.DefaultExplosion(Projectile); 
            }
        }
    }
}