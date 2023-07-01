using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using TRAEProject.NewContent.Projectiles;
using Terraria.ID;
using Terraria.Audio;

namespace TRAEProject.NewContent.NPCs.EchoStalker
{
    public class EchoStalker : ModNPC
    {
        static Texture2D headTexture;
        static Texture2D hairTexture;
        static Texture2D jawTexture;
        public override void SetStaticDefaults()
        {

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
            float fireRate = 15;
            float numberOfShots = 2;
            if(NPC.ai[0] == 100)
            {
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/SonicWave") with { MaxInstances = 0 }, NPC.Center);

            }
            if(NPC.ai[0] >= 107 && (NPC.ai[0] - 107) % fireRate == 0 && NPC.ai[0] <= 107 + fireRate * numberOfShots)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), MouthCenter, NPC.DirectionTo(Main.player[NPC.target].Center) * 6f, ModContent.ProjectileType<EchoStalkerSonicWave>(), 65 / 2, 0, Main.myPlayer, 1f);
                for (float i = 0; i < 1; i+= 1/20)
                {
                    Vector2 vel = NPC.velocity + (i * MathF.Tau).ToRotationVector2();
                    vel.X *= 0.5f;
                    vel.RotatedBy(NPC.rotation - GetExtraRot());
                    Dust dust = Dust.NewDustPerfect(MouthCenter + vel * 2, DustID.Shadowflame, vel * 10);
                    dust.noGravity = true;

                }
            } 
            if(NPC.ai[0] <= 107 && NPC.ai[0] > 50)
            {
                float dustRange = EaseInOut(Utils.GetLerpValue(50, 65, NPC.ai[0], true)) * 20;        
                Vector2 posOffset = Vector2.UnitX * dustRange * Main.rand.NextFloat() - new Vector2(9,0);
                posOffset = posOffset.RotatedBy(NPC.rotation - GetExtraRot() + NPC.spriteDirection * 0.17f);
                Dust.NewDustPerfect(NPC.Center + new Vector2(0, 4) + posOffset, DustID.CorruptTorch, Main.rand.NextVector2Circular(100, 100) / 100f);
                Dust.NewDustPerfect(NPC.Center + new Vector2(0,4) + posOffset, DustID.CorruptTorch, Main.rand.NextVector2Circular(100,100)/100f);                
            }
            NPC.ai[0]++;
            NPC.ai[0] %= 60 * 4;//loop
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (headTexture == null || hairTexture == null || jawTexture == null)
            {

                headTexture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerHead", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                jawTexture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerJaw", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                hairTexture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoStalker/EchoStalkerHair", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
            SpriteEffects spriteFX = GetSpriteFX();
            Vector2 origin = new Vector2(NPC.spriteDirection == 1 ? 14 : jawTexture.Width - 14, 18);
            Vector2 posOffset = (NPC.rotation - GetExtraRot()).ToRotationVector2() * -10;
            posOffset.Y += 2;
            GetHeadRotationOffset(out float headRot, out float jawRot);
            float opacity = Utils.GetLerpValue(60, 110, NPC.ai[0], true) * Utils.GetLerpValue(160, 140, NPC.ai[0], true) * 0.2f;
            Main.EntitySpriteDraw(jawTexture, NPC.Center - Main.screenPosition + posOffset, null, drawColor, NPC.rotation + jawRot, origin, NPC.scale, spriteFX);
            for (float i = 0; i < 17; i++)
            {
                Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy((i / 5.5f) * MathF.Tau + NPC.ai[0] * 0.1f) * 1;
                Main.EntitySpriteDraw(jawTexture, NPC.Center - Main.screenPosition + posOffset + blurOffset, null, new Color(255,255,255, 0) * opacity, NPC.rotation + jawRot, origin, NPC.scale, spriteFX);
            }
            Main.EntitySpriteDraw(headTexture, NPC.Center - Main.screenPosition + posOffset, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
            Main.EntitySpriteDraw(hairTexture, NPC.Center - Main.screenPosition + posOffset, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
            for (float i = 0; i < 17; i++)
            {
                Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy((i / 5.5f) * MathF.Tau + NPC.ai[0] * 0.1f) * 1;
                Main.EntitySpriteDraw(hairTexture, NPC.Center - Main.screenPosition + posOffset + blurOffset, null, new Color(255, 255, 255, 0) * opacity, NPC.rotation + headRot, origin, NPC.scale, spriteFX);
            }
            return false;
        }
        float GetExtraRot()
        {
            return NPC.spriteDirection == -1 ? MathF.PI : 0;
        }
        SpriteEffects GetSpriteFX()
        {
            return NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
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
            return MathF.Sin(10f * progress / MathF.PI) * MathF.Sin(10f * progress * MathF.PI) * 0.25f;
        }
        static float EaseInOut(float progress)
        {
            return -MathF.Cos(progress * MathF.PI) * 0.5f + 0.5f;
        }
        static float EasingBackIn(float progress)
        {
            const float c1 = 1.70158f;//thanks easings.net for the magic numbers
            const float c3 = c1 + 1;     
            return 1 + c3 * MathF.Pow(progress - 1, 3) + c1 * MathF.Pow(progress - 1, 2);            
        }
    }
}
