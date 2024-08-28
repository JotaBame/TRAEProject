using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Weapons.Ranged;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.FlamethrowerAmmo;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;
using static TRAEProject.NewContent.Items.FlamethrowerAmmo.IchorGelP;

namespace TRAEProject.NewContent.Items.DreadItems.BloodBoiler
{
  public  class BloodBoiler : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Blood Boiler");
            // Tooltip.SetDefault("Gives a new meaning to 'making your blood boil'");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 32;
            Item.damage = 12;
            Item.useAnimation = 45;
            Item.useTime = 7;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Gel;
            Item.knockBack = 2f;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<BloodFlameP>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34; // find flamethrower sound
        }
        int shotcount = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shotcount++;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 54f; //This gets the direction of the flame projectile, makes its length to 1 by normalizing it. It then multiplies it by 54 (the item width) to get the position of the tip of the flamethrower.
            if (Collision.CanHit(position, 6, 6, position + muzzleOffset, 6, 6))
            {
                position += muzzleOffset;
            }
            if (shotcount >= 12)
            {
                shotcount = 0;
                Projectile.NewProjectile(source, position, velocity, ProjectileType<BloodyGelP>(), damage, knockback, player.whoAmI);
            }

            // This is to prevent shooting through blocks and to make the fire shoot from the muzzle.
            return true;
        }
        public override Vector2? HoldoutOffset()
        // HoldoutOffset has to return a Vector2 because it needs two values (an X and Y value) to move your flamethrower sprite. Think of it as moving a point on a cartesian plane.
        {
            return new Vector2(0, -2); // If your own flamethrower is being held wrong, edit these values. You can test out holdout offsets using Modder's Toolkit.
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            // To make this item only consume ammo during the first jet, we check to make sure the animation just started. ConsumeAmmo is called 5 times because of item.useTime and item.useAnimation values in SetDefaults above.
            return player.itemAnimation >= player.itemAnimationMax - 4;
        }
    }
    public class BloodFlameP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            ColorMiddle = new Color(178, 11, 0, 200);
            ColorBack = new Color(255, 100, 90, 200);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(60, 60, 60, 100);
            dustID = DustID.Blood;
            dustScale = 0.75f;
            Projectile.ArmorPenetration = 25;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int bludChance = 1;
            if (Main.rand.NextBool(bludChance))
            { 
                float velX = Main.rand.NextFloat(-2f, 3f) * 3f;
                float velY = Main.rand.NextFloat(-4f, 4f) * 3f;
				int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, velX, velY, ProjectileType<BloodyGelP>(), Projectile.damage, 1f, Projectile.owner, 0f, 0f);
				Main.projectile[projectile].ArmorPenetration = 25;
            }
 
        }
    }
}
