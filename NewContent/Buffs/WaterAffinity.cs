using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Buffs
{
	public class WaterAffinity : ModBuff
	{
 

		public override void Update(Player player, ref int buffIndex)
		{
            if (!GetInstance<TRAEConfig>().MobilityRework)
            {
				player.accRunSpeed *= 1.15f;
                player.maxRunSpeed *= 1.15f;
            }
            player.moveSpeed += 0.3f;
		}
	}
}