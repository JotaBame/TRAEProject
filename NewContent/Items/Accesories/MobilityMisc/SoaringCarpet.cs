
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace TRAEProject.NewContent.Items.Accesories.MobilityMisc
{
    class SoaringCarpet : ModItem
    {
        public override void SetStaticDefaults()
		{
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// DisplayName.SetDefault("Soaring Carpet");
            // Tooltip.SetDefault("Grants a soaring carpet, use it by holding UP\nIncreases movement speed by 10%\nIncreases acceleration\nFall damage immunity");

        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(gold:2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SoaringCarpetEffect>().soaringCarpet = true;
			player.GetModPlayer<Mobility>().ankletAcc = true;
			player.noFallDmg = true;

		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FlyingCarpet)
				.AddIngredient(ItemID.AnkletoftheWind)
								.AddTile(TileID.TinkerersWorkbench)

				.Register();
		}
	}
	class SoaringCarpetEffect : ModPlayer
	{
		public bool soaringCarpet = false;
	    public int soaringCarpetTime = 0;
		public override void ResetEffects()
		{
			soaringCarpet = false;
		}
        public override void PostUpdateEquips()
        {
			if (soaringCarpet)
			{
		
				if (soaringCarpetTime < 240 && Player.controlUp && Player.gravDir != -1 && Player.velocity.Y != 0f && !Player.mount.Active)
				{
					Player.moveSpeed += 0.3f;
					Player.runAcceleration *= 1.5f;
			        Player.wingRunAccelerationMult *= 1.5f;
					if (Player.velocity.Y < 0)
					{
						Player.velocity.Y += 0.99f;
					}
					if (Player.velocity.Y > 0)
					{
						Player.velocity.Y -= 0.99f;
					}
					++soaringCarpetTime;
					for (int i = 0; i < 5; i++)
					{
						int dustID = Main.rand.NextFromList(DustID.GreenTorch, DustID.WhiteTorch, DustID.PinkTorch);
						Vector2 vector = new(Player.Bottom.X + Main.rand.Next(-Player.width, Player.width), Player.Bottom.Y + Main.rand.Next(-5, 0));
						Dust dust = Dust.NewDustDirect(vector,  1, 1, dustID, Scale: 1.15f * Main.rand.NextFloat(1, 1.3f));
						dust.noGravity = true;
					}
					
				}
				if (Player.velocity.Y == 0)
                {
					soaringCarpetTime = 0;
                }
			
			}
		}
    }

}
