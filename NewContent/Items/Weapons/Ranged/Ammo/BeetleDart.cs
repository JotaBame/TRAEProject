using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class BeetleDart: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Beetle Dart");
            // Tooltip.SetDefault("33% chance to stun enemies and deal triple damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 0, 0, 75);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 18;
            Item.height = 28;
            Item.shootSpeed = 8;
            Item.consumable = true;
            Item.shoot = ProjectileType<BeetleDartShot>();
            Item.ammo = AmmoID.Dart;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.BeetleHusk, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BeetleDartShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BeetleSHot");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.PoisonDartBlowgun;
            Projectile.CloneDefaults(ProjectileID.PoisonDartBlowgun);
            Projectile.timeLeft = 1200;
            Projectile.GetGlobalProjectile<ScopeAndQuiver>().AffectedByAlphaScope = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

            if (Main.rand.NextBool(3))
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item89 with { MaxInstances = 0 }, Projectile.position);

                modifiers.FinalDamage *= 3;
                target.GetGlobalNPC<Stun>().StunMe(target, 45);
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.PurpleMoss, 1f);
                    dust.noGravity = true;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.PurpleMoss, 1f);
                dust.noGravity = true;
            }
        }
    }
}



