using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Projectiles
{
    public class LilRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LilRocket");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.VortexBeaterRocket);           //The width of Projectile hitbox
            AIType = ProjectileID.VortexBeaterRocket;
            Projectile.scale = 0.8f;         
          Projectile.usesLocalNPCImmunity = true;
                    Projectile.localNPCHitCooldown = 10;
            Projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 10;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.33f;
            Projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.65f;
        }
        public override void Kill(int timeLeft)
        {       
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int num367 = 0; num367 < 4; num367++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
            }
            for (int num368 = 0; num368 < 10; num368++)
            {
                int num369 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0f, 0f, 200, default(Color), 2.5f);
                Main.dust[num369].noGravity = true;
                Dust dust = Main.dust[num369];
                dust.velocity *= 2f;
                num369 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0f, 0f, 200, default(Color), 1.5f);
                dust = Main.dust[num369];
                dust.velocity *= 1.2f;
                Main.dust[num369].noGravity = true;
            }
            for (int num370 = 0; num370 < 1; num370++)
            {
                int num371 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Projectile.width * Main.rand.Next(100) / 100f, Projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
                Gore gore = Main.gore[num371];
                gore.velocity *= 0.3f;
                Main.gore[num371].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                Main.gore[num371].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
            }
        }
    }
}