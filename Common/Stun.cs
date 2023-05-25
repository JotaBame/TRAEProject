using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Common
{
    public class Stun : GlobalNPC
    {

        public override bool InstancePerEntity => true;
        public float stunResist = 0f;
		public bool stunImmune = false;
		public override void SetDefaults(NPC npc)
        {
			stunResist = 1f - (float)npc.lifeMax / ((float)npc.lifeMax + 2000f);
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
                case NPCID.SolarSroller:
                    stunImmune = true;
                    return;
            }
        }
        private int stunTime = 0;
            private int stunCooldown = 0;

        //Use this to freeze NPCs
        public void StunMe(NPC npc, int time)
        {
            if(npc.aiStyle == 6 || stunImmune || stunCooldown > 0)
            {
                return;
            }
            stunCooldown = time;
            time = (int)(time * stunResist);
            if (time < 6)
            {
                return;
            }
            //this only happens when the npc isn't frozen yet
            if (stunTime == 0)
            {
                npc.velocity = Vector2.Zero;
            }
            stunTime = Math.Max(time, stunTime);
   
        }
        //Called whenever an NPC breaks out of the ice
        public override bool PreAI(NPC npc)
        {
            if (stunCooldown > 0 && stunTime == 0)
                stunCooldown--;
            if (stunTime > 0)
            {
                stunTime--;
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
            if (stunTime > 0)
            {
                float widthForScale = npc.width;
                if (widthForScale < 30)
                {
                    widthForScale = 30;
                }
                if (widthForScale > 300)
                {
                    widthForScale = 300;
                }
                float scale = widthForScale / 50f;
                float stunnedHorizontalMovement = (npc.width / 2) * 1.5f;
                float heightofStunned = (npc.height / 2) * 1.2f;
                stunTime += (int)Math.PI / 60;
                Texture2D texture = Request<Texture2D>("TRAEProject/Common/Stun").Value;
                //Main.NewText(MathF.Sin(stunTime));
                if (MathF.Cos(stunTime) > 0)
                {
                    Vector2 CenterOfStunned = new Vector2(npc.Center.X + MathF.Sin(stunTime) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);

                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunTime,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);

                    CenterOfStunned = new Vector2(npc.Center.X - MathF.Sin(stunTime) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunTime,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);
                }
                else
                {
                    Vector2 CenterOfStunned = new Vector2(npc.Center.X - MathF.Sin(stunTime) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);

                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunTime,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);

                    CenterOfStunned = new Vector2(npc.Center.X + MathF.Sin(stunTime) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunTime,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
