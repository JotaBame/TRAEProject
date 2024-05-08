using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using Terraria.ModLoader;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;

namespace TRAEProject.Changes.Weapon.Ranged.Rockets
{
    public class NewGrenades : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public void GrenadeAI(Projectile projectile)
        {
            projectile.rotation += projectile.rotation += projectile.velocity.X * 0.1f;


            ++projectile.ai[0];
            if (projectile.ai[0] > 15f)
            {
                if (projectile.velocity.Y == 0f)
                {
                    projectile.velocity.X *= 0.95f;
                }
                projectile.velocity.Y += 0.2f;
            }
            if (projectile.GetGlobalProjectile<NewRockets>().LuminiteRocket)
            {
                int num25 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 100);
                Main.dust[num25].scale *= 1f + Main.rand.Next(10) * 0.1f;
                Main.dust[num25].velocity *= 0.2f;
                Main.dust[num25].noGravity = true;
            }
            else
            {
                int num25 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100);
                Main.dust[num25].scale *= 2f + Main.rand.Next(10) * 0.1f;
                Main.dust[num25].velocity *= 0.2f;
                Main.dust[num25].noGravity = true;
            }
            int num31 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100);
            Main.dust[num31].scale *= 1f + Main.rand.Next(10) * 0.1f;
            Main.dust[num31].velocity *= 0.2f;
            Main.dust[num31].noGravity = true;
        }
    }

    public class Grenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, true);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
        }

        public override void AI()
        {
            --Projectile.timeLeft;
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
    }
    public class DestructiveGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().DestructiveRocketStats(Projectile);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            TRAEMethods.DefaultExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 3);
        }
    }
    public class SuperGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().SuperRocketStats(Projectile, true);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
    }
    public class DirectGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().DirectRocketStats(Projectile, false);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
    }
    public class MiniNukeGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, true);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;

        }

        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
    }
    public class DestructiveMiniNukeGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<NewRockets>().IsARocket = true;
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, true);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;

        }

        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            TRAEMethods.DefaultExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 7);
        }
    }
    public class ClusterGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, true);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<NewRockets>().ClusterRocketExplosion(Projectile);
        }

    }
    public class HeavyGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, true);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
            Projectile.GetGlobalProjectile<NewRockets>().HeavyRocket = true;

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
    }
    public class LuminiteGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().LuminiteStats(Projectile);
            Projectile.timeLeft = 120;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewGrenades>().GrenadeAI(Projectile);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileType<LuminiteBoom>(), 0, 0f, Projectile.owner, 0f, 0f);
            SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, Projectile.position);
            int NPCLimit = 0;
            Player player = Main.player[Projectile.owner];
            float range = 175f;
            for (int k = 0; k < 200; k++)
            {
                NPC nPC = Main.npc[k];
                if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(Projectile.Center, nPC.Center) <= range)
                {

                    if (NPCLimit < 3)
                    {
                        ++NPCLimit;
                        player.ApplyDamageToNPC(nPC, (int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 0f, 0, false);
                    }
                }
            }
            return true;
        }
    }
}