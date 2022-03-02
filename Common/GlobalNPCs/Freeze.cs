using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Common.GlobalNPCs
{
    public class Freeze : GlobalNPC
    {

        public override bool InstancePerEntity => true;
        public float freezeResist = 0f;
		 public bool freezeImmune = false;
		public override void SetDefaults(NPC npc)
        {
			freezeResist = 1f - (float)npc.lifeMax / ((float)npc.lifeMax + 2000f);
            switch (npc.type)
            {
                case NPCID.QueenBee:
                case NPCID.QueenSlimeBoss:
                case NPCID.BrainofCthulhu:
                case NPCID.MoonLordHand:
                case NPCID.MoonLordCore:
                case NPCID.MoonLordHead:
                case NPCID.GolemFistLeft:
                case NPCID.GolemFistRight:
                case NPCID.GolemHead:
                case NPCID.SkeletronHand:
                case NPCID.DukeFishron:
                case NPCID.WallofFlesh:
                case NPCID.WallofFleshEye:
                case NPCID.BigMimicCorruption:
                case NPCID.BigMimicCrimson:
                case NPCID.BigMimicHallow:            
case NPCID.ScutlixRider:
                case NPCID.SolarDrakomireRider:
                case NPCID.DD2Betsy:
                case NPCID.MartianSaucerCannon:
                case NPCID.MartianSaucerTurret:
                case NPCID.MartianSaucerCore:
                case NPCID.MartianSaucer:
                    freezeImmune = true;
                    return;

            }
		}
        private int freezeTime = 0; 
        private int freezeCooldown = 0;
        //Use this to freeze NPCs
        public void FreezeMe(NPC npc, int time)
        {

            //NPCs immune to frostburn are immune to getting frozen
            if (freezeCooldown > 0 || npc.buffImmune[BuffID.Frostburn] || npc.buffImmune[BuffID.Frostburn2] || npc.aiStyle == 6 || freezeImmune)
            {
                return;
            }
            freezeCooldown = time;

            time = (int)(time * freezeResist); 
            if (time < 6)
            {
                return;
            }
            //this only happens when the npc isn't frozen yet
            if (freezeTime == 0)
            {
                npc.velocity = Vector2.Zero;
            }
            freezeTime = Math.Max(time, freezeTime);
      

        }
        //Called whenever an NPC breaks out of the ice
        void Defrost(NPC npc)
        {
            for(int i =0; i <20; i++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ice);
            }
            SoundEngine.PlaySound(SoundID.Item27, npc.Center);
        }
        public override bool PreAI(NPC npc)
        {
            if (freezeCooldown > 0 && freezeTime == 0)
                freezeCooldown--;
            if(freezeTime > 0)
            {
                freezeTime--;
                if(freezeTime == 0)
                {
                    Defrost(npc);
                }
                if(npc.noGravity)
                {
                    npc.velocity = Vector2.Zero;
                }
                else
                {
                    npc.noTileCollide = false;
                    npc.velocity.X = 0;
                }
                return false;
            }
            return true;
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (freezeTime > 0)
            {
                float extraSize = 30;
                Texture2D texture = Request<Texture2D>("TRAEProject/Common/GlobalNPCs/IceBlock").Value;
                Vector2 stretch = new Vector2((float)(npc.width+extraSize) / (float)texture.Width, (float)(npc.height + extraSize) / (float)texture.Height);
                Vector2 offset = Vector2.Zero;
                if (freezeTime < 30)
                {
                    offset = new Vector2(-4 + Main.rand.Next(9), -4 + Main.rand.Next(9));
                }
                drawColor.A = 100;
                spriteBatch.Draw(texture, npc.position - screenPos + offset - (Vector2.One * extraSize * 0.5f), null, drawColor, 0, Vector2.Zero, stretch, SpriteEffects.None, 0);
                base.PostDraw(npc, spriteBatch, screenPos, drawColor);
            }
        }
    }
}
