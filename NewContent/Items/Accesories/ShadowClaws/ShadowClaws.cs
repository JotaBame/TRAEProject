﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Accesories.ShadowflameCharm;
using TRAEProject.NewContent.Buffs;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.NewContent.Items.Accesories.ShadowClaws
{
    public class ShadowClaws : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; 
            DisplayName.SetDefault("Shadow Claws");
            Tooltip.SetDefault("Increases melee speed by 12% and allows all melee weapons and whips to autoswing\nMinion damage is stored as Shadowflame energy, up to 2250\nWhip strikes spawn a friendly Shadowflame Apparition for every 600 damage stored");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = 50000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.autoReuseGlove = true;
            player.GetModPlayer<MeleeStats>().TRAEAutoswing = true;
            player.GetModPlayer<ShadowflameCharmPlayer>().ShadowflameCharm += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.FeralClaws, 1)
                .AddIngredient(ItemType<ShadowflameCharmItem>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
