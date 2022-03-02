using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using TRAEProject.Changes.Accesory;
using TRAEProject.NewContent.Items.Weapons.Launchers.CryoCannon;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;
namespace TRAEProject.Changes.Weapon
{
    public class StunningProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
         
            if (Main.player[projectile.owner].GetModPlayer<RangedStats>().AlphaScope > 0 && projectile.GetGlobalProjectile<MagicQuiverEffect>().AffectedByAlphaScope && crit)
            {
                int duration = 15 + 15 * Main.player[projectile.owner].GetModPlayer<RangedStats>().AlphaScope;
                target.GetGlobalNPC<Stun>().StunMe(target, duration);
            }
            if (Main.player[projectile.owner].GetModPlayer<RangedStats>().RocketsStun > 0 && projectile.GetGlobalProjectile<NewRockets>().IsARocket && crit)
            {
                int duration = 15 + 15 * Main.player[projectile.owner].GetModPlayer<RangedStats>().RocketsStun + Main.player[projectile.owner].GetModPlayer<RangedStats>().AlphaScope;
                target.GetGlobalNPC<Stun>().StunMe(target, duration);
            }
            if (projectile.GetGlobalProjectile<NewRockets>().HeavyRocket == true)
            {
                target.GetGlobalNPC<Stun>().StunMe(target, 30);
            }
            switch(projectile.type)
            {
                case ProjectileID.TheDaoofPow:
                    if (Main.rand.Next(3) == 0)
                    {
                        int duration = Main.rand.Next(60, 80);
                        target.GetGlobalNPC<Stun>().StunMe(target, duration); 
                    }
                
                    break;
            }
        }
    }
}
