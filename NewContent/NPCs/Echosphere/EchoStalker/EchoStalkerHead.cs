using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Echosphere.EchoStalker.Gore;
using TRAEProject.NewContent.Projectiles;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoStalker
{
    public class EchoStalkerHead : ModNPC
    {
        public static SoundStyle ShotSFXOld => new("TRAEProject/Assets/Sounds/SonicWave");//in case it is ever needed again
        public static SoundStyle ShotSFX => new("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerShot");
        public static SoundStyle HitSFX => new("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerHit");
        public static SoundStyle DeathSFX => new("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerDeath");
        static Asset<Texture2D> head;
        static Asset<Texture2D> hair;
        static Asset<Texture2D> jaw;
        static Asset<Texture2D> body;
        static Asset<Texture2D> bodyGlow;
        static Asset<Texture2D> body2;
        static Asset<Texture2D> body2Glow;
        static Asset<Texture2D> bodyWithHair;
        static Asset<Texture2D> bodyWithHairGlow;
        static Asset<Texture2D> body2WithHair;
        static Asset<Texture2D> body2WithHairGlow;
        static Asset<Texture2D> tail;
        static Asset<Texture2D> tailGlow;
        public bool HairVariant { get => NPC.localAI[0] == 1; set => NPC.localAI[0] = value ? -1 : 1; }
        public static Texture2D Tail => tail?.Value;
        public static Texture2D TailGlow => tailGlow?.Value;
        public static Texture2D Body => body?.Value;
        public static Texture2D BodyGlow => bodyGlow?.Value;
        public static Texture2D Body2 => body2?.Value;
        public static Texture2D Body2Glow => body2Glow?.Value;
        public static Texture2D BodyWithHair => bodyWithHair?.Value;
        public static Texture2D BodyWithHairGlow => bodyWithHairGlow?.Value;
        public static Texture2D Body2WithHair => body2WithHair?.Value;
        public static Texture2D Body2WithHairGlow => body2WithHairGlow?.Value;
        static int[] SegmentWidths => new int[5] { 28, 28, 28, 26, 28 };
        private float PurpleGlowinessAmount => Utils.GetLerpValue(60, 110, NPC.ai[0], true) * Utils.GetLerpValue(160, 140, NPC.ai[0], true) * 0.2f;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 100;
            NPCID.Sets.TrailingMode[Type] = 3;
            if (!Main.dedServ)
            {
                TextureLoading();
            }
        }
        Vector2 MouthCenter { get => NPC.Center + new Vector2(0, 4); }
        ref float IdlingTimer => ref NPC.localAI[2];
        public static int SegmentCount => 5;
        public override void SetDefaults()
        {
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Size = new(50);
            NPC.scale = 1.2f;
            NPC.lifeMax = 2250;
            NPC.defense = 15;
            NPC.damage = 50;
            NPC.knockBackResist = 0;
            NPC.HitSound = HitSFX;
            NPC.DeathSound = DeathSFX;
        }
        static int[] OrderOfSegmentIDsToSpawn => [ModContent.NPCType<EchoStalkerBody1>(),
               ModContent.NPCType<EchoStalkerBody2>(), ModContent.NPCType<EchoStalkerBody2>(),
             ModContent.NPCType<EchoStalkerTail>()];
        static int[] SegmentIDs => [ModContent.NPCType<EchoStalkerBody1>(), ModContent.NPCType<EchoStalkerBody2>(), ModContent.NPCType<EchoStalkerTail>()];
        public override void OnSpawn(IEntitySource source)
        {

            //NPC.localAI[0] = Main.rand.NextBool() ? -1 : 1;
            HairVariant = Main.rand.NextBool(4);

            int[] types = OrderOfSegmentIDsToSpawn;
            for (int i = 1; i < types.Length + 1; i++)
            {
                NPC.NewNPC(Terraria.Entity.GetSource_NaturalSpawn(), (int)NPC.Center.X - i * 40, (int)NPC.Center.Y, types[i - 1], NPC.whoAmI, NPC.whoAmI, i * 11, i);
            }
        }
        public override void AI()
        {
     

            EchosphereNPCHelper.SearchForSpaceLayerPlayers(NPC);
            if (NPC.target == -1 || NPC.target >= Main.maxPlayers)
            {
                NPC.dontTakeDamage = true;
                NPC.Opacity = .5f;
                IdlingTimer += .01f;
                NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(MathF.Sin(IdlingTimer), MathF.Cos(IdlingTimer * 1.61f) * .75f) * 5, .1f);
                NPC.rotation = NPC.velocity.ToRotation();
                NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
                SetSegmentPositionRotationSpriteDirectionAndOpacity();
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
                SoundEngine.PlaySound(ShotSFX with { MaxInstances = 0 }, NPC.Center);
            }
            if (NPC.ai[0] >= 107 && (NPC.ai[0] - 107) % fireRate == 0 && NPC.ai[0] <= 107 + fireRate * numberOfShots)
            {
                Vector2 projVel = NPC.DirectionTo(Main.player[NPC.target].Center) * 9;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), MouthCenter, projVel, ModContent.ProjectileType<EchoStalkerSonicWave>(), 30, 0, Main.myPlayer, .6f);
                for (float i = 0; i < 1; i += 1f / 40f)
                {
                    Vector2 vel = (i * MathF.Tau).ToRotationVector2();
                    vel.X *= 0.5f;
                    vel = vel.RotatedBy(projVel.ToRotation()) + projVel;
                    Dust dust = Dust.NewDustPerfect(MouthCenter + vel * 3, DustID.Shadowflame, vel * 3);
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
            SetSegmentPositionRotationSpriteDirectionAndOpacity();
        }
        static float Magnitude(Vector2 vec)
        {
            return MathF.Abs(vec.X) + MathF.Abs(vec.Y);
        }
        void Movement(Player player)
        {
            if (NPC.velocity.Length() == 0)
                NPC.velocity.X = 1f;
            float topSpeed = 7.5f;
            float acceleration = 0.35f;
            if (NPC.ai[0] >= 100 && NPC.ai[0] < 140)
            {
                topSpeed = 3;
            }
            Vector2 vector5 = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
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
            // DrawBody(screenPos, drawColor);
            Color additiveWhite = new(255, 255, 255, 0);
            SpriteEffects spriteFX = NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Vector2 origin = new(NPC.spriteDirection == 1 ? 8 : 4, NPC.spriteDirection == 1 ? -5 : -5);
            origin *= 0.5f;
            GetHeadRotationOffset(out float headRot, out float jawRot, out float rotationProgress);
            float opacity = PurpleGlowinessAmount;
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
            // if (NPC.localAI[0] == 1)
            if (HairVariant)
            {
                DrawWithSpectralCheck(hair.Value, NPC.Center - screenPos, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, spriteFX, spriteBatch);
                EchosphereNPCHelper.DrawEchoWormBlurOld(hair.Value, NPC.Center - screenPos, NPC.Opacity * opacity, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
            }
            return false;
        }
        void DrawWithSpectralCheck(Texture2D texture, Vector2 drawPos, Rectangle? frame, Color drawColor, float rotation, Vector2 origin, float scale, SpriteEffects spriteFX, SpriteBatch spriteBatch)
        {
            if (NPC.dontTakeDamage)
            {
                EchosphereNPCHelper.SpectralDraw(spriteBatch, NPC.Opacity, NPC.scale, rotation, drawPos, texture, spriteFX, frame, origin);
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
            Vector2 origin = new(NPC.spriteDirection == 1 ? 14 : jaw.Width() - 14, 18);
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

            bodyWithHair ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBodyWithHair");
            bodyWithHairGlow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBodyWithHairGlow");
            body2WithHair ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBody2WithHair");
            body2WithHairGlow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBody2WithHairGlow");
        }
        void DrawGlowy(Texture2D texture, Vector2 drawPos, Vector2 origin, SpriteEffects spriteFX, float rotation)
        {
            Color additiveWhite = new(255, 255, 255, 0);
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
            GetSegmentDrawParams(screenPos, out Vector2 origin, out int segments, out Vector2[] positions, out SpriteEffects[] directions, out float[] rotations, out Color[] colors);

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
                    EchosphereNPCHelper.SpectralDraw(Main.spriteBatch, NPC.Opacity, NPC.scale, rotation, drawPos, texture, spriteDir, null, origin);
                    EchosphereNPCHelper.SpectralDraw(Main.spriteBatch, NPC.Opacity, NPC.scale, rotation, drawPos, glow, spriteDir, null, origin);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, drawPos, null, segmentDrawColor, rotation, origin, NPC.scale, spriteDir);
                    Main.EntitySpriteDraw(glow, drawPos, null, segmentDrawColor, rotation, origin, NPC.scale, spriteDir);
                }
                DrawGlowy(glow, drawPos, origin, spriteDir, rotation);
            }
        }
        void SetSegmentPositionRotationSpriteDirectionAndOpacity()
        {
            NPC[] segments = SearchForBodySegments();
            int segmentCount = segments.Length;
            Vector2 lastSegmentCenter = NPC.Center;
            int[] segmentWidths = SegmentWidths;
            int lengthAcross = 0;
            float purpleGlowiness = PurpleGlowinessAmount;
            for (int i = 0; i < segmentCount; i++)
            {
                NPC curSegment = segments[i];
                int segmentWidth = segmentWidths[i];
                if (curSegment == null)
                {
                    continue;
                }
                Vector2 segmentCenter = NPC.oldPos[(int)segments[i].ai[1]] + NPC.Size / 2f;
                float rotation = (lastSegmentCenter - segmentCenter).ToRotation();
                segmentCenter = lastSegmentCenter - rotation.ToRotationVector2() * segmentWidth;
                rotation = (lastSegmentCenter - segmentCenter).ToRotation() + MathF.PI / 2f;
                Vector2 snapPos = segmentCenter;
                int spriteDir = segmentCenter.X >= lastSegmentCenter.X ? -1 : 1;//    -1 should be flip vertically remember!!
                segments[i].Center = snapPos;
                segments[i].spriteDirection = spriteDir;
                segments[i].rotation = rotation - MathF.PI / 2;
                segments[i].alpha = NPC.alpha;
                lengthAcross += segmentWidths[i];
                lastSegmentCenter = segmentCenter;
                EchoStalkerBody1.SetPurpleGlowinessAmount(segments[i], purpleGlowiness);
            }
        }
        private void GetSegmentDrawParams(Vector2 screenPos, out Vector2 origin, out int segments, out Vector2[] positions, out SpriteEffects[] directions, out float[] rotations, out Color[] colors)
        {
            origin = body.Size() / 2;
            segments = 4;
            Vector2 lastSegmentCenter = NPC.Center;


            //this whole thing is here for layering purposes
            positions = new Vector2[segments];
            directions = new SpriteEffects[segments];
            rotations = new float[segments];
            colors = new Color[segments];
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
            bodyWithHair = null;
            bodyWithHairGlow = null;
            body2WithHair = null;
            body2WithHairGlow = null;
        }


        public static bool IsHeadImmuneToItem(float headNPCIndex, int playerIndex)
        {
            int index = (int)headNPCIndex;
            if (!Main.npc.IndexInRange(index))
            {
                return true;
            }
            NPC npc = Main.npc[index];
            if (npc.type != ModContent.NPCType<EchoStalkerHead>())
            {
                return true;
            }
            Player player = Main.player[playerIndex];
            return player.meleeNPCHitCooldown[index] > 0;
        }
        public static void CopyProjIframesToOtherSegments(float headNPCIndex, int fromIndex, Projectile proj)
        {
            int index = (int)headNPCIndex;
            if (!Main.npc.IndexInRange(index) || !Main.npc.IndexInRange(fromIndex))
            {
                return;
            }
            NPC headNPC = Main.npc[index];
            if (headNPC.type != ModContent.NPCType<EchoStalkerHead>())
            {
                return;
            }
            if (headNPC.ModNPC is EchoStalkerHead stalkerHead)
            {
                NPC fromNPC = Main.npc[fromIndex];
                NPC[] segments = stalkerHead.SearchForBodySegments();
                if (segments != null && segments.Length > 0)
                {
                    for (int i = 0; i < segments.Length; i++)
                    {
                        NPC segment = segments[i];
                        if (segment == null || segment.whoAmI == fromIndex)
                        {
                            continue;
                        }
                        CopyProjIframesFromNPCToNPC(fromNPC, segment, proj);
                    }
                    if (headNPCIndex != fromIndex)
                    {
                        CopyProjIframesFromNPCToNPC(fromNPC, headNPC, proj);
                    }
                }
            }
      
        }
        public static void CopyItemIframesToOtherSegments(float headNPCIndex, int fromIndex, int playerIndex)
        {
            int index = (int)headNPCIndex;
            if (!Main.npc.IndexInRange(index) || !Main.npc.IndexInRange(fromIndex))
            {
                return;
            }
            NPC headNPC = Main.npc[index];
            if (headNPC.type != ModContent.NPCType<EchoStalkerHead>())
            {
                return;
            }
            if (headNPC.ModNPC is EchoStalkerHead stalkerHead)
            {
                NPC fromNPC = Main.npc[fromIndex];
                NPC[] segments = stalkerHead.SearchForBodySegments();
                if (segments != null && segments.Length > 0)
                {
                    for (int i = 0; i < segments.Length; i++)
                    {
                        NPC segment = segments[i];
                        if (segment.whoAmI == fromIndex)
                        {
                            continue;
                        }
                        CopyItemIframesFromNPCToNPC(fromNPC, segment, playerIndex);
                    }
                    if (headNPCIndex != fromIndex)
                    {
                        CopyItemIframesFromNPCToNPC(fromNPC, headNPC, playerIndex);
                    }
                }
            }
        }
        static void CopyItemIframesFromNPCToNPC(NPC from, NPC to, int playerIndex)
        {
            Player plr = Main.player[playerIndex];
            plr.meleeNPCHitCooldown[to.whoAmI] = plr.meleeNPCHitCooldown[from.whoAmI];
        }
        static void CopyProjIframesFromNPCToNPC(NPC from, NPC to, Projectile proj)
        {
            proj.localNPCImmunity[to.whoAmI] = proj.localNPCImmunity[from.whoAmI];
            to.immune[proj.owner] = from.immune[proj.owner];
            Projectile.perIDStaticNPCImmunity[proj.type][to.whoAmI] = Projectile.perIDStaticNPCImmunity[proj.type][from.whoAmI];
        }
        public static bool IsHeadImmuneToProj(float headNPCIndex, Projectile proj)
        {
            if (!proj.friendly)
            {
                return true;
            }
            int index = (int)headNPCIndex;
            if (index < 0 || index >= Main.maxNPCs)
            {
                return true;
            }
            NPC npc = Main.npc[index];
            if (npc.type != ModContent.NPCType<EchoStalkerHead>())
            {
                return true;
            }

            if (npc.immune[proj.owner] > 0)
            {
                return true;
            }
            if (Projectile.perIDStaticNPCImmunity[proj.type][npc.whoAmI] > 0)
            {
                return true;
            }
            if (proj.localNPCImmunity[npc.whoAmI] > 0)
            {
                return true;
            }
            return false;
        }
    
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return !IsHeadImmuneToItem(NPC.whoAmI, player.whoAmI);
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return !IsHeadImmuneToProj(NPC.whoAmI, projectile);
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            
            CopyProjIframesToOtherSegments(NPC.whoAmI, NPC.whoAmI, projectile);
        }
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
   
            CopyItemIframesToOtherSegments(NPC.whoAmI, NPC.whoAmI, player.whoAmI);
        }
        NPC[] SearchForBodySegments()
        {
            NPC[] segments = new NPC[SegmentCount - 1];
            int segmentsFound = 0;
            List<int> types = new() { ModContent.NPCType<EchoStalkerBody1>(), ModContent.NPCType<EchoStalkerBody2>(), ModContent.NPCType<EchoStalkerTail>() };
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && types.Contains(npc.type) && npc.ai[0] == NPC.whoAmI && segmentsFound < segments.Length)
                {
                    npc.realLife = npc.whoAmI;
                    npc.dontTakeDamage = false;
                    segments[segmentsFound] = npc;
                    segmentsFound++;
                }
            }
            NPC[] result = new NPC[segments.Length];
            for (int i = 0; i < segmentsFound; i++)
            {
                result[(int)segments[i].ai[2] - 1] = segments[i];
            }
            return result;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
              
              
                EchosphereNPCHelper.EchosphereEnemyDeathDust(NPC);
                List<NPC> segments = new(5);
                segments.Add(NPC);
                int[] types = SegmentIDs;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active)
                    {
                        continue;
                    }
                    if (types.Contains(npc.type) && npc.ai[2] == NPC.whoAmI)
                    {
                        segments.Add(npc);
                    }
                }
                if (segments.Count > 0)
                {
                    for (int i = 0; i < segments.Count; i++)
                    {
                        NPC npc = segments[i];
                        if (npc == null)
                        {
                            continue;
                        }
                        int[] goreTypes = GetGoreTypes(npc);
                        if (goreTypes.Length > 0)
                        {
                            for (int j = 0; j < goreTypes.Length; j++)
                            {
                                Terraria.Gore.NewGore(npc.GetSource_Death(), npc.Center, npc.position - npc.oldPosition, goreTypes[j]);
                            }
                        }
                    }
                }
                NPC.life = 0;
                 NPC.active = false;
              
            }
        }
        static int[] GetGoreTypes(NPC npc)
        {
            List<int> types = new(2);
            if (npc.type == ModContent.NPCType<EchoStalkerBody1>())
            {
                if (IsHairVariant(npc))
                {
                    types.Add(ModContent.GoreType<EchoStalkerGoreBody1Hair>());
                    types.Add(ModContent.GoreType<EchoStalkerGoreBody1Hairless>());
                }
                else
                {
                    types.Add(ModContent.GoreType<EchoStalkerGoreBody1>());
                }
            }
            else if (npc.type == ModContent.NPCType<EchoStalkerBody2>())
            {
                if (IsHairVariant(npc))
                {
                    types.Add(ModContent.GoreType<EchoStalkerGoreBody2Hair>());
                    types.Add(ModContent.GoreType<EchoStalkerGoreBody2Hairless>());
                }
                else
                {
                    types.Add(ModContent.GoreType<EchoStalkerGoreBody2>());
                }
            }
            else if (npc.type == ModContent.NPCType<EchoStalkerTail>())
            {
                types.Add(ModContent.GoreType<EchoStalkerGoreTail1>());
                types.Add(ModContent.GoreType<EchoStalkerGoreTail2>());
            }
            else//head
            {
                types.Add(ModContent.GoreType<EchoStalkerGoreHeadHairless>());
                if (IsHairVariant(npc))
                {
                    types.Add(ModContent.GoreType<EchoStalkerGoreHeadHair>());
                }
            }
            return types.ToArray();
        }
        public static bool IsHairVariant(NPC echoStalkerSegment)
        {
            return echoStalkerSegment.localAI[0] == 1;
        }
        internal static bool IsIdle(float headWhoAmI)
        {
            int index = (int)headWhoAmI;
            if (index < 0 || index >= Main.maxNPCs)
            {
                return false;
            }
            NPC stalkerNPC = Main.npc[index];
            return !stalkerNPC.HasPlayerTarget;
        }
    }
}
