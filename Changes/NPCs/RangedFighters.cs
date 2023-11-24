//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.ModLoader;
//using TRAEProject.Common;
//using TRAEProject.NewContent.Items.DreadItems.RedPearl;
//using TRAEProject.NewContent.NPCs.Underworld.Beholder;

//namespace TRAEProject.Changes.NPCs
//{
//    public class RangedFighters : GlobalNPC
//    {
//        public override void AI(NPC npc)
//        {
//            /*
//            if(npc.type == NPCID.GoblinArcher || npc.type == NPCID.SkeletonArcher || npc.type == NPCID.PirateCrossbower || npc.type == NPCID.ElfArcher || npc.type == NPCID.CultistArcherBlue)
//            {
//                Main.NewText(npc.ai[0] + ", " + npc.ai[1] + ", " + npc.ai[2] + ", " + npc.ai[3]);
//            }
//            if(npc.type == NPCID.PirateDeadeye || npc.type == NPCID.SkeletonSniper || npc.type == NPCID.TacticalSkeleton)
//            {
//                Main.NewText(npc.ai[0] + ", " + npc.ai[1] + ", " + npc.ai[2] + ", " + npc.ai[3]);
//            }
//            */
            
//        }
//        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
//        {
//            if(npc.type == NPCID.GoblinArcher || npc.type == NPCID.SkeletonArcher || npc.type == NPCID.PirateCrossbower || npc.type == NPCID.ElfArcher || npc.type == NPCID.CultistArcherBlue || npc.type == NPCID.PirateDeadeye || npc.type == NPCID.SkeletonSniper || npc.type == NPCID.TacticalSkeleton)
//            {
//                int shootCycleTime = 70;
//                if (npc.type == 379 || npc.type == 380)
//                    shootCycleTime = 80;

//                if (npc.type == 381 || npc.type == 382)
//                    shootCycleTime = 80;

//                if (npc.type == 520)
//                    shootCycleTime = 15;

//                if (npc.type == 350)
//                    shootCycleTime = 110;

//                if (npc.type == 291)
//                    shootCycleTime = 200;

//                if (npc.type == 292)
//                    shootCycleTime = 120;

//                if (npc.type == 293)
//                    shootCycleTime = 90;

//                if (npc.type == 111)
//                    shootCycleTime = 180;

//                if (npc.type == 206)
//                    shootCycleTime = 50;

//                if (npc.type == 481)
//                    shootCycleTime = 100;

//                if (npc.type == 214)
//                    shootCycleTime = 40;

//                if (npc.type == 215)
//                    shootCycleTime = 80;

//                if (npc.type == 290)
//                    shootCycleTime = 30;

//                if (npc.type == 411)
//                    shootCycleTime = 300;

//                if (npc.type == 409)
//                    shootCycleTime = 60;

//                if (npc.type == 424)
//                    shootCycleTime = 180;

//                if (npc.type == 426)
//                    shootCycleTime = 60;

//                bool flag17 = false;
//                if (npc.type == 216) 
//                {
//                    if (npc.localAI[2] >= 20f)
//                        flag17 = true;

//                    shootCycleTime = ((!flag17) ? 8 : 60);
//                }

//                int shootOn = shootCycleTime / 2;
//                if (npc.type == 424)
//                    shootOn = shootCycleTime - 1;

//                if (npc.type == 426)
//                    shootOn = shootCycleTime - 1;
                
//                if(npc.ai[1] > shootOn && npc.ai[1] < shootOn + 30)
//                {
//                    Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/RhuthiniumGuardian/laser").Value;
//                    spriteBatch.Draw(texture, npc.Center - screenPos, null, Color.Red, (Main.player[npc.target].Center - npc.Center).ToRotation() - MathF.PI / 2f, new Vector2(1, 0), new Vector2(1, 400), SpriteEffects.None, 0);
//                    //Main.NewText("Pew Pew");
//                }
//            }
//            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
//        }
//    }
//}