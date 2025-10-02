
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;
using TRAEProject.NewContent.Items.Materials;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.MobilityMisc
{

	[AutoloadEquip(EquipType.Wings)]
public class LeviathanWings : ModItem
	{

		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Grants flight, slow fall, and hover\n'I will show you the world...'\nReduces movement and jump speed by 15%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(0, 4f, 1f);

		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<EchoRectrix>(), 1)
				.AddIngredient(ItemID.SoulofFlight, 20)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.25f;
            player.GetModPlayer<GravitationPlayer>().noFlipGravity = true;
            for (int n = 3; n < 10; n++)
			{
				if (player.IsItemSlotUnlockedAndUsable(n) && player.armor[n].type == Item.type)
				{
					player.hideVisibleAccessory[n] = true;
				}
			}
		}
 

    }

}
