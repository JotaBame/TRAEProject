using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Items.Accesories.LifeCuffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Accesories.MasochistChains
{    
    [AutoloadEquip(EquipType.HandsOn, EquipType.Neck, EquipType.HandsOff)]
	class MasochistChains : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Masochist Chains");
            Tooltip.SetDefault("Getting hit will temporarily increase damage by 20% and movement speed");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = 75000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MasoCuffsEffect>().cuffs += 1; 
			player.panic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<LifeCuffs.LifeCuffs>(), 1)
                .AddIngredient(ItemID.PanicNecklace, 1)
				.AddIngredient(ItemID.DarkShard, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    class MasoCuffsEffect : ModPlayer
    {
        public int cuffs = 0;
        public override void ResetEffects()
        {
            cuffs = 0;
        }
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (cuffs > 0)
            {
                Player.AddBuff(BuffType<HeartAttack>(), cuffs * ((int)damage * 6 + 300));
            }
        }
    }
}
