using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Terraria.Audio;

using TRAEProject.Common.ModPlayers;
 
namespace TRAEProject.NewContent.Items.Armor.EchoHunter 
{
	[AutoloadEquip(EquipType.Body)]
    public class EchoHunterChestplate: ModItem
	{
        // Total stats:
        // 45 defense
        // +30% damage, +15% crit
        // +2 minions
        // +25% melee speed
        // +25% ranged velocity
        // -25% mana costs
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Underworld Warrior Chestplate");
			// Tooltip.SetDefault("15% increased damage\n25% increased ranged velocity");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(0, 6, 6, 6);
			Item.rare = ItemRarityID.Cyan;
			Item.width = 34;
			Item.height = 20;
			Item.defense = 15;
		}

		public override void UpdateEquip(Player player)
		{
            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetModPlayer<RangedStats>().rangedVelocity += 0.25f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemType<EchoHunterHelmet>() && legs.type == ItemType<EchoHunterGreaves>();
		}

		public override void UpdateArmorSet(Player player)
        {
            player.maxMinions += 2;
            player.setBonus = "Increased your maximum number of minions by 2\nDouble Tap Down to teleport to the cursor's location";
            player.GetModPlayer<EchoHunterSet>().EchoHunterSetBonus = true;
		}
    }
    public class EchoHunterSet : ModPlayer
    {
        public bool EchoHunterSetBonus;
        public override void ResetEffects()
        {
            EchoHunterSetBonus = false;
        }
        public override void PostUpdateEquips()
        {
            if (EchoHunterSetBonus)
            {
                if (Player.whoAmI == Main.myPlayer && Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] > 0 && Player.doubleTapCardinalTimer[0] != 15)
                {
          
 
                    
                }
            }
        }

    }
}




