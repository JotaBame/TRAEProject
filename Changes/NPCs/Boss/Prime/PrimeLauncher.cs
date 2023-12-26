using System;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeLauncher : ModNPC
    {
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if(NPC.life <= 0)
            {
                PrimeStats.ArmGore(NPC);
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossAdjustment);
        }
        
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 6;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 38;
            NPC.damage = 30;
            NPC.defense = 28;
            NPC.lifeMax = PrimeStats.launcherHealth;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.netAlways = true;
            NPC.aiStyle = -1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

            PrimeStats.RenderBones(NPC, spriteBatch, screenPos, -1);

            drawColor = NPC.GetNPCColorTintedByBuffs(drawColor);
            for (int num93 = 9; num93 >= 0; num93 -= 2) 
            {
                Color alpha9 = NPC.GetAlpha(drawColor);
                alpha9.R = (byte)(alpha9.R * (10 - num93) / 20);
                alpha9.G = (byte)(alpha9.G * (10 - num93) / 20);
                alpha9.B = (byte)(alpha9.B * (10 - num93) / 20);
                alpha9.A = (byte)(alpha9.A * (10 - num93) / 20);
                spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, 
                NPC.oldPos[num93] + NPC.Size * 0.5f - screenPos, 
                NPC.frame, alpha9, NPC.rotation, new Vector2(20f, 20f), NPC.scale, spriteEffects, 0f);
            }
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
                        NPC.frame, drawColor, NPC.rotation,
                        new Vector2(18f, 18f), 1f, spriteEffects, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Prime/PrimeLauncher_Glow").Value, NPC.Center - screenPos,
                        NPC.frame, Color.White, NPC.rotation,
                        new Vector2(18f, 18f), 1f, spriteEffects, 0f);
            
            return false;
        }
        int timer = 0;
        public override void AI()
        { 
			NPC.damage = 0;
            NPC.spriteDirection = -1;
            if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].aiStyle != 32 || (NPC.ai[0] == 0 && !SkeletronPrime.KeepPhase2Arms(Main.npc[(int)NPC.ai[1]])))
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server) 
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            if (Main.npc[(int)NPC.ai[1]].ai[1] == 3f)
                    NPC.EncourageDespawn(10);

            float yOffset = -100;
            float xOffset = 180;
            if(NPC.ai[0] != 0)
            {
                yOffset = 0;
                xOffset = 306;
            }
            if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y + yOffset) 
            {
                if (NPC.velocity.Y > 0f)
                    NPC.velocity.Y *= 0.96f;

                NPC.velocity.Y -= 0.1f;
                if (NPC.velocity.Y > 3f)
                    NPC.velocity.Y = 3f;
            }
            else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y + yOffset) 
            {
                if (NPC.velocity.Y < 0f)
                    NPC.velocity.Y *= 0.96f;

                NPC.velocity.Y += 0.1f;
                if (NPC.velocity.Y < -3f)
                    NPC.velocity.Y = -3f;
            }

            if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + xOffset) 
            {
                if (NPC.velocity.X > 0f)
                    NPC.velocity.X *= 0.96f;

                NPC.velocity.X -= 0.14f;
                if (NPC.velocity.X > 8f)
                    NPC.velocity.X = 8f;
            }

            if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + xOffset) 
            {
                if (NPC.velocity.X < 0f)
                    NPC.velocity.X *= 0.96f;

                NPC.velocity.X += 0.14f;
                if (NPC.velocity.X < -8f)
                    NPC.velocity.X = -8f;
            }

            NPC.TargetClosest(false);
            NPC.rotation = (NPC.Center - Main.npc[(int)NPC.ai[1]].Center).ToRotation();
            timer++;
            if(timer >= PrimeStats.ragedMissileCooldown && Main.npc[(int)NPC.ai[1]].ai[1] != 0f)
            {
                Fire(NPC);
                timer = 0;
            }
            if(timer % PrimeStats.missileBurstDelay == 0 && timer >= PrimeStats.missileBurstCooldown)
            {
                Fire(NPC);
                if(timer >= PrimeStats.missileBurstCooldown + PrimeStats.missileBurstDelay * PrimeStats.missileBurstSize)
                {
                    timer = 0;
                }
            }
        }
        static void Fire(NPC npc)
        {
            if(Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            SoundEngine.PlaySound(SoundID.Item11, npc.Center);
            int damage = PrimeStats.missileDamage;
            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + TRAEMethods.PolarVector(18f, npc.rotation), TRAEMethods.PolarVector(10, npc.rotation), ModContent.ProjectileType<PrimeMissile>(), damage, 0, Main.myPlayer);
        }
    }
}