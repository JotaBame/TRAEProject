using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Projectiles
{
   public class LightningBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("LightningBolt");    
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300; 
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 ProjectilePosition = Projectile.position;
                    ProjectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                    Projectile.alpha = 255;
                    int dust = Dust.NewDust(ProjectilePosition, 1, 1, 226, 0f, 0f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
                    Main.dust[dust].position = ProjectilePosition;
                    Main.dust[dust].velocity *= 0.2f;
                }
            }
            if (Projectile.position.Y > Projectile.localAI[1])
            {
                Projectile.tileCollide = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center); 
            const int NUM_DUSTS = 36;
            Dust.NewDust(Projectile.oldPosition, 1, 1, 226, 0f, 0f, 0, default, 1f);
            for (int i = 0; i < NUM_DUSTS; i++) 
            {
                // Create a new dust
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, 226, 2f, 1f); // pending, make a new Dust for this. Based on 226.
                dust.noGravity = true;
            }
        }
    }
    
}