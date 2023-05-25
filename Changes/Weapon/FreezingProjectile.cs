using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Weapons.Summoner.AbsoluteZero;
using TRAEProject.NewContent.Items.Weapons.Launchers.CryoCannon;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.FlamethrowerAmmo;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Weapon.Ranged.Rockets;

namespace TRAEProject.Changes.Projectiles
{
    public class FreezingProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        int duration = 0;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.GetGlobalProjectile<CryoRockets>().IceRocket)
            {
                duration = Main.rand.Next(100, 120);
                if (projectile.GetGlobalProjectile<NewRockets>().HeavyRocket)
                {
                    duration += 30;
                }
                if (projectile.GetGlobalProjectile<NewRockets>().LuminiteRocket)
                {
                    duration *= 2;
                }
                target.GetGlobalNPC<Freeze>().FreezeMe(target, duration);
            }
            if (projectile.type == ProjectileType<FrozenGelP>() && Main.rand.NextBool(5))
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, 60);
            }
            if (projectile.type == ProjectileType<AbsoluteZeroP>() && Main.rand.NextBool(5))
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, 45);
            }
            switch (projectile.type)
            {

                case ProjectileID.FrostBoltSword:
                    if (Main.rand.NextBool(5))
                    {
                        duration = Main.rand.Next(80, 120);
                        target.GetGlobalNPC<Freeze>().FreezeMe(target, duration);

                    }
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
                    if (Main.rand.NextBool(3))
                    {
                        target.GetGlobalNPC<Freeze>().FreezeMe(target, 75);
                    }
                    break;
                case ProjectileID.FrostBoltStaff:
                    if (Main.rand.NextBool(3))
                    {
                        target.GetGlobalNPC<Freeze>().FreezeMe(target, 90);
                    }
                    break;
            }
        }
    }
}
