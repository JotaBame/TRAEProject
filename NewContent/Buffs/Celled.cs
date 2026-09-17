using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Buffs
{
	public class Celled: ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
			// Makes higher game difficulties extend this buff's duration

            // DisplayName.SetDefault("Celled");
            // Description.SetDefault("Being eaten by cells");
        }
        public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<BuffChangesModPlayer>().Celled = true;
		}
 
	}
}