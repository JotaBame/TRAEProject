
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.MobilityJumps
{
    [AutoloadEquip(EquipType.Shoes)]
    public class Moonwalkers : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            // DisplayName.SetDefault("Moonwalkers");
            // Tooltip.SetDefault("Increases jump height, prevents fall damage and grants extended flight\nPress DOWN to fall faster\nAllows reducing gravity by hodling up");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 9);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SpaceBalloonPlayer>().SpaceBalloon += 1;
            player.rocketBoots = 2;
            player.rocketTimeMax += 10;
            player.GetModPlayer<AccesoryEffects>().FastFall = true;
            player.noFallDmg = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.ObsidianWaterWalkingBoots)
               .AddIngredient(ItemType<SpaceBalloonItem>())
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
        }
    }
}