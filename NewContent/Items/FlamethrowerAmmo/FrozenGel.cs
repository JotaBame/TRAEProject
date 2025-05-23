
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.Common;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.CryoCannon;
namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class FrozenGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Glacial Gel");
            // Tooltip.SetDefault("Create the coldest winds");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99; AmmoID.Sets.IsSpecialist[Item.type] = true;

        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1f;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 24;
            Item.height = 24;
            Item.shootSpeed = 0;
            Item.consumable = true;
            Item.shoot = ProjectileType<FrozenGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(200).AddIngredient(ItemType<IceQueenJewel>())
                .AddIngredient(ItemID.Gel, 20)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
    public class FrozenGelP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            ColorMiddle = new Color(95, 120, 255, 240);
            ColorBack = new Color(215, 255, 249, 240);
            ColorLerp = new Color(95, 160, 255, 240);
            ColorSmoke = new Color(33, 125, 202, 200);
            dustID = 135;
            maxScale = 0.2f;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.15f;
			Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Frostburn2;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 180;
			Projectile.penetrate = 5;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                target.GetGlobalNPC<Freeze>().FreezeMe(target, Main.rand.Next(45, 60));
            }
        }
        public override bool PreAI()
        {
            if (maxScale < 2.25f)               
                maxScale += 2f / 60;
            return true;
        }

    }
}



