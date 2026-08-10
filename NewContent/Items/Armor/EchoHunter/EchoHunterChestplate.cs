using ChangesArmor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common.ModPlayers;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Misc.Potions;
using static Terraria.ModLoader.ModContent;
 
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
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.width = 34;
			Item.height = 20;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
            player.GetDamage<GenericDamageClass>() += 0.09f;

            player.maxMinions += 1;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemType<EchoHunterHelmet>() && legs.type == ItemType<EchoHunterGreaves>();
		}
 		public override void UpdateArmorSet(Player player)
        {
             player.setBonus = "Increases maximum mana by 40 and melee speed, chance not to consume ammo and whip range by 25%\nDouble tap down to activate Echo Hunter Mode, granting increased senses and lower falling speed at the cost of sight";
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
            player.ammoCost75 = true;
            player.statManaMax2 += 40;
            player.whipRangeMultiplier += 0.25f;
            player.GetModPlayer<EchoHunterSet>().EchoHunterSetBonus = true;
        }
 
        public override void AddRecipes()
        {
            CreateRecipe()
     
                .AddIngredient(ItemType<EchoHeart>(), 7)
                .AddIngredient(ItemID.SoulofFright, 7)
                .AddIngredient(ItemID.SoulofSight, 7)
                .AddIngredient(ItemID.SoulofMight, 7)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class EchoHunterSet : ModPlayer
    {
        public bool EchoHunterSetBonus;
        public bool EchoHunterMode;
        public override void ResetEffects()
        {
            EchoHunterSetBonus = false;
         }
        public override void UpdateDead()
        {
            EchoHunterSetBonus = false;
            EchoHunterMode = false;

        }
        public override void PostUpdateEquips()
        {
            if (EchoHunterSetBonus)
            {
                if (Player.whoAmI == Main.myPlayer && Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] > 0 && Player.doubleTapCardinalTimer[0] != 15)
                {
                    EchoHunterMode = !EchoHunterMode;
                }

            }
            else
                EchoHunterMode = false;

            if (EchoHunterMode)
            {
                Player.AddBuff(BuffID.Hunter, 1);
                Player.AddBuff(BuffID.Dangersense, 1);
                Player.AddBuff(BuffType<EchoSense>(), 1);
				                Player.AddBuff(BuffID.Blackout, 1);
                  Player.slowFall = true; Player.shroomiteStealth = true;

                Player.GetModPlayer<ShroomiteEffects>().traeStealth = 1f;
 
                // make the invisibility special effect
            }
        }
 
 
    }
}




