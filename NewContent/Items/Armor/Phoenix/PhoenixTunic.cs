using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using Terraria.Audio;
using TRAEProject.NewContent.Items.Armor.Ronin;

namespace TRAEProject.NewContent.Items.Armor.Phoenix
{
	[AutoloadEquip(EquipType.Body)]
    public class PhoenixTunic : ModItem
    {
        // Total stats:
        // 36 defense
        // +33% ranged and summon damage
        // +3 minions
        // +11% movement speed, +10% jump speed 
        // +15% movement and jump speed (sort of)
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phoenix Tunic");
			// Tooltip.SetDefault("11% increased ranged and summon damage\nIncreased your maximum number of minions by 1");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(0, 7, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.width = 34;
			Item.height = 28;
			Item.defense = 12;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemType<MagicalAsh>(), 4)
				.AddIngredient(ItemType<ObsidianScale>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
		}

		public override void UpdateEquip(Player player)
		{
            player.GetDamage<SummonDamageClass>() += 0.11f;
            player.GetDamage<RangedDamageClass>() += 0.11f;
            player.maxMinions++;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemType<PhoenixHeadgear>() && legs.type == ItemType<PhoenixLeggings>();
		}

		public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<PhoenixSet>().PhoenixSetBonus = true;
            player.maxMinions += 2;
            player.setBonus = "Increases your max number of minions by 2\nStrike an enemy with a whip to refresh flight and increase mobility";
		}
    }
    public class PhoenixSet : ModPlayer
    {
        public bool PhoenixSetBonus;
        public override void ResetEffects()
        {
            PhoenixSetBonus = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PhoenixSetBonus && ProjectileID.Sets.IsAWhip[proj.type])
            {     
                if (!Player.HasBuff(BuffType<PhoenixRush>()))
                {
                    SoundEngine.PlaySound(SoundID.Item20 with { MaxInstances = 0 }, Player.Center);
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(3.6f, 3.6f);
                        Dust d = Dust.NewDustPerfect(Player.Center, DustID.Torch, speed * 2.5f, Scale: 3f);
                        d.noGravity = true;
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust7 = Dust.NewDustDirect(Player.Center - Player.Size / 4f, Player.width / 2, Player.height / 2, 31);
                        dust7.noGravity = true;
                        dust7.velocity *= 2.3f;
                        dust7.fadeIn = 1.5f;
                        dust7.noLight = true;
                    }
                }
                Player.AddBuff(BuffType<PhoenixRush>(), 5 * 60);
                Player.RefreshMovementAbilities(); 
    

            }
        }
    }
    public class PhoenixRush : ModBuff
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ash Rush");
            // Description.SetDefault("15% increased movement and jump speed");
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.15f;
            player.jumpSpeedBoost += 0.75f; // 15%!

        }
    }
}




