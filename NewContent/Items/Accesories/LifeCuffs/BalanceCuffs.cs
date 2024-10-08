﻿
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Accesories.LifeCuffs
{    
[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
   public class BalanceCuffs : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Balance Cuffs");
            // Tooltip.SetDefault("Temporarily increases damage by 20% and restore mana when damaged");
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 48;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 4);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 20;
 
            player.GetModPlayer<LifeCuffsEffect>().cuffs += 1; 
            player.GetModPlayer<LifeCuffsEffect>().balanceCuffs += 1;

            player.GetModPlayer<OnHitEffects>().magicCuffsCount += 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<LifeCuffs>(), 1)
             .AddIngredient(ItemID.MagicCuffs, 1)
                                            

             .AddIngredient(ItemID.DarkShard, 1)
             .AddIngredient(ItemID.LightShard, 1).AddIngredient(ItemID.SoulofLight, 5)
                                             .AddIngredient(ItemID.SoulofNight, 5)
             .AddTile(TileID.TinkerersWorkbench)
             .Register();
            CreateRecipe().AddIngredient(ItemID.ManaRegenerationBand, 1)
            .AddIngredient(ItemID.Shackle, 1)
            .AddIngredient(ItemID.DarkShard, 1)
            .AddIngredient(ItemID.LightShard, 1).AddIngredient(ItemID.SoulofLight, 5)
                                                .AddIngredient(ItemID.SoulofNight, 5)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
