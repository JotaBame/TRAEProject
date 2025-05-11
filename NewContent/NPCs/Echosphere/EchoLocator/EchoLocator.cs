using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoLocator
{
    /// <summary>
    /// UNTESTED
    /// </summary>
    public class EchoLocator : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 18;
            NPC.defense = 32;
            NPC.lifeMax = 250;
            NPC.damage = 70;
            NPC.DeathSound = SoundID.NPCDeath4;//bat/mouse death sound
            NPC.HitSound = SoundID.NPCHit1;//common organic hit sound
            NPC.noGravity = true;
        }
        static int RegularStateDuration => 500;
        static int FastStateDuration => 200;
        bool FastState => NPC.ai[0] % (RegularStateDuration + FastStateDuration) >= RegularStateDuration;
        bool JustEnteredFastState => NPC.ai[0] % (RegularStateDuration + FastStateDuration) == RegularStateDuration;
        Vector2 IdlePosition
        {
            get => new Vector2(NPC.ai[1], NPC.ai[2]);
            set
            {
                NPC.ai[1] = value.X;
                NPC.ai[2] = value.Y;
            }
        }
        bool JustStartedIdling { get => NPC.localAI[0] == 1; set => NPC.localAI[0] = value ? 1 : 0; }
        public override void AI()
        {
            float maxSpeedX = 4;
            float accelerationX = 0.1f;
            float maxSpeedY = 1.5f;
            float accelerationY = 0.04f;
            FindTargetAndSetJustStartedIdlingFlag();
            if (IdlePosition == default)
            {
                IdlePosition = NPC.Center;
            }
            if (NPC.target < 0 || NPC.target >= Main.maxPlayers)
            {
                if (JustStartedIdling)
                {
                    NPC.netUpdate = true;
                    JustStartedIdling = false;
                    Dust.QuickDust(IdlePosition, Color.White);

                }
                if (Main.rand.NextFloat() < 0.8f)//8% chance of dust
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PinkTorch, NPC.velocity.X, NPC.velocity.Y, 0, default, 1.5f);
                    d.noGravity = true;
                }
                NPC.direction = MathF.Sign(IdlePosition.X - NPC.Center.X);
                NPC.directionY = MathF.Sign(IdlePosition.Y - NPC.Center.Y);
                BatMovement(maxSpeedX, accelerationX, maxSpeedY, accelerationY);
                NPC.rotation = NPC.velocity.X * .1f;
                NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
                if (FastState)
                {
                    NPC.ai[0] = RegularStateDuration - 1;
                }
                NPC.Opacity = .5f;
                NPC.dontTakeDamage = true;
                return;
            }
            IdlePosition = NPC.Center;
            NPC.Opacity = 1;
            NPC.dontTakeDamage = false;
            Player player = Main.player[NPC.target];
            NPC.direction = MathF.Sign(player.Center.X - NPC.Center.X);
            NPC.directionY = MathF.Sign(player.Center.Y - NPC.Center.Y);
            NPC.ai[0]++;

            if (FastState)
            {
                if (JustEnteredFastState)
                {
                    NPC.netUpdate = true;
                    int numDots = 32;
                    for (float i = 0; i < 1; i += 1f / numDots)
                    {
                        Vector2 offset = (i * MathF.Tau).ToRotationVector2() * 20;
                        offset.X *= .5f;
                        offset = offset.RotatedBy(NPC.velocity.ToRotation());
                        Dust d = Dust.NewDustPerfect(NPC.Center + offset, DustID.PinkTorch, offset * .1f, 0, Color.White, 2);
                        d.noGravity = true;
                    }
                }
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PinkTorch, NPC.velocity.X, NPC.velocity.Y);
                dust.noGravity = true;
                dust.scale += 1;
                maxSpeedX *= 3;
                accelerationX *= 3;
                maxSpeedY *= 3;
                accelerationY *= 8;
            }
            BatMovement(maxSpeedX, accelerationX, maxSpeedY, accelerationY);
            NPC.rotation = NPC.velocity.X * .1f;
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (FastState)
            {
                NPC.frameCounter++;
            }
            int frameSpeed = 7;
            int framey = (int)(NPC.frameCounter / frameSpeed % Main.npcFrameCount[Type]) * frameHeight;
            NPC.frame.Y = framey;
        }
        private void BatMovement(float maxSpeedX, float accelerationX, float maxSpeedY, float accelerationY)
        {
            if (NPC.collideX)
            {
                NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = 2f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            if (NPC.collideY)
            {
                NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                {
                    NPC.velocity.Y = 1f;
                }
                if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                {
                    NPC.velocity.Y = -1f;
                }
            }
            if (NPC.direction == -1 && NPC.velocity.X > -maxSpeedX)
            {
                NPC.velocity.X -= accelerationX;
                if (NPC.velocity.X > maxSpeedX)
                {
                    NPC.velocity.X -= accelerationX;
                }
                else if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X += accelerationX * 0.5f;
                }
                if (NPC.velocity.X < -maxSpeedX)
                {
                    NPC.velocity.X = -maxSpeedX;
                }
            }
            else if (NPC.direction == 1 && NPC.velocity.X < maxSpeedX)
            {
                NPC.velocity.X += accelerationX;
                if (NPC.velocity.X < -maxSpeedX)
                {
                    NPC.velocity.X += accelerationX;
                }
                else if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X -= accelerationX * .5f;
                }
                if (NPC.velocity.X > maxSpeedX)
                {
                    NPC.velocity.X = maxSpeedX;
                }
            }
            if (NPC.directionY == -1 && NPC.velocity.Y > -maxSpeedY)
            {
                NPC.velocity.Y -= accelerationY;
                if (NPC.velocity.Y > maxSpeedY)
                {
                    NPC.velocity.Y -= accelerationY * 1.25f;
                }
                else if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y += accelerationY * 0.3f;
                }
                if (NPC.velocity.Y < -maxSpeedY)
                {
                    NPC.velocity.Y = -maxSpeedY;
                }
            }
            else if (NPC.directionY == 1 && NPC.velocity.Y < maxSpeedY)
            {
                NPC.velocity.Y += accelerationY;
                if (NPC.velocity.Y < -maxSpeedY)
                {
                    NPC.velocity.Y += accelerationY * 1.25f;
                }
                else if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y -= accelerationY * .75f;
                }
                if (NPC.velocity.Y > maxSpeedY)
                {
                    NPC.velocity.Y = maxSpeedY;
                }
            }
            if (NPC.wet)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= 0.95f;
                }
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
               FindTargetAndSetJustStartedIdlingFlag();
            }
            else
            {
                if (NPC.direction == -1 && NPC.velocity.X > -maxSpeedX)
                {
                    NPC.velocity.X -= accelerationX;
                    if (NPC.velocity.X > maxSpeedX)
                    {
                        NPC.velocity.X -= accelerationX;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X += accelerationX * 0.5f;
                    }
                    if (NPC.velocity.X < -maxSpeedX)
                    {
                        NPC.velocity.X = -maxSpeedX;
                    }
                }
                else if (NPC.direction == 1 && NPC.velocity.X < maxSpeedX)
                {
                    NPC.velocity.X += accelerationX;
                    if (NPC.velocity.X < -maxSpeedX)
                    {
                        NPC.velocity.X += accelerationX;
                    }
                    else if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X -= accelerationX * .5f;
                    }
                    if (NPC.velocity.X > maxSpeedX)
                    {
                        NPC.velocity.X = maxSpeedX;
                    }
                }
                if (NPC.directionY == -1 && NPC.velocity.Y > -maxSpeedY)
                {
                    NPC.velocity.Y -= accelerationY;
                    if (NPC.velocity.Y > maxSpeedY)
                    {
                        NPC.velocity.Y -= accelerationY * 1.25f;
                    }
                    else if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y += accelerationY * 0.75f;
                    }
                    if (NPC.velocity.Y < -maxSpeedY)
                    {
                        NPC.velocity.Y = -maxSpeedY;
                    }
                }
                else if (NPC.directionY == 1 && NPC.velocity.Y < maxSpeedY)
                {
                    NPC.velocity.Y += accelerationY;
                    if (NPC.velocity.Y < -maxSpeedY)
                    {
                        NPC.velocity.Y += accelerationY * 1.25f;
                    }
                    else if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y -= accelerationY * .75f;
                    }
                    if (NPC.velocity.Y > maxSpeedY)
                    {
                        NPC.velocity.Y = maxSpeedY;
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;

            if (NPC.Opacity != 1)
            {
                EchosphereHelper.SpectralDrawMinusOneIsNoFlip(NPC, spriteBatch, screenPos, texture);
            }
            else
            {
                float opacity = 1;
                if (FastState)
                {
                    drawColor.A = 0;
                    opacity = .3f;
                }
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor * opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
            if (FastState)
            {
                drawColor.A = 0;
                for (float i = 1; i <= 10; i++)
                {
                    Vector2 offset = Vector2.Normalize(NPC.velocity) * i;
                    spriteBatch.Draw(texture, NPC.Center - screenPos + offset, NPC.frame, drawColor * .3f, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.Draw(texture, NPC.Center - screenPos - offset, NPC.frame, drawColor * .3f, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                }
            }
            return false;
        }
        void FindTargetAndSetJustStartedIdlingFlag()
        {
            int oldTarget = NPC.target;
            EchosphereHelper.SearchForAirbornePlayers(NPC);
            if(NPC.target == -1 && oldTarget != -1)
            {
                JustStartedIdling = true;
            }
        }
    }
}
