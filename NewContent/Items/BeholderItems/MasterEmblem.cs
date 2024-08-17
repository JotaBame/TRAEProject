
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.NewContent.Items.BeholderItems
{
 public   class MasterEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Master Emblem");
            // Tooltip.SetDefault("12% increased melee speed\n12% increased ranged critical strike damage\n12% increased mana regeneration\nIncreases your maximum number of minions by 1");
        }
        public override void SetDefaults()
        {            
		        Item.expert = true;
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 66666;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.GetModPlayer<CritDamage>().rangedCritDamage += 0.12f; 
            player.GetModPlayer<Mana>().manaRegenBoost += 0.12f;
            player.maxMinions++;
        }
    }
}
