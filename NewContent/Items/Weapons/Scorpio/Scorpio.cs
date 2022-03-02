using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Weapons.Scorpio
{
    class Scorpio : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jungla");
            Tooltip.SetDefault("Shoots three darts at once");
        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 34;
            Item.damage = 40;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Dart;
            Item.knockBack = 2f;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.PoisonDart;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5; 
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 3; 
            float rotation = MathHelper.ToRadians(10);
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 10f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f; // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}