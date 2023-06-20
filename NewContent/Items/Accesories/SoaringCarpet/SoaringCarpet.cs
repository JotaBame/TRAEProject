using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TRAEProject.NewContent.Items.Accesories.SoaringCarpet
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
            Item.value = 10000;
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
			/*
			class SoaringCarpetEffect : ModPlayer
			{
				public bool soaringCarpet = false;
				public override void ResetEffects()
				{
					soaringCarpet = false;
				}
				public override void SetStaticDefaults()
				{
					IL.Terraria.Player.CarpetMovement += CarpetHook;

				}
				private void CarpetHook(ILContext il)
				{

					var c = new ILCursor(il);
					c.Emit(OpCodes.Ldarg_0);
					c.EmitDelegate<Action<Player>>((Player) =>
					{
						bool flag = false;
						if (Player.grappling[0] == -1 && soaringCarpet && !Player.mount.Active)
						{
							if (Player.controlDown && Player.canCarpet)
							{
								Player.canCarpet = false;
								Player.carpetTime = 300;
							}
							if (Player.carpetTime > 0 && Player.controlDown)
							{
								Player.fallStart = (int)(Player.position.Y / 16f);
								flag = true;
								Player.carpetTime--;
								float num = Player.gravity;
								if (Player.gravDir == 1f && Player.velocity.Y > 0f - num)
								{
									Player.velocity.Y = 0f - (num + 1E-06f);
								}
								else if (Player.gravDir == -1f && Player.velocity.Y < num)
								{
									Player.velocity.Y = num + 1E-06f;
								}
								Player.carpetFrameCounter += 1f + Math.Abs(Player.velocity.X * 0.5f);
								if (Player.carpetFrameCounter > 8f)
								{
									Player.carpetFrameCounter = 0f;
									Player.carpetFrame++;
								}
								if (Player.carpetFrame < 0)
								{
									Player.carpetFrame = 0;
								}
								if (Player.carpetFrame > 5)
								{
									Player.carpetFrame = 0;
								}
							}
						}
						if (!flag)
						{
							Player.carpetFrame = -1;
						}
						else
						{
							Player.slowFall = false;
							//Player.carpet = true;
						}
					});
				}

				public override void PostUpdateRunSpeeds()
				{


				}
			}
			*/
		}
