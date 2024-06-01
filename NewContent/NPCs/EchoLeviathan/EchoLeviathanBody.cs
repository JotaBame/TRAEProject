using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.EchoLeviathan
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
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {

        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {

        }
        public override void AI()
        {

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            drawColor *= NPC.Opacity;

            Main.EntitySpriteDraw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, texture.Size()/ 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
            return false;
        }
        protected virtual int SegmentWidth => 80;
        protected virtual string GlowPathTexture => "TRAEProject/NewContent/NPCs/EchoLeviathan/EchoLeviathanBody1Glow";
    }
    public class EchoLeviathanBody2 : EchoLeviathanBody1
    {
        protected override string GlowPathTexture => "TRAEProject/NewContent/NPCs/EchoLeviathan/EchoLeviathanBody2Glow";

    }
    public class EchoLeviathanBody3 : EchoLeviathanBody1
    {
        protected override string GlowPathTexture => "TRAEProject/NewContent/NPCs/EchoLeviathan/EchoLeviathanBody3Glow";
    }
}
