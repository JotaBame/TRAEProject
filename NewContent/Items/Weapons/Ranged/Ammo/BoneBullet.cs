using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.Common;
using TRAEProject.Changes.Accesory;
using System;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class BoneBullet: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bone Bullet");
            // Tooltip.SetDefault("Chance to break into bones");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Blue;
            Item.width = 12;
            Item.height = 12;
            Item.shootSpeed = 3;
            Item.consumable = true;
            Item.shoot = ProjectileType<BoneBulletShot>();
            Item.ammo = AmmoID.Bullet;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID.MusketBall, 50)
                .AddIngredient(ItemID.Bone, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BoneBulletShot: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BoneSHot");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.Bullet;

            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.GetGlobalProjectile<ScopeAndQuiver>().AffectedByReconScope = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);;
         
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = 0 }, Projectile.Center);
            if (Main.myPlayer == Main.player[Projectile.owner].whoAmI && Main.rand.NextBool(2))
            {
                float num602 = (0f - Projectile.velocity.X) * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                float num603 = (0f - Projectile.velocity.Y) * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + num602, Projectile.position.Y + num603, num602, num603, ProjectileID.Bone, (int)(Projectile.damage * 0.5), 0f, Projectile.owner);
            }
        }
    }
}


