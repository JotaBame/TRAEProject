
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{        
    public class CrystalGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Gel");
            Tooltip.SetDefault("Splits 8 times and ignores 25 defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 24;
            Item.height = 22;
            Item.shootSpeed = 4f;
            Item.consumable = true;
            Item.shoot = ProjectileType<CrystalGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 3000;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(4986)
                .AddIngredient(ItemID.Gel, 10)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
    public class CrystalGelP : ModProjectile
    {   
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Flamethrower");     //The English name of the Projectile
        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 5;
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.alpha = 255;
            Projectile.penetrate = 2;
            Projectile.extraUpdates = 2;
            Projectile.width = Projectile.height = 2;
       Projectile.GetGlobalProjectile<ProjectileStats>().armorPenetration = 25;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }
        int DustColor = 0;
        bool runOnce = true;
        public override void AI()
        {
            if (runOnce)
            {
                DustColor = Main.rand.NextFromList(DustID.BlueTorch, DustID.PinkTorch, DustID.PurpleTorch);
                runOnce = false;
            }
            float dustScale = 1f;
            if (Projectile.ai[0] == 0f)
                dustScale = 0.25f;
            else if (Projectile.ai[0] == 1f)
                dustScale = 0.5f;
            else if (Projectile.ai[0] == 2f)
                dustScale = 0.75f;

            if (Main.rand.Next(3) == 0)
            {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustColor, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

                    // Some dust will be large, the others small and with gravity, to give visual variety.
                    if (Main.rand.NextBool(3))
                    {
                        dust.noGravity = true;
                        dust.scale *= 3f;
                        dust.velocity.X *= 2f;
                        dust.velocity.Y *= 2f;
                    }

                    dust.scale *= 1.5f;
                    dust.velocity *= 1.2f;
                    dust.scale *= dustScale;               
            }
            Projectile.ai[0] += 1f;
            Projectile.localAI[0] += 1f;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 30;
            hitbox.X -= size;
            hitbox.Y -= size;
            hitbox.Width += size * 2;
            hitbox.Height += size * 2;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[1] < 1f)
            {		
		        float rotation = MathHelper.ToRadians(60);
                for (int i = 0; i < 4; ++i)
                {
	                 Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (2 - 1))) * 0.2f; // Watch out for dividing by 0 if there is only 1 projectile.
                     int Gel = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + perturbedSpeed.X, Projectile.velocity.Y + perturbedSpeed.Y, Projectile.type, (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, Projectile.ai[1] + 1f);
                     Main.projectile[Gel].timeLeft = 30;	    
                }    
            }
        }
    }
}


