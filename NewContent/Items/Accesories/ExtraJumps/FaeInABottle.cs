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
    public class FaeInABottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            // DisplayName.SetDefault("Fae in a bottle");
            // Tooltip.SetDefault("A weak double jump that grant immunity frames");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
			Item.value = Item.buyPrice(0, 35);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetJumpState<FaeJump>().Enable();
        }
    }
}