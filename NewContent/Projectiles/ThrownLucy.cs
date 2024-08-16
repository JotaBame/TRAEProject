using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace TRAEProject.NewContent.Projectiles
{
    public class ThrownLucy : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CombatWrench);           //The width of Projectile hitbox
            AIType = ProjectileID.CombatWrench;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1.1f;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = -1;
        }
   
    }
    public class LucyJank : ModPlayer
    {
        public override void PostItemCheck()  
        {
            if (Player.HeldItem.type == ItemID.LucyTheAxe && Player.ownedProjectileCounts[ProjectileType<ThrownLucy>()] > 0)
            {
                Player.HeldItem.noMelee = false;
                Player.HeldItem.noUseGraphic = false;
                Player.HeldItem.shoot = ProjectileType<Blank>();
            }
        }
    }
  
}

