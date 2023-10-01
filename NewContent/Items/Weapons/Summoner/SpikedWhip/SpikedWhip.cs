using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Summoner.Whip;
using TRAEProject.Changes.Weapon;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.Changes.Weapon.Summon.Minions;
using System.Collections.Generic;
using System;

namespace TRAEProject.NewContent.Items.Weapons.Summoner.SpikedWhip
{
    public class SpikedWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spiked Whip");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override void SetDefaults()
        {
            Item.autoReuse = false; 
			Item.DamageType = DamageClass.SummonMeleeSpeed;
			Item.useStyle = 1;
            Item.width = 46;
            Item.height = 34;
            Item.shoot = ProjectileType<SpikedWhipP>();
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.damage = 16;
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 2f;
            Item.shootSpeed = 4f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 80, 0);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WormTooth, 18)
				.AddIngredient(ItemID.Leather, 2)
				.AddTile(TileID.Anvils)
				.Register();
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
 
    public class SpikedWhipP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("SpikedWhip");
        }
        public override void WhipDefaults()
        {
            originalColor = new Color(152, 118, 84);
            whipRangeMultiplier = 1f;
            fallOff = 0.18f;
            tag = BuffType<SpikedTag>();
            whipSegments = 13;
            tipScale = 1.33f;
        }
    }
    public class SpikedTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spiked Tag");
            // Description.SetDefault("Do you think there could've been a cooler name than 'Spiked Whip'? Sometimes simple is better, i suppose.");
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<Tag>().Damage += 2;
            npc.GetGlobalNPC<Tag>().Crit += 11;
        }
    }
}
