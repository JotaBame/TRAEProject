using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class PinkGel : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            AmmoID.Sets.IsSpecialist[ItemID.PinkGel] = true;

        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.PinkGel)
            {
                item.damage = 10;
                item.DamageType = DamageClass.Ranged;
                item.knockBack = 1f;
                item.consumable = true;
                item.shoot = ProjectileType<PinkGelP>();
                item.ammo = AmmoID.Gel;
                item.maxStack = 9999;
            }    
        }
    }
    public class PinkGelP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            ColorMiddle = new Color(255, 64, 226, 200);
            ColorBack = new Color(255, 145, 193, 200);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(80, 80, 80, 100);
            dustID = DustID.PinkTorch;
            dustScale = 0.75f;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.OnFire;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 300;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.15f;
            dieInWater = true;
        }
    }
}


