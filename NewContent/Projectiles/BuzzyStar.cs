using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Projectiles
{
    public class BuzzyStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BuzzyStar");     //The English name of the Projectile
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.HallowStar);
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            AIType = ProjectileID.HallowStar;
            DrawOffsetX = -5;
            DrawOriginOffsetY = -12;
            DrawOriginOffsetX = -8;
        }
        public override void Kill(int timeLeft)
        {
            {
                int[] array = new int[10];
                int num6 = 0;
                int num7 = 1200;
                int num8 = 20;
                for (int j = 0; j < 200; j++)
                {
                    if (Main.npc[j].CanBeChasedBy(this, false))
                    {
                        float num9 = (Projectile.Center - Main.npc[j].Center).Length();
                        if (num9 > num8 && num9 < num7)
                        {
                            array[num6] = j;
                            num6++;
                            //if (num6 >= 9)
                            //{
                            //    break;
                            //}
                        }
                    }
                }
                if (num6 > 0)
                {
                    num6 = Main.rand.Next(num6);
                    Vector2 value2 = Main.npc[array[num6]].Center - Projectile.Center;
                    float scaleFactor2 = Projectile.velocity.Length();
                    value2.Normalize();
                    value2 *= scaleFactor2;
                    for (int i = 0; i < 4; i++)
                    {
                        float random = Main.rand.NextFloat(0.1f, 1f);
                        int bee = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.oldPosition.X, Projectile.oldPosition.Y, 0, 1, ProjectileType<CosmicStingy>(), Projectile.damage / 2, 0, Projectile.owner, 0f, 0f);
                        Main.projectile[bee].velocity = value2 * random;
                    }
                }
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
    }
}