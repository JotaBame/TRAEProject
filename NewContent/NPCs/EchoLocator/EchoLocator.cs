using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.EchoLocator
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
            NPC.noGravity = true;
        }
        static int RegularStateDuration => 700;
        static int FastStateDuration => 200;
        bool FastState => NPC.ai[0] % (RegularStateDuration + FastStateDuration) >= RegularStateDuration;
        bool JustEnteredFastState => NPC.ai[0] % (RegularStateDuration + FastStateDuration) == RegularStateDuration;
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.ai[0]++;
            float maxSpeedX = 4;
            float accelerationX = 0.1f;
            float maxSpeedY = 1.5f;
            float accelerationY = 0.04f;
            if (FastState)
            {
                if (JustEnteredFastState)
                {
                    int numDots = 32;
                    for (float i = 0; i < 1; i += 1f/numDots)
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
                maxSpeedX *= 3;
                accelerationX *= 3;
                maxSpeedY *= 3;
                accelerationY *= 3;
            }
            BatMovement(maxSpeedX, accelerationX, maxSpeedY, accelerationY);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (FastState)
            {
                NPC.frameCounter++;
            }
            int frameSpeed = 7;
            int framey = (int)((NPC.frameCounter / frameSpeed) % Main.npcFrameCount[Type]) * frameHeight;
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
                NPC.TargetClosest();
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

            Main.EntitySpriteDraw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

            if (FastState)
            {
                drawColor.A = 0;
                for (float i = 1; i <= 10; i++)
                {
                    Vector2 offset = Vector2.Normalize(NPC.velocity) * i;
                    Main.EntitySpriteDraw(texture, NPC.Center - screenPos + offset, NPC.frame, drawColor * .3f, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                    Main.EntitySpriteDraw(texture, NPC.Center - screenPos - offset, NPC.frame, drawColor * .3f, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

                }
            }
            return false;
        }
    }
}
