
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;
using Microsoft.Xna.Framework;
using TRAEProject.Changes.Weapon.Ranged;
using TRAEProject.NewContent.NPCs.Underworld.Phoenix;
using TRAEProject.NewContent.Items.Materials;

namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class SolarGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sun Gel");
            // Tooltip.SetDefault("Shoots a concentrated Solar Flare");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99; AmmoID.Sets.IsSpecialist[Item.type] = true;

        }

        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 20;
            Item.height = 18;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<SolarGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(250).AddIngredient(ItemType<MagicalAsh>())
                .AddIngredient(ItemID.Gel, 50)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
    public class SolarGelP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            dustID = DustID.HeatRay;
            maxScale = 0.67f;

            ColorMiddle = new Color(222, 83, 43, 100);
            ColorBack = new Color(250, 247, 86, 100);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(150, 55, 27, 100);
            dustScale = 0.67f; 

            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 120;
            Projectile.penetrate = 5;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.5f;

            Projectile.extraUpdates = 1;

        }
        public override void PostAI()
        {
            if (Projectile.GetGlobalProjectile<ProjectileStats>().FirstHit)
            {
                Projectile.velocity *= 0.95f;
                Projectile.position -= Projectile.velocity;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 20; i++)
            {
                // Create a new dust
                Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.HeatRay, 0f, 0f);
                dust.position = (dust.position + Projectile.Center) / 2f;
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
        }
    }
}


