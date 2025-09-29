using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRAEProject.NewContent.Items.Materials
{
    public class EchoRectrix : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 2, 50, 0);
        }
    }  
}
