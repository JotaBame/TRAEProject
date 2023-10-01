using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    public class LevitationJuice : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Levitation Juice");
            // Tooltip.SetDefault("Provides a long lasting double jump");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
			Item.value = Item.buyPrice(0, 35);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetJumpState<LevitationJump>().Enable();
        }
    }
}