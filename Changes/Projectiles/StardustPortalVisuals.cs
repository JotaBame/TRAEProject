using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.TextureSwaps;

namespace TRAEProject.Changes.Projectiles
{
    public class StardustPortalVisuals : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.MoonlordTurret;
        }
        static readonly Color[] goldPortalColors = new Color[6] { new Color(255, 255, 255, 0), new Color(250, 234, 192, 0), new Color(250, 216, 124, 0), new Color(250, 176, 0, 0), new Color(183, 106, 3, 0), new Color(91, 57, 29, 0) };
        static readonly Color[] bluePortalColors = new Color[6] { Color.White, new Color(115, 223, 255), new Color(35, 200, 255), new Color(104, 214, 255), new Color(0, 174, 238), new Color(0, 106, 185) };

        static void DrawPortal(Color[] colors, Vector2 pos, float rotation, float colorMult = 1, SpriteEffects spriteEffects = SpriteEffects.None, bool fullyOpaque = false, float scale = 1, byte opacity = 255)
        {
            for (int i = 0; i < colors.Length; i++)//INPUT ARRAY SHOULD BE 6 ELEMENTS LONG
            {
                string texturePath = "TRAEProject/Assets/SpecialTextures/PortalLayer" + (i + 1);
                if (fullyOpaque)
                    texturePath += "O";
                Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;

                Color colorToDraw = colors[i] * colorMult;
                colorToDraw *= opacity / 255f;
                Main.EntitySpriteDraw(texture, pos, null, colorToDraw, rotation, texture.Size() / 2, scale, spriteEffects, 0);
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {

            if (!ModContent.GetInstance<TRAEConfig>().Resprites)
                return true;
            float rotationColorOscillation = 0.95f + (projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            Color white = Color.White;
            float projRotation = (float)(projectile.rotation / 1.75f);
            Color lightTransparentGray = white * 0.8f;
            lightTransparentGray.A /= 2;
            float drawScale = projectile.scale * 1.2f * rotationColorOscillation;
            DrawPortal(bluePortalColors, drawPos, projRotation + 0.35f, 0.7f, SpriteEffects.FlipHorizontally, true, drawScale);
            DrawPortal(bluePortalColors, drawPos, projRotation, 1, SpriteEffects.FlipHorizontally, true, projectile.scale);
            DrawPortal(bluePortalColors, drawPos, -projRotation, 0.8f, SpriteEffects.None, true, 0.9f * projectile.scale);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < goldPortalColors.Length; j++)//INPUT ARRAY SHOULD BE 6 ELEMENTS LONG
                {
                    Texture2D portalLayer = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/PortalLayer" + (i + 1) + "O").Value;
                    Color colorToDraw = goldPortalColors[j];
                    colorToDraw.A = 255;
                    Main.EntitySpriteDraw(portalLayer, drawPos, null, colorToDraw, projRotation * 0.7f, portalLayer.Size() / 2, projectile.scale, SpriteEffects.FlipHorizontally, 0);
                    Main.EntitySpriteDraw(portalLayer, drawPos, null, colorToDraw, -projRotation * 0.7f, portalLayer.Size() / 2, projectile.scale, SpriteEffects.None, 0);
                }
            }

            //additive
            DrawPortal(goldPortalColors, drawPos, projRotation * 0.7f, 0.6f, SpriteEffects.FlipHorizontally, true, projectile.scale);
            DrawPortal(goldPortalColors, drawPos, -projRotation * 0.7f, 0.6f, SpriteEffects.None, false, projectile.scale);

            Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GoldenShine").Value;
            Color shineColor = Color.White;
            float waveAmplitude = 100;
            float triangleWave = (float)(1 / Math.PI * Math.Asin(Math.Sin(2 * Math.PI / waveAmplitude * Main.timeForVisualEffects)) + 0.5);
            Color altShineColor = shineColor;
            shineColor *= triangleWave;
            altShineColor *= 1 - triangleWave;
            Main.EntitySpriteDraw(texture, drawPos, null, shineColor, -projRotation, texture.Size() / 2, projectile.scale, SpriteEffects.None, 0);
            texture = TextureAssets.Extra[57].Value;
            Main.EntitySpriteDraw(texture, drawPos, null, altShineColor, projRotation, texture.Size() / 2, projectile.scale, SpriteEffects.None, 0);
            return false;

        }
    }
}
