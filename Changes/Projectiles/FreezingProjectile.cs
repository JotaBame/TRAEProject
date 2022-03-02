using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Items.Summoner.AbsoluteZero;
using TRAEProject.Common.GlobalNPCs;
using TRAEProject.Items.FlamethrowerAmmo;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Projectiles
{
    public class FreezingProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        int duration = 0;
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
<<<<<<< Updated upstream:Changes/Projectiles/FreezingProjectile.cs
            if (target.HasBuff(BuffType<AbsoluteZeroTag>()) && crit == true && projectile.minion)
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, damage / 4);
=======
            if (projectile.GetGlobalProjectile<CryoRockets>().IceRocket && projectile.GetGlobalProjectile<NewRockets>().HeavyRocket == true)
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, 180);
            }
            if (projectile.GetGlobalProjectile<CryoRockets>().IceRocket)
            {
                int duration = Main.rand.Next(80, 120);
                target.GetGlobalNPC<Freeze>().FreezeMe(target, duration);
>>>>>>> Stashed changes:Changes/Weapon/FreezingProjectile.cs
            }
            if (projectile.type == ProjectileType<FrozenGelP>() && Main.rand.Next(10) == 0)
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, 30);
            }
            if (projectile.type == ProjectileType<AbsoluteZeroP>() && Main.rand.Next(5) == 0)
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, 45);
            }
            switch (projectile.type)
            {

                case ProjectileID.FrostBoltSword:
<<<<<<< Updated upstream:Changes/Projectiles/FreezingProjectile.cs
                    target.GetGlobalNPC<Freeze>().FreezeMe(target, 120);
=======
                    duration = Main.rand.Next(80, 120);
                    target.GetGlobalNPC<Freeze>().FreezeMe(target, duration); 
>>>>>>> Stashed changes:Changes/Weapon/FreezingProjectile.cs
                    break;
                case ProjectileID.BallofFrost:
                    duration = Main.rand.Next(80, 120);
                    target.GetGlobalNPC<Freeze>().FreezeMe(target, duration);
                    break;
                case ProjectileID.FrostArrow:
                    duration = Main.rand.Next(80, 120);
                    target.GetGlobalNPC<Freeze>().FreezeMe(target, duration);
                    break;
                case ProjectileID.IceBoomerang:
                    if (Main.rand.Next(3) == 0)
                    {
                        target.GetGlobalNPC<Freeze>().FreezeMe(target, 75);
                    }
                    break;
                case ProjectileID.FrostBoltStaff:
                    if (Main.rand.Next(3) == 0)
                    {
                        target.GetGlobalNPC<Freeze>().FreezeMe(target, 90);
                    }
                    break;
            }
        }
    }
}
