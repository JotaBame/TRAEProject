using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    
    public class PrimeMissile : ModProjectile
    {
        public static int StartTimeLeft => 2700;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 5000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.hostile = true;
            Projectile.penetrate = 3;
            Projectile.light = 0.75f;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = StartTimeLeft;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
        }
        public override bool CanHitPlayer(Player target)
        {
            if(Projectile.timeLeft > 2)
            {
                return false;
            }
            return true;
        }
        void Explode(Vector2 here)
        {
            if(Projectile.timeLeft > 2)
            {
                Projectile.timeLeft = 2;
                Projectile.width = 180; // make it 200 and scale up the reticle appropiately for master
                Projectile.height = 180;
                Projectile.position = here - Projectile.Size * 0.5f;
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item62, here);
                for (int i = 0; i < 100; i++)
                {
                    float rot = MathF.PI * 2f * ((float)i / 100f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TheDestroyer, TRAEMethods.PolarVector(15f, rot));
                    d.noGravity = true;
                    d.scale = 1f;
                }
                for(int i = 0; i < 60; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TheDestroyer, TRAEMethods.PolarVector(Main.rand.NextFloat(1f, 15f), Main.rand.NextFloat(0f, MathF.PI * 2f)));
                    d.noGravity = true;
                }
                for(int i = 0; i < 20; i++)
                {
                    int num914 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, TRAEMethods.PolarVector(Main.rand.NextFloat(1f, 6f), Main.rand.NextFloat(0f, MathF.PI * 2f)), Main.rand.Next(61, 64));
                }
            }
        }
        Vector2? boomHere = null;
        bool triggeredSound = false;
        float rotSpeed = MathF.PI / 120f;
       
        public override void AI()
        {
            if(boomHere != null)
            {
                Vector2 toBoom = (Vector2)boomHere;
                if(!triggeredSound)
                {
                    triggeredSound = true;
                    SoundStyle style = PrimeStats.ReticleAppear1;//repeated
                    if (Projectile.ai[0] == 1)//rocket fired while in rapid fire state
                    {
                        style = PrimeStats.ReticleAppear2;//single beep
                    }

                    //style = PrimeStats.GetRandomReticleAppearSound();
                    SoundEngine.PlaySound(style, toBoom);
                }
                float speed = 7f;
                Projectile.velocity = TRAEMethods.PolarVector(speed, Projectile.rotation);
                //Projectile.velocity += TRAEMethods.PolarVector(2f, (toBoom - Projectile.Center).ToRotation());
                //Projectile.velocity *= 0.95f;
                Projectile.rotation.SlowRotation((toBoom - Projectile.Center).ToRotation(), rotSpeed);
                rotSpeed += MathF.PI / 480f;
                if((Projectile.Center - toBoom).Length() < 20f)
                {
                    Explode(toBoom);
                    
                }
                else
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center + TRAEMethods.PolarVector(-18, Projectile.rotation), DustID.TheDestroyer, Vector2.Zero);
                    d.noGravity = true;
                    d.scale = 0.5f;
                }
            }
            else
            {
                Projectile.velocity *= 0.94f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                if(Projectile.velocity.Length() < 1f)
                {
                    float closest = 10000;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < Main.maxPlayers; i++)
                        {
                            if (Main.player[i].active && !Main.player[i].dead && (Projectile.Center - Main.player[i].Center).Length() < closest)
                            {
                                closest = (Projectile.Center - Main.player[i].Center).Length();
                                boomHere = Main.player[i].Center + TRAEMethods.PolarVector(Main.rand.NextFloat(250f, 500f), Main.rand.NextFloat(0f, MathF.PI * 2f));
                            }
                        }
                        Projectile.netUpdate = true;
                    }
                    if(boomHere == null)
                    {
                        Explode(Projectile.Center);
                    }
                }
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            bool sendBoom = (boomHere != null);
            writer.Write(sendBoom);
            if(sendBoom)
            {
                writer.WriteVector2((Vector2)boomHere);
            }
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bool boom = reader.ReadBoolean();
            if(boom)
            {
                boomHere = reader.ReadVector2();
            }
            else
            {
                boomHere = null;
            }
            base.ReceiveExtraAI(reader);
        }
        public static void DrawReticle(Vector2 drawPos, float time, Projectile proj)
        {
            Vector2 projScreenPos = proj.Center - Main.screenPosition;
            float closenessMultOverride = Utils.Remap(projScreenPos.Distance(drawPos), 0f, 80, -50f, 1f);
            Texture2D tex = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Prime/ReticleSheetPremultiplied").Value;
            ulong seed = (ulong)proj.identity;
            float rotationDirection = Utils.RandomInt(ref seed, 2) * 2 % 2 - 1;
            float closenessMultOverrideAmount = EaseInOutCubic(Utils.GetLerpValue(1f, 0f, closenessMultOverride, true));
            float offsetMultLines = MathHelper.Lerp(Utils.Remap(MathF.Sin(time / 20f), -1, 1, 0, 20), closenessMultOverride, closenessMultOverrideAmount);
            float offsetMultCircleFrac = MathHelper.Lerp(Utils.Remap(MathF.Sin(time / 20f), -1, 1, 0, 16), closenessMultOverride, closenessMultOverrideAmount);
            float rotationOffset = EaseOut(Utils.GetLerpValue(0f, 10f, time, true)) * MathF.PI * 0.5f * rotationDirection;
            Color additiveWhite = Color.White with { A = 0 };
            for (int i = 0; i < 4; i++)//lines
            {
                Rectangle frame = tex.Frame(2, 4, 1, i);
                Vector2 offset = Vector2.UnitX.RotatedBy((-i / 4f) * MathF.Tau + MathF.PI * 0.5) * offsetMultLines;
                Main.EntitySpriteDraw(tex, drawPos + offset, frame, additiveWhite, rotationOffset, frame.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < 4; i++)//not lines
            {
                Rectangle frame = tex.Frame(2, 4, 0, i);
                Vector2 offset = -Vector2.UnitX.RotatedBy((-i / 4f) * MathF.Tau - MathF.PI * 0.25f) * offsetMultCircleFrac;
                Main.EntitySpriteDraw(tex, drawPos + offset, frame, additiveWhite, rotationOffset, frame.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        static float EaseInOutCubic(float progress)
        {
            if (progress < 0.5f)
            {
                return 4f * progress * progress * progress;
            }
            else
            {
                float t = 2f * progress - 2f;
                return 0.5f * t * t * t + 1f;
            }
        }
        static float EaseOut(float progress)
        {
            progress = 1 - progress;
            progress *= progress * progress;
            return 1 - progress;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.timeLeft > 2)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                            new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                            new Vector2(18, 10), 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Prime/PrimeMissile_Glow").Value, Projectile.Center - Main.screenPosition,
                            new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                            new Vector2(18, 10), 1f, SpriteEffects.None, 0);
                if(boomHere != null)
                {
                    Vector2 renderHere = (Vector2)boomHere;
                    //Texture2D ret = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Prime/TargetRetical").Value;
                    //float trig = 1f + 0.25f * MathF.Sin(MathF.PI * Projectile.timeLeft / 60f);
                    //Main.EntitySpriteDraw(ret, renderHere - Main.screenPosition, null, Color.White, 0, ret.Size() * 0.5f, trig, SpriteEffects.None, 0);
                    DrawReticle(renderHere - Main.screenPosition, StartTimeLeft - Projectile.timeLeft, Projectile);
                }
            }
            return false;
        }
    }
}