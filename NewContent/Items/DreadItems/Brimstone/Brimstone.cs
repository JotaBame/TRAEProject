using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace TRAEProject.NewContent.Items.DreadItems.Brimstone
{
    class Brimstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Brimstone");
            // Tooltip.SetDefault("Blood laser barrage");
        }
        public override void SetDefaults()
		{
			Item.damage = 100;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 17;
			Item.rare = ItemRarityID.LightPurple;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 20;
			Item.UseSound = SoundID.Item170;
			Item.useStyle = 5;
			Item.shootSpeed = 14f;
			Item.useAnimation = 20;
			Item.shoot = ProjectileType<BrimstoneBeam>();
			Item.value = Item.sellPrice(gold: 3);

        }
        //public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        //{
        //    Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 54f; //This gets the direction of the flame projectile, makes its length to 1 by normalizing it. It then multiplies it by 54 (the item width) to get the position of the tip of the flamethrower.
        //    if (Collision.CanHit(position, 6, 6, position + muzzleOffset, 6, 6))
        //    {
        //        position += muzzleOffset;
        //    }
        //    // This is to prevent shooting through blocks and to make the fire shoot from the muzzle.
        //    return true;
        //}
        //public override Vector2? HoldoutOffset()
        //// HoldoutOffset has to return a Vector2 because it needs two values (an X and Y value) to move your flamethrower sprite. Think of it as moving a point on a cartesian plane.
        //{
        //    return new Vector2(0, -2); // If your own flamethrower is being held wrong, edit these values. You can test out holdout offsets using Modder's Toolkit.
        //}
    }
    class BrimstoneBeam : ModProjectile
	{
		// Use a different style for constant so it is very clear in code when a constant is used

		// The maximum charge value
		private const float MAX_CHARGE = 60f;
		//The distance charge particle from the player center
		private const float MOVE_DISTANCE = 60f;

		// The actual distance is stored in the ai0 field
		// By making a property to handle this it makes our life easier, and the accessibility more readable
		public float Distance
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		// The actual charge value is stored in the localAI0 field
		public float Charge
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		// Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
		public bool IsAtMaxCharge => Charge == MAX_CHARGE;

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hide = true;
			Projectile.timeLeft = 110;
		}

        public const int chargeTime = 30;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			time += 2;
        }
        public override bool PreDraw(ref Color lightColor)

		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// We start drawing the laser if we have charged up
			if (IsAtMaxCharge)
			{
				float scaleY = time - 30;
				if (scaleY < 0)
				{
					scaleY = 0;
				}
				scaleY = scaleY / 45f;
				scaleY = 1.5f - scaleY;
				DrawLaser(texture, Main.player[Projectile.owner].Center,
					Projectile.velocity, 10, Projectile.damage, -1.57f, 1f * scaleY, 1000f, Color.White, (int)MOVE_DISTANCE);
			}
			return false;
		}

		// The core function of drawing a laser
		public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default, int transDist = 50)
		{
			float r = unit.ToRotation() + rotation;

			// Draws the laser 'body'
			for (float i = transDist; i <= Distance; i += step)
			{
				Color c = Color.White;
				var origin = start + i * unit;
				Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
					new Rectangle(0, 26, 30, 30), i < transDist ? Color.Transparent : c, r,
					new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
			}

			// Draws the laser 'tail'
			Main.EntitySpriteDraw(texture, start + unit * (transDist - step) - Main.screenPosition,
					new Rectangle(0, 0, 24, 24), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

			// Draws the laser 'head'
			Main.EntitySpriteDraw(texture, start + (Distance + step) * unit - Main.screenPosition,
					new Rectangle(0, 52, 28, 28), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
		}

		// Change the way of collision check of the projectile
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// We can only collide if we are at max charge, which is when the laser is actually fired
			if (!IsAtMaxCharge) return false;

            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center, player.Center + unit * Distance, 22, ref point))

            {
                for (int i = 0; i < 50; ++i)
                {
                    Vector2 dustPos = player.Center + Projectile.velocity * Distance;
                    float num384 = Main.rand.Next(-10, 11);
                    float num385 = Main.rand.Next(-10, 11);
                    float num386 = Main.rand.Next(9, 18);
                    float num387 = MathF.Sqrt(num384 * num384 + num385 * num385);
                    num387 = num386 / num387;
                    num384 *= num387;
                    num385 *= num387;
                    int num388 = Dust.NewDust(new Vector2(dustPos.X, dustPos.Y), 50, 50, DustID.Blood, num384, num385, 100, default, 1.88f);
                    Main.dust[num388].noGravity = true;
                    Main.dust[num388].position.X += Main.rand.Next(10, 20) * Main.rand.Next(-1, 1);
                    Main.dust[num388].position.Y += Main.rand.Next(10, 20) * Main.rand.Next(-1, 1);
                    Main.dust[num388].velocity.X = num384;
                    Main.dust[num388].velocity.Y = num385;
                }
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center,
                    player.Center + unit * Distance, 22, ref point);
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
        }
        int time = 0;
        // The AI of the projectile
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = player.Center + Projectile.velocity * MOVE_DISTANCE;
            Projectile.timeLeft = 2;

            // By separating large AI into methods it becomes very easy to see the flow of the AI in a broader sense
            // First we update player variables that are needed to channel the laser
            // Then we run our charging laser logic
            // If we are fully charged, we proceed to update the laser's position
            // Finally we spawn some effects like dusts and light

            UpdatePlayer(player);
            ChargeLaser(player);

            // If laser is not charged yet, stop the AI here.
            if (Charge < MAX_CHARGE) return;

            if (Charge >= MAX_CHARGE)
            {
                time++;
                if (time >= 90)
                {
                    Projectile.Kill();
                }
            }

            SetLaserPosition(player);
            SpawnDusts(player);
            CastLights();
        }

        private void SpawnDusts(Player player)
        {
            Vector2 unit = Projectile.velocity * -1;
            Vector2 dustPos = player.Center + Projectile.velocity * Distance;

            for (int i = 0; i < 2; ++i)
            {
                float num1 = Projectile.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2(MathF.Cos(num1) * num2, MathF.Sin(num1) * num2);
                Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, DustID.Blood, dustVel.X, dustVel.Y)];
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust = Dust.NewDustDirect(Main.player[Projectile.owner].Center, 0, 0, DustID.Blood,
                    -unit.X * Distance, -unit.Y * Distance);
                dust.fadeIn = 0f;
                dust.noGravity = true;
                dust.scale = 0.88f;
                dust.color = Color.Cyan;
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 offset = Projectile.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                Dust dust = Main.dust[Dust.NewDust(dustPos + offset - Vector2.One * 4f, 8, 8, DustID.Blood, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                unit = dustPos - Main.player[Projectile.owner].Center;
                unit.Normalize();
                dust = Main.dust[Dust.NewDust(Main.player[Projectile.owner].Center + 55 * unit, 8, 8, DustID.Blood, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
            }
        }

        /*
        * Sets the end of the laser position based on where it collides with something
        */
        private void SetLaserPosition(Player player)
			{
				for (Distance = MOVE_DISTANCE; Distance <= 2200f; Distance += 5f)
				{
					var start = player.Center + Projectile.velocity * Distance;
					if (!Collision.CanHit(player.Center, 1, 1, start, 1, 1))
					{
						Distance -= 5f;
						break;
					}
				}
			}

		bool dontDoItAgain = false;
		int timer = 0;
		private void ChargeLaser(Player player)
		{
			// Kill the projectile if the player stops channeling
			if (!player.channel)
			{
				Projectile.Kill();
			}
			else
            {
                timer++;
				if (!player.CheckMana(player.inventory[player.selectedItem].mana, false))
                    time += 5;
                // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore
                if (timer >= 10)
                {
					player.CheckMana(player.inventory[player.selectedItem].mana, true);
                    timer = 0;
				}
				Vector2 offset = Projectile.velocity;
				offset *= MOVE_DISTANCE - 20;
				Vector2 pos = player.Center + offset - new Vector2(10, 10);
				if (Charge < MAX_CHARGE)
				{
					Charge++;
				}
				if (Charge == MAX_CHARGE && !dontDoItAgain)
				{
					dontDoItAgain = true;
					SoundEngine.PlaySound(SoundID.NPCDeath13 with { MaxInstances = 0 }, Projectile.Center);
				}
				int chargeFact = (int)(Charge / 20f);
				Vector2 dustVelocity = Vector2.UnitX * 18f;
				dustVelocity = dustVelocity.RotatedBy(Projectile.rotation - 1.57f);
				Vector2 spawnPos = Projectile.Center + dustVelocity;
				for (int k = 0; k < chargeFact + 1; k++)
				{
					Vector2 spawn = spawnPos + ((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - chargeFact * 2);
					Dust dust = Main.dust[Dust.NewDust(pos, 20, 20, DustID.Blood, Projectile.velocity.X / 2f, Projectile.velocity.Y / 2f)];
					dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - chargeFact * 2f) / 10f;
					dust.noGravity = true;
					dust.scale = Main.rand.Next(10, 20) * 0.05f;
				}
			}
		}

		private void UpdatePlayer(Player player)
			{
				// Multiplayer support here, only run this code if the client running it is the owner of the projectile
				if (Projectile.owner == Main.myPlayer)
				{
					Vector2 diff = Main.MouseWorld - player.Center;
					diff.Normalize();
					Projectile.velocity = diff;
					Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					Projectile.netUpdate = true;
				}
				int dir = Projectile.direction;
				player.ChangeDir(dir); // Set player direction to where we are shooting
				player.heldProj = Projectile.whoAmI; // Update player's held projectile
				player.itemTime = 2; // Set item time to 2 frames while we are used
				player.itemAnimation = 2; // Set item animation time to 2 frames while we are used
				player.itemRotation = MathF.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir); // Set the item rotation to where we are shooting
			}

			private void CastLights()
			{
				// Cast a light along the line of the laser
				DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
				Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
			}

			public override bool ShouldUpdatePosition() => false;

			/*
			* Update CutTiles so the laser will cut tiles (like grass)
			*/
			public override void CutTiles()
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Vector2 unit = Projectile.velocity;
				Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
			}
		}
    
}
