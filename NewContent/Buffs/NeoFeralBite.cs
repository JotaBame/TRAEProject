using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace TRAEProject.NewContent.Buffs
{
	public class NeoFeralBite : ModBuff
	{
		public override string Texture => "TRAEProject/NewContent/Buffs/NeoFeralBite";

		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			// DisplayName.SetDefault("Feral Bite");
			// Description.SetDefault("Causes confusion");
		}
		int timer = 0;

		public override void Update(Player player, ref int buffIndex)
		{
			if (NPC.AnyNPCs(NPCID.DukeFishron))
				player.DelBuff(buffIndex);
			timer++;
			if (timer > 540)
            {
				player.AddBuff(BuffID.Obstructed, 1);

			}
			if (timer == 599) // not 600, that's when the buff runs out
			{
				float duration = Main.rand.Next(20, 30);
				player.AddBuff(BuffID.Confused, (int)duration);
				timer = 0;
			}
		}
	}
}