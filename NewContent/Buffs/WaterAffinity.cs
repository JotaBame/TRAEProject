using Terraria;
using Terraria.ModLoader;
namespace TRAEProject.NewContent.Buffs
{
	public class WaterAffinity : ModBuff
	{
		public override void SetStaticDefaults() 
		{
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.3f;
		}
	}
}