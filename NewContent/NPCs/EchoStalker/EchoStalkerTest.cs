using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Projectiles;

namespace TRAEProject.NewContent.NPCs.EchoStalker
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
            NPCID.Sets.TrailCacheLength[Type] = 50;
            NPCID.Sets.TrailingMode[Type] = 3;
        }
        Vector2 MouthCenter { get => NPC.Center + new Vector2(0, 4); }
        public override void SetDefaults()
        {
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Size = new(20);
            NPC.lifeMax = 1250;
            NPC.defense = 10;
            NPC.damage = 50;

            NPC.knockBackResist = 0;

        }
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target > 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            NPC.rotation = (player.Center - NPC.Center).ToRotation() + GetExtraRot();

            NPC.velocity = NPC.DirectionTo(Main.MouseWorld) * 10;

            float fireRate = 15;
            float numberOfShots = 2;
            if (NPC.ai[0] == 100)
            {
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/SonicWave") with { MaxInstances = 0 }, NPC.Center);
            }
            if (NPC.ai[0] >= 107 && (NPC.ai[0] - 107) % fireRate == 0 && NPC.ai[0] <= 107 + fireRate * numberOfShots)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), MouthCenter, NPC.DirectionTo(Main.player[NPC.target].Center) * 6f, ModContent.ProjectileType<EchoStalkerSonicWave>(), 65 / 2, 0, Main.myPlayer, 1f);
                for (float i = 0; i < 1; i += 1 / 20)
                {
                    Vector2 vel = NPC.velocity + (i * MathF.Tau).ToRotationVector2();
                    vel.X *= 0.5f;
                    vel.RotatedBy(NPC.rotation - GetExtraRot());
                    Dust dust = Dust.NewDustPerfect(MouthCenter + vel * 2, DustID.Shadowflame, vel * 10);
                    dust.noGravity = true;
                }
            }
            if (NPC.ai[0] <= 107 && NPC.ai[0] > 50)
            {
                float dustRange = EaseInOut(Utils.GetLerpValue(50, 65, NPC.ai[0], true)) * 20;
                Vector2 posOffset = Vector2.UnitX * dustRange * Main.rand.NextFloat() - new Vector2(9, 0);
                posOffset = posOffset.RotatedBy(NPC.rotation - GetExtraRot() + NPC.spriteDirection * 0.17f);
                Dust.NewDustPerfect(NPC.Center + new Vector2(0, 4) + posOffset, DustID.CorruptTorch, Main.rand.NextVector2Circular(100, 100) / 100f);
                Dust.NewDustPerfect(NPC.Center + new Vector2(0, 4) + posOffset, DustID.CorruptTorch, Main.rand.NextVector2Circular(100, 100) / 100f);
            }
            NPC.ai[0]++;
            NPC.ai[0] %= 60 * 4;//loop
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TextureLoading();
            DrawBody(screenPos, drawColor);
            Color additiveWhite = new Color(255, 255, 255, 0);
            SpriteEffects spriteFX = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 origin = new Vector2(NPC.spriteDirection == 1 ? 14 : jaw.Width() - 14, 18);
            Vector2 posOffset = (NPC.rotation - GetExtraRot()).ToRotationVector2() * -10;
            posOffset.Y += 2;
            GetHeadRotationOffset(out float headRot, out float jawRot);
            float opacity = Utils.GetLerpValue(60, 110, NPC.ai[0], true) * Utils.GetLerpValue(160, 140, NPC.ai[0], true) * 0.2f;
            Main.EntitySpriteDraw(jaw, NPC.Center - screenPos + posOffset, null, drawColor, NPC.rotation + jawRot, origin, NPC.scale, spriteFX);
            if (opacity > 0)
            {
                for (float i = 0; i < 17; i++)
                {
                    Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy((i / 5.5f) * MathF.Tau + NPC.ai[0] * 0.1f);
                    Main.EntitySpriteDraw(jaw, NPC.Center - screenPos + posOffset + blurOffset, null, additiveWhite * opacity, NPC.rotation + jawRot, origin, NPC.scale, spriteFX);
                }
            }
            Main.EntitySpriteDraw(head, NPC.Center - screenPos + posOffset, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
            Main.EntitySpriteDraw(hair, NPC.Center - screenPos + posOffset, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
            if (opacity > 0)
            {
                for (float i = 0; i < 17; i++)
                {
                    Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy((i / 5.5f) * MathF.Tau + NPC.ai[0] * 0.1f);
                    Main.EntitySpriteDraw(hair, NPC.Center - screenPos + posOffset + blurOffset, null, additiveWhite * opacity, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
                }
            }
            return false;
        }

        private static void TextureLoading()
        {
            head ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerHead");
            jaw ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerJaw");
            hair ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerHair");
            body ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerBody");
            bodyGlow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerBodyGlow");
            body2 ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerBody2");
            body2Glow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerBody2Glow");
            tail ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerTail");
            tailGlow ??= ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerTailGlow");
        }
        void DrawGlowy(Texture2D texture, Vector2 drawPos, Vector2 origin, SpriteEffects spriteFX)
        {
            Color additiveWhite = new Color(255, 255, 255, 0);
            GetHeadRotationOffset(out float headRot, out float jawRot);
            float opacity = Utils.GetLerpValue(60, 110, NPC.ai[0], true) * Utils.GetLerpValue(160, 140, NPC.ai[0], true) * 0.2f;
            if (opacity > 0)
            {
                for (float i = 0; i < 17; i++)
                {
                    Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy((i / 5.5f) * MathF.Tau + NPC.ai[0] * 0.1f);
                    Main.EntitySpriteDraw(texture, drawPos + blurOffset, null, additiveWhite * opacity, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
                }
            }
        }
        void DrawBody(Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = body.Size() / 2;
            int segments = 4;
            Vector2 lastSegmentCenter = NPC.Center;
            Texture2D texture = body.Value;
            Texture2D glow = bodyGlow.Value;
            int segmentWidth = 36;
            for (int i = 1; i < segments; i++)
            {
                if (i == segments - 1)
                {
                    texture = tail.Value;
                    glow = tailGlow.Value;
                }
                else if (i % 2 == 0)
                {
                    texture = body2.Value;
                    glow = body2Glow.Value;
                }
                Vector2 segmentCenter = NPC.oldPos[i * 10] + NPC.Size / 2f;
                float rotation = (lastSegmentCenter - segmentCenter).ToRotation();
                segmentCenter = lastSegmentCenter - new Vector2(segmentWidth, 0f).RotatedBy(rotation, Vector2.Zero);
                rotation = (lastSegmentCenter - segmentCenter).ToRotation() + (float)Math.PI / 2f;
                Vector2 drawPos = segmentCenter - screenPos;
                SpriteEffects spriteDir = ((!(segmentCenter.X < lastSegmentCenter.X)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                lastSegmentCenter = segmentCenter;
                Color segmentDrawColor = NPC.GetAlpha(Lighting.GetColor(segmentCenter.ToTileCoordinates()));
                Main.EntitySpriteDraw(texture, drawPos, null, segmentDrawColor, rotation, origin, NPC.scale, spriteDir);
                DrawGlowy(glow, drawPos, origin, spriteDir);
            }
        }
        float GetExtraRot()
        {
            return NPC.spriteDirection == -1 ? MathF.PI : 0;
        }
        void GetHeadRotationOffset(out float headRot, out float jawRot)
        {
            headRot = 0;
            jawRot = 0;
            float animationProgress = Utils.Remap(NPC.ai[0], 0, 60 * 3, 0, 5);
            switch ((int)animationProgress)
            {
                case 0:
                    break;
                case 1:
                    headRot += EaseInOut(animationProgress % 1) * 0.15f * NPC.spriteDirection;
                    jawRot -= EaseInOut(animationProgress % 1) * 0.15f * NPC.spriteDirection;
                    break;
                case 2:
                    headRot += 0.15f * NPC.spriteDirection;
                    jawRot -= 0.15f * NPC.spriteDirection;
                    if (animationProgress > 2.75f)
                    {
                        animationProgress %= 1;
                        animationProgress = Utils.Remap(animationProgress, 0.75f, 1, 0, 1);
                        headRot -= EaseInOut(animationProgress) * 0.65f * NPC.spriteDirection;
                        jawRot += EaseInOut(animationProgress) * 0.65f * NPC.spriteDirection;
                    }
                    break;
                case 3:
                    headRot -= ((WobblyEffect(animationProgress % 1)) * 0.6f + 0.5f) * NPC.spriteDirection;
                    jawRot += ((WobblyEffect(animationProgress % 1)) * 0.6f + 0.5f) * NPC.spriteDirection;
                    break;
                case 4:
                    headRot -= (1 - EaseInOut(animationProgress % 1)) * 0.5f * NPC.spriteDirection;
                    jawRot += (1 - EaseInOut(animationProgress % 1)) * 0.5f * NPC.spriteDirection;
                    break;
                case 5:
                    break;
                case 6:
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
