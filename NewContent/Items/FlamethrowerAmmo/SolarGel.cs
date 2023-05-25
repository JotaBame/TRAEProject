
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;
using Microsoft.Xna.Framework;

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
            Item.damage = 36;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 6;
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
            CreateRecipe(100).AddIngredient(ItemID.LunarTabletFragment)
                .AddIngredient(ItemID.Gel, 10)
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
            scalemodifier = 0.67f;

            ColorMiddle = new Color(222, 83, 43, 150);
            ColorBack = new Color(250, 247, 86, 150);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(150, 55, 27, 100);
            dustScale = 0.67f;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 120;
            Projectile.penetrate = -1; 
            Projectile.extraUpdates = 1;

        }
        public override void PostAI()
        {
            if (Projectile.damage == 0)
            {
                Projectile.velocity *= 0.95f;
                Projectile.position -= Projectile.velocity;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = 0;
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


