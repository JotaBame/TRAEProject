using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace TRAEProject.Buffs
{
	public class NeoFeralBite : ModBuff
	{
		public override string Texture => "TRAEProject/Buffs/NeoFeralBite";

		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Feral Bite");
			Description.SetDefault("Causes confusion");
		}

		public override void Update(Player player, ref int buffIndex)
		{
<<<<<<< Updated upstream:Buffs/NeoFeralBite.cs
			if (Main.rand.Next(600) == 0)
			{
				float duration = Main.rand.Next(15, 20);
=======
			timer++;
			if (timer > 540)
            {
				player.AddBuff(BuffID.Obstructed, 1);
			}
			if (timer == 600)
			{
				float duration = Main.rand.Next(10, 25);
>>>>>>> Stashed changes:NewContent/Buffs/NeoFeralBite.cs
				player.AddBuff(BuffID.Confused, (int)duration);
			}
		}
	}
}