
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.LifeCuffs
{    
    [AutoloadEquip(EquipType.HandsOn, EquipType.Neck, EquipType.HandsOff)]
	public class MasochistChains : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Masochist Chains");
            // Tooltip.SetDefault("Getting hit will temporarily increase damage by 20% and movement speed");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.width = 42;
            Item.height = 42;
            Item.value = Item.sellPrice(gold: 5);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<LifeCuffsEffect>().cuffs += 1;
            player.GetModPlayer<OnHitEffects>().panicNecklaces += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<LifeCuffs>(), 1)
              .AddIngredient(ItemID.PanicNecklace, 1)
              .AddIngredient(ItemID.DarkShard, 2)
              .AddIngredient(ItemID.SoulofNight, 10)
              .AddTile(TileID.TinkerersWorkbench)
              .Register();
        }
    }
  
}
