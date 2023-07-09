
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.LifeCuffs
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    class LifeCuffs : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Life Cuffs");
            // Tooltip.SetDefault("Getting hit will temporarily increase damage by 20%");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.width = 48;
            Item.height = 48;
            Item.value = Item.sellPrice(gold: 2);
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
        public override void OnHurt(Player.HurtInfo info)

        {
            if(cuffs > 0)
            {
                Player.AddBuff(BuffType<HeartAttack>(), cuffs * ((int)info.Damage * 3 + 300));
            }
        }
    }
    class HeartAttack : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heart Attack!");
            // Description.SetDefault("Damage increased by 20%");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.2f;
        }
    }
}
