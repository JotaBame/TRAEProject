using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.DreadItems.DreadMask
{
    [AutoloadEquip(EquipType.Head)]
    public class DreadMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Dreadnautilus Mask");
            // Tooltip.SetDefault("");
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