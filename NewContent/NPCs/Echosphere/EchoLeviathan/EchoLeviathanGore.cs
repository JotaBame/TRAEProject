using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan
{
    public class EchoLeviathanGore1 : ModGore
    {
        public override bool Update(Terraria.Gore gore) => EchosphereNPCHelper.EchosphereEnemyGoreUpdate(gore);
        public override Color? GetAlpha(Terraria.Gore gore, Color lightColor) => EchosphereNPCHelper.EchosphereEnemyGoreGetAlpha(gore, lightColor);
    }
    public class EchoLeviathanGoreHead : EchoLeviathanGore1
    {
    }
 
    public class EchoLeviathanGore2 : EchoLeviathanGore1
    {
    }
    public class EchoLeviathanGore3 : EchoLeviathanGore1
    {
    }
    public class EchoLeviathanGoreTail : EchoLeviathanGore1
    {
    }
}
