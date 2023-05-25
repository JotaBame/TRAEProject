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
using Terraria.GameContent.Achievements;

namespace TRAEProject.NewContent.Items.BeholderItems
{
    public class WandOfDisintegration : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wand of Disintegration");
            Item.staff[Item.type] = true;
            // Tooltip.SetDefault("Disintegrate everything in your path\nUse at your own risk");
        }

        public override void SetDefaults()
        {
            Item.damage = 33;
            Item.DefaultToStaff(ProjectileType<DisintegrationBeam>(), 10, 10, 20);
            Item.noMelee = true;
            Item.CountsAsClass<MagicDamageClass>(); 
            Item.channel = true; //Channel so that you can held the weapon [Important]

            Item.autoReuse = true;
            Item.rare = ItemRarityID.Lime;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item13;
            Item.value = Item.sellPrice(silver: 3);
        }


    }
    // The following laser shows a channeled ability, after charging up the laser will be fired
    // Using custom drawing, dust effects, and custom collision checks for tiles
    public class DisintegrationBeam : ModProjectile
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
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.townNPC)
            {
                return true;
            }
            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            // We start drawing the laser if we have charged up
            if (IsAtMaxCharge)
            {
                float scaleY = 0.8f;
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

        // Change the way of collision check of the Projectile
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
                Vector2 laserpos = player.Center + Projectile.velocity * Distance;


            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center,
                player.Center + unit * Distance, 22, ref point);
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 10;
        }
        int destructotimer = 0;
        // The AI of the Projectile
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
            destructotimer++;

            UpdatePlayer(player);
            ChargeLaser(player);

            // If laser is not charged yet, stop the AI here.
            if (Charge < MAX_CHARGE) return;

            SetLaserPosition(player);
            SpawnDusts(player);
            CastLights();
            // important that this goes last
            if (destructotimer >= 30)
                destructotimer = 0;
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
                Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, DustID.PurpleTorch, dustVel.X, dustVel.Y)];
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust = Dust.NewDustDirect(Main.player[Projectile.owner].Center, 0, 0, 31,
                    -unit.X * Distance, -unit.Y * Distance);
                dust.fadeIn = 0f;
                dust.noGravity = true;
                dust.scale = 0.88f;
                dust.color = Color.Cyan;
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 offset = Projectile.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                Dust dust = Main.dust[Dust.NewDust(dustPos + offset - Vector2.One * 4f, 8, 8, DustID.PurpleTorch, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                unit = dustPos - Main.player[Projectile.owner].Center;
                unit.Normalize();
                dust = Main.dust[Dust.NewDust(Main.player[Projectile.owner].Center + 55 * unit, 8, 8, DustID.PurpleTorch, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
            }
        }

        /*
		* Sets the end of the laser position based on where it collides with something
		*/
        private void SetLaserPosition(Player player)
        {
            for (Distance = MOVE_DISTANCE; Distance <= 600f; Distance += 5f)
            {
                var start = player.Center + Projectile.velocity * Distance;
                if (!Collision.CanHit(player.Center, 1, 1, start, 1, 1))
                {
                   
                    Distance -= 5f;
                    break;
                }
                if (Collision.CanHit(player.Center, 1, 1, start, 1, 1) && destructotimer >= 30)
                {
                    int minTileX = (int)(start.X / 16f - (float)3);
                    int maxTileX = (int)(start.X / 16f + (float)3);
                    int minTileY = (int)(start.Y / 16f - (float)3);
                    int maxTileY = (int)(start.Y / 16f + (float)3);
                    if (minTileX < 0)
                    {
                        minTileX = 0;
                    }
                    if (maxTileX > Main.maxTilesX)
                    {
                        maxTileX = Main.maxTilesX;
                    }
                    if (minTileY < 0)
                    {
                        minTileY = 0;
                    }
                    if (maxTileY > Main.maxTilesY)
                    {
                        maxTileY = Main.maxTilesY;
                    }
                    bool canKillWalls = false;
                    for (int x = minTileX; x <= maxTileX; x++)
                    {
                        for (int y = minTileY; y <= maxTileY; y++)
                        {
                            float diffX = Math.Abs((float)x - start.X / 16f);
                            float diffY = Math.Abs((float)y - start.Y / 16f);
                            double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                            if (distance < (double)16 && Main.tile[x, y] != null && Main.tile[x, y].WallType == 0)
                            {
                                canKillWalls = true;
                                break;
                            }
                        }
                    }
                    AchievementsHelper.CurrentlyMining = true;
                    for (int i = minTileX; i <= maxTileX; i++)
                    {
                        for (int j = minTileY; j <= maxTileY; j++)
                        {
                            float diffX = Math.Abs((float)i - start.X / 16f);
                            float diffY = Math.Abs((float)j - start.Y / 16f);
                            double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                            if (distanceToTile < (double)3)
                            {
                                bool canKillTile = true;
                                Tile tile = Main.tile[i, j];
                                if (tile.HasTile)
                                {
                                    canKillTile = true;
                                    if (Main.tileDungeon[(int)tile.TileType] || tile.TileType == 88 || tile.TileType == 21 || tile.TileType == 26 || tile.TileType == 107 || tile.TileType == 108 || tile.TileType == 111 || tile.TileType == 226 || tile.TileType == 237 || tile.TileType == 221 || tile.TileType == 222 || tile.TileType == 223 || tile.TileType == 211 || tile.TileType == 404)
                                    {
                                        canKillTile = false;
                                    }
                                    if (!Main.hardMode && tile.TileType == 58)
                                    {
                                        canKillTile = false;
                                    }
                                    if (!TileLoader.CanExplode(i, j))
                                    {
                                        canKillTile = false;
                                    }
                                    if (canKillTile)
                                    {
                                        WorldGen.KillTile(i, j, false, false, false);
                                        if (!tile.HasTile && Main.netMode != NetmodeID.SinglePlayer)
                                        {
                                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                                        }
                                    }
                                }
                                if (canKillTile)
                                {
                                    for (int x = i - 1; x <= i + 1; x++)
                                    {
                                        for (int y = j - 1; y <= j + 1; y++)
                                        {
                                            if (Main.tile[x, y] != null && Main.tile[x, y].WallType > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile[x, y].WallType))
                                            {
                                                WorldGen.KillWall(x, y, false);
                                                if (Main.tile[x, y].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer)
                                                {
                                                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    AchievementsHelper.CurrentlyMining = false;
                }
            }
        }
        bool dontDoItAgain = false;

        int timer = 0;
        private void ChargeLaser(Player player)
        {
            // Kill the Projectile if the player stops channeling
            if (!player.channel)
            {
                Projectile.Kill();
            }
            else
            {
                timer++;
                if (!player.CheckMana(player.inventory[player.selectedItem].mana, false))
                    Projectile.Kill();
                // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore
                if (timer >= 20)
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
                    SoundEngine.PlaySound(SoundID.Zombie104 with { MaxInstances = 0 }, Projectile.position);
                }
                int chargeFact = (int)(Charge / 20f);
                Vector2 dustVelocity = Vector2.UnitX * 18f;
                dustVelocity = dustVelocity.RotatedBy(Projectile.rotation - 1.57f);
                Vector2 spawnPos = Projectile.Center + dustVelocity;
                for (int k = 0; k < chargeFact + 1; k++)
                {
                    Vector2 spawn = spawnPos + ((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - chargeFact * 2);
                    Dust dust = Main.dust[Dust.NewDust(pos, 20, 20, DustID.PurpleTorch, Projectile.velocity.X / 2f, Projectile.velocity.Y / 2f)];
                    dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - chargeFact * 2f) / 10f;
                    dust.noGravity = true;
                    dust.scale = Main.rand.Next(10, 20) * 0.05f;
                }
            }
        }

        private void UpdatePlayer(Player player)
        {
            // Multiplayer support here, only run this code if the client running it is the owner of the Projectile
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
            player.heldProj = Projectile.whoAmI; // Update player's held Projectile
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