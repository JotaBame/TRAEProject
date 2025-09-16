using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace TRAEProject.Changes.NPCs.Miniboss.Everscream
{
    public class Everstar : ModProjectile
    {
        public override void SetDefaults()
        {
             Projectile.hostile = true;
             Projectile.width = 22;
            Projectile.height = 22;

        }
        public override void AI()
        {
              float num262 = 100f;
            float num263 = num262 - 50f;
            if (Main.rand.Next(Math.Max(4, 8 - (int)Projectile.velocity.Length())) == 0)
            {
                int num264 = 5;
                int num265 = Dust.NewDust(Projectile.position + new Vector2(num264, num264), Projectile.width - num264 * 2, Projectile.height - num264 * 2, 43, 0f, 0f, 254, new Color(255, 255, 0));
                Main.dust[num265].velocity = Projectile.velocity * 0.75f;
            }
            if (Projectile.ai[0] > num263)
            {
                Projectile.velocity *= 0.9f;
                Projectile.rotation *= 0.9f;
            }
            else
            {
                Projectile.rotation += 0.2f;
                if (Projectile.rotation > (float)Math.PI * 2f)
                {
                    Projectile.rotation -= (float)Math.PI * 2f;
                }
            }
            float num266 = Projectile.ai[0];
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 50 && Projectile.ai[0] % 10f == 0f)
            {
                float num248 = MathF.PI / 2f * (float)((Projectile.ai[0] % 20f != 0f) ? 1 : (-1));
                num248 *= (float)((Projectile.whoAmI % 2 != 0) ? 1 : (-1));
                num248 += (float)Main.rand.Next(-5, 5) * MathHelper.Lerp(0.2f, 0.03f, Projectile.ai[0] / 50f);
                Vector2 v = Projectile.velocity.RotatedBy(num248);
                v = v.SafeNormalize(Vector2.Zero);
                v *= Math.Max(2.5f, (50f - Projectile.ai[0]) / 50f * (7f + (-2f + (float)Main.rand.Next(2) * 2f)));
                int num249 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, v, ModContent.ProjectileType<Ornament>(), Projectile.damage, Projectile.knockBack * 0.25f, Projectile.owner, 0f, Main.rand.Next(4));
            }
            if (num266 <= num263 && Projectile.ai[0] > num263)
            {
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] > num262)
            {
                Projectile.Kill();
            }
 

        }
    }
    public class Ornament : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
                Projectile.CloneDefaults(ProjectileID.OrnamentFriendly);
                 AIType = ProjectileID.OrnamentFriendly;
                Projectile.timeLeft = 120;
                Projectile.hostile = true;
                Projectile.friendly = false;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == -1)
            {
                Projectile.Kill();
            }

        }
        public override void OnKill(int timeLeft)
        {
            int Cluster = ProjectileID.OrnamentHostileShrapnel; // snowman cannon's projectile, doesn't damage the player
            float num852 = (MathF.PI * 2f);
            float fragmentCount = 59.167f * 4/*Main.rand.Next(2, 3)*/;
            for (float c = 0f; c < 1f; c += fragmentCount / (678f * MathF.PI))
            {

                float f2 = num852 + c * (MathF.PI * 2f);
                Vector2 velocity = f2.ToRotationVector2() * (8f + Main.rand.NextFloat() * 2f);
                velocity += Vector2.UnitY * -1f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, Cluster, Projectile.damage, 0f,Projectile.owner);
            }
        }
    }
    class ShardChange : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
          
            if (projectile.type == ProjectileID.OrnamentHostileShrapnel)
            {
                projectile.timeLeft = 60;
            }
        }
  
    }

}
