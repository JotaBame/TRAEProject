using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.MechanicalEye
{
    public class EyeOfTheDestroyer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            // DisplayName.SetDefault("Eye Of The Destroyer");
            // Tooltip.SetDefault("10% increased critical strike chance\nRocket critical strikes stun enemies for 1 second");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 42;
            Item.height = 42;
            Item.value = Item.sellPrice(gold: 10);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangedStats>().RocketsStun += 1;
            player.GetCritChance<RangedDamageClass>() += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.EyeoftheGolem, 1)
                .AddIngredient(ItemType<MechanicalEyeItem>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

}
