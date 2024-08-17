using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.DreadItems.BloodWings
{
	[AutoloadEquip(EquipType.Wings)]
public	class BloodWings : ModItem
    {
		public override void SetStaticDefaults()
		{
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// DisplayName.SetDefault("Bloodfin Wings");
			// Tooltip.SetDefault("Allows flight and slow fall\nFlight is boosted upon taking damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 6.25f);
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
        public override void UpdateEquip(Player player)
        {
			player.GetModPlayer<BloodWingEffects>().hasWings = true;
        }
    }
	class BloodWingEffects : ModPlayer
    {
		public bool hasWings = false;
        public override void ResetEffects()
        {
			hasWings = false;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
			if (hasWings)
			{
				int duration = info.Damage * 4;
				if (duration > 300) // max 5 seconds at 75 damage
				Player.RefreshMovementAbilities();
				Player.AddBuff(BuffType<BloodRush>(), duration);
			}
        }
    }
}
