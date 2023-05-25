using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Weapon.Ranged.Rockets
{
    public class NewRockets : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool IsARocket = false;
        public bool HeavyRocket = false;
        public bool DryRocket = false;
        public bool WetRocket = false;
        public bool LavaRocket = false;
        public bool HoneyRocket = false;
        public bool LuminiteRocket = false;

        public void RocketAI(Projectile projectile)
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Math.Abs(projectile.velocity.X) >= 8f || Math.Abs(projectile.velocity.Y) >= 8f)
            {
                for (int n = 0; n < 2; n++)
                {
                    float num23 = 0f;
                    float num24 = 0f;
                    if (n == 1)
                    {
                        num23 = projectile.velocity.X * 0.5f;
                        num24 = projectile.velocity.Y * 0.5f;
                    }
                    int num25 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num23, projectile.position.Y + 3f + num24) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 6, 0f, 0f, 100);
                    Main.dust[num25].scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[num25].velocity *= 0.2f;
                    Main.dust[num25].noGravity = true;
                    num25 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num23, projectile.position.Y + 3f + num24) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 31, 0f, 0f, 100, default, 0.5f);
                    Main.dust[num25].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[num25].velocity *= 0.05f;
                }
            }
            if (Math.Abs(projectile.velocity.X) < 15f && Math.Abs(projectile.velocity.Y) < 15f)
            {
                projectile.velocity *= 1.1f;
            }
        }
        public void DestroyTiles(Projectile projectile, int explosionRadius)
        {

            int minTileX = (int)(projectile.Center.X / 16f - (float)explosionRadius);
            int maxTileX = (int)(projectile.Center.X / 16f + (float)explosionRadius);
            int minTileY = (int)(projectile.Center.Y / 16f - (float)explosionRadius);
            int maxTileY = (int)(projectile.Center.Y / 16f + (float)explosionRadius);
            if (minTileX < 0)
            {
                minTileX = 0;
            }
            if (maxTileX > Main.maxTilesX)
            {
                maxTileX = Main.maxTilesX;
            }
            if (minTileY < 0)
            {
                minTileY = 0;
            }
            if (maxTileY > Main.maxTilesY)
            {
                maxTileY = Main.maxTilesY;
            }
            bool canKillWalls = false;
            for (int x = minTileX; x <= maxTileX; x++)
            {
                for (int y = minTileY; y <= maxTileY; y++)
                {
                    float diffX = Math.Abs((float)x - projectile.Center.X / 16f);
                    float diffY = Math.Abs((float)y - projectile.Center.Y / 16f);
                    double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distance < (double)explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].WallType == 0)
                    {
                        canKillWalls = true;
                        break;
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = true;
            for (int i = minTileX; i <= maxTileX; i++)
            {
                for (int j = minTileY; j <= maxTileY; j++)
                {
                    float diffX = Math.Abs((float)i - projectile.Center.X / 16f);
                    float diffY = Math.Abs((float)j - projectile.Center.Y / 16f);
                    double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distanceToTile < (double)explosionRadius)
                    {
                        bool canKillTile = true;
                        Tile tile = Main.tile[i, j];
                        if (tile.HasTile)
                        {
                            canKillTile = true;
                            if (Main.tileDungeon[(int)tile.TileType] || tile.TileType == 88 || tile.TileType == 21 || tile.TileType == 26 || tile.TileType == 107 || tile.TileType == 108 || tile.TileType == 111 || tile.TileType == 226 || tile.TileType == 237 || tile.TileType == 221 || tile.TileType == 222 || tile.TileType == 223 || tile.TileType == 211 || tile.TileType == 404)
                            {
                                canKillTile = false;
                            }
                            if (!Main.hardMode && tile.TileType == 58)
                            {
                                canKillTile = false;
                            }
                            if (!TileLoader.CanExplode(i, j))
                            {
                                canKillTile = false;
                            }
                            if (canKillTile)
                            {
                                WorldGen.KillTile(i, j, false, false, false);
                                if (!tile.HasTile && Main.netMode != NetmodeID.SinglePlayer)
                                {
                                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                                }
                            }
                        }
                        if (canKillTile)
                        {
                            for (int x = i - 1; x <= i + 1; x++)
                            {
                                for (int y = j - 1; y <= j + 1; y++)
                                {
                                    if (Main.tile[x, y] != null && Main.tile[x, y].WallType > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile[x, y].WallType))
                                    {
                                        WorldGen.KillWall(x, y, false);
                                        if (Main.tile[x, y].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer)
                                        {
                                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = false;
        }
        public void ClusterRocketExplosion(Projectile projectile)
        {
            SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);

            Color transparent7 = Color.Transparent;
            for (int i = 0; i < 30; i++)
            {
                Dust dust57 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent7, 1.5f);
                Dust dust = dust57;
                dust.velocity *= 1.4f;
            }
            for (int n = 0; n < 40; n++)
            {
                Dust dust58 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 228, 0f, 0f, 100, transparent7, 3.5f);
                dust58.noGravity = true;
                Dust dust = dust58;
                dust.velocity *= 7f;
                dust58 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 228, 0f, 0f, 100, transparent7, 1.3f);
                dust = dust58;
                dust.velocity *= 4f;
                dust58.noGravity = true;
            }
            for (int num847 = 0; num847 < 8; num847++)
            {
                Dust dust59 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 100, transparent7, 1.3f);
                Dust dust = dust59;
                dust.velocity *= 4f;
                dust59.noGravity = true;
            }
            for (int num848 = 2; num848 <= 2; num848++)
            {
                for (int num849 = -1; num849 <= 1; num849 += 2)
                {
                    for (int num850 = -1; num850 <= 1; num850 += 2)
                    {
                        Gore gore12 = Gore.NewGoreDirect(projectile.GetSource_FromThis(), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
                        Gore gore = gore12;
                        gore.velocity *= ((num848 == 1) ? 0.4f : 0.8f);
                        gore = gore12;
                        gore.velocity += new Vector2(num849, num850);
                    }
                }
            }
            if (projectile.owner == Main.myPlayer)
            {
                int Cluster = 862; // snowman cannon's projectile, doesn't damage the player
                float num852 = (MathF.PI * 2f);
                float fragmentCount = 59.167f * 6/*Main.rand.Next(2, 3)*/;
                for (float c = 0f; c < 1f; c += fragmentCount / (678f * MathF.PI))
                {

                    float f2 = num852 + c * (MathF.PI * 2f);
                    Vector2 velocity = f2.ToRotationVector2() * (4f + Main.rand.NextFloat() * 2f);
                    velocity += Vector2.UnitY * -1f;
                    int num854 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, velocity, Cluster, projectile.damage / 4, 0f, projectile.owner); Projectile pRojectile = Main.projectile[num854];
                    Projectile projectile2 = pRojectile;
                    projectile2.timeLeft = 30;
                }
            }
        }
        public void LiquidRocket(Projectile projectile)
        {

            SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);

            Color transparent5 = Color.Transparent;
            int num840 = 31;
            if (WetRocket)
            {
                num840 = Dust.dustWater();
            }
            if (LavaRocket)
            {
                num840 = 35;
            }
            if (HoneyRocket)
            {
                num840 = 152;
            }
            for (int num841 = 0; num841 < 30; num841++)
            {
                Dust dust51 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent5, 1.5f);
                Dust dust = dust51;
                dust.velocity *= 1.4f;
            }
            for (int num842 = 0; num842 < 80; num842++)
            {
                Dust dust52 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num840, 0f, 0f, 100, transparent5, 1.2f);
                Dust dust = dust52;
                dust.velocity *= 7f;
                dust52 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num840, 0f, 0f, 100, transparent5, 0.3f);
                dust = dust52;
                dust.velocity *= 4f;
            }
            for (int num843 = 1; num843 <= 2; num843++)
            {
                for (int num844 = -1; num844 <= 1; num844 += 2)
                {
                    for (int num845 = -1; num845 <= 1; num845 += 2)
                    {
                        Gore gore9 = Gore.NewGoreDirect(projectile.GetSource_FromThis(), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
                        Gore gore = gore9;
                        gore.velocity *= ((num843 == 1) ? 0.4f : 0.8f);
                        gore = gore9;
                        gore.velocity += new Vector2(num844, num845);
                    }
                }
            }
            if (Main.netMode != 1)
            {
                if (DryRocket)
                {
                    Point pt5 = projectile.Center.ToTileCoordinates();
                    projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt5, 3.5f, DelegateMethods.SpreadDry);
                }
                if (WetRocket)
                {
                    Point pt5 = projectile.Center.ToTileCoordinates();
                    projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt5, 3.5f, DelegateMethods.SpreadWater);
                }
                if (LavaRocket)
                {
                    Point pt5 = projectile.Center.ToTileCoordinates();
                    projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt5, 3.5f, DelegateMethods.SpreadLava);
                }
                if (HoneyRocket)
                {
                    Point pt5 = projectile.Center.ToTileCoordinates();
                    projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt5, 3.5f, DelegateMethods.SpreadHoney);
                }
            }
        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (DryRocket || WetRocket || LavaRocket || HoneyRocket)
            {
                LiquidRocket(projectile);
            }

        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (DryRocket || WetRocket || LavaRocket || HoneyRocket)
            {
                LiquidRocket(projectile);
                return false;
            }
            return true;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (DryRocket || WetRocket || LavaRocket || HoneyRocket)
            {
                LiquidRocket(projectile);
            }
            if (LuminiteRocket)
            {
                Projectile.NewProjectile(projectile.GetSource_OnHit(target), projectile.Center, projectile.velocity, ProjectileType<LuminiteBoom>(), projectile.damage, projectile.knockBack);
                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);                   
       
            }
        }

        public void RocketStats(Projectile projectile, bool defaultExplosion)
        {
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Ranged;
            projectile.penetrate = 4;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 600;
            IsARocket = true;
            projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            if (defaultExplosion)
            {
                projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = true;
            }
            projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 120;
        }
        public void DestructiveRocketStats(Projectile projectile)
        {
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Ranged;
            projectile.penetrate = 4;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 600;
            IsARocket = true;

        }
        public void SuperRocketStats(Projectile projectile, bool defaultExplosion)
        {
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Ranged;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 600;
            IsARocket = true;
            projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            if (defaultExplosion)
            {
                projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = true;
            }
            projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 180;
        }
        public void DirectRocketStats(Projectile projectile, bool defaultExplosion)
        {
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Ranged;
            projectile.extraUpdates = 1;
            projectile.penetrate = 3; projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            IsARocket = true;
            projectile.GetGlobalProjectile<ProjectileStats>().FirstHitDamage = 1.4f;
           projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 80;
            if (defaultExplosion)
            {
                projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = true;
            }
        }
        public void MiniNukeStats(Projectile projectile, bool defaultExplosion)
        {
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Ranged;
            projectile.penetrate = 5; projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            IsARocket = true;
            projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            if (defaultExplosion)
            {
                projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = true;
            }
            projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 250;
        }

        public void LuminiteStats(Projectile projectile)
        {
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Ranged;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            LuminiteRocket = true; 
            IsARocket = true;

        }
        public void LuminiteRocketAI(Projectile projectile)
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (projectile.alpha < 170)
            {
                float num222 = 3f;
                for (int num223 = 0; num223 < num222; num223++)
                {
                    int num224 = Dust.NewDust(projectile.position, 1, 1, 229);
                    Main.dust[num224].position = projectile.Center - projectile.velocity / num222 * num223;
                    Dust dust3 = Main.dust[num224];
                    dust3.velocity *= 0f;
                    Main.dust[num224].noGravity = true;
                    Main.dust[num224].alpha = 200;
                    Main.dust[num224].scale = 0.5f;
                }
            }
            float num225 = MathF.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
            float num226 = projectile.localAI[0];
            if (num226 == 0f)
            {
                projectile.localAI[0] = num225;
                num226 = num225;
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
        }
        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (LuminiteRocket)
            {
                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);
                projectile.position = projectile.Center;
                projectile.width = (projectile.height = 144);
                projectile.position.X -= projectile.width / 2;
                projectile.position.Y -= projectile.height / 2;
                for (int num276 = 0; num276 < 4; num276++)
                {
                    Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                }
                for (int num278 = 0; num278 < 40; num278++)
                {
                    int num279 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 0, default(Color), 2.5f);
                    Main.dust[num279].noGravity = true;
                    Dust dust2 = Main.dust[num279];
                    dust2.velocity *= 3f;
                    num279 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 100, default(Color), 1.5f);
                    dust2 = Main.dust[num279];
                    dust2.velocity *= 2f;
                    Main.dust[num279].noGravity = true;
                }
            }
        }
    }
    public class Rocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14; AIType = ProjectileType<Rocket>();

            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, true);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
    }
    public class DestructiveRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            AIType = ProjectileType<Rocket>();

            Projectile.GetGlobalProjectile<NewRockets>().DestructiveRocketStats(Projectile);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            TRAEMethods.DefaultExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 3);
        }
    }
    public class SuperRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            AIType = ProjectileType<Rocket>();
            Projectile.GetGlobalProjectile<NewRockets>().SuperRocketStats(Projectile, true);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
    }
    public class DirectRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            AIType = ProjectileType<Rocket>();
            Projectile.GetGlobalProjectile<NewRockets>().DirectRocketStats(Projectile, true);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
    }
    public class MiniNuke : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            AIType = ProjectileType<Rocket>();
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, true);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
    }
    public class DestructiveMiniNuke : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            AIType = ProjectileType<Rocket>();
            Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, true);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            TRAEMethods.DefaultExplosion(Projectile);
            Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 7);
        }
    }
    public class ClusterRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            AIType = ProjectileType<Rocket>();
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, true);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.GetGlobalProjectile<NewRockets>().ClusterRocketExplosion(Projectile);
        }
    }
    public class HeavyRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            AIType = ProjectileType<Rocket>();
            Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, true);
            Projectile.GetGlobalProjectile<NewRockets>().HeavyRocket = true;

        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().RocketAI(Projectile);
        }
    }
    public class LuminiteRocketP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 14;
            Projectile.GetGlobalProjectile<NewRockets>().LuminiteStats(Projectile);
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<NewRockets>().LuminiteRocketAI(Projectile);
        }
    }
    public class LuminiteBoom : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 250;
            Projectile.timeLeft = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
			Projectile.alpha = 255;
            Projectile.GetGlobalProjectile<NewRockets>().IsARocket = true;
        }
        bool onlyOnce = true;
        public override void AI()
        {
            if (onlyOnce)
            {
                onlyOnce = false;
                for (int num276 = 0; num276 < 4; num276++)
                {
                    Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                }
                for (int num278 = 0; num278 < 40; num278++)
                {
                    int num279 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0f, 0f, 0, default(Color), 2.5f);
                    Main.dust[num279].noGravity = true;
                    Dust dust2 = Main.dust[num279];
                    dust2.velocity *= 3f;
                    num279 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0f, 0f, 100, default(Color), 1.5f);
                    dust2 = Main.dust[num279];
                    dust2.velocity *= 2f;
                    Main.dust[num279].noGravity = true;
                }
            }
        }
    }
}

