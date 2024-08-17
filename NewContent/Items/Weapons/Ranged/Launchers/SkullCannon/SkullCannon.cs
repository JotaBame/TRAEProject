using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.SkullCannon
{
    public class SkullCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Skull Cannon");
            // Tooltip.SetDefault("Shoots Rockets");
        }
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 34;
            Item.damage = 50;
            Item.useAnimation = 39;
            Item.useTime = 39;
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
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-11, -8);
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