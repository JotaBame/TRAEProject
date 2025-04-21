using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Projectiles;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoStalker
{
    public class EchoStalker : ModNPC
    {
        static Asset<Texture2D> head;
        static Asset<Texture2D> hair;
        static Asset<Texture2D> jaw;
        static Asset<Texture2D> body;
        static Asset<Texture2D> bodyGlow;
        static Asset<Texture2D> body2;
        static Asset<Texture2D> body2Glow;
        static Asset<Texture2D> tail;
        static Asset<Texture2D> tailGlow;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 100;
            NPCID.Sets.TrailingMode[Type] = 3;
        }
        Vector2 MouthCenter { get => NPC.Center + new Vector2(0, 4); }
        ref float IdlingTimer => ref NPC.localAI[2];
        public override void SetDefaults()
        {
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Size = new(50);
            NPC.scale = 1.2f;
            NPC.lifeMax = 1500;
            NPC.defense = 15;
            NPC.damage = 70;
            NPC.knockBackResist = 0;
        }
        public override void AI()
        {

            EchosphereHelper.SearchForAirbornePlayers(NPC);
            if(NPC.target == -1 || NPC.target >= Main.maxPlayers)
            {
                NPC.dontTakeDamage = true;
                NPC.Opacity = .5f;
                IdlingTimer += .01f;
                NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(MathF.Sin(IdlingTimer), MathF.Cos(IdlingTimer * 1.61f) * .75f) * 5, .1f);
                NPC.rotation = NPC.velocity.ToRotation();
                NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
                return;
            }
            NPC.Opacity = 1;
            NPC.dontTakeDamage = false;
            Player player = Main.player[NPC.target];

            Movement(player);

            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            float fireRate = 10;
            float numberOfShots = 2;
            if (NPC.ai[0] == 100)
            {
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/SonicWave") with { MaxInstances = 0 }, NPC.Center);
            }
            if (NPC.ai[0] >= 107 && (NPC.ai[0] - 107) % fireRate == 0 && NPC.ai[0] <= 107 + fireRate * numberOfShots)
            {
                Vector2 projVel = NPC.DirectionTo(Main.player[NPC.target].Center) * 18;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), MouthCenter, projVel, ModContent.ProjectileType<EchoStalkerSonicWave>(), 65, 0, Main.myPlayer, .6f);
                for (float i = 0; i < 1; i += 1f / 40f)
                {
                    Vector2 vel = (i * MathF.Tau).ToRotationVector2();
                    vel.X *= 0.5f;
                    vel = vel.RotatedBy(projVel.ToRotation()) + projVel;
                    Dust dust = Dust.NewDustPerfect(MouthCenter + vel * 3, DustID.Shadowflame, vel * 5);
                    dust.noGravity = true;
                }
            }
            if (NPC.ai[0] <= 107 && NPC.ai[0] > 50)
            {
                float dustRange = EaseInOut(Utils.GetLerpValue(50, 65, NPC.ai[0], true)) * 12;
                Vector2 posOffset = Vector2.UnitX * dustRange * Main.rand.NextFloat() - new Vector2(9, 0);
                posOffset = posOffset.RotatedBy(NPC.rotation + NPC.spriteDirection * 0.17f);
                Dust.NewDustPerfect(NPC.Center + new Vector2(0, 4) + posOffset, DustID.CorruptTorch, Main.rand.NextVector2Circular(1, 1));
                Dust.NewDustPerfect(NPC.Center + new Vector2(0, 4) + posOffset, DustID.CorruptTorch, Main.rand.NextVector2Circular(1, 1));
            }
            NPC.ai[0]++;
            float diff = MathF.Abs(NPC.velocity.ToRotation() - (player.Center - NPC.Center).ToRotation());
            if (diff > MathF.PI)
                diff = MathF.Tau - diff;
            if (diff > .6f && NPC.ai[0] == 100)
            {
                NPC.ai[0] = 99;
            }
            NPC.ai[0] %= 60 * 4;//loop

        }
        static float Magnitude(Vector2 vec)
        {
            return MathF.Abs(vec.X) + MathF.Abs(vec.Y);
        }
        void Movement(Player player)
        {

            float topSpeed = 8;
            float acceleration = 0.3f;
            if (NPC.ai[0] >= 100 && NPC.ai[0] < 140)
            {
                topSpeed = 3;
            }
            Vector2 vector5 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float maxSpeedX = player.position.X + player.width / 2;
            float maxSpeedY = player.position.Y + player.height / 2;
            maxSpeedX = (int)(maxSpeedX / 16f) * 16;
            maxSpeedY = (int)(maxSpeedY / 16f) * 16;
            vector5.X = (int)(vector5.X / 16f) * 16;
            vector5.Y = (int)(vector5.Y / 16f) * 16;
            maxSpeedX -= vector5.X;
            maxSpeedY -= vector5.Y;
            float num68 = (float)Math.Sqrt(maxSpeedX * maxSpeedX + maxSpeedY * maxSpeedY);
            float num71 = Math.Abs(maxSpeedX);
            float num72 = Math.Abs(maxSpeedY);
            float normalizingFactor = topSpeed / num68;
            maxSpeedX *= normalizingFactor;
            maxSpeedY *= normalizingFactor;

            Vector2 targetCenter = player.Center;
            Vector2 targetPos = player.position;
            bool goDown = false;
            if ((NPC.velocity.X > 0f && maxSpeedX < 0f || NPC.velocity.X < 0f && maxSpeedX > 0f || NPC.velocity.Y > 0f && maxSpeedY < 0f || NPC.velocity.Y < 0f && maxSpeedY > 0f) && Magnitude(NPC.velocity) > acceleration / 2f && num68 < 300f)
            {
                goDown = true;
                if (Magnitude(NPC.velocity) < topSpeed)
                {
                    NPC.velocity *= 1.1f;
                }
            }
            if (NPC.position.Y > targetPos.Y || targetPos.Y / 16f > Main.worldSurface || player.dead)
            {
                goDown = true;
                if (Math.Abs(NPC.velocity.X) < topSpeed / 2f)
                {
                    if (NPC.velocity.X == 0f)
                    {
                        NPC.velocity.X -= NPC.direction;
                    }
                    NPC.velocity.X *= 1.1f;
                }
                else if (NPC.velocity.Y > 0f - topSpeed)
                {
                    NPC.velocity.Y -= acceleration;
                }
            }


            if (!goDown)
            {
                if (NPC.velocity.X > 0f && maxSpeedX > 0f || NPC.velocity.X < 0f && maxSpeedX < 0f || NPC.velocity.Y > 0f && maxSpeedY > 0f || NPC.velocity.Y < 0f && maxSpeedY < 0f)
                {
                    if (NPC.velocity.X < maxSpeedX)
                    {
                        NPC.velocity.X += acceleration;
                    }
                    else if (NPC.velocity.X > maxSpeedX)
                    {
                        NPC.velocity.X -= acceleration;
                    }
                    if (NPC.velocity.Y < maxSpeedY)
                    {
                        NPC.velocity.Y += acceleration;
                    }
                    else if (NPC.velocity.Y > maxSpeedY)
                    {
                        NPC.velocity.Y -= acceleration;
                    }
                    if (Math.Abs(maxSpeedY) < topSpeed * 0.2 && (NPC.velocity.X > 0f && maxSpeedX < 0f || NPC.velocity.X < 0f && maxSpeedX > 0f))
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y += acceleration * 2f;
                        }
                        else
                        {
                            NPC.velocity.Y -= acceleration * 2f;
                        }
                    }
                    if (Math.Abs(maxSpeedX) < topSpeed * 0.2 && (NPC.velocity.Y > 0f && maxSpeedY < 0f || NPC.velocity.Y < 0f && maxSpeedY > 0f))
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X += acceleration * 2f;
                        }
                        else
                        {
                            NPC.velocity.X -= acceleration * 2f;
                        }
                    }
                }
                else if (num71 > num72)
                {
                    if (NPC.velocity.X < maxSpeedX)
                    {
                        NPC.velocity.X += acceleration * 1.1f;
                    }
                    else if (NPC.velocity.X > maxSpeedX)
                    {
                        NPC.velocity.X -= acceleration * 1.1f;
                    }
                    if (Magnitude(NPC.velocity) < topSpeed * 0.5)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y += acceleration;
                        }
                        else
                        {
                            NPC.velocity.Y -= acceleration;
                        }
                    }
                }
                else
                {
                    if (NPC.velocity.Y < maxSpeedY)
                    {
                        NPC.velocity.Y += acceleration * 1.1f;
                    }
                    else if (NPC.velocity.Y > maxSpeedY)
                    {
                        NPC.velocity.Y -= acceleration * 1.1f;
                    }
                    if (Magnitude(NPC.velocity) < topSpeed * 0.5)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X += acceleration;
                        }
                        else
                        {
                            NPC.velocity.X -= acceleration;
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TextureLoading();
            DrawBody(screenPos, drawColor);
            Color additiveWhite = new Color(255, 255, 255, 0);

            SpriteEffects spriteFX = NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;

            Vector2 origin = new Vector2(NPC.spriteDirection == 1 ? 8 : 4, NPC.spriteDirection == 1 ? -5 : -5);
            origin *= 0.5f;
            GetHeadRotationOffset(out float headRot, out float jawRot, out float rotationProgress);

            float opacity = Utils.GetLerpValue(60, 110, NPC.ai[0], true) * Utils.GetLerpValue(160, 140, NPC.ai[0], true) * 0.2f;
            Vector2 jawOffset = AngleLerp(default, origin.RotatedBy(jawRot + NPC.rotation), MathF.Abs(jawRot));// jawRot.ToRotationVector2() * 10;
            jawOffset += NPC.rotation.ToRotationVector2() * MathF.Abs(jawRot) * 10;
            jawRot += NPC.rotation;
            origin = head.Size() / 2;
            DrawWithSpectralCheck(jaw.Value, NPC.Center - screenPos + jawOffset, null, drawColor, jawRot, origin, NPC.scale, spriteFX, spriteBatch);
            if (opacity > 0)
            {
                for (float i = 0; i < 17; i++)
                {
                    Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy(i / 5.5f * MathF.Tau + NPC.ai[0] * 0.1f);
                    Main.EntitySpriteDraw(jaw.Value, NPC.Center - screenPos + blurOffset + jawOffset, null, additiveWhite * opacity, jawRot, origin, NPC.scale, spriteFX);
                }
            }
            DrawWithSpectralCheck(head.Value, NPC.Center - screenPos, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, spriteFX, spriteBatch);
            DrawWithSpectralCheck(hair.Value, NPC.Center - screenPos, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, spriteFX, spriteBatch);
            if (opacity > 0)
            {
                for (float i = 0; i < 17; i++)
                {
                    Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy(i / 5.5f * MathF.Tau + NPC.ai[0] * 0.1f);
                    Main.EntitySpriteDraw(hair.Value, NPC.Center - screenPos + blurOffset, null, additiveWhite * opacity, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
                }
            }
            return false;
        }
        void DrawWithSpectralCheck(Texture2D texture, Vector2 drawPos, Rectangle? frame, Color drawColor, float rotation, Vector2 origin, float scale, SpriteEffects spriteFX, SpriteBatch spriteBatch)
        {
            if (NPC.dontTakeDamage)
            {
                EchosphereHelper.SpectralDraw(spriteBatch, NPC.Opacity, NPC.scale, rotation, drawPos, texture, spriteFX, frame, origin);
            }
            else
            {
                Main.EntitySpriteDraw(texture, drawPos, null, drawColor, rotation, origin, NPC.scale, spriteFX);
            }
        }
        static Vector2 AngleLerp(Vector2 a, Vector2 b, float t)
        {
            float length = MathHelper.Lerp(a.Length(), b.Length(), t);
            float angle = a.ToRotation().AngleLerp(b.ToRotation(), t);
            return angle.ToRotationVector2() * length;
        }
        Vector2 GetOrigin(float rotation)
        {
            Vector2 origin = new Vector2(NPC.spriteDirection == 1 ? 14 : jaw.Width() - 14, 18);
            return origin.RotatedBy(NPC.rotation) + new Vector2(17, 33);
        }
        private static void TextureLoading()
        {
            head ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerHead");
            jaw ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerJaw");
            hair ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerHair");
            body ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBody");
            bodyGlow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBodyGlow");
            body2 ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBody2");
            body2Glow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBody2Glow");
            tail ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerTail");
            tailGlow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerTailGlow");
        }
        void DrawGlowy(Texture2D texture, Vector2 drawPos, Vector2 origin, SpriteEffects spriteFX, float rotation)
        {
            Color additiveWhite = new Color(255, 255, 255, 0);
            float opacity = Utils.GetLerpValue(60, 110, NPC.ai[0], true) * Utils.GetLerpValue(160, 140, NPC.ai[0], true) * 0.2f * NPC.Opacity;
            if (opacity > 0)
            {
                for (float i = 0; i < 17; i++)
                {
                    Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy(i / 5.5f * MathF.Tau + NPC.ai[0] * 0.1f);
                    Main.EntitySpriteDraw(texture, drawPos + blurOffset, null, additiveWhite * opacity, rotation, origin, NPC.scale, spriteFX);
                }
            }
        }
        void DrawBody(Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = body.Size() / 2;
            int segments = 5;
            Vector2 lastSegmentCenter = NPC.Center;


            //this whole thing is here for layering purposes
            Vector2[] positions = new Vector2[segments];
            SpriteEffects[] directions = new SpriteEffects[segments];
            float[] rotations = new float[segments];
            Color[] colors = new Color[segments];
            for (int i = 1; i < segments; i++)
            {
                int segmentWidth = 28;
                if (i == segments - 1)
                {
                    //tail
                    segmentWidth = 28;
                }
                else if (i > 2)
                {
                    //alt body
                    segmentWidth = 26;
                }
                Vector2 segmentCenter = NPC.oldPos[i * 10] + NPC.Size / 2f;
                float rotation = (lastSegmentCenter - segmentCenter).ToRotation();
                segmentCenter = lastSegmentCenter - rotation.ToRotationVector2() * segmentWidth;
                rotation = (lastSegmentCenter - segmentCenter).ToRotation() + MathF.PI / 2f;
                Vector2 drawPos = segmentCenter - screenPos;
                SpriteEffects spriteDir = segmentCenter.X >= lastSegmentCenter.X ? SpriteEffects.FlipVertically : SpriteEffects.None;
                lastSegmentCenter = segmentCenter;
                colors[i] = NPC.GetAlpha(Lighting.GetColor(segmentCenter.ToTileCoordinates()));
                positions[i] = drawPos;
                directions[i] = spriteDir;
                rotations[i] = rotation;
            }


            for (int i = segments - 1; i >= 1; i--)
            {
                Texture2D texture = body.Value;
                Texture2D glow = bodyGlow.Value;
                if (i == segments - 1)
                {
                    texture = tail.Value;
                    glow = tailGlow.Value;
                }
                else if (i > 2)
                {
                    texture = body2.Value;
                    glow = body2Glow.Value;
                }
                Vector2 drawPos = positions[i];
                SpriteEffects spriteDir = directions[i];
                float rotation = rotations[i];
                rotation -= MathF.PI / 2;
                Color segmentDrawColor = colors[i];
                if (NPC.dontTakeDamage)
                {
                    EchosphereHelper.SpectralDraw(Main.spriteBatch, NPC.Opacity, NPC.scale, rotation, drawPos, texture, spriteDir, null, origin);
                    EchosphereHelper.SpectralDraw(Main.spriteBatch, NPC.Opacity, NPC.scale, rotation, drawPos, glow, spriteDir, null, origin);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, drawPos, null, segmentDrawColor, rotation, origin, NPC.scale, spriteDir);
                    Main.EntitySpriteDraw(glow, drawPos, null, segmentDrawColor, rotation, origin, NPC.scale, spriteDir);
                }
                DrawGlowy(glow, drawPos, origin, spriteDir, rotation);
            }
        }
        void GetHeadRotationOffset(out float headRot, out float jawRot, out float animationProgress)
        {
            headRot = 0;
            jawRot = 0;
            animationProgress = Utils.Remap(NPC.ai[0], 0, 60 * 3, 0, 5);
            if (NPC.dontTakeDamage)
            {
                return;
            }
            switch ((int)animationProgress)
            {
                case 1:
                    animationProgress %= 1;
                    animationProgress *= 0.15f;
                    animationProgress = EaseInOut(animationProgress);
                    headRot += animationProgress * NPC.spriteDirection;
                    jawRot -= animationProgress * NPC.spriteDirection;
                    break;
                case 2:
                    headRot += 0.15f * NPC.spriteDirection;
                    jawRot -= 0.15f * NPC.spriteDirection;
                    if (animationProgress > 2.75f)
                    {
                        animationProgress %= 1;
                        animationProgress = Utils.Remap(animationProgress, 0.75f, 1, 0, 1);
                        animationProgress = EaseInOut(animationProgress) * 0.65f;
                        headRot -= animationProgress * NPC.spriteDirection;
                        jawRot += animationProgress * NPC.spriteDirection;
                    }
                    else
                    {
                        animationProgress %= 1;
                        animationProgress += 0.15f;
                    }
                    break;
                case 3:
                    animationProgress = WobblyEffect(animationProgress % 1) * 0.6f + 0.5f;
                    headRot -= animationProgress * NPC.spriteDirection;
                    jawRot += animationProgress * NPC.spriteDirection;
                    break;
                case 4:
                    animationProgress = (1 - EaseInOut(animationProgress % 1)) * 0.5f;
                    headRot -= animationProgress * NPC.spriteDirection;
                    jawRot += animationProgress * NPC.spriteDirection;
                    break;
                default:
                    animationProgress = 0;
                    break;
            }
        }
        static float WobblyEffect(float progress)
        {
            progress = MathF.Sin(10f * progress / MathF.PI);
            return progress * progress * 0.25f;
        }
        static float EaseInOut(float progress)
        {
            return -MathF.Cos(progress * MathF.PI) * 0.5f + 0.5f;
        }
        static float EasingBackIn(float progress)
        {
            //thanks easings.net for the magic numbers
            return 1 + 2.70158f * MathF.Pow(progress - 1, 3) + 1.70158f * MathF.Pow(progress - 1, 2);
        }
        public override void Unload()
        {
            head = null;
            jaw = null;
            hair = null;
            body = null;
            bodyGlow = null;
            body2 = null;
            body2Glow = null;
            tail = null;
            tailGlow = null;
        }
    }
}
