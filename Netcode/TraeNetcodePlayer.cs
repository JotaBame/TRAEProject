using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Netcode
{
    public class TraeNetcodePlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMethods.SendRequestEchospherePositionData();
            }
        }
    }
}
