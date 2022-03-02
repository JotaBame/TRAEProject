using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Accesories.LifeCuffs
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    class LifeCuffs : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Life Cuffs");
            Tooltip.SetDefault("Getting hit will temporarily increase damage by 20%");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = 20000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<LifeCuffsEffect>().cuffs += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Shackle, 1)
                .AddIngredient(ItemID.BandofRegeneration, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    class LifeCuffsEffect : ModPlayer
    {
        public int  cuffs = 0;
        public override void ResetEffects()
        {
            cuffs = 0;
        }
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if(cuffs > 0)
            {
                Player.AddBuff(BuffType<HeartAttack>(), cuffs * ((int)damage * 3 + 300));
            }
        }
    }
    class HeartAttack : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart Attack!");
            Description.SetDefault("Damage increased by 20%");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.2f;
        }
    }
}
