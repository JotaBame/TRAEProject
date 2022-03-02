using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TRAEProject.Items.DreadItems.DreadMask
{
    [AutoloadEquip(EquipType.Head)]
    public class DreadMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Dreadnautilus Mask");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = 1;

            Item.vanity = true;
            Item.width = 20;
            Item.height = 20;
        }
    }
}