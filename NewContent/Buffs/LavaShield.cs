using TRAEProject.NPCs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace TRAEProject.NewContent.Buffs
{
	public class LavaShield : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Lava Shield");
			// Description.SetDefault("Increases defense by 12");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 12;
			if (!GetInstance<TRAEConfig>().DefenseRework)
            {
                player.statDefense -= 5;
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            if (!GetInstance<TRAEConfig>().DefenseRework)
            {
                tip = "Defense increased by 7";
            }
        }
    }
}