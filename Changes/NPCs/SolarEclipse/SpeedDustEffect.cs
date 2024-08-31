using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TRAEProject.Changes.NPCs.SolarEclipse
{
    public class SpeedDustEffect : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale = 1f;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}