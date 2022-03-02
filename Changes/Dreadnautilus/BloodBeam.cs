using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRAEProject;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Dreadnautilus
{
    public class BloodBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = chargeTime + duration;
        }
        public const int beamLength = 4000;
        public const int chargeTime = 25;
        public const int duration = 60;
        int timer = 0;
        public override void AI()
        {

            NPC owner = Main.npc[(int)Projectile.ai[0]];
            if (!owner.active || owner.type != NPCID.BloodNautilus)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public static void Draw(Projectile Projectile)
        {
            if (Projectile.timeLeft < chargeTime)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                int frame = (Projectile.timeLeft / 6) % 4;
                float scaleY = (Projectile.timeLeft - (duration-20));
                if(scaleY < 0)
                {
                    scaleY = 0;
                }
                scaleY = scaleY / 20f;
                scaleY = 1f - scaleY;
                for (float k = 0; k < beamLength; k += texture.Width)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center + TRAEMethods.PolarVector(k, Projectile.rotation) - Main.screenPosition, new Rectangle(0, frame * 32, 32, 40), Color.White, Projectile.rotation, Vector2.UnitY * 13, new Vector2(1f, scaleY), SpriteEffects.None, 0);
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            if (Projectile.timeLeft < chargeTime)
            {
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + TRAEMethods.PolarVector(beamLength, Projectile.rotation), 26, ref point);
            }
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

    }
}
