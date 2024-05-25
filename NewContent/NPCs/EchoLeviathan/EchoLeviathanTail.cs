using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.EchoLeviathan
{
    internal class EchoLeviathanTail : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lifeMax = 18000;
            NPC.defense = 34;
            NPC.damage = 70;
            NPC.width = NPC.height = 70;
            NPC.HitSound = SoundID.DD2_BetsyHurt;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;//initially invincible
            NPC.alpha = 255;//initially invisible
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            drawColor *= NPC.Opacity;
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
            return false;
        }
    }
}
