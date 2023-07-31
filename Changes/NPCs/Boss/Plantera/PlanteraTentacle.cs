using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Plantera
{
    class PlanteraTentacle : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Vector2? anchor = null;
        Entity anchoredTo = null;
        Vector2 anchorOffset = Vector2.Zero;
        int timer = 0;
        public override void SetDefaults(NPC npc)
        {
            if (GetInstance<TRAEConfig>().PlanteraRework)
            {
                if (npc.type == NPCID.PlanterasTentacle)
                {
                    npc.lifeMax = 1;
                    npc.behindTiles = true;
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)/* tModPorter Note:bossAdjustment -> balance (bossAdjustment is different, see the docs for details) */
        {
            if (GetInstance<TRAEConfig>().PlanteraRework)
            {
                if (npc.type == NPCID.PlanterasTentacle)
                {
                    npc.lifeMax = 1;
                    
                }
            }
        }
        float reachTime = 120;
        public override bool PreAI(NPC npc)
        {
            if (GetInstance<TRAEConfig>().PlanteraRework)
            {
                if (npc.type == NPCID.PlanterasTentacle)
                {
                    if (anchor == null)
                    {
                        anchor = npc.Center;
                        if (npc.localAI[0] == -1 && npc.localAI[1] != -1)
                        {
                            anchoredTo = Main.projectile[(int)npc.localAI[1]];
                            anchorOffset = npc.Center - anchoredTo.Center;
                        }
                        if (npc.localAI[0] != -1 && npc.localAI[1] == -1)
                        {
                            anchoredTo = Main.npc[(int)npc.localAI[0]];
                            anchorOffset = npc.Center - anchoredTo.Center;
                        }
                        npc.TargetClosest();
                    }
                    if (anchoredTo != null && anchoredTo.active)
                    {
                        anchor = anchoredTo.Center + anchorOffset;
                    }
                    timer++;
                    Player player = Main.player[npc.target];
                    if (!player.active || player.statLife <= 0 || anchor == null)
                    {
                        npc.life = 0;
                        npc.active = false;
                    }
                    else
                    {
                        if (timer > reachTime * 2)
                        {
                            npc.life = 0;
                            npc.active = false;
                        }
                        float outAmount = timer;
                        if (outAmount > reachTime)
                        {
                            outAmount = reachTime * 2 - outAmount;
                        }
                        npc.Center = ((player.Center - (Vector2)anchor) * ((float)outAmount / reachTime)) + (Vector2)anchor;

                        npc.rotation = (player.Center - npc.Center).ToRotation() + MathF.PI;
                    }
                    return false;
                }
            }
            return base.PreAI(npc);
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (GetInstance<TRAEConfig>().PlanteraRework)
            {
                if (npc.type == NPCID.PlanterasTentacle)
                {
                    if (anchor != null)
                    {
                        Texture2D vine = TextureAssets.Chain27.Value;
                        float dist = (npc.Center - (Vector2)anchor).Length();
                        float rot = ((Vector2)anchor - npc.Center).ToRotation();
                        for (int k = 0; k < dist; k += vine.Height)
                        {
                            Vector2 pos = npc.Center + TRAEMethods.PolarVector(k, rot);
                            spriteBatch.Draw(vine, pos - screenPos, null, Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16), rot + MathF.PI / 2f, new Vector2(vine.Width / 2, vine.Height), 1f, SpriteEffects.None, 0);
                        }
                    }
                }
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
}
