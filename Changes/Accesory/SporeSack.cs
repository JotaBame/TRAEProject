using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Buffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Accesory
{
    public class SporeSackProj : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.type == ProjectileID.SporeGas || projectile.type == ProjectileID.SporeGas2 || projectile.type == ProjectileID.SporeGas3)
            {
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 10;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(projectile.type == ProjectileID.SporeGas || projectile.type == ProjectileID.SporeGas2 || projectile.type == ProjectileID.SporeGas3)
            {
                Projectile.perIDStaticNPCImmunity[ProjectileID.SporeGas][target.whoAmI] = Main.GameUpdateCount + 10;
                Projectile.perIDStaticNPCImmunity[ProjectileID.SporeGas2][target.whoAmI] = Main.GameUpdateCount + 10;
                Projectile.perIDStaticNPCImmunity[ProjectileID.SporeGas3][target.whoAmI] = Main.GameUpdateCount + 10;
            }
        }
    }
}