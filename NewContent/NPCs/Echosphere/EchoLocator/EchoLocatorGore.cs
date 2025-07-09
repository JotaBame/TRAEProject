using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoLocator
{
    public class EchoLocatorGore1 : ModGore
    {
        public override bool Update(Gore gore) => EchosphereNPCHelper.EchosphereEnemyGoreUpdate(gore);
        public override Color? GetAlpha(Gore gore, Color lightColor) => EchosphereNPCHelper.EchosphereEnemyGoreGetAlpha(gore, lightColor);
    }
    public class EchoLocatorGore2 : EchoLocatorGore1 { }
}
