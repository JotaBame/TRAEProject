using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.Threading;

namespace TRAEProject.Changes.NPCs.Boss 
{
    public static class SpazPhase3 
    {
        const float flameTime = 400f;
        public static void Header(NPC npc)
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest();
            }
            bool dead3 = Main.player[npc.target].dead;
            float angVel = (MathF.PI / 180f) + (MathF.PI / 150f) * (1f -((float)npc.ai[2] / flameTime)) * (1f -((float)npc.ai[2] / flameTime));
            npc.rotation.SlowRotation((Main.player[npc.target].Center - npc.Center).ToRotation() - MathF.PI/2, angVel);
            
            if (Main.rand.Next(5) == 0)
            {
                int num458 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + (float)npc.height * 0.25f), npc.width, (int)((float)npc.height * 0.5f), 5, npc.velocity.X, 2f);
                Main.dust[num458].velocity.X *= 0.5f;
                Main.dust[num458].velocity.Y *= 0.1f;
            }
            if (Main.netMode != 1 && !Main.dayTime && !dead3 && npc.timeLeft < 10)
            {
                for (int num459 = 0; num459 < 200; num459++)
                {
                    if (num459 != npc.whoAmI && Main.npc[num459].active && (Main.npc[num459].type == 125 || Main.npc[num459].type == 126))
                    {
                        npc.DiscourageDespawn(Main.npc[num459].timeLeft - 1);
                    }
                }
            }
            Vector2 vector43 = Vector2.Zero;
            npc.reflectsProjectiles = false;
            if (Main.dayTime || dead3)
            {
                npc.velocity.Y -= 0.04f;
                npc.EncourageDespawn(10);
                return;
            }
        }
        public static void Flame(NPC npc)
        {
            Header(npc);
            float maxSpeed = 8f;
            float distModifier = MathF.Min(1000f, (Main.player[npc.target].Center - npc.Center).Length()) / 1000f;
            float speed = maxSpeed - maxSpeed * (npc.ai[2] / flameTime);
            speed = distModifier * (maxSpeed - speed) + speed;
            float acceleration = 0.4f - 0.3f * (npc.ai[2] / flameTime);
            acceleration = distModifier * (0.4f - acceleration) + acceleration;
            int num480 = 1;
            if (npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)Main.player[npc.target].width)
            {
                num480 = -1;
            }
            Vector2 vector46 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
            float num481 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) + (float)(num480 * 180) - vector46.X;
            float num482 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector46.Y;
            float num483 = MathF.Sqrt(num481 * num481 + num482 * num482);
            
            
            num483 = speed / num483;
            num481 *= num483;
            num482 *= num483;
            if (npc.velocity.X < num481)
            {
                npc.velocity.X += acceleration;
                if (npc.velocity.X < 0f && num481 > 0f)
                {
                    npc.velocity.X += acceleration;
                }
            }
            else if (npc.velocity.X > num481)
            {
                npc.velocity.X -= acceleration;
                if (npc.velocity.X > 0f && num481 < 0f)
                {
                    npc.velocity.X -= acceleration;
                }
            }
            if (npc.velocity.Y < num482)
            {
                npc.velocity.Y += acceleration;
                if (npc.velocity.Y < 0f && num482 > 0f)
                {
                    npc.velocity.Y += acceleration;
                }
            }
            else if (npc.velocity.Y > num482)
            {
                npc.velocity.Y -= acceleration;
                if (npc.velocity.Y > 0f && num482 < 0f)
                {
                    npc.velocity.Y -= acceleration;
                }
            }
            
            npc.ai[2] += 1f;
            if (npc.ai[2] >= (float)flameTime)
            {
                npc.ai[1] = 1f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.target = 255;
                npc.netUpdate = true;
            }
            
            if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                npc.localAI[2] += 1f;
                if (npc.localAI[2] > 22f)
                {
                    npc.localAI[2] = 0f;
                    SoundEngine.PlaySound(SoundID.Item34 with { MaxInstances = 0 }, npc.position);
                }
                if (Main.netMode != 1)
                {
                    npc.localAI[1] += 6;
                    if (npc.localAI[1] > 8f)
                    {
                        npc.localAI[1] = 0f;
                        int attackDamage_ForProjectiles7 = npc.GetAttackDamage_ForProjectiles(30f, 27f);
                        
                        float multiplier = 1 + 3f * (npc.ai[2] / flameTime);
                        Vector2 shootFrom = npc.Center + TRAEMethods.PolarVector(2, npc.rotation + MathF.PI/2);
                        Vector2 vel = TRAEMethods.PolarVector(6f * multiplier, npc.rotation + MathF.PI/2);
                        int num487 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootFrom, vel, 101, attackDamage_ForProjectiles7, 0f, Main.myPlayer);

                        //Main.projectile[num487].timeLeft += (int)(npc.ai[2] / 3f);
                    }
                }
            }
        }
        public static void Update(NPC npc)
        {
            bool dead2 = Main.player[npc.target].dead;
            if (Main.netMode != 1 && !Main.dayTime && !dead2 && npc.timeLeft < 10)
            {
                for (int num403 = 0; num403 < 200; num403++)
                {
                    if (num403 != npc.whoAmI && Main.npc[num403].active && (Main.npc[num403].type == 125 || Main.npc[num403].type == 126))
                    {
                        npc.DiscourageDespawn(Main.npc[num403].timeLeft - 1);
                    }
                }
            }
            if (Main.dayTime || dead2)
            {
                npc.velocity.Y -= 0.04f;
                npc.EncourageDespawn(10);
                return;
            }
            npc.HitSound = SoundID.NPCHit4;
            npc.damage = (int)((double)npc.defDamage * 1.5);
            npc.defense = npc.defDefense + 18;
            if(npc.ai[1] == 3)
            {
                Cauldron(npc);
            }
            else if(npc.ai[1] == 0)
            {
                Flame(npc);
            }
            else
            {
                Charge(npc);
            }
        }
        public static void Cauldron(NPC npc)
        {
            float angVel = (MathF.PI / 12f);
            npc.rotation.SlowRotation(MathF.PI, angVel);
            float accY = 0.2f;
            float maxVelY = 5f;
            float accX = 0.2f;
            float maxVelX = 20f;
            
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];

            npc.velocity.X += accX * Math.Sign(player.Center.X - npc.Center.X);
            if(Math.Abs(npc.velocity.X) > maxVelX)
            {
                npc.velocity.X = maxVelX * Math.Sign(player.Center.X - npc.Center.X);
            }

            npc.velocity.Y += accY * Math.Sign(player.Center.Y - npc.Center.Y);
            if(Math.Abs(npc.velocity.Y) > maxVelY)
            {
                npc.velocity.Y = maxVelY * Math.Sign(player.Center.Y - npc.Center.Y);
            }

            if(Math.Sign(npc.velocity.X) != Math.Sign(player.Center.X - npc.Center.X))
            {
                npc.velocity.X *= 0.98f;
            }
            npc.ai[2]++;
            int timer = 13; // make it 10 on master 
            if((int)npc.ai[2] % timer == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int attackDamage_ForProjectiles7 = npc.GetAttackDamage_ForProjectiles(30f, 27f);
                Vector2 shootFrom = npc.Center + TRAEMethods.PolarVector(6, npc.rotation + MathF.PI/2);
                Vector2 vel = TRAEMethods.PolarVector(12f + npc.velocity.Y, npc.rotation + MathF.PI/2) + TRAEMethods.PolarVector(Main.rand.Next(-4, 5), npc.rotation); 
                int num487 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootFrom, vel, ModContent.ProjectileType<BouncingFlames>(), attackDamage_ForProjectiles7, 0f, Main.myPlayer);
            }
            if(npc.ai[2] >= 280)
            {
                npc.ai[2] = 0;
                npc.ai[1] = 0;
            }

            
        }
        public static void Charge(NPC npc)
        {
            if (npc.ai[1] == 1f)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, npc.Center);
                //SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                npc.TargetClosest();
                npc.rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathF.PI/2;
                float num489 = 24f;
                //if (Main.masterMode)
                //{
                //    num489 += 5f;
                //}
                Vector2 vector48 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num490 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector48.X;
                float num491 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector48.Y;
                float num492 = MathF.Sqrt(num490 * num490 + num491 * num491);
                num492 = num489 / num492;
                npc.velocity.X = num490 * num492;
                npc.velocity.Y = num491 * num492;
                npc.ai[1] = 2f;
            }
            else
            {
                if (npc.ai[1] != 2f)
                {
                    return;
                }
                npc.ai[2] += 1f;
                if (Main.masterMode)
                {
                    npc.ai[2] += 0.5f; // make it 0.5 on master
                }
                if (npc.ai[2] >= 30f)
                {
                    npc.velocity.X *= 0.93f;
                    npc.velocity.Y *= 0.93f;
                    if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                    {
                        npc.velocity.X = 0f;
                    }
                    if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
                    {
                        npc.velocity.Y = 0f;
                    }
                }
                else
                {
                    npc.rotation = MathF.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
                }
                if (npc.ai[2] >= 60f)
                {
                    npc.ai[3] += 1f;
                    npc.ai[2] = 0f;
                    npc.target = 255;
                    npc.rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathF.PI/2;
                    if (npc.ai[3] >= 4f)
                    {
                        npc.ai[1] = 3f;
                        npc.ai[3] = 0f;
                    }
                    else
                    {
                        npc.ai[1] = 1f;
                    }
                }
            }
        }
        public static void Phase3Draw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            
            Texture2D texture = TextureAssets.Npc[npc.type].Value;
            Color color = drawColor;
            Vector2 halfSize = new Vector2(55f, 107f);
            float num35 = 0f;
            float num36 = Main.NPCAddHeight(npc);
            Texture2D effectTexture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/GreenEffect").Value;
            Vector2 Pos = new Vector2(
                npc.position.X - screenPos.X + (float)(npc.width / 2) - (float)TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale, 
                npc.position.Y - screenPos.Y + (float)npc.height - (float)TextureAssets.Npc[npc.type].Height() * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
            
            if(npc.ai[0] == 4f)
            {
                float prog = npc.ai[1] / 100f;
                for(int i =0; i <4; i++)
                {
                    float rot = ((float)i / 4f) * 2f * MathF.PI;
                    rot += prog * MathF.PI / 2f;
                    float radius = (1f-prog) * 200;
                    float c = (1f * prog) * 0.3f;
                    Color color2 = new Color(c, c, c, c);
                    Vector2 effectPos = Pos + TRAEMethods.PolarVector(radius, rot);
                    spriteBatch.Draw(effectTexture, effectPos, npc.frame, color2, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
                
            }
            if(npc.ai[0] == 5f)
            {
                float prog = npc.ai[2] / 0.5f;
                for(int i =0; i <16; i++)
                {
                    float rot = ((float)i / 16f) * 2f * MathF.PI;
                    float radius = (1f-prog) * 8000;
                    float c = (1f * prog) * 0.3f;
                    Color color2 = new Color(c, c, c, c);
                    Vector2 effectPos = Pos + TRAEMethods.PolarVector(radius, rot);
                    spriteBatch.Draw(effectTexture, effectPos, npc.frame, color2, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
            }
            if((npc.ai[0] > 5f))
            {
                for(int i =0; i < npc.oldPos.Length; i++)
                {
                    float c = 255f * ((float)i / npc.oldPos.Length);
                    Color color2 = new Color(c, c, c, c)* 0.3f;
                    Vector2 effectPos = (Pos - npc.position) + npc.oldPos[i];
                    /* new Vector2(
                    npc.oldPos[i].X - screenPos.X + (float)(npc.width / 2) - (float)TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale, 
                    npc.oldPos[i].Y - screenPos.Y + (float)npc.height - (float)TextureAssets.Npc[npc.type].Height() * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35); */
                    spriteBatch.Draw(effectTexture, effectPos, npc.frame, color2, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
            }
            
            
            spriteBatch.Draw(texture, Pos, npc.frame, color, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
        }
        public static void Start(NPC npc)
        {
            if (npc.ai[0] == 4f)
            {
                
                npc.ai[2] += 0.005f;
                if (npc.ai[2] > 0.5f)
                {
                    npc.ai[2] = 0.5f;
                }
            }
            else
            {
                npc.ai[2] -= 0.005f;
                if (npc.ai[2] < 0f)
                {
                    npc.ai[2] = 0f;
                }
            }
            //npc.rotation += npc.ai[2];
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 100f)
            {
                npc.ai[0] += 1f;
                npc.ai[1] = 0f;
                if (npc.ai[0] == 6f)
                {
                    npc.ai[2] = 0f;
                }
                else
                {
                    //SoundEngine.PlaySound(3, (int)npc.position.X, (int)npc.position.Y);
                    for (int num477 = 0; num477 < 20; num477++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
                    }
                    //SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);

                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, npc.Center);
                }
            }
            Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
            npc.velocity.X *= 0.98f;
            npc.velocity.Y *= 0.98f;
            if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
            {
                npc.velocity.Y = 0f;
            }
        }
    }
    public class BouncingFlames : ModProjectile
    {
        public override void SetDefaults()
        {
            
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 8;
            Projectile.hostile = true;
            Projectile.light = 0.8f;
            Projectile.alpha = 100;
            AIType = 95;
            Projectile.timeLeft = 240;
        }
    }
}