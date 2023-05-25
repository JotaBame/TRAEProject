using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TRAEProject.NewContent.NPCs.Banners;
using TRAEProject.NewContent.Items.Materials;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;
namespace TRAEProject.NewContent.NPCs.GraniteOvergrowth

{

    public class GraniteOvergrowthNPC : ModNPC
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Granite Overgrowth"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 16; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 160;
            NPC.height = 60;
            NPC.damage = 100;
            NPC.defense = 50;
            NPC.lifeMax = 15000;
            NPC.rarity = 5;
            //NPC.scale = 1.2f;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.knockBackResist = 0f;
            Banner = NPC.type;
            BannerItem = ItemType<GraniteOvergrowthBanner>();
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note:bossAdjustment -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = 10000; // 20k on expert
                                 // 30k on master 
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
                new FlavorTextBestiaryInfoElement("A sentient tumor of the Granite Cave. It is neverendlessly absorbing nutrients and biolectricity, which it must constantly release to avoid selfdestructing")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].type == NPCType<GraniteOvergrowthNPC>())
                    return 0f;
            }
            if (spawnInfo.Granite && Main.hardMode && spawnInfo.SpawnTileType == TileID.Granite)
            {
                return 0.08f;
            }
            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<GraniteBattery>(), 1, 2, 5));
        }
        public override void OnKill()
        {
            for (int f = 0; f < 10; f++)
            {
                Vector2 vector22 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                vector22.Normalize();
                vector22 *= Main.rand.Next(10, 101) * 0.02f;
                int goreType = GoreID.GraniteGolemBody;
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, vector22, goreType);
            }
            for (int i = 0; i < 50; i++)
            {
                Dust dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X - 2f, NPC.position.Y - 2f), NPC.width + 4, NPC.height + 4, DustID.Electric, NPC.velocity.X * 0.4f, NPC.velocity.Y * 0.4f, 180, default, 1f);
                dust3.noGravity = true;
                dust3.velocity *= 0.8f;
                dust3.velocity.X *= 0.6f;
                dust3.velocity.Y -= 0.8f;
                if (Main.rand.Next(3) == 0)
                {
                    dust3.velocity.Y += 1f;

                }
                if (Main.rand.Next(2) == 0)
                {
                    
                    dust3.noGravity = false;
                    dust3.scale *= 1.5f;
                }
            }
            if (!TRAEWorld.downedOvergrowth)
            {
                TRAEWorld.downedOvergrowth = true;
                int i = (int)NPC.Center.X * 1;
                int y = (int)NPC.Center.Y * 1;
                NPC.NewNPC(NPC.GetSource_FromAI(),i, y, NPCID.Cyborg);
             
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, NPC.velocity.X, NPC.velocity.Y, 180, default, 1f);
                dust3.noGravity = true;
                dust3.velocity *= 0.8f;
                dust3.velocity.X *= 0.6f;
                dust3.velocity.Y -= 0.8f; 
                if (Main.rand.Next(4) == 0)
                {
                    dust3.noGravity = false;
                    dust3.scale *= 1.5f;
                }
            }
               
         
            for (int i = 0; i < 15; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, DustID.Electric, vel.X, vel.Y);
                dust.scale = 0.5f;
            }
        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
         
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                frame++; 
                if (frame > 7)
                {
                    frame = 0;
                }
                NPC.frame.Y = frameHeight * frame;
                NPC.frameCounter = 0;
            }
            if (TRAEWorld.downedOvergrowth)
            {
                NPC.frame.Y = frameHeight * (frame + 8);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NewContent/NPCs/GraniteOvergrowth/GraniteOvergrowthNPC_Glow");

            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }
        int RingRange = 250;
        public override void AI()
        {
            NPC.direction = 1;
            NPC.TargetClosest(false);
            
            Player player = Main.player[NPC.target];
            bool isPlayernottargetable = player.dead || Math.Abs(NPC.position.X - player.position.X) > 2000f || Math.Abs(NPC.position.Y - player.position.Y) > 2000f;
            if (isPlayernottargetable)
            {
                if (isPlayernottargetable)
                {
                    NPC.EncourageDespawn(300);
                }
            }

            if (RingRange > 250 && NPC.ai[1] < 1000)
            {
                RingRange -= 3;
            }
            if (RingRange < 250)
            {
                RingRange = 250;
            }
            if (!isPlayernottargetable && Main.netMode != 1)
            {
                if (NPC.ai[1] % 150 == 0 && NPC.ai[1] < 900)
                {
                    int healAmount = NPC.lifeMax - NPC.life;
                    if (healAmount > 1000)
                        healAmount = 1000;
                    if (healAmount > 0)
                    {
                        NPC.HealEffect(NPC.Hitbox, healAmount, true);
                        NPC.life += healAmount;
                    }
                    
                }
                if (NPC.ai[1] % 300 == 0 && NPC.ai[1] < 900)
                {
                    int i = (int)NPC.Center.X * 1;
                    int y = (int)NPC.Center.Y * 1;
                    NPC.NewNPC(NPC.GetSource_FromAI(), i, y, NPCType<GraniteCore>());
                }
                if (NPC.ai[1] == 1000)
                {
                    SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 });
                }
                if (NPC.ai[1] > 1000 && NPC.ai[1] < 1500)
                {
                    RingRange += 1;

                    Dust dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X - 2f, NPC.position.Y - 2f), NPC.width + 4, NPC.height + 4, DustID.Electric, NPC.velocity.X * 0.4f, NPC.velocity.Y * 0.4f, 180, default, 1.5f);
                    dust3.noGravity = true;
                    dust3.velocity *= 0.8f;
                    dust3.velocity.X *= 0.6f;
                    dust3.velocity.Y -= 0.8f;
                    if (Main.rand.Next(4) == 0)
                    {
                        dust3.noGravity = false;
                        dust3.scale *= 1.5f;
                    }
                }
                if (NPC.ai[1] > 1500)
                {
                    SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, NPC.position);
                    NPC.ai[1] = 0;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 0), ProjectileType<GraniteBoom>(), 100, 0f);
                }
                // ring

                int dusts = RingRange / 250 * 10;

                float dustScale = 1.3f;
                for (int i = 0; i < dusts; i++)
                {
                    Vector2 spawnPos = NPC.Center + Main.rand.NextVector2CircularEdge(RingRange, RingRange);
                    Vector2 offset = spawnPos - player.Center;
                    if (Math.Abs(offset.X) > Main.screenWidth * 0.6f || Math.Abs(offset.Y) > Main.screenHeight * 0.6f) //dont spawn dust if its pointless
                        continue;
                    Dust dust = Main.dust[Dust.NewDust(spawnPos, 0, 0, DustID.Electric, 0, 0, 100, default, dustScale)];
                    dust.velocity = NPC.velocity;
                    if (Main.rand.NextBool(3))
                    {
                        dust.velocity += Vector2.Normalize(NPC.Center - dust.position) * Main.rand.NextFloat(5f);
                        dust.position += dust.velocity * 5f;
                    }
                    dust.noGravity = true;
                }

                NPC.ai[1]++;
                float shootToX = player.position.X + player.width * 0.5f - NPC.Center.X;
                float shootToY = player.position.Y + player.height * 0.5f - NPC.Center.Y;
                float distance2 = MathF.Sqrt((shootToX * shootToX + shootToY * shootToY));
                if (NPC.ai[1] % 30 == 0)
                {
                    if (distance2 < (RingRange + 25))
                    {
                        if (Collision.CanHit(NPC.Center, 0, 0, player.Center, 0, 0))
                        {
                            SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 }, NPC.position);
                            //Dividing the factor of 2f which is the desired velocity by distance2
                            distance2 = 1f / distance2;

                            //Multiplying the shoot trajectory with distance2 times a multiplier if you so choose to
                            shootToX *= distance2 * 5f;
                            shootToY *= distance2 * 5f;
                            Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(0));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<GraniteBolt>(), 50, 0);
                        }

                    }
                }               
            }
        }
    }
    public class GraniteCore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Granite Core"); // Automatic from .lang files
        }
        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 42;
            NPC.damage = 25;
            NPC.defense = 30;
            NPC.lifeMax = 650;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.knockBackResist = 1f;
        }
        public override void HitEffect(NPC.HitInfo hit)

        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 vel = new(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, DustID.Electric, vel.X, vel.Y);
                dust.scale = 0.5f;
            }
        }
        public override void OnKill()
        {
            for (int f = 0; f < 2; f++)
            {
                Vector2 vector22 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                vector22.Normalize();
                vector22 *= Main.rand.Next(10, 101) * 0.02f;
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, vector22, GoreID.GraniteGolemBody);
            }
            int bolt = ProjectileType<GraniteBolt>(); 
            float num852 = (MathF.PI * 2f);
            float boltCount = 59.167f * 6;
            for (float c = 0f; c < 1f; c += boltCount / (678f * MathF.PI))
            {
                float f2 = num852 + c * (MathF.PI * 2f);
                Vector2 velocity = f2.ToRotationVector2() * (4f + Main.rand.NextFloat() * 2f);
                velocity += Vector2.UnitY * -1f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, bolt, 25, 0f);
            }

        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NewContent/NPCs/GraniteOvergrowth/GraniteCore_Glow");

            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }
        public override void AI()
        {
            NPC.rotation += 1;
            for (int index1 = 0; index1 < 255; index1++)
            {
                if (index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead)
                {
                    //if (NPC.distance2(Main.player[index1].Center) <= (double)100)
                    //    return;
                    Vector2 unitY = NPC.DirectionTo(Main.player[index1].Center);
                    if (unitY.HasNaNs())
                        unitY = Vector2.UnitY;
                    NPC.velocity = Vector2.Multiply(unitY, 4f);
                }
            }
        }
    }
    public class GraniteBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Granite Bolt");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 50;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Electrified, 300);
        }
        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                Projectile.tileCollide = true;
                for (int i = 0; i < 2; i++)
                {
                    Vector2 ProjectilePosition = Projectile.position;
                    ProjectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                    Projectile.alpha = 255;
                    int dust = Dust.NewDust(ProjectilePosition, 1, 1, DustID.Electric, 0f, 0f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
                    Main.dust[dust].position = ProjectilePosition;
                    Main.dust[dust].velocity *= 0.2f;
                }
            }

        }
    }
    public class GraniteBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Granite Boom");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1275;
            Projectile.height = 1275;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }
        bool runonce = false;
        public override void AI()
        {
            if (!runonce)
            {
                runonce = true;
                for (int i = 0; i < 120; i++)
                {
                    int Dust = Terraria.Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 200, default, 2.5f);
                    Main.dust[Dust].noGravity = true;
                    Dust dust = Main.dust[Dust];
                    dust.velocity *= 2f;
                    Dust = Terraria.Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 200, default, 1.5f);
                    dust = Main.dust[Dust];
                    dust.velocity *= 1.2f;
                    Main.dust[Dust].noGravity = true;
                }
                for (int i = 0; i < 1; i++)
                {
                    int num371 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Projectile.width * Main.rand.Next(100) / 100f, Projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
                    Gore gore = Main.gore[num371];
                    gore.velocity *= 0.3f;
                    Main.gore[num371].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[num371].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }
        }
    }
    public class CyborgSpawn : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.Mechanic && TRAEWorld.downedOvergrowth && !NPC.downedPlantBoss)
            {
                int findCyborg = NPC.FindFirstNPC(NPCID.Cyborg);
                if (findCyborg == -1)
                {
                    int i = (int)npc.Center.X * 1;
                    int y = (int)npc.Center.Y * 1;
                    NPC.NewNPC(npc.GetSource_FromAI(), i, y, NPCID.Cyborg);
                }
            }
        }
    }
}