using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TRAEProject.Changes.NPCs.SolarEclipse
{

    public class Eyezor : GlobalNPC
    {
        const int eyeStartTime = 4 * 60;
        const int lowHealthTimeBoostMax = 4 * 60;
        const int eyeChargeTime = 40;
        static Vector2 headOffset(NPC npc)
        {
            return new Vector2(8 * npc.spriteDirection, -8);
        }
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.Eyezor)
            {
                npc.ai[2] = 0f; //shuts off vanilla laser
                if(npc.confused)
                {
                    npc.localAI[1] = 0 + (1f - ((float)npc.life / (float)npc.lifeMax)) * (float)lowHealthTimeBoostMax;
                }
                if(npc.velocity.Y == 0)
                {
                    npc.localAI[1]++;
                    int eyeTime = (int)npc.localAI[1];
                    if(eyeTime >= eyeStartTime + eyeChargeTime)
                    {
                        npc.localAI[1] = 0 + (1f - ((float)npc.life / (float)npc.lifeMax)) * (float)lowHealthTimeBoostMax;
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + headOffset(npc), TRAEMethods.PolarVector(15f, npc.localAI[3]), ProjectileID.EyeLaser, 60, 0f, Main.myPlayer);
                        }
                        
                        return false;
                    }
                    if(eyeTime > eyeStartTime)
                    {
                        npc.velocity.X *= 0.99f;
                        npc.TargetClosest(true);
                        npc.localAI[3] = (Main.player[npc.target].Center - npc.Center).ToRotation();
                        return false;
                    }
           
                }
            }
            return base.PreAI(npc);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(npc.type == NPCID.Eyezor)
            {
                int eyeTime = (int)npc.localAI[1];
                if(eyeTime > eyeStartTime)
                {
                    DrawPointedLaser(npc, spriteBatch, npc.localAI[3], Math.Max(0.1f, (float)(eyeTime - eyeStartTime) / (float)eyeChargeTime), 800);
                    //Color color = new Color(98, 49, 253);
                    //Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/laser").Value;
                    //spriteBatch.Draw(texture, npc.Center + headOffset(npc) - screenPos, null, color, npc.localAI[3] - MathF.PI / 2f, new Vector2(1, 0), new Vector2(1, 400), SpriteEffects.None, 0);
                    //Main.NewText("Pew Pew");
                }
            }
        }

        public static void DrawPointedLaser(NPC npc, SpriteBatch spriteBatch, float dir, float opacity, float length)
        {
            Color color = new Color(106, 93, 255);
            Vector2 pos = npc.Center + headOffset(npc) - Main.screenPosition;
            Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/PointedWarning").Value;
            Vector2 texScale = new Vector2(length / 10, 6 * opacity / 9);
            spriteBatch.Draw(texture, pos, new Rectangle(0, 0, 10, 9), color * opacity * 0.75f, dir, new Vector2(0, 4.5f), texScale, SpriteEffects.None, 0);
        }
    }
}