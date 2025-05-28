using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        //const int paramIndexTex1 = 1;
        //const int paramIndexTex2 = 2;
        //const int paramIndexTex3 = 1;
        //const int paramIndexTex4 = 1;
        //const int param = 1;
        //const int paramIDTex1 = 1;
        public static void LoadTextures()
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
            meowmereShader = ModContent.Request<Effect>("TRAEProject/Changes/Weapon/Melee/MeowmereEffect/MeowmereShader");
        }
        public static void SetShaderParamsAndLoadTextures(float opacity, Rectangle frame, Vector2 screenPos)
        {
            LoadTextures();
            meowmereShader.Value.Parameters["cellNoiseTexture1"].SetValue(noiseTex1.Value);
            meowmereShader.Value.Parameters["cellNoiseTexture2"].SetValue(noiseTex2.Value);
            meowmereShader.Value.Parameters["cellNoiseTexture3"].SetValue(noiseTex3.Value);
            meowmereShader.Value.Parameters["cellNoiseTexture4"].SetValue(noiseTex4.Value);
            meowmereShader.Value.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * .2f);
            meowmereShader.Value.Parameters["opacity"].SetValue(opacity);
            meowmereShader.Value.Parameters["coordOffset"].SetValue(Vector2.Zero);
            
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
            On_SoundID.GetLegacyStyle += On_SoundID_GetLegacyStyle; ;
        }

        private static SoundStyle? On_SoundID_GetLegacyStyle(On_SoundID.orig_GetLegacyStyle orig, int type, int style)
        {
            SoundStyle? result = orig(type, style);
           
            Console.WriteLine();
            Main.chatText = "type:" + type + " style: " + style;
            if (type == 37)//meowmere sound OR sword swing sound effect
            {
                SoundStyle nonNullSoundStyle = result.Value;
                nonNullSoundStyle.MaxInstances = 99;
                return nonNullSoundStyle;//automatically converted back to nullable
            }
            return result;
        }

        public static void DrawBladeAura(SpriteBatch spritebatch, Projectile proj, Rectangle frame, float progress, SpriteEffects effects, float rotation, Vector2 origin, float scale, Vector2 drawPos, Texture2D texture, float opacity)
        {
            if (!Main.gamePaused)
            {
              //  proj.localAI[0] -= 0.9f;
            }
            LoadTextures();
            opacity = Utils.GetLerpValue(0, .4f, opacity, true);
            meowmereShader.Value.Parameters["rotation"].SetValue(-proj.direction * rotation);
            meowmereShader.Value.Parameters["scale"].SetValue(scale);
            ApplyShaderToSpritebatch(spritebatch);
            frame = texture.Frame(1, 1, 0, 0);
            SetShaderParamsAndLoadTextures(opacity, frame, Main.screenPosition);
            spritebatch.Draw(auraTexture.Value, drawPos, null, Color.White, 0, origin, scale, effects, 0);
            ResetSpritebatchToVanilla(spritebatch);
        }
    }
}
