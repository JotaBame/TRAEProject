using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Steamworks;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using TRAEProject.Changes.Prefixes;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Projectiles;
using static AssGen.Assets;
using static System.Net.Mime.MediaTypeNames;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Melee.Stormbreaker
{
    public class Stormbreaker : ModItem

    {


        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.knockBack = 6.75f;
            Item.width = 30;
            Item.height = 10;
            Item.damage = 70;
            Item.scale = 1.1f;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<StormbreakerP>();
            Item.shootSpeed = 20f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 5, silver: 50);
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.channel = true;
            Item.noMelee = true; Item.GetGlobalItem<GiveWeaponsPrefixes>().canGetMeleeOtherModifers = true;

        }
        public override bool CanUseItem(Player player)
        {

            if (player.ownedProjectileCounts[ProjectileType<StormbreakerP>()] > 0)
            {
                return false;
            }
            return true;

        }
    }
    public class StormbreakerP : ModProjectile
    {
        private const string ChainTexture = "TRAEProject/NewContent/Items/Weapons/Melee/Stormbreaker/Stormbreaker_Chain";


        private static Asset<Texture2D> chainTexture;




        public override void Load()
        {
            chainTexture = ModContent.Request<Texture2D>(ChainTexture);


        }

        public override void SetStaticDefaults()
        {
            // These lines facilitate the trail drawing
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 24;
            Projectile.height = 24;
            DrawOffsetX = -12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;




        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {



                Projectile.ai[1] += 1f;

                if (Projectile.ai[1] >= 10f)
                {
                    Projectile.velocity.Y += 0.5f;
                    if (Projectile.velocity.Y < 0f)
                    {
                        Projectile.velocity.Y += 0.35f;
                    }
                    Projectile.velocity.X *= 0.95f;
                    if (Projectile.velocity.Y > 16f)
                    {
                        Projectile.velocity.Y = 16f;
                    }
                    if (Vector2.Distance(Projectile.Center, Main.player[Projectile.owner].Center) > 800f)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.netUpdate = true;
                    }
                }

                else if (Projectile.ai[1] >= 30f)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.tileCollide = false;
                float num32 = 16f;
                float num33 = 4f;


                Vector2 vector4 = new(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num34 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector4.X;
                float num35 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector4.Y;
                float num36 = (float)Math.Sqrt(num34 * num34 + num35 * num35);


                if (num36 > 3000f)
                {
                    Projectile.Kill();
                }
                num36 = num32 / num36;
                num34 *= num36;
                num35 *= num36;

                Vector2 vector5 = new Vector2(num34, num35) - Projectile.velocity;
                if (vector5 != Vector2.Zero)
                {
                    Vector2 vector6 = vector5;
                    vector6.Normalize();
                    Projectile.velocity += vector6 * Math.Min(num33, vector5.Length());
                }

                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                    if (rectangle.Intersects(value))
                    {
                        Projectile.Kill();
                    }
                }
            }


            if (Projectile.ai[0] == 0f)
            {
                Vector2 v = Projectile.velocity;
                v = v.SafeNormalize(Vector2.Zero);
                Projectile.rotation = (float)Math.Atan2(v.Y, v.X) + 1.57f;
            }
            else
            {
                Vector2 v2 = Projectile.Center - Main.player[Projectile.owner].Center;
                v2 = v2.SafeNormalize(Vector2.Zero);
                Projectile.rotation = (float)Math.Atan2(v2.Y, v2.X) + 1.57f;
            }



        }
        // PreDraw is used to draw a chain and trail before the projectile is drawn normally.
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] != 1f)
            {
                spawnLightning();

            }
            Projectile.velocity *= 0f;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 1f)
            {
                spawnLightning();

            }

        }
        static int projectileCount = 4;
        static int spreadX = 500;
        int[] spread = { 1, 2 };
        static int spreadY = 900;
        void spawnLightning()
        {
             Projectile.ai[0] = 1f;
            Projectile.velocity *= 0f;

            SoundEngine.PlaySound(SoundID.Item92 with { MaxInstances = 0 }, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 }, Projectile.position);

            for (int i = 0; i < projectileCount; ++i)
            {
                // where the projectile spawns
                int offSetX = Main.rand.Next(spreadX) * (i % 2 == 0 ? 1 : -1);
                float x = Projectile.Center.X + offSetX;
                float y = Projectile.Center.Y - Main.rand.Next((int)(spreadY * 0.8), (int)(spreadY * 1.2));
                //Calculate Velocity
                Vector2 Position = new Vector2(x, y);
                float velocityX = -offSetX;
             
                float velocityY = Projectile.Center.Y + (Main.rand.Next(spread) * 100) - Position.Y;
                float effVelocity = MathF.Sqrt(velocityX * velocityX + velocityY * velocityY);
                effVelocity = 10 / effVelocity;
                velocityX *= effVelocity;
                velocityY *= effVelocity;
                /// 

                int lightning = Projectile.NewProjectile(Projectile.GetSource_FromThis(), x, y, velocityX, velocityY, ProjectileType<LightningBolt>(), Projectile.damage, Projectile.knockBack / 2f);
                // once the projectile reaches the base's position, it will no longer go through tiles.
                                for (int j = 0; j < 50; j++)
                {
                    float radius = 50f / 62.5f;
                    // Why 62.5f and not 41.67?
                    // This is 150% of 41.67, because below the extra dusts get increased distance, with a max of 50% more.
                    // Therefore, the circle of flames more or less accurately represents the radius of the fire ring.
                    Vector2 speed = Main.rand.NextVector2CircularEdge(radius, radius);
                    Dust d = Dust.NewDustPerfect(Position, 226, speed * 5);
                    if (Main.rand.NextBool(3))
                    {
                        d.scale *= Main.rand.NextFloat(1.25f, 1.5f);
                        d.velocity *= Main.rand.NextFloat(1.25f, 1.5f);
                    }
                    d.noGravity = true;
                }

                Main.projectile[lightning].localAI[1] += Projectile.Center.Y;

            }
         }
        public override bool PreDraw(ref Color lightColor)
        {

            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

            // This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
            playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

            Rectangle? chainSourceRectangle = null;
            // Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
            float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

            Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
            {
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            }
            float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (chainLengthRemainingToDraw > 0f)
            {
                Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));


                var chainTextureToDraw = chainTexture;


                Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }

            // Add a motion trail when moving forward, like most flails do (don't add trail if already hit a tile)

            return true;
        }
    }
 
    
}
