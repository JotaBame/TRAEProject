
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.MobilityMisc
{

	[AutoloadEquip(EquipType.Wings)]
public class MagicalCarpet : ModItem
	{

		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Grants flight, slow fall, and hover\n'I will show you the world...'\nReduces movement and jump speed by 15%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(135, 4f, 1f/*, hasHoldDownHoverFeatures: true, 6f, 6f*/);

		}

		public override void SetDefaults()
		{
			Item.width = 66;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.FlyingCarpet)
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
				.AddIngredient(ItemID.SoulofFlight, 20)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			for (int n = 3; n < 10; n++)
			{
				if (player.IsItemSlotUnlockedAndUsable(n) && player.armor[n].type == Item.type)
				{
					player.hideVisibleAccessory[n] = true;
				}
			}
		}
		public override bool WingUpdate(Player player, bool inUse)
        {
            if (inUse && player.TryingToHoverDown && player.wingTime > 0)
            {
                player.runAcceleration *= 3;
				player.velocity.Y = 0.00001f;
				return false;
            }
            return true;

        }

    }

}
