 
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Buffs
{
    class ShackledDefenses : ModBuff
    {
		public override void SetStaticDefaults()
		{
 			Main.buffNoSave[Type] = true;
			// DisplayName.SetDefault("Shackled Defenses");
			// Description.SetDefault("Defense increased by 10");
		}
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
            if (!GetInstance<TRAEConfig>().DefenseRework)
            {
                player.statDefense -= 5;
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            if (!GetInstance<TRAEConfig>().DefenseRework)
            {
                tip = "Defense increased by 5";
            }
        }
    }
}
