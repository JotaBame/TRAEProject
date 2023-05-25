using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.TRAEDebuffs;
using Terraria.Audio;

namespace TRAEProject.NewContent.Items.Weapons.Ammo
{
    public class SolarArrow: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sun Arrow");
            // Tooltip.SetDefault("5% chance to deal greatly increased damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 14;
            Item.height = 40;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<SolarArrowShot>();
            Item.ammo = AmmoID.Arrow;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID.WoodenArrow, 50)
                .AddIngredient(ItemID.LunarTabletFragment, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class SolarArrowShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("SolarArrow");     //The English name of the Projectile

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
                int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.HeatRay, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
                Projectile.localAI[0] = 0;
            }

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.OnFire3, 240);
            if (Main.rand.NextBool(20))
            {
                modifiers.FinalDamage *= 2.5f;
                for (int i = 0; i < 30; i++)
                {
                    // Create a new dust
                    Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.HeatRay, 0f, 0f);
                    dust.position = (dust.position + Projectile.Center) / 2f;
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 0 }, Projectile.Center);
            target.AddBuff(BuffID.Daybreak, 60);
            }
        }

        public override void Kill(int timeLeft)    
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.HeatRay, 1f);
                dust.noGravity = true;
            }
        }
    }
}


