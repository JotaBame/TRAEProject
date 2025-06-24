using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Structures.Echosphere
{
    public class EchosphereSystem : ModSystem
    {
        public static float EchosphereBorderPaddingX => 16 * 46;
        public static float EchosphereBorderPaddingY => 16 * 46;
        public static Vector2 EchosphereBorderPaddingVec => new Vector2(EchosphereBorderPaddingX, EchosphereBorderPaddingY);
        public static bool PlayerInEchosphere(Player player)
        {
            Vector2 plrPos = player.Center;
            GetPaddedCorners(out Vector2 topLeft, out Vector2 bottomRight);

            if (plrPos.X < topLeft.X || plrPos.X > bottomRight.X || plrPos.Y > bottomRight.Y || plrPos.Y < topLeft.Y)
            {
                return false;
            }
            return true; 
        }

        public static void GetPaddedCorners(out Vector2 topLeft, out Vector2 bottomRight)
        {
            topLeft = EchosphereGeneratorSystem.echosphereTopLeft;
            bottomRight = EchosphereGeneratorSystem.echosphereBottomRight;
            Vector2 padding = EchosphereBorderPaddingVec;
            topLeft -= padding;
            bottomRight += padding;
        }
        public static void GetPaddedCorners(out Vector2 topLeft, out Vector2 bottomRight, out Vector2 topRight, out Vector2 bottomLeft)
        {
            topLeft = EchosphereGeneratorSystem.echosphereTopLeft;
            bottomRight = EchosphereGeneratorSystem.echosphereBottomRight;
            Vector2 padding = EchosphereBorderPaddingVec;
            topLeft -= padding;
            bottomRight += padding;

            bottomLeft = new(topLeft.X, bottomRight.Y);
            topRight = new(bottomRight.X, topLeft.Y);
        }
    }
}
