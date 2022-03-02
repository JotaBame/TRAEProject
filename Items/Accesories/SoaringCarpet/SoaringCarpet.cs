
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TRAEProject.Items.Accesories.SoaringCarpet
{
    class SoaringCarpet : ModItem
    {
        public override void SetStaticDefaults()
<<<<<<< Updated upstream:Items/Accesories/SoaringCarpet/SoaringCarpet.cs
        {
            DisplayName.SetDefault("Soaring Carpet");
            Tooltip.SetDefault("Grants a soaring carpet, use it by holding DOWN\n15% increased movement speed");
=======
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			DisplayName.SetDefault("Soaring Carpet");
            Tooltip.SetDefault("Grants a soaring carpet, use it by holding UP\n25% movement speed\nFall damage immunity");
>>>>>>> Stashed changes:NewContent/Items/Accesories/SoaringCarpet/SoaringCarpet.cs
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
<<<<<<< Updated upstream:Items/Accesories/SoaringCarpet/SoaringCarpet.cs
			player.moveSpeed += 0.1f;
			player.jumpSpeedBoost += 0.5f;
=======

			player.moveSpeed += 0.25f;
			player.noFallDmg = true;

>>>>>>> Stashed changes:NewContent/Items/Accesories/SoaringCarpet/SoaringCarpet.cs
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FlyingCarpet)
				.AddIngredient(ItemID.AnkletoftheWind)
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
				if (soaringCarpetTime < 240 && Player.controlDown && Player.gravDir != -1)
				{
					Player.runAcceleration *= 1.5f;
			        Player.wingRunAccelerationMult *= 1.5f;				
					if (Player.velocity.Y < 0)
					{
						Player.velocity.Y += 0.7f;
					}
					if (Player.velocity.Y > 0)
					{
						Player.velocity.Y -= 0.7f;
						++soaringCarpetTime;
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
