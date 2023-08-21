using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.TRAEDebuffs;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class ChargedArrow: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Charged arrow");
            // Tooltip.SetDefault("Creates a damaging ring around hit enemies\nStacks up to 5 times, increasing range");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 12;
            Item.height = 15;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<ChargedArrowShot>();
            Item.ammo = AmmoID.Arrow;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(500).AddIngredient(ItemID.WoodenArrow, 500)
                .AddIngredient(ItemType<GraniteBattery>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class ChargedArrowShot: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("ChargedArrow");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.timeLeft = 1200;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] % 3 == 0)
            {
                int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.Electric, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
                Projectile.localAI[0] = 0;
            }



            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.4f);

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ChargedArrowStacks GB = TRAEDebuff.Apply<ChargedArrowStacks>(target, damageDone, 5);
            if (GB != null)
            {
                GB.SetProjectileAndPlayer(Projectile, Main.player[Projectile.owner]);
            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Electric, 1f);
                dust.noGravity = true;
            }
        }
    }
}


