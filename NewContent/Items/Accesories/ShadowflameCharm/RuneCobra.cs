
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.NewContent.Items.Accesories.ShadowflameCharm
{
    public class RuneCobra : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; 
            // DisplayName.SetDefault("Rune Cobra");
            // Tooltip.SetDefault("Increases your maximum number of minions by 1 and minion critical strike chance by 5%\nMinion damage is stored as Shadowflame energy, up to 3000\nWhip strikes spawn a friendly Shadowflame Apparition for every 750 damage stored");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 40;
            Item.height = 42;
            Item.value = Item.sellPrice(gold:9);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
	 
            player.GetModPlayer<ShadowflameCharmPlayer>().ShadowflameCharm += 1;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.GetModPlayer<SummonStats>().minionCritChance += 13;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.NecromanticScroll, 1)
                .AddIngredient(ItemType<ShadowClaws>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register(); 
            CreateRecipe().AddIngredient(ItemID.NecromanticScroll, 1)
                .AddIngredient(ItemID.FeralClaws, 1)
                .AddIngredient(ItemType<ShadowflameCharmItem>(), 1)

                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
