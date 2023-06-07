using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.DreadItems.BloodWings
{
    public class BloodRush : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blood Rush");
            // Description.SetDefault("Fly away!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.runAcceleration *= 1.5f;
            player.jumpSpeedBoost += 1f;
            player.moveSpeed += 0.5f;
        }
    }
}
