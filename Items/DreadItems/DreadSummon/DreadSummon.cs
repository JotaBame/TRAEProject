using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.DreadItems.DreadSummon
{
    class DreadSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Sea Wasp");
            Tooltip.SetDefault("Use to catch a powerful Blood Moon monster");

        }
        public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
            Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 0, 10);
            Item.bait = 50;
        }
    }
    class BatiDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if(npc.type == NPCID.BloodEelHead || npc.type == NPCID.GoblinShark)
            {
                npcLoot.Add(ItemDropRule.Common(ItemType<DreadSummon>(), 1, 1, 1));
            }
        }
    }
}
