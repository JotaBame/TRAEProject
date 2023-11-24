using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace TRAEProject.NewContent.Items.Armor.LeatherArmor
{
	[AutoloadEquip(EquipType.Body)]
    public class LeatherTunic : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Leather Tunic");
			// Tooltip.SetDefault("4% increased summon damage");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.width = 36;
			Item.height = 20;
			Item.defense = 4;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Leather, 2)
				.AddIngredient(ItemID.WormTooth, 15)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void UpdateEquip(Player player)
        {
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.08f;

          
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemType<LeatherHat>() && legs.type == ItemType<LeatherPants>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increased maximum number of minions by 2\n25% decreased non-summoner damage";
            player.GetDamage<MeleeDamageClass>() *= 0.75f;
            player.GetDamage<RangedDamageClass>() *= 0.75f;
            player.GetDamage<MagicDamageClass>() *= 0.75f;

            player.maxMinions += 2;
		}
    }
}




