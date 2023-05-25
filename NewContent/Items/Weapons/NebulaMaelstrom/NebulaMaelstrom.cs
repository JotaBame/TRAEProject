using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Materials;
using System;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;

namespace TRAEProject.NewContent.Items.Weapons.NebulaMaelstrom
{
    class NebulaMaelstrom : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Nebula Maelstrom");
            // Tooltip.SetDefault("Unleash a particle storm on your opponents\nRight-click to uncast ");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.damage = 85;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.mana = 50;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.shootSpeed = 4f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<MaelstromBall>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item117;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == ProjectileType<MaelstromBall>() && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].Kill();
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.FragmentNebula, 18)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class MaelstromBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nebula Ball");
            Main.projFrames[Projectile.type] = 5;

        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            Projectile.scale = 1.1f;
            Projectile.timeLeft = 3000;
            Projectile.tileCollide = false;
        }
        public float angletimer = 0;
        public int manaDrain = 0;
        public int BeforeItStartsAttacking = 30;
        public int attacktimer = 8;
        public int lasertimer = 0;
        public int zaptimer = 0;
        public override void AI()     
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0]++;
            float distance = 0;
            int Range = 800;
            for (int k = 0; k < 200; k++)
            {
                NPC nPC = Main.npc[k];
                if (!nPC.immortal && nPC.active && !nPC.friendly && !nPC.dontTakeDamage && Vector2.Distance(Projectile.Center, nPC.Center) <= Range/* || (nPC.type == NPCID.MoonLordCore || nPC.type == NPCID.MoonLordHand || nPC.type == NPCID.MoonLordHead)*/)
                {
                    if (Projectile.Distance(nPC.Center) > (double)50)
                    {
                        Vector2 unitY = Projectile.DirectionTo(nPC.Center);
                        if (unitY.HasNaNs())
                            unitY = Vector2.UnitY;
                        Projectile.velocity = Vector2.Multiply(unitY, 5f);
                    }
                    distance = Projectile.Distance(nPC.Center);
                    zaptimer++;
                    if (zaptimer >= 25 && Projectile.ai[0] >= BeforeItStartsAttacking)
                    {
                        zaptimer = 0;
                        SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 }, Projectile.position);

                        float shootToX = nPC.position.X + nPC.width * 0.5f - Projectile.Center.X;
                        float shootToY = nPC.position.Y + nPC.height * 0.5f - Projectile.Center.Y;
                        float distance2 = (float)Math.Sqrt((shootToX * shootToX + shootToY * shootToY));//Dividing the factor of 2f which is the desired velocity by distance2
                        distance2 = 1f / distance2;

                        //Multiplying the shoot trajectory with distance2 times a multiplier if you so choose to
                        shootToX *= distance2 * 10f;
                        shootToY *= distance2 * 10f;
                        Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(0));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<MaelstromZap>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }

                }
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 12)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }


            angletimer += 0.03f;
            if (angletimer > 360)
            {
                angletimer = 0;
            }

            lasertimer++;
            if (distance != 0)
            {
                if (lasertimer >= 75)
                {
                    SoundEngine.PlaySound(SoundID.Item12 with { MaxInstances = 0 }, Projectile.Center);

                    lasertimer = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 direction = Projectile.velocity.RotatedBy(360 - 45 * (i - 1)) * 7f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ProjectileType<MaelstromLaser>(), (int)(Projectile.damage * 1.5f), 1f, Projectile.owner);
                    }
                }
                if (Projectile.ai[0] >= BeforeItStartsAttacking + attacktimer)
                {
                    SoundEngine.PlaySound(SoundID.Item9 with { MaxInstances = 0 }, Projectile.Center);
                    Projectile.ai[0] = BeforeItStartsAttacking;
                    Vector2 direction = Vector2.One.RotatedBy(angletimer);
                    for (int i = 0; i < 2; i++)
                    {
                        player.statMana -= (int)(5 * player.manaCost);
                        float velocity = i < 1 ? 8f : -8f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * velocity, ProjectileType<MaelstromSubshot>(), (int)(Projectile.damage * 0.5f), 2f, Projectile.owner, Main.rand.NextFloat(0.75f, 1f));
                    }
                }
            }
            if (player.statMana <= 0)
            {
                Projectile.Kill();
            }
        }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item20 with { MaxInstances = 0 }, Projectile.Center);
            for (int num368 = 0; num368 < 10; num368++)
            {
                int num369 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 242, 0f, 0f, 200, default, 2.5f);
                Main.dust[num369].noGravity = true;
                Dust dust = Main.dust[num369];
                dust.velocity *= 2f;
                num369 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 242, 0f, 0f, 200, default, 1.5f);
                dust = Main.dust[num369];
                dust.velocity *= 1.2f;
                Main.dust[num369].noGravity = true;
            }
            return true;
        }
    }
    public class MaelstromSubshot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nebula Bolt");

        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 150;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.scale = Projectile.ai[0];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 30)
            {
                int Range = 1000;
                for (int k = 0; k < 200; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC.active && !nPC.friendly && !nPC.dontTakeDamage && Vector2.Distance(Projectile.Center, nPC.Center) <= Range)
                    {
                        Vector2 projPosition = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
                        float npcX = nPC.position.X + nPC.width / 2 - projPosition.X;
                        float npcY = nPC.position.Y + nPC.height / 2 - projPosition.Y;
                        float distance = (float)Math.Sqrt(npcX * npcX + npcY * npcY);
                        float num176 = 9f;
                        distance = num176 / distance;
                        Projectile.velocity.X = npcX * distance;
                        Projectile.velocity.Y = npcY * distance;
                    }
                }
            }
        }
    }
    public class MaelstromLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nebula Laser");

        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 60;
            Projectile.scale = 1.25f;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

        }
    }
    public class MaelstromZap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nebula Zap");

        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 100;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.tileCollide = true;

            Vector2 ProjectilePosition = Projectile.position;
            Projectile.alpha = 255;
            int dust = Dust.NewDust(ProjectilePosition, 1, 1, 242, 0f, 0f, 0, default, 2.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].position = ProjectilePosition;
            Main.dust[dust].velocity *= 0.2f;
        }
    }
}

