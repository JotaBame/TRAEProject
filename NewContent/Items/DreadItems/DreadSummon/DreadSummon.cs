using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.DreadItems.DreadSummon
{
    public class DreadSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Bloody Sea Wasp");
            // Tooltip.SetDefault("Use to catch a powerful Blood Moon monster");

        }
        public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.width = 34;
			Item.height = 36;
            Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 0, 10);
            Item.bait = 50;
        }
    }
    public  class BatiDrop : GlobalNPC
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
