using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.LunaticCultist
{
    public class CultistSpells : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.type == ProjectileID.CultistBossIceMist || projectile.type == ProjectileID.CultistBossLightningOrb)
            {
                projectile.hostile = false;
            }
            if(projectile.type == ProjectileID.AncientDoomProjectile)
            {
                projectile.light = 1f;
            }
        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (Main.expertMode)
            {
                if (projectile.type == ProjectileID.CultistBossFireBall)
                {
                    target.AddBuff(BuffID.OnFire, Main.rand.Next(180, 240));
                }
                if (projectile.type == ProjectileID.CultistBossIceMist)
                {
                    target.AddBuff(BuffID.Frozen, Main.rand.Next(20, 25));
                    target.AddBuff(BuffID.Chilled, Main.rand.Next(60, 120));
                }
                if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    target.AddBuff(BuffID.Electrified, Main.rand.Next(75, 150));
                }
            }
        }
        public override void AI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.CultistBossIceMist)
            {
                if (projectile.ai[1] == 1f)
                {


                    if (projectile.ai[0] >= 130f)
                    {
                    }
                    else
                    {
                        projectile.alpha += 6;
                    }
                    if (projectile.alpha >= 40 && projectile.ai[0] < 10)
                    {
                        projectile.ai[0] = 1;
                    }
                    else
                    {
                        projectile.hostile = true;
                    }
                }
                else
                {
                    projectile.hostile = true;
                }
            }
        }
        /*
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.CultistBossIceMist && projectile.ai[1] != 1f)
            {
                if (projectile.localAI[1] == 0f)
                {
                    projectile.localAI[1] = 1f;
                    SoundEngine.PlaySound(SoundID.Item120, projectile.position);
                }
                projectile.ai[0]++;
                projectile.hostile = true;
                projectile.position -= projectile.velocity;
                if (projectile.ai[0] >= 40f)
                {
                    projectile.alpha += 3;
                }
                else
                {
                    projectile.alpha -= 40;
                }
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                }
                if (projectile.ai[0] >= 90f)
                {
                    projectile.Kill();
                    return false;
                }
                Vector2 value38 = new Vector2(0f, -720f).RotatedBy(projectile.velocity.ToRotation());
                float scaleFactor3 = projectile.ai[0] / 45f;
                Vector2 spinningpoint13 = value38 * scaleFactor3;
                for (int num725 = 0; num725 < 6; num725++)
                {
                    Vector2 vector52 = projectile.Center + spinningpoint13.RotatedBy((float)num725 * (MathF.PI * 2f) / 6f);
                    Lighting.AddLight(vector52, 0.3f, 0.75f, 0.9f);
                    for (int num726 = 0; num726 < 2; num726++)
                    {
                        int num727 = Dust.NewDust(vector52 + Utils.RandomVector2(Main.rand, -8f, 8f) / 2f, 8, 8, 197, 0f, 0f, 100, Color.Transparent);
                        Main.dust[num727].noGravity = true;
                    }
                }
                return false;
            }
            return base.PreAI(projectile);
        }
        public override bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projectile.type == ProjectileID.CultistBossIceMist && projectile.ai[1] != 1f)
            {
                Vector2 value9 = new Vector2(0f, -720f).RotatedBy(projectile.velocity.ToRotation());
                float scaleFactor5 = projectile.ai[0] / 45f;
                Vector2 spinningpoint = value9 * scaleFactor5;
                for (int num12 = 0; num12 < 6; num12++)
                {
                    float num13 = (float)num12 * (MathF.PI * 2f) / 6f;
                    if (Utils.CenteredRectangle(projectile.Center + spinningpoint.RotatedBy(num13), new Vector2(30f, 30f)).Intersects(targetHitbox))
                    {
                        return true;
                    }
                }
            }
                return base.Colliding(projectile, projHitbox, targetHitbox);
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileID.CultistBossIceMist && projectile.ai[1] != 1f)
            {
                Vector2 vector43 = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                Texture2D value108 = TextureAssets.Projectile[projectile.type].Value;
                Microsoft.Xna.Framework.Color color70 = projectile.GetAlpha(lightColor);
                Vector2 origin18 = new Vector2(value108.Width, value108.Height) / 2f;
                float num295 = projectile.rotation;
                Vector2 vector44 = Vector2.One * projectile.scale;
                Microsoft.Xna.Framework.Rectangle? sourceRectangle2 = null;


                value108 = TextureAssets.Extra[35].Value;
                Microsoft.Xna.Framework.Rectangle rectangle20 = value108.Frame(1, 3);
                origin18 = rectangle20.Size() / 2f;
                Vector2 value115 = new Vector2(0f, -720f).RotatedBy(projectile.velocity.ToRotation());
                float scaleFactor3 = projectile.ai[0] / 45f;
                Vector2 spinningpoint7 = value115 * scaleFactor3;
                for (int num304 = 0; num304 < 6; num304++)
                {
                    float num305 = (float)num304 * (MathF.PI * 2f) / 6f;
                    Vector2 value116 = projectile.Center + spinningpoint7.RotatedBy(num305);
                    Main.EntitySpriteDraw(value108, value116 - Main.screenPosition, rectangle20, color70, num305 + projectile.velocity.ToRotation() + MathF.PI, origin18, projectile.scale, SpriteEffects.None, 0);
                    rectangle20.Y += rectangle20.Height;
                    if (rectangle20.Y >= value108.Height)
                    {
                        rectangle20.Y = 0;
                    }
                }

                return false;
            }
                return base.PreDraw(projectile, ref lightColor);
        }
        */
    }
    public class SpellNPCs : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            if(npc.type == NPCID.AncientLight)
            {
                npc.lifeMax = 1000;
                npc.chaseable = true;
            }
            if(npc.type == NPCID.AncientDoom)
            {
                npc.damage = 0;
            }
        }
    }
}
