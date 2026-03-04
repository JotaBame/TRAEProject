using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoStalker
{
    public class EchoStalkerGoreBody1Hairless : ModGore
    {
        public override bool Update(Terraria.Gore gore) => EchosphereNPCHelper.EchosphereEnemyGoreUpdate(gore);
        public override Color? GetAlpha(Terraria.Gore gore, Color lightColor) => EchosphereNPCHelper.EchosphereEnemyGoreGetAlpha(gore, lightColor);
    }
    public class EchoStalkerGoreBody1 : EchoStalkerGoreBody1Hairless
    {
    }
 
    public class EchoStalkerGoreBody2 : EchoStalkerGoreBody1Hairless
    {
    }
 
    public class EchoStalkerGoreTail1 : EchoStalkerGoreBody1Hairless
    {
    }
 
    public class EchoStalkerGoreHeadHair : EchoStalkerGoreBody1Hairless
    {
    }
    public class EchoStalkerGoreHeadHairless : EchoStalkerGoreBody1Hairless
    {
    }
}
