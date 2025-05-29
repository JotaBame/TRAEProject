using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Weapon.Melee.MeowmereEffect
{
    public static class MeowmereAuraEffectDrawer
    {
        public static Asset<Texture2D> noiseTex1;
        public static Asset<Texture2D> noiseTex2;
        public static Asset<Texture2D> noiseTex3;
        public static Asset<Texture2D> noiseTex4;
        public static Asset<Texture2D> auraTexture;
        public static Asset<Effect> meowmereShader;
        public static void LoadAssets()
        {
            noiseTex1 ??= ModContent.Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/Noise1", AssetRequestMode.ImmediateLoad);
            noiseTex2 ??= ModContent.Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/Noise2", AssetRequestMode.ImmediateLoad);
            noiseTex3 ??= ModContent.Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/Noise3", AssetRequestMode.ImmediateLoad);
            noiseTex4 ??= ModContent.Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/Noise4", AssetRequestMode.ImmediateLoad);
            auraTexture ??= ModContent.Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/MeomwereAura");
            meowmereShader ??= ModContent.Request<Effect>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/MeowmereShader", AssetRequestMode.ImmediateLoad);
        }
        public static void LoadShader()
        {
            meowmereShader = ModContent.Request<Effect>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/MeowmereShader", AssetRequestMode.ImmediateLoad);
        }
        public static float ShaderTime => Main.GlobalTimeWrappedHourly * .2f;//temporarily multiply by 0 for debug reasons
        public static void SetShaderParamsAndLoadAssets(float opacity, float scale, float rotation)
        {
            LoadAssets();
            meowmereShader.Value.Parameters["cellNoiseTexture1"].SetValue(noiseTex1.Value);
            meowmereShader.Value.Parameters["cellNoiseTexture2"].SetValue(noiseTex2.Value);
            meowmereShader.Value.Parameters["cellNoiseTexture3"].SetValue(noiseTex3.Value);
            meowmereShader.Value.Parameters["cellNoiseTexture4"].SetValue(noiseTex4.Value);
            meowmereShader.Value.Parameters["time"].SetValue(ShaderTime);
            meowmereShader.Value.Parameters["opacity"].SetValue(opacity);
            meowmereShader.Value.Parameters["coordOffset"].SetValue(Vector2.Zero);
            meowmereShader.Value.Parameters["rotation"].SetValue(rotation);
            meowmereShader.Value.Parameters["scale"].SetValue(scale);
        }
        public static void ApplyShaderToSpritebatch(SpriteBatch spritebatch)
        {
            LoadShader();
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, meowmereShader.Value, Main.Transform);
        }
        public static void ResetSpritebatchToVanilla(SpriteBatch spritebatch)
        {
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        }
        public static void FixMeomwereSound_CallOnLoad()
        {
            On_SoundID.GetLegacyStyle += On_SoundID_GetLegacyStyle;
        }

        private static SoundStyle? On_SoundID_GetLegacyStyle(On_SoundID.orig_GetLegacyStyle orig, int type, int style)
        {
            SoundStyle? result = orig(type, style);           
            if (type == 37)//meowmere sound
            {
                SoundStyle nonNullSoundStyle = result.Value;
                nonNullSoundStyle.MaxInstances = 99;
                return nonNullSoundStyle;//automatically converted back to nullable
            }
            return result;
        }
        public static Color GetSparkleColor(float opacity, Vector2 drawPos, Projectile proj)
        {
            drawPos += Main.screenPosition;//undo the screen position subtraction to get world position
            drawPos -= proj.Center;//center on proj center
            float hue = drawPos.X / 170f;
            hue /= 2;//gradient coord scaling value inside the shader
            hue += ShaderTime * 3;//I hardcoded a 3x multiplier inside the shader (oops)
            hue += .5f;
            hue = Loop01(hue);
            opacity = RemapOpacity(opacity);
            Color col = Main.hslToRgb(hue, 1f, 0.65f) * opacity;
            col.A = 0;
            return col;
        }
        public static float Loop01(float value)
        {
            return (value % 1f + 1f) % 1f;
        }
        public static Color GetEdgeColor(float opacity, Vector2 drawPos, Projectile proj)
        {
            drawPos += Main.screenPosition;//undo the screen position subtraction to get world position
            drawPos -= proj.Center;//center on proj center
            float hue = drawPos.X / 170f;
            hue /= 2;//gradient coord scaling value inside the shader
            hue += ShaderTime * 3;//I hardcoded a 3x multiplier inside the shader (oops)
            hue += .5f;
            hue = Loop01(hue);
            //opacity = RemapOpacity(opacity);
            Color col = Main.hslToRgb(hue, 1f, 0.65f) * opacity * 0.8f;
            col.A = 0;
            return col;
        }
        public static float GetEdgeCount(float progress)
        {
            return Utils.Remap(progress, 0f, .4f, 7, 15f);
        }
        public static void DrawBladeAura(SpriteBatch spritebatch, Projectile proj, Rectangle frame, float progress, SpriteEffects effects, float rotation, Vector2 origin, float scale, Vector2 drawPos, Texture2D texture, float opacity)
        {
            if (!Main.gamePaused)
            {
                //  proj.localAI[0] -= 0.9f;
            }
            LoadAssets();
            opacity = RemapOpacity(opacity);

            ApplyShaderToSpritebatch(spritebatch);
            SetShaderParamsAndLoadAssets(opacity, scale, -proj.direction * rotation);
            spritebatch.Draw(auraTexture.Value, drawPos, null, Color.White, 0, origin, scale, effects, 0);
            ResetSpritebatchToVanilla(spritebatch);
        }

        private static float RemapOpacity(float opacity)
        {
            opacity = Utils.GetLerpValue(0, 0.8f, opacity, true);
            return opacity;
        }
    }
}
