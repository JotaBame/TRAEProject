using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRAEProject.NewContent.Items.Materials
{
    public class LuminiteFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Luminite Feather");
            // Tooltip.SetDefault("Use to make the ultimate wings");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(0, 50, 0, 0);
        }
    }  
}
