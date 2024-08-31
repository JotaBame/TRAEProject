using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace TRAEProject.Changes.NPCs.SolarEclipse
{

    public class DrFlyMan : GlobalNPC
    {
        const float splashZoneWidth = 30 * 16;
        const float splashZoneHeight = 4 * 16;
        static bool IsEclipseFighter(int type)
        {
            return type == NPCID.Frankenstein || type == NPCID.SwampThing || type == NPCID.CreatureFromTheDeep || type == NPCID.Fritz || type == NPCID.ThePossessed || type == NPCID.Butcher;
        }
        static void Splash(NPC npc, int type)
        {
            int dustType = DustID.SilverFlame;
            switch(type)
            {
                case 1:
                    dustType = DustID.SilverFlame;
                break;
                case 2:
                    dustType = ModContent.DustType<SpeedDustEffect>();
                break;
                case 3:
                    dustType = ModContent.DustType<EnduranceDustEffect>();
                break;
            }
            SoundEngine.PlaySound(SoundID.Item107, npc.Center);
            for(int i = 0;  i < splashZoneWidth; i += 8)
            {
                Dust d = Dust.NewDustPerfect(npc.Bottom + Vector2.UnitX * (i - (splashZoneWidth)/2f), dustType, Vector2.UnitY * (-8f * Main.rand.NextFloat()));
                d.noGravity = true;
                d.scale *= 2;
            }
            for(int i = 0; i < Main.npc.Length; i++)
            {
                if(Main.npc[i].active && (IsEclipseFighter(Main.npc[i].type) || type != 1) && MathF.Abs(Main.npc[i].Center.X - npc.Center.X) < splashZoneWidth/2 && MathF.Abs(npc.Bottom.Y - Main.npc[i].Bottom.Y) < splashZoneHeight && MathF.Abs(npc.Bottom.Y - Main.npc[i].Bottom.Y) >= -6)
                {
                    switch(type)
                    {
                        case 1:
                            Main.npc[i].AddBuff(ModContent.BuffType<TheFlysLevitation>(), 60 * 2);
                        break;
                        case 2:
                            Main.npc[i].AddBuff(ModContent.BuffType<TheFlysSpeed>(), 60 * 5);
                        break;
                        case 3:
                            Main.npc[i].AddBuff(ModContent.BuffType<TheFlysEndurance>(), 60 * 5);
                        break;
                    }
                }
            }
        }
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.DrManFly)
            {
                if(npc.localAI[0] > 0)
                {
                    npc.velocity.X = 0;
                    npc.ai[1]--;
                    if(npc.ai[1] <=0)
                    {
                        Splash(npc, (int)npc.localAI[0]);
                        npc.ai[1] = 0;
                        npc.localAI[0] = 0;
                    }
                    return false;
                }
            }
            return base.PreAI(npc);
        }
        public override void PostAI(NPC npc)
        {
            if(npc.type == NPCID.DrManFly)
            {
                //Main.NewText(npc.localAI[0] + ", " + npc.localAI[1] + ", " + npc.localAI[2] + ", " + npc.localAI[3]);
                //Main.NewText(npc.ai[0] + ", " + npc.ai[1] + ", " + npc.ai[2] + ", " + npc.ai[3]);
                if(npc.ai[1] <= 0)
                {
                    for(int i = 0; i < Main.npc.Length; i++)
                    {
                        if(Main.npc[i].active && IsEclipseFighter(Main.npc[i].type) && MathF.Abs(Main.npc[i].Center.X - npc.Center.X) < splashZoneWidth/2 && MathF.Abs(npc.Bottom.Y - Main.npc[i].Bottom.Y) < splashZoneHeight && MathF.Abs(npc.Bottom.Y - Main.npc[i].Bottom.Y) >= -6)
                        {
                            if(Main.player[npc.target].Center.Y - npc.Center.Y < -200 && !Main.npc[i].HasBuff(ModContent.BuffType<TheFlysLevitation>()))
                            {
                                npc.localAI[0] = 1;
                            }
                            else if(MathF.Abs(Main.player[npc.target].Center.X - npc.Center.X) > 600 && !Main.npc[i].HasBuff(ModContent.BuffType<TheFlysSpeed>()))
                            {
                                npc.localAI[0] = 2;
                            }
                            else if(!Main.npc[i].HasBuff(ModContent.BuffType<TheFlysEndurance>()))
                            {
                                npc.localAI[0] = 3;
                            }
                            if(npc.localAI[0] != 0)
                            {
                                npc.ai[1] = 80;
                                break;
                            }
                        }
                    }
                }
            }
            base.PostAI(npc);
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if(npc.type == NPCID.DrManFly)
            {
                if(npc.localAI[0] > 0)
                {
                    if(npc.ai[1] > 15)
                    {
                        npc.frame.Y = frameHeight * 16;
                    }
                    else if(npc.ai[1] > 10)
                    {
                        npc.frame.Y = frameHeight * 17;
                    }
                    else if(npc.ai[1] > 5)
                    {
                        npc.frame.Y = frameHeight * 18;
                    }
                    else
                    {
                        npc.frame.Y = frameHeight * 19;
                    }
                }
            }
        }
    }
}