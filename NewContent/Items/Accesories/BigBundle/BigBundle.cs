
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Accesories.SpaceBalloon;
using TRAEProject.NewContent.Items.Accesories.WeirdBundle;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Accesories.BigBundle
{
    [AutoloadEquip(EquipType.Balloon)]
    public class BigBundle : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            // DisplayName.SetDefault("The Big Bundle");
            // Tooltip.SetDefault("Allows the player to sextuple jump!\nIncreases jump height, life regeneration and prevents fall damage\nAllows reducing gravity by holding up\nReleases Bees and covers you in honey when damaged\nMultiple combs increase efficiency and life regeneration");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 9);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.hasJumpOption_Sandstorm = true;
            player.hasJumpOption_Blizzard = true;
            player.hasJumpOption_Cloud = true;
            player.hasJumpOption_Fart = true;
            player.hasJumpOption_Sail = true;
            player.jumpBoost = true;
            player.GetModPlayer<HoneyCombPlayer>().combs += 1;
            player.noFallDmg = true;
            player.GetModPlayer<SpaceBalloonPlayer>().SpaceBalloon += 1;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.BundleofBalloons)
               .AddIngredient(ItemType<BundleOfWeirdBalloons>())
               .AddIngredient(ItemType<SpaceBalloonItem>())
               .AddTile(TileID.TinkerersWorkbench)
               .Register(); 
            CreateRecipe().AddIngredient(ItemID.HorseshoeBundle)
               .AddIngredient(ItemType<BundleOfWeirdBalloons>())
               .AddIngredient(ItemType<SpaceBalloonItem>())
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
        }
        
    }
}