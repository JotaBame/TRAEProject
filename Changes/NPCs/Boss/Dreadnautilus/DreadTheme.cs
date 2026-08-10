using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Dreadnautilus
{
    public class DreadTheme : ModSceneEffect
    {
        static int dreadSlot;
        public override int Music => dreadSlot;
        public override SceneEffectPriority Priority => (SceneEffectPriority)int.MaxValue;
        public override bool IsSceneEffectActive(Player player)
        {
            if (dreadSlot == 0)
            {
                Main.NewText(dreadSlot);
                dreadSlot = MusicLoader.GetMusicSlot("TRAEProject/Changes/NPCs/Boss/Dreadnautilus/DreadTheme");

            }
            return NPC.AnyNPCs(NPCID.BloodNautilus);
        }
    }
}