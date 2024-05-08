using Terraria;
using Terraria.ModLoader;
namespace TRAEProject.NewContent.Buffs
{
	public class WishForSpeedBuff : ModBuff
	{
		public override void SetStaticDefaults() 
		{
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.15f;
            player.buffTime[buffIndex] = 2;
		}
	}
}