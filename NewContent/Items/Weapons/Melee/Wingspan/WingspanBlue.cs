using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Prefixes;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Melee.Wingspan
{
    public class WingspanBlue : ModItem

    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        void WingsSpanBaseStats()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.knockBack = 2.25f;
            Item.width = 32;
            Item.height = 32;
            Item.damage = 18;
            Item.crit = 3;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<WingspanBlueP>();
            Item.shootSpeed = 14f;
            Item.UseSound = SoundID.Item39;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.channel = true;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.GetGlobalItem<GiveWeaponsPrefixes>().canGetMeleeOtherModifers = true;
        }
        public override void SetDefaults()
        {
            WingsSpanBaseStats();

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int count = 3;
            if (Main.rand.NextBool(4))
            {
                count++;
            }
            if (Main.rand.NextBool(8))
            {
                count++;
            }
            if (Main.rand.NextBool(16))
            {
                count++;
            }
            // average is about 3.4 knives
            for (int I = 0; I < count; I++)
            {
                float num69 = velocity.X;
                float num70 = velocity.Y;
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(28) / player.GetAttackSpeed<MeleeDamageClass>());
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback);
            }
            return false;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Feather, 8)
            .AddIngredient(ItemID.SoulofLight, 12)
            .AddIngredient(ItemID.SoulofFlight, 10)
                        .AddIngredient(ItemID.Sapphire, 8)

            .AddTile(TileID.MythrilAnvil)
        .Register();
        }
    }
    public class WingspanBlueP : ModProjectile
    {





        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.ArmorPenetration = 10;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.CloneDefaults(ProjectileID.VampireKnife);
            AIType = ProjectileID.VampireKnife;
            Projectile.scale = 0.9f;


        }
        public override void AI()
        {
 
  
                Projectile.ai[0] += 0.5f;
            
        }
    }
}