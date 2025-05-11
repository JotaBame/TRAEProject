using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoStalker
{
    public class EchoStalkerBody : ModNPC
    {
        public int HeadIndex { get => (int)NPC.ai[0]; set => NPC.ai[0] = value; }
        public override void SetDefaults()
        {
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Size = new(36);
            NPC.lifeMax = 1250;
            NPC.defense = 10;
            NPC.damage = 50;
            NPC.knockBackResist = 0;
        }
    }
}
