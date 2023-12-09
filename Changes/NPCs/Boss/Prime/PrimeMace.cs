using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeMace : ModNPC
    {
        int timer = 0;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 6;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 60;
            NPC.defense = 20;
            NPC.lifeMax = 8000;
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
            int headIndex = (int)MathF.Abs(NPC.ai[1]);
            SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
            float side = -1;
            Vector2 vector7 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - 5f * side, NPC.position.Y + 20f);
            for (int k = 0; k < 2; k++) 
            {
                float num22 = Main.npc[headIndex].position.X + (float)(Main.npc[headIndex].width / 2) - vector7.X;
                float num23 = Main.npc[headIndex].position.Y + (float)(Main.npc[headIndex].height / 2) - vector7.Y;
                float num24 = 0f;
                if (k == 0) 
                {
                    num22 -= 200f * side;
                    num23 += 130f;
                    num24 = (float)Math.Sqrt(num22 * num22 + num23 * num23);
                    num24 = 92f / num24;
                    vector7.X += num22 * num24;
                    vector7.Y += num23 * num24;
                }
                else 
                {
                    num22 -= 50f * side;
                    num23 += 80f;
                    num24 = (float)Math.Sqrt(num22 * num22 + num23 * num23);
                    num24 = 60f / num24;
                    vector7.X += num22 * num24;
                    vector7.Y += num23 * num24;
                }

                float rotation7 = (float)Math.Atan2(num23, num22) - 1.57f;
                Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
                spriteBatch.Draw(TextureAssets.BoneArm2.Value, new Vector2(vector7.X - screenPos.X, vector7.Y - screenPos.Y), new Rectangle(0, 0, TextureAssets.BoneArm.Width(), TextureAssets.BoneArm.Height()), color7, rotation7, new Vector2((float)TextureAssets.BoneArm.Width() * 0.5f, (float)TextureAssets.BoneArm.Height() * 0.5f), 1f, SpriteEffects.None, 0f);
                if (k == 0) 
                {
                    vector7.X += num22 * num24 / 2f;
                    vector7.Y += num23 * num24 / 2f;
                }
                else if (!Main.gamePaused) 
                {
                    vector7.X += num22 * num24 - 16f;
                    vector7.Y += num23 * num24 - 6f;
                    int num25 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10, DustID.Torch, num22 * 0.02f, num23 * 0.02f, 0, default(Microsoft.Xna.Framework.Color), 2.5f);
                    Main.dust[num25].noGravity = true;
                }
            }

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
            int swingTime = 120;
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
                if (NPC.ai[2] <= -200f || Main.npc[headIndex].ai[1] != 0f)
                {
                    NPC.TargetClosest(false);
                    NPC.ai[2] = 1f;
                    Vector2 swingAt = Main.player[NPC.target].Center + swingTime * Main.player[NPC.target].velocity * 0.5f;
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
                float theta =  MathF.PI * 2f * NPC.ai[2] / swingTime;
                Vector2 ellipseCenter = (restingPosition + swingAt) * 0.5f;
                float ellipselength = (swingAt - restingPosition).Length();
                float ellipseWidth = 300f;
                goTo = ellipseCenter + new Vector2(ellipselength * -0.5f * MathF.Cos(theta), ellipseWidth * 0.5f * MathF.Sin(theta)).RotatedBy((swingAt - restingPosition).ToRotation());

                if (NPC.ai[2] >= swingTime)
                {
                    NPC.ai[2] = 0;
                    NPC.netUpdate = true;
                }
                NPC.velocity = (goTo - NPC.Center) * (1/4f);
            }
            
            //Dust.NewDustPerfect(goTo, DustID.Torch, Vector2.Zero);
            
            Vice_Rotate();
        }

    }
}