
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.MobilityJumps
{ [AutoloadEquip(EquipType.Waist)]
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
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 5);
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetJumpState<FaeJump>().Enable();
        }
    }
}