using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Weapons.Scorpio
{
    class Scorpio : ModItem
    {
        public override void SetStaticDefaults()
        {
<<<<<<< Updated upstream:Items/Weapons/Scorpio/Scorpio.cs
            DisplayName.SetDefault("Double Scorpio");
            Tooltip.SetDefault("Shoots two darts at once");
=======
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Jungla");
            Tooltip.SetDefault("Shoots three darts at once");
>>>>>>> Stashed changes:NewContent/Items/Weapons/Scorpio/Scorpio.cs
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
            float numberProjectiles = 2; // 3, 4, or 5 shots
            float rotation = MathHelper.ToRadians(30);
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}