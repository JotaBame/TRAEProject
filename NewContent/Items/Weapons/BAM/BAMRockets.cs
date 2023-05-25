using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.BAM
{
    public class BAMRockets : GlobalProjectile
    {
        //public override bool InstancePerEntity => true;
        public void BAMExplosion(Projectile projectile)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default, 1.5f);
            }
            for (int i = 0; i < 40; i++)
            {
                int Dust = Terraria.Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 200, default, 2.5f);
                Main.dust[Dust].noGravity = true;
                Dust dust = Main.dust[Dust];
                dust.velocity *= 2f;
                Dust = Terraria.Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 200, default, 1.5f);
                dust = Main.dust[Dust];
                dust.velocity *= 1.2f;
                Main.dust[Dust].noGravity = true;
            }
            for (int i = 0; i < 1; i++)
            {
                int num371 = Gore.NewGore(projectile.GetSource_FromThis(), projectile.position + new Vector2(projectile.width * Main.rand.Next(100) / 100f, projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
                Gore gore = Main.gore[num371];
                gore.velocity *= 0.3f;
                Main.gore[num371].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                Main.gore[num371].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
            }
        }
        public void BAMRocketAI(Projectile projectile)
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            projectile.localAI[1] += 1f;
            if (projectile.timeLeft < 3)
            {
                projectile.alpha = 255; // Projectile becomes invisible
            }
            for (int l = 0; l < 2; l++)
            {
                float num14 = 0f;
                float num15 = 0f;
                if (l == 1)
                {
                    num14 = projectile.velocity.X * 0.5f;
                    num15 = projectile.velocity.Y * 0.5f;
                }
                if (!(projectile.localAI[1] > 9f))
                {
                    continue;
                }
                if (Main.rand.Next(2) == 0)
                {
                    int num16 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num14, projectile.position.Y + 3f + num15) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 229, 0f, 0f, 100, Scale: 1f);
                    Main.dust[num16].scale *= 1.4f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[num16].velocity *= 0.2f;
                    Main.dust[num16].noGravity = true;
                    if (Main.dust[num16].type == 152)
                    {
                        Main.dust[num16].scale *= 0.5f;
                        Main.dust[num16].velocity += projectile.velocity * 0.1f;
                    }
                    else if (Main.dust[num16].type == 35)
                    {
                        Main.dust[num16].scale *= 0.5f;
                        Main.dust[num16].velocity += projectile.velocity * 0.1f;
                    }
                    else if (Main.dust[num16].type == Dust.dustWater())
                    {
                        Main.dust[num16].scale *= 0.65f;
                        Main.dust[num16].velocity += projectile.velocity * 0.1f;
                    }
                    if (projectile.type == ProjectileType<BAMRocketMiniNuke>() || projectile.type == ProjectileType<BAMRocketDMiniNuke>())
                    {
                        Dust dust4 = Main.dust[num16];
                        if (dust4.dustIndex != 6000)
                        {
                            dust4 = Dust.NewDustPerfect(dust4.position, dust4.type, dust4.velocity, dust4.alpha, dust4.color, dust4.scale);
                            dust4.velocity = Main.rand.NextVector2Circular(3f, 3f);
                            dust4.noGravity = true;
                        }
                        if (dust4.dustIndex != 6000)
                        {
                            dust4 = Dust.NewDustPerfect(dust4.position, dust4.type, dust4.velocity, dust4.alpha, dust4.color, dust4.scale + 0.5f);
                            dust4.velocity = ((float)Math.PI * 2f * (projectile.timeLeft / 20f)).ToRotationVector2() * 3f;
                            dust4.noGravity = true;
                        }
                    }
                }
                if (Main.rand.Next(2) == 0)
                {
					int num17 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num14, projectile.position.Y + 3f + num15) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 31, 0f, 0f, 100, default(Color), 0.5f);
                    Main.dust[num17].fadeIn = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[num17].velocity *= 0.05f;
                }
            }
            float num18 = projectile.position.X;
            float num19 = projectile.position.Y;
            float num20 = 600f;
            bool flag3 = false;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 30f && projectile.ai[0] < 60f)
            {
                projectile.ai[0] = 30f;
                for (int m = 0; m < 200; m++)
                {
                    if (Main.npc[m].CanBeChasedBy(this))
                    {
                        float num21 = Main.npc[m].position.X + (float)(Main.npc[m].width / 2);
                        float num22 = Main.npc[m].position.Y + (float)(Main.npc[m].height / 2);
                        float num23 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num21) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num22);
                        if (num23 < num20 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[m].position, Main.npc[m].width, Main.npc[m].height))
                        {
                            num20 = num23;
                            num18 = num21;
                            num19 = num22;
                            flag3 = true;
                        }
                    }
                }
            }
            if (!flag3)
            {
                num18 = projectile.position.X + (float)(projectile.width / 2) + projectile.velocity.X * 100f;
                num19 = projectile.position.Y + (float)(projectile.height / 2) + projectile.velocity.Y * 100f;
            }
            float num24 = 16f;
            Vector2 vector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num25 = num18 - vector.X;
            float num26 = num19 - vector.Y;
            float num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
            num27 = num24 / num27;
            num25 *= num27;
            num26 *= num27;
            projectile.velocity.X = (projectile.velocity.X * 11f + num25) / 12f;
            projectile.velocity.Y = (projectile.velocity.Y * 11f + num26) / 12f;
        }
    }

    public class BAMRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.scale = 1.33f;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketDestructive : ModProjectile
    {
       public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().DestructiveRocketStats(Projectile);
            Projectile.scale = 1.33f;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        { 
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 3);
        }
    }
    public class BAMRocketSuper : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().SuperRocketStats(Projectile, false);
            Projectile.scale = 1.33f;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketDirect : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().DirectRocketStats(Projectile, false);
            Projectile.scale = 1.33f;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketMiniNuke : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, false);
            Projectile.scale = 1.33f;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketDMiniNuke : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, false);
            Projectile.scale = 1.33f;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 6);
        }
    }
    public class BAMRocketCluster : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.scale = 1.33f;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
            if (Projectile.owner == Main.myPlayer)
            {
                int Cluster = 862; // snowman cannon's projectile, doesn't damage the player
                float num852 = ((float)Math.PI * 2f);
                float fragmentCount = 59.167f * 6/*Main.rand.Next(2, 3)*/;
                for (float c = 0f; c < 1f; c += fragmentCount / (678f * (float)Math.PI))
                {

                    float f2 = num852 + c * ((float)Math.PI * 2f);
                    Vector2 velocity = f2.ToRotationVector2() * (4f + Main.rand.NextFloat() * 2f);
                    velocity += Vector2.UnitY * -1f;
                    int num854 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, Cluster, Projectile.damage / 4, 0f, Projectile.owner); Projectile pRojectile = Main.projectile[num854];
                    Projectile projectile2 = pRojectile;
                    projectile2.timeLeft = 30;
                }
            }
        }
    }
    public class BAMRocketHeavy : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.scale = 1.33f;
            Projectile.GetGlobalProjectile<NewRockets>().HeavyRocket = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketDry : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.scale = 1.33f;
            Projectile.GetGlobalProjectile<NewRockets>().DryRocket = true;

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketWet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.scale = 1.33f;
            Projectile.GetGlobalProjectile<NewRockets>().WetRocket = true;
       
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketLava : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.scale = 1.33f;
            Projectile.GetGlobalProjectile<NewRockets>().LavaRocket = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
              public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketHoney : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
            Projectile.scale = 1.33f;
            Projectile.GetGlobalProjectile<NewRockets>().HoneyRocket = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMExplosion(Projectile);
        }
    }
    public class BAMRocketLuminite: ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.scale = 1.33f; 
            Projectile.GetGlobalProjectile<NewRockets>().LuminiteStats(Projectile);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<BAMRockets>().BAMRocketAI(Projectile);
        }
    }
}