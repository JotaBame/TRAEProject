 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
using Microsoft.Xna.Framework;

namespace TRAEProject.Changes.Items
{
 
   public class PetProjectiles : GlobalProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.LightPet[ProjectileID.ChesterPet] = true;
            Main.lightPet[BuffID.ChesterPet] = true;
            Main.projPet[ProjectileID.ChesterPet] = false;
            Main.vanityPet[BuffID.ChesterPet] = false;
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if(projectile.type == ProjectileID.ChesterPet)
            {
                return false;
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if(projectile.type == ProjectileID.ChesterPet)
            {
                fallThrough = false;
                if(Main.player[projectile.owner].Center.Y > projectile.Center.Y)
                {
                    fallThrough = true;
                }
            }
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}