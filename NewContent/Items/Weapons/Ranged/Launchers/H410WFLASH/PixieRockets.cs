using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.H410WFLASH
{
    public class PixieRockets : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool PixieExplosion = false;

        bool target = false;
        public void PixieRocketAI(Projectile projectile)
        {     
		    projectile.rotation = MathF.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            if (projectile.ai[0] < 25)
            {
                ++projectile.ai[0];
            }
            /// control it
            else 
            {
                if(target)
                {
                    Vector2 move = new Vector2(projectile.ai[1], projectile.ai[2]);
                    float magnitude = MathF.Sqrt(move.X * move.X + move.Y * move.Y);
                    if (magnitude > 10f)
                    {
                        move /= magnitude;
                    }
                    projectile.velocity += move * 2f;
                    projectile.velocity *= 0.95f;
                }
                if (Main.LocalPlayer == Main.player[projectile.owner])
                {
                    Vector2 newMove = Main.MouseWorld - projectile.Center;
                    projectile.ai[1] = newMove.X;
                    projectile.ai[2] = newMove.Y;
                    projectile.netUpdate = true;
                }
                target = true;
            }
            /// Dusts
            if ((Math.Abs(projectile.velocity.X) >= 8f || Math.Abs(projectile.velocity.Y) >= 8f) && Main.rand.Next(2) == 0)
            {

                float positionX = 0f;
                float positionY = 0f;
                positionX = projectile.velocity.X * 0.5f;
                positionY = projectile.velocity.Y * 0.5f;

                Vector2 position71 = new Vector2(projectile.position.X + 3f + positionX, projectile.position.Y + 3f + positionY) - projectile.velocity * 0.5f;
                int width67 = projectile.width - 8;
                int height67 = projectile.height - 8;
                int dustType = DustID.Torch;
                float scale = 1f;
                if (projectile.GetGlobalProjectile<NewRockets>().LuminiteRocket)
                {
                    dustType = 229;
                    scale = 0.5f;
                }
                Dust dust = Dust.NewDustDirect(position71, width67, height67, dustType, 0f, 0f, 100, default, scale);
                dust.scale *= 2f + Main.rand.Next(10) * 0.1f;
                dust.velocity *= 0.2f;
                dust.noGravity = true;
                Vector2 position72 = new Vector2(projectile.position.X + 3f + positionX, projectile.position.Y + 3f + positionY) - projectile.velocity * 0.5f;
                int width68 = projectile.width - 8;
                int height68 = projectile.height - 8;
                dustType = DustID.Pixie; 
                if (projectile.GetGlobalProjectile<NewRockets>().LuminiteRocket)
                {
                    dustType = 229;
                }
                Dust otherdust = Dust.NewDustDirect(position72, width68, height68, dustType, 0f, 0f, 100, default, 0.5f);
                otherdust.fadeIn = 1f + Main.rand.Next(5) * 0.1f;
                otherdust.velocity *= 0.05f;

            }
            if (Math.Abs(projectile.velocity.X) < 15f && Math.Abs(projectile.velocity.Y) < 15f)
            {
                projectile.velocity *= 1.1f;
            }
        }
        public void HallowExplosion(Projectile projectile)
        {
                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);
                float num846 = 3f;
                for (int num847 = 0; num847 < 30; num847++)
                {
                    Dust dust53 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default, 2f);
                    dust53.velocity = (dust53.position - projectile.Center).SafeNormalize(Vector2.Zero);
                    Dust dust = dust53;
                    dust.velocity *= 2f + (float)Main.rand.Next(5) * 0.1f;
                    dust53.velocity.Y -= num846 * 0.5f;
                    dust53.color = Color.Pink * 0.9f;
                    if (Main.rand.Next(2) == 0)
                    {
                        dust53.scale = 0.5f;
                        dust53.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                        dust53.color = Color.Yellow * 0.8f;
                    }
                }
                for (int num848 = 0; num848 < 20; num848++)
                {
                    Dust dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100);
                dust54.noGravity = true;
                    dust54.fadeIn = 1.4f;
                    dust54.velocity = (dust54.position - projectile.Center).SafeNormalize(Vector2.Zero);
                    Dust dust = dust54;
                    dust.velocity *= 5.5f + (float)Main.rand.Next(61) * 0.1f;
                    dust54.velocity.Y -= num846 * 0.5f;
                    dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Pixie, 0f, 0f, 100);
                    dust54.velocity = (dust54.position - projectile.Center).SafeNormalize(Vector2.Zero);
                    dust54.velocity.Y -= num846 * 0.25f;
                    dust = dust54;
                    dust.velocity *= 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    dust54.fadeIn = 0f;
                    dust54.scale = 0.6f;
                    dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default, 1.5f);
                    dust54.noGravity = num848 % 2 == 0;
                    dust54.velocity = (dust54.position - projectile.Center).SafeNormalize(Vector2.Zero);
                    dust = dust54;
                    dust.velocity *= 3f + (float)Main.rand.Next(21) * 0.2f;
                    dust54.velocity.Y -= num846 * 0.5f;
                    dust54.fadeIn = 1.2f;
                    if (!dust54.noGravity)
                    {
                        dust54.scale = 0.4f;
                        dust54.fadeIn = 0f;
                    }
                    else
                    {
                        dust = dust54;
                        dust.velocity *= 2f + (float)Main.rand.Next(5) * 0.2f;
                        dust54.velocity.Y -= num846 * 0.5f;
                    }
                }

        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (PixieExplosion)
            {
                HallowExplosion(projectile);
            }

        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (PixieExplosion)
            {
                HallowExplosion(projectile);
                return false;
            }
            return true;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PixieExplosion)
            {
               HallowExplosion(projectile);
            }
        }
    }
    public class PixieRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class DestructivePixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().DestructiveRocketStats(Projectile);

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<PixieRockets>().HallowExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 3);
        }
    }
    public class SuperPixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().SuperRocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = false;
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class DirectPixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().DirectRocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = false;
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class MiniNukePixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class DestructiveMiniNukePixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, false);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
      
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<PixieRockets>().HallowExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 7);
        }
    }
    public class ClusterPixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int Cluster = 862; // snowman cannon's projectile, doesn't damage the player
                float num852 = (MathF.PI * 2f);
                for (float c = 0f; c < 1f; c += 355f / (678f * MathF.PI))
                {
                    float f2 = num852 + c * (MathF.PI * 2f);
                    Vector2 velocity = f2.ToRotationVector2() * (4f + Main.rand.NextFloat() * 2f);
                    velocity += Vector2.UnitY * -1f;
                    int num854 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, Cluster, Projectile.damage / 4, 0f, Projectile.owner);
                    Projectile pRojectile = Main.projectile[num854];
                    Projectile projectile2 = pRojectile;
                    projectile2.timeLeft = 30;
                }
            }
        }
    }
    public class HeavyPixie: ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
            Projectile.GetGlobalProjectile<NewRockets>().HeavyRocket = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class DryPixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
            Projectile.GetGlobalProjectile<NewRockets>().DryRocket = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class WetPixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
            Projectile.GetGlobalProjectile<NewRockets>().WetRocket = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class LavaPixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
            Projectile.GetGlobalProjectile<NewRockets>().LavaRocket = true;

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class HoneyPixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.GetGlobalProjectile<PixieRockets>().PixieExplosion = true;
            Projectile.GetGlobalProjectile<NewRockets>().HoneyRocket = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
    public class LuminitePixie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 20;
            Projectile.width = 20;
            Projectile.GetGlobalProjectile<NewRockets>().LuminiteStats(Projectile);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<PixieRockets>().PixieRocketAI(Projectile);
        }
    }
}