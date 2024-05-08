using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.CryoCannon;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.CryoCannon
{
    class CryoCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Cryo Cannon");
            // Tooltip.SetDefault("Shoots Freezing Rockets");
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.damage = 70;
            Item.useAnimation = 43;
            Item.useTime = 43;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 8);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileID.RocketI;
            Item.knockBack = 6f;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item28;
			Item.scale = 1.15f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddRecipeGroup("Adamantite", 12)
                .AddIngredient(ItemID.SoulofLight, 18)
                .AddIngredient(ItemID.FrostCore, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
		public override Vector2? HoldoutOffset() {
			return new Vector2(0f, 0f);
		}
    }
    public class ShootCryoRockets : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            if (weapon.type == ItemType<CryoCannon>())
            {
                switch (ammo.type)
                {
                    case ItemID.RocketI:
                        type = ProjectileType<CryoRocket>();
                        break;
                    case ItemID.RocketII:
                        type = ProjectileType<DestructiveCryo>();
                        break;
                    case ItemID.RocketIII:
                        type = ProjectileType<SuperCryo>();
                        break;
                    case ItemID.RocketIV:
                        type = ProjectileType<DirectCryo>();
                        break;
                    case ItemID.MiniNukeI:
                        type = ProjectileType<MiniNukeCryo>();
                        break;
                    case ItemID.MiniNukeII:
                        type = ProjectileType<DestructiveMiniNukeCryo>();
                        break;
                    case ItemID.ClusterRocketI:
                        type = ProjectileType<ClusterCryo>();
                        break;
                    case ItemID.ClusterRocketII:
                        type = ProjectileType<HeavyCryo>();
                        break;
                    case ItemID.DryRocket:
                        type = ProjectileType<DryCryo>();
                        break;
                    case ItemID.WetRocket:
                        type = ProjectileType<WetCryo>();
                        break;
                    case ItemID.LavaRocket:
                        type = ProjectileType<LavaCryo>();
                        break;
                    case ItemID.HoneyRocket:
                        type = ProjectileType<HoneyCryo>();
                        break;
                }
                if (ammo.type == ItemType<LuminiteRocket>())
                {
                    type = ProjectileType<LuminiteCryo>();
                }
            }
            
        }
    }
}