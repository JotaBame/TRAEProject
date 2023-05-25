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

namespace TRAEProject.Changes.NPCs.Boss 
{
    public static class RetPhase3
    {
        static int tpTime = 60;
        static int tpCount = 3;
        static int shotTime = 10;
        static int rapidShotTime = 3;
        static int shotCount = 4;
        static int waitTime = 80;
        static int periodTime = (tpCount * tpTime + shotTime * shotCount + waitTime);
        public static void Update(NPC npc)
        {
            npc.defense = npc.defDefense + 10;
			npc.HitSound = SoundID.NPCHit4;
            npc.velocity = Vector2.Zero;
            npc.ai[2]++;
            if(npc.ai[1] != -1 && npc.ai[3] != -1)
            {
                Teleport(npc);
            }
            int periodicTimer = (int)npc.ai[2] % periodTime;
            int periodCount = (int)npc.ai[2] / periodTime;
            if(periodicTimer < tpCount * tpTime)
            {
                if(periodicTimer % tpTime < 8)
                {
                    npc.scale = ((periodicTimer % tpTime) / 8f);
                }
                else if(periodicTimer % tpTime > 32 && periodicTimer < (tpCount - 1) * tpTime)
                {
                    npc.scale = (tpTime - (periodicTimer % tpTime)) / 8f;
                }
                else
                {
                    npc.scale = 1f;
                }
                if(npc.scale <= 0)
                {
                    npc.scale = 0.01f;
                }
                if(periodicTimer % tpTime == 0)
                {
                    SetupTeleport(npc);
                }
            }
            if(periodicTimer > tpCount * tpTime + shotTime * shotCount)
            {
                if(periodicTimer == periodTime - 1 && periodCount % 3 == 1 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SpawnNukes(npc);
                    
                }
                /*
                if(npc.ai[2] > tpCount * tpTime + shotTime * shotCount + waitTime)
                {
                    Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), npc.Center + Vector2.UnitY * -1000, Vector2.Zero, ModContent.ProjectileType<EyeNuke>(), npc.GetAttackDamage_ForProjectiles(35f, 30f), 0);
                }
                */
            }
            else if(periodicTimer > tpCount * tpTime)
            {
                if(periodCount % 3 == 2)
                {
                    if(periodicTimer % rapidShotTime == 0)
                    {
                        Shoot(npc);
                    }
                }
                else
                {
                    if(periodicTimer % shotTime == 0)
                    {
                        Shoot(npc);
                    }
                }
            }
            
        }
        public static float RotateModifer(NPC npc)
        {
            if(npc.ai[0] > 5)
            {
                int periodicTimer = (int)npc.ai[2] % periodTime;
                int periodCount = (int)npc.ai[2] / periodTime;
                if(periodicTimer >= (tpCount - 1) * tpTime && periodCount % 3 == 2 &&  periodicTimer < tpCount * tpTime + shotTime * shotCount)
                {
                    int timer = (tpCount * tpTime + shotTime * shotCount) - periodicTimer;
                    if(timer > (shotTime * shotCount))
                    {
                        timer = (shotTime * shotCount);
                    }
                    float ratio = (float)timer / (shotTime * shotCount);
                    float spread = MathF.PI;
                    float angle = spread * ratio - spread / 2f;
                    return angle;
                }
            }
            return 0f;
        }
        public static void SpawnNukes(NPC npc)
        {
            for(int i =0; i < Main.player.Length; i++)
            {
                if(Main.player[i].active && !Main.player[i].dead)
                {
                    Projectile p = Main.projectile[Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), Main.player[i].Center + Vector2.UnitY * -1000, Vector2.Zero, ModContent.ProjectileType<EyeNuke>(), npc.GetAttackDamage_ForProjectiles(35f, 30f), 0, 255, i)];
                    //p.ai[0] = i;
                }
            }
        }
        static void Teleport(NPC npc)
        {

            TeleportDust(npc.Center);
            npc.Center = new Vector2(npc.ai[1], npc.ai[3]);
            TeleportDust(npc.Center);
            SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 });
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            npc.rotation = (player.Center - npc.Center).ToRotation() - MathF.PI /2;

            npc.ai[1] = npc.ai[3] = -1;
        }
        static void SetupTeleport(NPC npc)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                float r = Main.rand.NextFloat() * MathF.PI * 2;
                Vector2 offset = TRAEMethods.PolarVector(400, r);
                offset.X *= 1.4f;
                Vector2 teleToHere = player.Center + offset;
                npc.ai[1] = teleToHere.X;
                npc.ai[3] = teleToHere.Y;
                npc.netUpdate = true;
            }
        }
        static void TeleportDust(Vector2 center, bool pullIn = false)
        {
            for (int i = 0; i < 100; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                float radius = 160;
                Dust dust = Dust.NewDustPerfect(center + (pullIn ? TRAEMethods.PolarVector(radius , theta): Vector2.Zero), DustID.SilverFlame, TRAEMethods.PolarVector( (pullIn ? -1 : 1) * radius / 10f, theta));
                dust.color = Color.Red;
                dust.noGravity = true;
            }
        }
        static void Shoot(NPC npc)
        {
            if (Main.netMode != 1)
            {
				float shootSpeed = 10f;
                int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(35f, 30f);
                int num413 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), npc.Center + TRAEMethods.PolarVector(25 * 9, npc.rotation + MathF.PI /2), TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI/2), ProjectileID.DeathLaser, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
            }
        }
        public static void Start(NPC npc)
        {
            if (npc.ai[0] == 4f)
            {
                npc.ai[2] += 0.005f;
                if ((double)npc.ai[2] > 0.5)
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
                    npc.ai[1] = -1;
                    npc.ai[3] = -1;
                    npc.netUpdate = true;
                }
                else
                {
                    for (int num477 = 0; num477 < 20; num477++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
                    }

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
        public static void Phase3Draw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            
            Texture2D texture = TextureAssets.Npc[npc.type].Value;
            Color color = drawColor;
            Vector2 halfSize = new Vector2(55f, 107f);
            float num35 = 0f;
            float num36 = Main.NPCAddHeight(npc);
            Texture2D effectTexture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/RedEffect").Value;
            Vector2 Pos = new Vector2(
                npc.position.X - screenPos.X + (float)(npc.width / 2) - (float)TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale, 
                npc.position.Y - screenPos.Y + (float)npc.height - (float)TextureAssets.Npc[npc.type].Height() * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
            
            if(npc.ai[0] == 4f)
            {
                float prog = npc.ai[2] / 0.5f;
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
            if(((byte)npc.ai[0] > 5f))
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
        
    }
    public class EyeNuke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eye Nuke");
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 60;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 10 * 60;
            DrawOffsetX = 150;
            DrawOriginOffsetX = 150;

        }
        void Explode(Projectile projectile)
        {
            projectile.width = projectile.height = 500;
            projectile.position += Vector2.One * -1 * (500 - 60) / 2f;
            SoundEngine.PlaySound(SoundID.Item62 with { MaxInstances = 0 }, Projectile.position);
            for (int i = 0; i < 200; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                float radius = 250;
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.SilverFlame, TRAEMethods.PolarVector(radius / 10f, theta), Scale: 1.5f);
                dust.color = Color.Red;
                dust.noGravity = true;
                dust = Dust.NewDustPerfect(projectile.Center, DustID.SilverFlame, TRAEMethods.PolarVector(radius / 8f, theta), Scale: 1.5f);
                dust.color = Color.Red;
                dust.noGravity = true;
            }
        }
        public override void AI()
        {
            if(Projectile.timeLeft == 1)
            {
                Explode(Projectile);
            }
            if(Projectile.timeLeft % 12 == 0)
            {
                Projectile.frame += 1;
            }
            if (Projectile.frame >= 3)
            { 
                Projectile.frame = 0;
            }
            if(Projectile.timeLeft < 60)
            {
            }
            else
            {
                if(Projectile.ai[0] < 0 || Projectile.ai[0] > 255)
                {
                    Projectile.timeLeft = 60;
                    return;
                }
                Player player = Main.player[(int)Projectile.ai[0]];
                if(!player.active|| player.dead)
                {
                    Projectile.timeLeft = 60;
                    return;
                }
                float flytowards = (player.Center - Projectile.Center).ToRotation();
                float speedBonus = (player.Center - Projectile.Center).Length() / 80f;
                Projectile.rotation.SlowRotation( flytowards- MathF.PI/2, MathF.PI/60f);
                Projectile.velocity = TRAEMethods.PolarVector(4f + speedBonus, Projectile.rotation + MathF.PI/2f);
            }
        }

        public static Entity FindTarget(Projectile projectile)
        {
            Entity target = null;
            float maxRange = 10000;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro < maxRange)
                {
                    target = Main.player[i];
                    maxRange = (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro;
                }
            }
            return target;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.timeLeft >1)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Texture2D glowTexture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/EyeNuke_Glow").Value;
                Vector2 offset = Vector2.Zero;
                if(Projectile.timeLeft < 60)
                {
                    offset = new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
                }
                int frameCount = Main.projFrames[Projectile.type];
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + offset,
                            new Rectangle(0, Projectile.frame * (texture.Height / frameCount), texture.Width, (texture.Height / frameCount)), lightColor, Projectile.rotation,
                            new Vector2(texture.Width * 0.5f, (texture.Height / frameCount) * 0.5f), 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition + offset,
                            new Rectangle(0, Projectile.frame * (texture.Height / frameCount), texture.Width, (texture.Height / frameCount)), Color.White, Projectile.rotation,
                            new Vector2(texture.Width * 0.5f, (texture.Height / frameCount) * 0.5f), 1f, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}