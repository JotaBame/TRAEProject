using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;


namespace TRAEProject.NewContent.Items.Weapons.Ranged.Tribow
{
    public class Tribow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

    
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 60;
            Item.damage = 42;
            Item.useAnimation = 27;
            Item.useTime = 27;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 9);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Arrow;
            Item.knockBack = 5f;
            Item.shootSpeed = 12f;
            Item.scale = 1.1f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<Trirrow>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5; 
        }
        public override Vector2? HoldoutOffset()
         {
            return new Vector2(0, -1); // 
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 3; 
            float rotation = MathHelper.ToRadians(3f) * player.GetModPlayer<RangedStats>().spreadModifier;
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileType<Trirrow>();
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 10f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f; // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Marrow)
                .AddIngredient(ItemID.IceBow)
                .AddIngredient(ItemID.ShadowFlameBow)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class Trirrow: ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FrostArrow);
            AIType = ProjectileID.FrostArrow;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.ShadowFlame;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffMinDuration = 240;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 300;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffChance = 2;


            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, Main.rand.Next(60,90));
            }
        }
        public override void AI()
        {
            
            int type = Main.rand.NextFromList(DustID.RedTorch, DustID.GreenTorch);
            { 
                Vector2 ProjectilePosition = Projectile.Center;
                int dust = Dust.NewDust(ProjectilePosition, 1, 1, type, 0f, 0f, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position = ProjectilePosition;
                Main.dust[dust].velocity *= 0.2f;
            }
        }

    }
}