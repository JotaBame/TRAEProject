using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.Plantera
{
    class PlanteraSpikeBall : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.type == ProjectileID.ThornBall && GetInstance<TRAEConfig>().PlanteraRework)
            {
                projectile.alpha = 0;
                projectile.scale = 1.5f;
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.ThornBall && Main.netMode == NetmodeID.SinglePlayer && GetInstance<TRAEConfig>().PlanteraRework)
            {
                if (projectile.ai[1] == 0)
                {

                }
                else
                {
                    return true;
                }
                //Main.NewText(projectile.ai[0] + ", " + projectile.ai[1] + ", " + projectile.localAI[0] + ", " + projectile.localAI[1]);
                return false;
            }
            return true;
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            
            if (projectile.type == ProjectileID.ThornBall && Main.netMode == NetmodeID.SinglePlayer && GetInstance<TRAEConfig>().PlanteraRework)
            {
                projectile.localAI[0] = 1;
                if(projectile.ai[1] != 0)
                {
                    return true;
                }
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                return false;
            }
            return true;
        }
        
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileID.ThornBall && Main.netMode == NetmodeID.SinglePlayer && GetInstance<TRAEConfig>().PlanteraRework)
            {
                Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
                Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition,
                            null, lightColor, projectile.rotation,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0);
                return false;
            }
            return base.PreDraw(projectile, ref lightColor);
        }
    }
}
