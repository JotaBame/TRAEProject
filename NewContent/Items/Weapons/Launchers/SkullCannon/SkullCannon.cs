using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Weapons.Launchers.SkullCannon
{
    class SkullCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skull Cannon");
            Tooltip.SetDefault("Shoots Rockets"); CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 24;
            Item.damage = 45;
            Item.useAnimation = 42;
            Item.useTime = 42;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 3);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileID.RocketI;
            Item.knockBack = 2f;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.HellstoneBar, 12)
                .AddIngredient(ItemID.Bone, 50)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}