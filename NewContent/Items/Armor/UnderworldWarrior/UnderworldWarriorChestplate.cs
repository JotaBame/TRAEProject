using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Terraria.Audio;

using TRAEProject.Common.ModPlayers;

namespace TRAEProject.NewContent.Items.Armor.UnderworldWarrior
{
	[AutoloadEquip(EquipType.Body)]
    public class UnderworldWarriorChestplate: ModItem
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
            player.GetDamage<GenericDamageClass>() += 0.10f;
            player.GetModPlayer<RangedStats>().rangedVelocity += 0.25f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemType<UnderworldWarriorHelmet>() && legs.type == ItemType<UnderworldWarriorGreaves>();
		}

		public override void UpdateArmorSet(Player player)
        {
            player.maxMinions += 2;
            player.setBonus = "Increased your maximum number of minions by 2\nDouble Tap Down to teleport to the cursor's location";
            player.GetModPlayer<UnderworldWarriorSet>().UnderworldWarriorSetBonus = true;
		}
    }
    public class UnderworldWarriorSet : ModPlayer
    {
        public bool UnderworldWarriorSetBonus;
        public override void ResetEffects()
        {
            UnderworldWarriorSetBonus = false;
        }
        public override void PostUpdateEquips()
        {
            if (!Player.HasBuff<BurntOut>() && UnderworldWarriorSetBonus)
            {
                if (Player.whoAmI == Main.myPlayer && Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] > 0 && Player.doubleTapCardinalTimer[0] != 15)
                {
                    int xcursor = (int)(Main.MouseWorld.X / 16);
                    int ycursor = (int)(Main.MouseWorld.Y / 16);
                    Tile tile = Main.tile[xcursor, ycursor];
                    if ((tile != null && !tile.HasTile || !Main.tileSolid[tile.TileType]) && !Player.HasBuff(BuffType<BurntOut>())) //Checks if mouse is in valid postion
                    {
                        Player.grappling[0] = -1; //Remove grapple hooks
                        Player.grapCount = 0;
                        for (int p = 0; p < 1000; p++)
                        {
                            if (Main.projectile[p].active && Main.projectile[p].owner == Player.whoAmI && Main.projectile[p].aiStyle == 7)
                            {
                                Main.projectile[p].Kill();
                            }
                        }
                        Main.SetCameraLerp(0.1f, 0);
                        Player.position.X = Main.MouseWorld.X;
                        Player.position.Y = Main.MouseWorld.Y - (Player.height);

                        for (int i = 0; i < 50; i++)
                        {
                            Vector2 speed = Main.rand.NextVector2CircularEdge(3.6f, 3.6f);
                            Dust d = Dust.NewDustPerfect(Player.Center, DustID.Torch, speed * 2.5f, Scale: 3f);
                            d.noGravity = true;
                        }

                        SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, Player.Center);
                        Player.AddBuff(BuffType<BurntOut>(), 24 * 60);

                    }
                }
            }
        }

    }
    public class BurntOut : ModBuff
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Burnt Out");
            // Description.SetDefault("Need To Recharge");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}




