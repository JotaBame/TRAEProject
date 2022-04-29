using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Projectiles
{
    public class CosmicStingy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("CosmicStingy");     //The English name of the Projectile
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.light = 0.1f;
            Projectile.penetrate = 2;
            Projectile.aiStyle = 36;
            Projectile.timeLeft = 1200;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 5;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            int finalDefense = target.defense - (int)(player.GetArmorPenetration(DamageClass.Generic));
            target.ichor = false;
            target.betsysCurse = false;
            if (finalDefense < 0)
            {
                finalDefense = 0;
            }
            damage += finalDefense / 2;
            crit = false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
            if (Projectile.damage <= 1)
            {
                Projectile.Kill();
            }
        }
    }
}

