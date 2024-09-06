using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria;

using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;

namespace TRAEProject.Changes.NPCs
{
    public class Necromancer : GlobalNPC
    {
        public const int beamLength = 200;
        public const int chargeTime = 45;
        const int cycleTime = 100;
        const int scanAttempts = 9;
        public override void AI(NPC npc)
        {
            if(npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored)
            { 
                npc.ai[1] = 2; //shuts off vanilla Shooting

                npc.TargetClosest(true);
                Player player = Main.player[npc.target];
                bool doubleBreak = false;
                npc.ai[2]++;
                if(npc.localAI[3] <= 0)
                {
                    for(int i = 0; i < scanAttempts; i++)
                    {
                        float initialDir = (player.Center - npc.Center).ToRotation() + ((float)i / (float)scanAttempts) * 2f * MathF.PI + npc.ai[2];
                        Vector2 scannerVel = TRAEMethods.PolarVector(6, initialDir);
                        Vector2 scannerPos = npc.Center;

                        for(int step = 0; step < beamLength; step++)
                        {
                            if(Collision.TileCollision(scannerPos, scannerVel, 4, 4, true, true) != scannerVel)
                            {
                                Vector2 oldVel = scannerVel;
                                scannerVel = Collision.TileCollision(scannerPos, scannerVel, 4, 4, true, true);
                                if(scannerVel.X != oldVel.X)
                                {
                                    scannerPos.X += scannerVel.X;
                                    scannerVel.X = 0f - oldVel.X;
                                }
                                if(scannerVel.Y != oldVel.Y)
                                {
                                    scannerPos.Y += scannerVel.Y;
                                    scannerVel.Y = 0f - oldVel.Y;
                                }
                            }
                            scannerPos += scannerVel;
                            //player.getRect().Contains((int)scannerPos.X, (int)scannerPos.Y)
                            if((player.Center - scannerPos).Length() < 10)
                            {
                                //Main.NewText("i: " + i);
                                int projDamage = npc.GetAttackDamage_ForProjectiles(30, (float)30 * 0.8f);
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, TRAEMethods.PolarVector(6, initialDir), ModContent.ProjectileType<NeoShadowbeam>(), projDamage, 0f, Main.myPlayer);
                                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, TRAEMethods.PolarVector(6, initialDir), ProjectileID.ShadowBeamHostile, projDamage, 0f, Main.myPlayer);
                                doubleBreak = true;
                                npc.localAI[3] = cycleTime;
                                break;
                            }
                        }
                        if(doubleBreak)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    npc.localAI[3]--;
                }
                

                //Main.NewText(npc.localAI[0] + ", " + npc.localAI[1] + ", " + npc.localAI[2] + ", " + npc.localAI[3]);
                //Main.NewText(npc.ai[0] + ", " + npc.ai[1] + ", " + npc.ai[2] + ", " + npc.ai[3]);
            }
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if(npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored)
            {
                npc.frame.Y = 0;
                if((int)npc.localAI[3] < cycleTime - chargeTime && npc.localAI[3] > (cycleTime - chargeTime) - 30)
                {
                    npc.frame.Y = 1 * frameHeight;
                }
            }
        }
    }
    public class NeoShadowbeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            //Projectile.aiStyle = 48;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = Necromancer.beamLength;
            Projectile.timeLeft = Necromancer.beamLength;
            Projectile.penetrate = -1;
        }
        List<Vector2> points = new List<Vector2>();
        bool donePlanning = false;
        bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                runOnce = false;
                points.Add(Projectile.Center);
            }
            if(Projectile.timeLeft <= 2 && !donePlanning)
            {
                points.Add(Projectile.Center);
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = Necromancer.chargeTime;
                Projectile.extraUpdates = 0;
                donePlanning = true;
            }
            //Main.NewText(points.Count);
            if(Projectile.timeLeft == 2 && donePlanning)
            {
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                for(int i = 0; i < points.Count; i++)
                {
                    if(i < points.Count - 1)
                    {
                        Vector2 start = points[i];
                        Vector2 end = points[i + 1];
                        float dir = (end - start).ToRotation();
                        float length = (end - start).Length();
                        for(int step = 0; step < length; step += 6)
                        {
                            for (int dustCounter = 0; dustCounter < 3; dustCounter++)
                            {
                                Vector2 dustPosition = start + TRAEMethods.PolarVector(step, dir);
                                dustPosition -= TRAEMethods.PolarVector(6, dir) * ((float)dustCounter * 0.3334f);
                                int dustIndex = Dust.NewDust(dustPosition, 1, 1, 173);
                                Main.dust[dustIndex].position = dustPosition;
                                Main.dust[dustIndex].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                                Dust dust2 = Main.dust[dustIndex];
                                dust2.velocity *= 0.2f;
                            }
                        }
                    }
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(!donePlanning)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.position.X += Projectile.velocity.X;
                    Projectile.velocity.X = 0f - oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.position.Y += Projectile.velocity.Y;
                    Projectile.velocity.Y = 0f - oldVelocity.Y;
                }
                points.Add(Projectile.Center);
            }
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Slow, Main.rand.Next(300, 900));
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.timeLeft <= 2 && donePlanning)
            {
                for(int i = 0; i < points.Count; i++)
                {
                    if(i < points.Count - 1)
                    {
                        float useless = 0;
                        if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), points[i], points[i + 1], 4, ref useless))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(donePlanning && Projectile.timeLeft > 2)
            {
                for(int i = 0; i < points.Count; i++)
                {
                    if(i < points.Count - 1)
                    {
                        DrawLaser(points[i], points[i + 1], Math.Max(0.1f, 1f - ((float)Projectile.timeLeft / (float)Necromancer.chargeTime)), Color.Lavender);
                    }
                }
            }
            return false;
        }
        public static void DrawLaser(Vector2 start, Vector2 end, float opacity, Color color, bool pointed = false)
        {
            float dir = (end - start).ToRotation();
            float length = (end - start).Length();
            Vector2 segPos = start - Main.screenPosition;
            Texture2D blankTexture = TextureAssets.Extra[178].Value;
            Rectangle frame = new Rectangle(0, 0, 1, 1);
            Vector2 origin = new Vector2(0, 0.5f);
            if(pointed)
            {
                blankTexture = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/PointedWarning").Value;
                frame = new Rectangle(0, 0, 10, 9);
                origin = new Vector2(0, 4.5f);
            }
            Vector2 texScale = new Vector2(length, 6 * opacity);
            Main.EntitySpriteDraw(blankTexture, segPos, frame, color * opacity * 0.75f, dir, new Vector2(0, 0.5f), texScale, SpriteEffects.None, 0);
        }
    }
}