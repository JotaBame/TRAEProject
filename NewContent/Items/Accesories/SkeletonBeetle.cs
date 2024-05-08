using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories
{
	class SkeletonBeetle : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tribal Beetle");
			// Tooltip.SetDefault("Increases your maximum number of sentries by 2\nIncreased summon knockback");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 36;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
        public override void UpdateEquip(Player player)
        {
			player.maxTurrets += 2;
			player.GetKnockback(DamageClass.Summon) = player.GetKnockback(DamageClass.Summon)  + 2f;

        }
        public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.HerculesBeetle)
				.AddIngredient(ItemID.PygmyNecklace)
				.AddIngredient(ItemID.SoulofSight, 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}

