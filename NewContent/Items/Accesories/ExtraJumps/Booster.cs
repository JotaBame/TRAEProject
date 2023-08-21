
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    public class Booster : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Booster");
            //DisplayName.SetDefault("Christmas Booster");
            // Tooltip.SetDefault("This is how santa gets down the chimmney so quickly");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30; 
            Item.value = Item.sellPrice(gold: 5);
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TRAEJumps>().boosterCount++;
            player.GetJumpState<JetJump>().Enable();
        }
    }
}
