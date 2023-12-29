using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeMace : ModNPC
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
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 55;
            NPC.defense = 30;
            NPC.lifeMax = PrimeStats.macHealth;
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
                NPC.frame, alpha9, NPC.rotation, new Vector2(30f, 30f), NPC.scale, spriteEffects, 0f);
            }
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
                        NPC.frame, drawColor, NPC.rotation,
                        new Vector2(30f, 30f), 1f, spriteEffects, 0f);
            
            return false;
        }
        void Vice_Rotate(float xOffset = 200f)
        {
            int headIndex = (int)MathF.Abs(NPC.ai[1]);
            float side = -1;
            Vector2 vector56 = NPC.Center;
            float num512 = Main.npc[headIndex].Center.X - xOffset * side - vector56.X;
            float num513 = Main.npc[headIndex].position.Y + 230f - vector56.Y;
            NPC.rotation = (float)Math.Atan2(num513, num512) + 1.57f;
        }
        public override void AI()
        {
            int headIndex = (int)MathF.Abs(NPC.ai[1]);
            //Main.NewText("huh");

            if (!Main.npc[(int)headIndex].active || Main.npc[(int)headIndex].aiStyle != 32 || (NPC.ai[1] >= 0 && !SkeletronPrime.KeepPhase2Arms(Main.npc[headIndex]))) 
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server) 
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            
            if (Main.npc[headIndex].ai[1] == 3f)
                    NPC.EncourageDespawn(10);
            Vector2 restingPosition = Main.npc[headIndex].Center + new Vector2(125f, 260f);
            Vector2 goTo = restingPosition;
            if (NPC.ai[2] <= 0f)
            {
                NPC.ai[2] -= 1f;
                if (NPC.ai[2] <= -1 * PrimeStats.primeMaceNonRageCooldown || Main.npc[headIndex].ai[1] != 0f)
                {
                    NPC.TargetClosest(false);
                    NPC.ai[2] = 1f;
                    Vector2 swingAt = Main.player[NPC.target].Center + PrimeStats.primeMaceSwingTime * Main.player[NPC.target].velocity * 0.5f;
                    NPC.ai[0] = swingAt.X;
                    NPC.ai[3] = swingAt.Y;
                    NPC.netUpdate = true; 
                }
                if (NPC.position.Y > goTo.Y) 
                {
                    if (NPC.velocity.Y > 0f)
                        NPC.velocity.Y *= 0.96f;

                    NPC.velocity.Y -= 0.1f;
                    if (NPC.velocity.Y > 3f)
                        NPC.velocity.Y = 3f;
                }
                else if (NPC.position.Y < goTo.Y) 
                {
                    if (NPC.velocity.Y < 0f)
                        NPC.velocity.Y *= 0.96f;

                    NPC.velocity.Y += 0.1f;
                    if (NPC.velocity.Y < -3f)
                        NPC.velocity.Y = -3f;
                }

                if (NPC.Center.X > goTo.X) 
                {
                    if (NPC.velocity.X > 0f)
                        NPC.velocity.X *= 0.94f;

                    NPC.velocity.X -= 0.3f;
                    if (NPC.velocity.X > 9f)
                        NPC.velocity.X = 9f;
                }

                if (NPC.Center.X < goTo.X) 
                {
                    if (NPC.velocity.X < 0f)
                        NPC.velocity.X *= 0.94f;

                    NPC.velocity.X += 0.2f;
                    if (NPC.velocity.X < -8f)
                        NPC.velocity.X = -8f;
                }
            }
            else
            {
                Vector2 swingAt = new Vector2(NPC.ai[0], NPC.ai[3]);
                NPC.ai[2] += 1f;
                float theta =  MathF.PI * 2f * NPC.ai[2] / PrimeStats.primeMaceSwingTime;
                Vector2 ellipseCenter = (restingPosition + swingAt) * 0.5f;
                float ellipselength = (swingAt - restingPosition).Length();
                float ellipseWidth = 600f;
                goTo = ellipseCenter + new Vector2(ellipselength * -0.5f * MathF.Cos(theta), ellipseWidth * 0.5f * MathF.Sin(theta)).RotatedBy((swingAt - restingPosition).ToRotation());

                if (NPC.ai[2] >= PrimeStats.primeMaceSwingTime)
                {
                    NPC.ai[2] = 0;
                    NPC.netUpdate = true;
                }
                NPC.velocity = (goTo - NPC.Center) * (1/8f);
            }
            
            //Dust.NewDustPerfect(goTo, DustID.Torch, Vector2.Zero);
            
            Vice_Rotate();
        }

    }
}