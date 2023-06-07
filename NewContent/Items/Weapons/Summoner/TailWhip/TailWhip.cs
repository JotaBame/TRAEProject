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
using TRAEProject.NewContent.Items.Materials;

namespace TRAEProject.NewContent.Items.Weapons.Summoner.TailWhip
{
    public class TailWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tail Whip");
            // Tooltip.SetDefault("Your summons will focus struck enemies\n11 summon tag damage\nDecreases defense by 16 points on hit");
             CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.useStyle = 1;
            Item.width = 46;
            Item.height = 32;
            Item.shoot = ProjectileType<TailWhipP>();
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.damage = 96;
            Item.useTime = Item.useAnimation = 24;
            // 240 dps
            Item.knockBack = 2f;
            Item.shootSpeed = 10f;
			// 30 tiles range
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
		{
            CreateRecipe(1)
                              .AddIngredient(ItemType<SalamanderTail>(), 3)
                              .AddIngredient(ItemType<ObsidianScale>(), 1)

                              .AddTile(TileID.MythrilAnvil)
                              .Register();
        }
	}

    public class TailWhipP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tail Whip"); ProjectileID.Sets.IsAWhip[Type] = true;

        }
        public override void WhipDefaults()
        {
            originalColor = new Color(29, 0, 0);
            whipRangeMultiplier = 1f;
            fallOff = 0.2f;
            tag = BuffType<TailWhipTag>();
            whipSegments = 30;
            tipScale = 1.15f;
        }
    }
    public class TailWhipTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("TailWhipTag");
            // Description.SetDefault("I wanted the crit tag on this to be equal to pokemon's normal critical strike chance, but that's 6.25% and it was way too low");
            Main.debuff[Type] = true;

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<Tag>().Damage += 11;
        }
    }
}
