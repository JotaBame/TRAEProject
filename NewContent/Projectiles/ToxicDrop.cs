using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Projectiles
{
    public class ToxicDrop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("ToxicDrop");     //The English name of the Projectile
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;              
            Projectile.height = 22;            
            Projectile.friendly = true;       
            Projectile.hostile = false;         
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 90;
            Projectile.scale = 0.9f;
            Projectile.penetrate = 1;           
            Projectile.timeLeft = 600;          
            Projectile.tileCollide = true;          
            Projectile.extraUpdates = 0;            
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Main.rand.Next(3) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 256, 1, Projectile.velocity.Y * -0.33f, 0, default, 0.7f);

            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);            
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, 256, 1f);
                dust.noGravity = true;
            }
        }
    }    
}

