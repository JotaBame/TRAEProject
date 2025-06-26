using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Structures.Echosphere.ScreenEffect
{
    public static class EchosphereBorderEffect
    {
        public const string FilterName = "EchosphereBorderFilter";
        public static Asset<Texture2D> perlin;
        public static float intensity = 0f;
        public static float FadeSpeed => MaxIntensity / 30f;
        public static float MaxIntensity => 2f;
        public static float TintStrength => 0.1f;
        public static float SwirlFadeStart => 0.4f;
        public static float SwirlFadeEnd => 0.5f;
        public static Vector3 EffectColor => new(1f, .25f, 1f);//both tint and swirl effect color
        public static bool DistortAbsurdly => Main.zenithWorld;//couldn't think of a better name

        static bool deactivated = true;
        public static void CallOnLoad()
        {
            Asset<Effect> effect = ModContent.Request<Effect>("TRAEProject/NewContent/Structures/Echosphere/ScreenEffect/EchosphereBorder", AssetRequestMode.ImmediateLoad);
            Filters.Scene[FilterName] = new Filter(new ScreenShaderData(effect, "BorderPass"), EffectPriority.VeryHigh);
            Filters.Scene[FilterName].Load();
            perlin = ModContent.Request<Texture2D>("TRAEProject/NewContent/Structures/Echosphere/ScreenEffect/Perlin");
        }
        public static void Update()
        {
            if (EchosphereSystem.PlayerInEchosphere(Main.LocalPlayer))
            {
                intensity += FadeSpeed;
            }
            else
            {
                intensity -= FadeSpeed;
            }
            intensity = MathHelper.Clamp(intensity, 0f, MaxIntensity);
            if (intensity > 0f)
            {
                deactivated = false;
                ScreenShaderData shader = Filters.Scene[FilterName].GetShader();
                shader.UseColor(EffectColor);
                shader.UseImage(perlin, 1, SamplerState.LinearWrap);
                shader.UseIntensity(intensity);
                shader.UseSecondaryColor(TintStrength, SwirlFadeStart, SwirlFadeEnd);//workaround for no custom params
                shader.UseProgress(DistortAbsurdly ? 1f : 0f);//workaround for no custom params
                Filters.Scene.Activate(FilterName);
            }
            else if (!deactivated)
            {
                deactivated = true;
                Filters.Scene.Deactivate(FilterName);
            }
        }
    }
}
