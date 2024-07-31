using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan
{
    public class EchoLeviathanBody1 : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lifeMax = 18000;
            NPC.defense = 40;
            NPC.damage = 80;
            NPC.width = NPC.height = 70;
            NPC.HitSound = SoundID.DD2_BetsyHurt;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;//initially invincible
            NPC.alpha = 255;//initially invisible
        }
        //todo prevent despawning segments
        //public override  despawn
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void AI()
        {
            int parent = (int)NPC.ai[0];
            if (parent < 0 || parent >= Main.maxNPCs || !Main.npc[parent].active || Main.npc[parent].type != ModContent.NPCType<EchoLeviathanHead>())
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                return;
            }
            NPC.realLife = parent;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            if (NPC.Opacity != 1)
            {
                EchosphereHelper.SpectralDrawVerticalFlip(NPC, spriteBatch, screenPos, texture);
                return false;
            }
            drawColor *= NPC.Opacity;
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
            return false;
        }
        protected virtual int SegmentWidth => 80;
        protected virtual string GlowPathTexture => "TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanBody1Glow";
    }
    public class EchoLeviathanBody2 : EchoLeviathanBody1
    {
        protected override string GlowPathTexture => "TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanBody2Glow";

    }
    public class EchoLeviathanBody3 : EchoLeviathanBody1
    {
        protected override string GlowPathTexture => "TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanBody3Glow";
    }
}
