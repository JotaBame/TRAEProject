using System;
using Microsoft.CodeAnalysis;
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
            Projectile.timeLeft = 2700;
            Projectile.aiStyle = -1;
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
                Projectile.width = 200;
                Projectile.height = 200;
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
                    SoundEngine.PlaySound(SoundID.Coins  with { MaxInstances = 0, Volume = 1.5f, Pitch = 0.5f}, toBoom);
                }
                float speed = 9f;
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
                                boomHere = Main.player[i].Center + TRAEMethods.PolarVector(Main.rand.NextFloat(200f, 500f), Main.rand.NextFloat(0f, MathF.PI * 2f));
                            }
                        }
                    }
                    if(boomHere == null)
                    {
                        Explode(Projectile.Center);
                    }
                }
            }
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
                    Texture2D ret = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Prime/TargetRetical").Value;
                    float trig = 1f + 0.2f * MathF.Sin(MathF.PI * Projectile.timeLeft / 60f);
                    Main.EntitySpriteDraw(ret, renderHere - Main.screenPosition, null, Color.White, 0, ret.Size() * 0.5f, trig, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}