using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Weapons.Magic.DreamEater.ExperimentalShader
{
    public static class SquareShaderApplier
    {
        public static Asset<Texture2D> noise1;
        public static Asset<Texture2D> noise2;
        public static Asset<Texture2D> noise3;
        public static Asset<Texture2D> noise4;
        public static Asset<Effect> shader;
        public static void Load()
        {
            shader = ModContent.Request<Effect>("TRAEProject/NewContent/Items/Weapons/Magic/DreamEater/ExperimentalShader/SquareShader");
            noise1 = ModContent.Request<Texture2D>("TRAEProject/NewContent/Items/Weapons/Magic/DreamEater/ExperimentalShader/grayscale_normalized_texture1");
            noise2 = ModContent.Request<Texture2D>("TRAEProject/NewContent/Items/Weapons/Magic/DreamEater/ExperimentalShader/grayscale_normalized_texture2");
            noise3 = ModContent.Request<Texture2D>("TRAEProject/NewContent/Items/Weapons/Magic/DreamEater/ExperimentalShader/grayscale_normalized_texture3");
            noise4 = ModContent.Request<Texture2D>("TRAEProject/NewContent/Items/Weapons/Magic/DreamEater/ExperimentalShader/grayscale_normalized_texture4");
        }

    }
}
