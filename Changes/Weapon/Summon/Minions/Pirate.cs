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

namespace TRAEProject.Changes.Weapon.Summon.Minions
{
    public static class Pirate
    {
        public static bool AI(Projectile projectile)
        {
            //Main.NewText(projectile.ai[0] + ", " + projectile.ai[1] + ", " + projectile.localAI[0] + ", " + projectile.localAI[1]);
            //Main.NewText(projectile.frame);
            if (projectile.GetGlobalProjectile<MinionChanges>().hook != null && projectile.GetGlobalProjectile<MinionChanges>().hook.active)
            {
                if (projectile.GetGlobalProjectile<MinionChanges>().hook.ai[1] == 1)
                {
                    projectile.tileCollide = false;
                    projectile.ai[0] = 0;
                    projectile.velocity = (projectile.GetGlobalProjectile<MinionChanges>().hook.Center - projectile.Center).SafeNormalize(-Vector2.UnitY) * 15f;
                    projectile.spriteDirection = Math.Sign(projectile.velocity.X);
                    projectile.frame = 14;
                    if ((projectile.GetGlobalProjectile<MinionChanges>().hook.Center - projectile.Center).Length() < 20)
                    {
                        projectile.GetGlobalProjectile<MinionChanges>().hook.Kill();
                        projectile.GetGlobalProjectile<MinionChanges>().hook = null;
                    }

                    Player player = Main.player[projectile.owner];
                    if (Main.player[projectile.owner].dead)
                    {
                        Main.player[projectile.owner].pirateMinion = false;
                    }
                    if (Main.player[projectile.owner].pirateMinion)
                    {
                        projectile.timeLeft = 2;
                    }
                    return false;
                }
            }
            else
            {
                projectile.tileCollide = true;
                projectile.localAI[1] += 1f;
                if (projectile.localAI[1] > 120f)
                {
                    if (TRAEMethods.ClosestNPC(ref projectile.GetGlobalProjectile<MinionChanges>().target, 2000, projectile.Center, false, Main.player[projectile.owner].MinionAttackTargetNPC))
                    {
                        if (projectile.GetGlobalProjectile<MinionChanges>().target.Distance(projectile.Center) > 300 || projectile.ai[0] == 1)
                        {
                            projectile.localAI[1] = 0;
                            projectile.GetGlobalProjectile<MinionChanges>().hook = Main.projectile[Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, TRAEMethods.PolarVector(12, (projectile.GetGlobalProjectile<MinionChanges>().target.Center - projectile.Center).ToRotation()), ProjectileType<PirateHook>(), projectile.damage, 0, projectile.owner)];
                            for (int n = 0; n < 200; n++)
                            {
                                if (Main.npc[n] != projectile.GetGlobalProjectile<MinionChanges>().target)
                                {
                                    projectile.GetGlobalProjectile<MinionChanges>().hook.localNPCImmunity[n] = -1;
                                }
                            }
                            projectile.GetGlobalProjectile<MinionChanges>().hook.ai[0] = projectile.GetGlobalProjectile<MinionChanges>().target.whoAmI;
                        }
                    }
                }
            }
            return true;
        }
        public static void DrawHook(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileID.PirateCaptain || projectile.type == ProjectileID.OneEyedPirate || projectile.type == ProjectileID.SoulscourgePirate)
            {
                if (projectile.GetGlobalProjectile<MinionChanges>().hook != null && projectile.GetGlobalProjectile<MinionChanges>().hook.active && projectile.GetGlobalProjectile<MinionChanges>().hook.type == ProjectileType<PirateHook>())
                {
                    Vector2 mountedCenter = projectile.GetGlobalProjectile<MinionChanges>().hook.Center;

                    float num128 = projectile.Center.X;
                    float num129 = projectile.Center.Y;
                    float x9 = projectile.velocity.X;
                    float num130 = projectile.velocity.Y;
                    if (x9 == 0f && num130 == 0f)
                    {
                        num130 = 0.0001f;
                        num130 = 0.0001f;
                    }
                    float num131 = (float)Math.Sqrt(x9 * x9 + num130 * num130);
                    num131 = 20f / num131;
                    if (projectile.ai[0] == 0f)
                    {
                        num128 -= projectile.velocity.X * num131;
                        num129 -= projectile.velocity.Y * num131;
                    }
                    else
                    {
                        num128 += projectile.velocity.X * num131;
                        num129 += projectile.velocity.Y * num131;
                    }
                    Vector2 vector26 = new Vector2(num128, num129);
                    x9 = mountedCenter.X - vector26.X;
                    num130 = mountedCenter.Y - vector26.Y;
                    float rotation22 = (float)Math.Atan2(num130, x9) - 1.57f;
                    if (projectile.alpha == 0)
                    {
                        int num132 = -1;
                        if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
                        {
                            num132 = 1;
                        }
                    }
                    bool flag24 = true;
                    while (flag24)
                    {
                        float num133 = (float)Math.Sqrt(x9 * x9 + num130 * num130);
                        if (num133 < 25f)
                        {
                            flag24 = false;
                            continue;
                        }
                        if (float.IsNaN(num133))
                        {
                            flag24 = false;
                            continue;
                        }
                        num133 = 12f / num133;
                        x9 *= num133;
                        num130 *= num133;
                        vector26.X += x9;
                        vector26.Y += num130;
                        x9 = mountedCenter.X - vector26.X;
                        num130 = mountedCenter.Y - vector26.Y;
                        Microsoft.Xna.Framework.Color color30 = Lighting.GetColor((int)vector26.X / 16, (int)(vector26.Y / 16f));
                        Main.EntitySpriteDraw(TextureAssets.Chain.Value, new Vector2(vector26.X - Main.screenPosition.X, vector26.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.Chain.Width(), TextureAssets.Chain.Height()), color30, rotation22, new Vector2((float)TextureAssets.Chain.Width() * 0.5f, (float)TextureAssets.Chain.Height() * 0.5f), 1f, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
    public class PirateHook : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 1;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 720;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];
            Projectile.timeLeft = 2;
            if (Projectile.ai[1] != 1)
            {
                if (target != null && target.active)
                {
                    Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 14f;
                    Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                }
                else
                {
                    Projectile.Kill();
                }

            }
            else
            {
                Sticking();
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.ai[1] = 1;
        }
        void Sticking()
        {
            int aiFactor = 15; // Change projectile factor to change the 'lifetime' of projectile sticking javelin
            bool killProj = false; // if true, kill projectile at the end
            bool hitEffect = false; // if true, perform a hit effect
            Projectile.localAI[0] += 1f;
            // Every 30 ticks, the javelin will perform a hit effect
            hitEffect = Projectile.localAI[0] % 30f == 0f;
            int projTargetIndex = (int)Projectile.ai[0];
            if (Projectile.localAI[0] >= (float)(60 * aiFactor)// If it's time for projectile javelin to die, kill it
                || (projTargetIndex < 0 || projTargetIndex >= 200)) // If the index is past its limits, kill it
            {
                killProj = true;
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage) // If the target is active and can take damage
            {
                // Set the projectile's position relative to the target's center
                Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                if (hitEffect) // Perform a hit effect here
                {
                    Main.npc[projTargetIndex].HitEffect(0, 1.0);
                }
            }
            else // Otherwise, kill the projectile
            {
                killProj = true;
            }

            if (killProj) // Kill the projectile
            {
                Projectile.Kill();
            }
        }
    }
}
