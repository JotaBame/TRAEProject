using TRAEProject.NewContent.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TRAEProject.NewContent.NPCs.Banners;

using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.ItemDropRules;
using TRAEProject.NewContent.Items.Misc.Mounts;

namespace TRAEProject.NewContent.NPCs.Underworld.Salalava
{
    public class SalalavaNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
            DisplayName.SetDefault("Salalava");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 184;
            NPC.height = 34;
            NPC.aiStyle = 3;
            AIType = NPCID.DesertBeast;
            //AnimationType = NPCID.WalkingAntlion;
            NPC.value = 5000;
            NPC.damage = 70;
            NPC.defense = 40;
            NPC.lifeMax = 6000;
          
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.DD2_DrakinHurt;
            NPC.DeathSound = SoundID.DD2_DrakinDeath;
            NPC.knockBackResist = 0f;
            DrawOffsetY = -2;
            NPC.scale = 1f;
            Banner = NPC.type;
            NPC.GetGlobalNPC<UnderworldEnemies>().HellMinibossThatSpawnsInPairs = true;
            BannerItem = ItemType<FroggabombaBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("An elder Reptilian Lava Walker, awoken from hibernation by the chaos released into the world.")
            });
        }
        float dustTimer = 0;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<SalamanderTail>(), 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.Hotdog, 33));
        }

        float jump = 0;
        float attackTimer = 0;
        float teleportTimer;
        public override void AI()
        {
            teleportTimer += 0.2f + Main.rand.NextFloat(0.3f, 1.3f);
            if (NPC.Distance(NPC.GetTargetData().Center) >= 800f)
            {
                teleportTimer += 2f;
            }
            if (teleportTimer >= 600f)
            {
                teleportTimer = 0;
                int targetTileX = (int)Main.player[NPC.target].Center.X / 16;
                int targetTileY = (int)Main.player[NPC.target].Center.Y / 16;

                Vector2 chosenTile = Vector2.Zero;
                if (AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY))
                {
                    NPC.position += NPC.netOffset;
                    SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            
                    NPC.position -= NPC.netOffset;
                    NPC.position.X = chosenTile.X * 16f - (float)(NPC.width / 2) + 8f;
                    NPC.position.Y = chosenTile.Y * 16f - (float)NPC.height;
                    NPC.netOffset *= 0f;
                    NPC.velocity.X = 0f;
                    NPC.velocity.Y = 0f;

                    for (int num70 = 0; num70 < 30; num70++)
                    {

                        int num78 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 2.5f);
                        Dust dust = Main.dust[num78];
                        dust.velocity *= 3f;

                    }
                }

            }

            NPC.noGravity = false;
            int num = 1;
            int num2 = 1;
            int num3 = (int)((NPC.position.X + (NPC.width / 2)) / 16f);
            int num4 = (int)(NPC.Bottom.Y / 16f);
            for (int j = num3 - num; j <= num3 + num; j++)
            {
                for (int k = num4 - num2; k < num4 + num2; k++)
                {
                    if (Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 0 ||
                        Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 2 ||
                        Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 1
                        )
                    {
                        if (NPC.velocity.Y > 0)
                            NPC.velocity.Y = 0;
                        NPC.noGravity = true;
                        jump++;
                        if (NPC.Distance(NPC.GetTargetData().Center) <= 300f)
                            jump += 3; // jumps way more often if it can reach you
                        if (jump >= 900f) // We have to force it to jump, its normal AI won't let it jump while "water walking"
                        {
                            jump = 0;
                            NPC.velocity.Y = -8f;
                            NPC.velocity.X *= 1.25f;
                        }
                    }
                }
            }
            int lavamandies = 0;

            if (NPC.Distance(NPC.GetTargetData().Center) <= 600f)
            {

                attackTimer++;
               
                if (attackTimer >= 240f)
                {
                    NPC.FaceTarget();
                    if (attackTimer == 240f)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_DrakinBreathIn, NPC.Center);
                    }
                    NPC.velocity.X = 0;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].type == NPCType<Lavalarva>())
                        {
                            lavamandies++;
                        }
                    }
                    if (lavamandies < 7)
                    {
                        if (attackTimer % 10 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.DD2_OgreRoar, NPC.Center);
                            NPC npc = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<Lavalarva>());
                            npc.velocity.X = Main.rand.NextFloat(-3f, 3f);
                            npc.velocity.Y = Main.rand.NextFloat(-5f, -7f);

                        }
                    }
                    else
                    {
                        if (attackTimer % 10 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.DD2_DrakinShot, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 vector3 = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * (NPC.width/* + 20*/) / 2f + NPC.Center;
                                NPC.NewNPC(NPC.GetBossSpawnSource(NPC.target), (int)vector3.X, (int)vector3.Y + 17, NPCType<LavaBubble>());

                            }
                        }
                        NPC.TargetClosest(true);
                    }
                    if (attackTimer > 300f)
                        attackTimer = 0f;
                }
            }
        }
        public bool AI_AttemptToFindTeleportSpot(ref Vector2 chosenTile, int targetTileX, int targetTileY, int rangeFromTargetTile = 25, int telefragPreventionDistanceInTiles = 15, int solidTileCheckFluff = 1, bool solidTileCheckCentered = false, bool teleportInAir = true)
        {
            int num = (int)NPC.Center.X / 16;
            int num2 = (int)NPC.Center.Y / 16;
            int num3 = 0;
            bool flag = false;
            float num4 = 20f;
            if (Math.Abs(num * 16 - targetTileX * 16) + Math.Abs(num2 * 16 - targetTileY * 16) > 2000)
            {
                num3 = 100;
                flag = false;
            }
            while (!flag && num3 < 100)
            {
                num3++;
                int num5 = Main.rand.Next(targetTileX - rangeFromTargetTile, targetTileX + rangeFromTargetTile + 1);
                for (int i = Main.rand.Next(targetTileY - rangeFromTargetTile, targetTileY + rangeFromTargetTile + 1); i < targetTileY + rangeFromTargetTile; i++)
                {
                    if ((i >= num2 - 1 && i <= num2 + 1 && num5 >= num - 1 && num5 <= num + 1) || (!teleportInAir && !Main.tile[num5, i].IsActuated))
                    {
                        continue;
                    }
                    bool flag2 = true;
                    if (Main.tile[num5, i - 1].LiquidType == 1)
                    {
                        flag2 = false;
                    }

                    if (!((!solidTileCheckCentered) ? (!Collision.SolidTiles(num5 - solidTileCheckFluff, num5 + solidTileCheckFluff, i - 3 - solidTileCheckFluff, i - 1)) : (!Collision.SolidTiles(num5 - solidTileCheckFluff, num5 + solidTileCheckFluff, i - solidTileCheckFluff, i + solidTileCheckFluff))))
                    {
                        continue;
                    }
                    Rectangle rectangle = new Rectangle(num5 * 16, i * 16, 16, 16);
                    rectangle.Inflate(telefragPreventionDistanceInTiles * 16, telefragPreventionDistanceInTiles * 16);
                    for (int j = 0; j < Main.player.Length; j++)
                    {
                        Player player = Main.player[j];
                        if (player != null && player.active && !player.DeadOrGhost)
                        {
                            Rectangle value = player.Hitbox;
                            Rectangle value2 = value.Modified((int)(player.velocity.X * num4), (int)(player.velocity.Y * num4), 0, 0);
                            Rectangle.Union(ref value2, ref value, out value2);
                            if (value2.Intersects(rectangle))
                            {
                                flag2 = false;
                                flag = false;
                                break;
                            }
                        }
                    }

                    if (flag2)
                    {

                        chosenTile = new Vector2(num5, i);

                        flag = true;

                    }
                    break;
                }

            }
            return flag;
        }

        public override bool PreKill()
        {
            for (int i = 0; i < 4; i++)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SalalavaGore1").Type, 1f);
            }
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SalalavaGore2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SalalavaGore3").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SalalavaGore4").Type, 1f);
            return false;

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.GetGlobalNPC<UnderworldEnemies>().MinibossSpawn();

        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (attackTimer >= 240f)
                NPC.frame.Y = 5 * frameHeight;
            else
            {
                if (NPC.velocity.X == 0f)
                {
                    NPC.frame.Y = 0;
                }
                if (NPC.direction < 0 && NPC.velocity.X < 0f)
                {
                    NPC.spriteDirection = -1;
                }
                if (NPC.direction > 0 && NPC.velocity.X > 0f)
                {
                    NPC.spriteDirection = 1;
                }
                //if (NPC.frame.Y / frameHeight < 2)
                //{
                //	NPC.frame.Y = frameHeight * 2;
                //}
                NPC.frameCounter += 1f + Math.Abs(NPC.velocity.X) / 2f;
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y / frameHeight > 4)
                {
                    NPC.frame.Y = 0;
                }
                if (NPC.velocity.Y != 0f)
                {
                    NPC.frame.Y = 0;
                }
            }
        }
    }
    public class LavaBubble : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Daybreak,
                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
            DisplayName.SetDefault("Lava Bubble");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 38;
            NPC.aiStyle = 70;
            AIType = NPCID.DetonatingBubble;
            //AnimationType = NPCID.WalkingAntlion;
            NPC.value = 0;
            NPC.damage = 70;
            NPC.lifeMax = 1;
            NPC.lavaImmune = true;
            NPC.scale = 1f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }
        public override void AI()
        {
            NPC.velocity *= 0.96f;
        }
        public override void OnKill()
        {
            SoundEngine.PlaySound(SoundID.Item10, NPC.Center);
            if (NPC.life <= 0)
            {
                Vector2 center = NPC.Center;
                for (int num267 = 0; num267 < 60; num267++)
                {
                    int num268 = 25;
                    Vector2 vector24 = ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * Main.rand.Next(24, 41) / 8f;
                    int num269 = Dust.NewDust(NPC.Center - Vector2.One * num268, num268 * 2, num268 * 2, DustID.Lava);
                    Dust dust61 = Main.dust[num269];
                    Vector2 vector25 = Vector2.Normalize(dust61.position - NPC.Center);
                    dust61.position = NPC.Center + vector25 * 25f * NPC.scale;
                    if (num267 < 30)
                    {
                        dust61.velocity = vector25 * dust61.velocity.Length();
                    }
                    else
                    {
                        dust61.velocity = vector25 * Main.rand.Next(45, 91) / 10f;
                    }
                    dust61.color = Main.hslToRgb((float)(0.40000000596046448 + Main.rand.NextDouble() * 0.20000000298023224), 0.9f, 0.5f);
                    dust61.color = Color.Lerp(dust61.color, Color.White, 0.3f);
                    dust61.noGravity = true;
                    dust61.scale = 0.7f;
                }
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            NPC.life = 0;
            NPC.active = false;
        }

    }
    public class Lavalarva : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
            DisplayName.SetDefault("Magmander");
            Main.npcFrameCount[NPC.type] = 5;

        }

        public override void SetDefaults()
        {
            NPC.width = 122;
            NPC.height = 22;
            NPC.aiStyle = 3;
            AIType = NPCID.DesertBeast;
            //AnimationType = NPCID.WalkingAntlion;
            NPC.value = 0;
            NPC.damage = 70;
            NPC.defense = 20;
            NPC.value = 6000;
            NPC.lifeMax = 500;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit26;
            NPC.DeathSound = SoundID.NPCDeath29;
            NPC.knockBackResist = 0.1f;
            DrawOffsetY = -2;
            NPC.scale = 1f;
        }
        public override bool PreKill()
        {

            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("LavalarvaGore1").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("LavalarvaGore2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("LavalarvaGore3").Type, 1f);
            return false;

        }
        float jump = 0;

        public override void AI()
        {

            NPC.noGravity = false;
            int num = 1;
            int num2 = 1;
            int num3 = (int)((NPC.position.X + (NPC.width / 2)) / 16f);
            int num4 = (int)(NPC.Bottom.Y / 16f);
            for (int j = num3 - num; j <= num3 + num; j++)
            {
                for (int k = num4 - num2; k < num4 + num2; k++)
                {
                    if (Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 0 ||
                        Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 2 ||
                        Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 1
                        )
                    {
                        if (NPC.velocity.Y > 0)
                            NPC.velocity.Y = 0;
                        NPC.noGravity = true;
                        jump++;
                        if (NPC.Distance(NPC.GetTargetData().Center) <= 300f)
                            jump += 3; // jumps way more often if it can reach you
                        if (jump >= 750f) // We have to force it to jump, its normal AI won't let it jump while "water walking"
                        {
                            jump = 0;
                            NPC.velocity.Y = -8f;
                            NPC.velocity.X *= 2f;

                        }
                    }
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Advanced stage of a Reptilian Lava Walker. The amount of lava absorbed in its body makes it far more aggresive than its younger stage.")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                if (NPC.downedPlantBoss)
                    return SpawnCondition.Underworld.Chance * 0.2f;
                return SpawnCondition.Underworld.Chance * 0.15f;
            }

            return 0f;
        }
        public override void FindFrame(int frameHeight)
        {

            if (NPC.velocity.X == 0f)
            {
                NPC.frame.Y = 0;
            }
            if (NPC.direction < 0 && NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
            }
            if (NPC.direction > 0 && NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = 1;
            }
            if (NPC.frame.Y / frameHeight < 2)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            NPC.frameCounter += 1f + Math.Abs(NPC.velocity.X) / 2f;
            if (NPC.frameCounter > 12.0)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y / frameHeight >= Main.npcFrameCount[NPC.type])
            {
                NPC.frame.Y = frameHeight * 2;
            }
            if (NPC.velocity.Y != 0f)
            {
                NPC.frame.Y = 0;
            }
        }
    }
}