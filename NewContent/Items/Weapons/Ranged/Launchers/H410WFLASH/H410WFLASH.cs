using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.H410WFLASH;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.H410WFLASH
{
    public class H410WFLASH : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("H410W FLASH");
            // Tooltip.SetDefault("'Fairy Launching Assault Shoulder weapon'\nShoots explosive pixie Rockets that follow your cursor");
        }
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 36;
            Item.damage = 72;
            Item.useAnimation = 38;
            Item.useTime = 38;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 10);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileID.RocketI;
            Item.knockBack = 6f;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.PixieDust, 20)
                .AddIngredient(ItemID.SoulofLight, 15)
                .AddIngredient(ItemID.SoulofSight, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
		public override Vector2? HoldoutOffset() {
			return new Vector2(-16f, -5f);
		}
    }
    public class ShootPixieRockets : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            if (weapon.type == ItemType<H410WFLASH>())
            {
                switch (ammo.type)
                {
                    case ItemID.RocketI:
                        type = ProjectileType<PixieRocket>();
                        break;
                    case ItemID.RocketII:
                        type = ProjectileType<DestructivePixie>();
                        break;
                    case ItemID.RocketIII:
                        type = ProjectileType<SuperPixie>();
                        break;
                    case ItemID.RocketIV:
                        type = ProjectileType<DirectPixie>();
                        break;
                    case ItemID.MiniNukeI:
                        type = ProjectileType<MiniNukePixie>();
                        break;
                    case ItemID.MiniNukeII:
                        type = ProjectileType<DestructiveMiniNukePixie>();
                        break;
                    case ItemID.ClusterRocketI:
                        type = ProjectileType<ClusterPixie>();
                        break;
                    case ItemID.ClusterRocketII:
                        type = ProjectileType<HeavyPixie>();
                        break;
                    case ItemID.DryRocket:
                        type = ProjectileType<DryPixie>();
                        break;
                    case ItemID.WetRocket:
                        type = ProjectileType<WetPixie>();
                        break;
                    case ItemID.LavaRocket:
                        type = ProjectileType<LavaPixie>();
                        break;
                    case ItemID.HoneyRocket:
                        type = ProjectileType<HoneyPixie>();
                        break;
                }
                if (ammo.type == ItemType<LuminiteRocket>())
                {
                    type = ProjectileType<LuminitePixie>();
                }
            }
          

        }
    }
}