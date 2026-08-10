using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using TRAEProject.Changes.Weapon;
using TRAEProject.Changes.Weapon.Summon.Minions;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Weapons.Summoner.Whip;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Summoner.Echolalia
{
    public class Echolalia : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tail Whip");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.useStyle = 1;
            Item.width = 46;
            Item.height = 30;
            Item.shoot = ProjectileType<EcholaliaP>();
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.damage = 63;
            Item.useTime = Item.useAnimation = 35;
             Item.knockBack = 2f;
            Item.shootSpeed = 5.7f;
             Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 6, 0, 0);
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
		{
            CreateRecipe(1)
                              .AddIngredient(ItemID.Leather, 7)
                              .AddIngredient(ItemID.SoulofFright, 8)
                                                            .AddIngredient(ItemID.SoulofLight, 16)

                              .AddTile(TileID.MythrilAnvil)
                              .Register();
        }
	}

    public class EcholaliaP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tail Whip"); ProjectileID.Sets.IsAWhip[Type] = true;

        }
        public override void WhipDefaults()
        {
            originalColor = new Color(29, 0, 0);
            whipRangeMultiplier = 1f;
            fallOff = 0.25f;
            tag = BuffType<EcholaliaTag>();
            whipSegments = 35;
            tipScale = 1.15f;
        }
    }
    public class EcholaliaTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("EcholaliaTag");
            // Description.SetDefault("I wanted the crit tag on this to be equal to pokemon's normal critical strike chance, but that's 6.25% and it was way too low");
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
  
             npc.GetGlobalNPC<Tag>().Damage += 12;
        }
    }
}
