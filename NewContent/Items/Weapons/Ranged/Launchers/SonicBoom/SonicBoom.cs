using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Boss.Plantera;
using TRAEProject.Common.ModPlayers;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.SonicBoom
{
    public class SonicBoom : ModItem
    {
        public static SoundStyle ShotSFX => new("TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerShot");

        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Cryo Cannon");
            // Tooltip.SetDefault("Shoots Freezing Rockets");
        }
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 28;
            Item.damage = 120;
            Item.useAnimation = 45;
            Item.useTime = 15;
            Item.reuseDelay = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 4);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileType<SonicRocket>();
            Item.knockBack = 6f;
            Item.shootSpeed = 6f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = ShotSFX with { PitchVariance = 0.3f, MaxInstances = 1, }; 
            Item.scale = 1f;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation > 0)
            {
                return  true; } return false;
        }
 
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float waveSize = 0.65f;
            int projToShoot3 = 14;
            float speed3 = 14f;
            int Damage3 = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            float KnockBack3 = player.inventory[player.selectedItem].knockBack;
            player.PickAmmo(player.inventory[player.selectedItem], out projToShoot3, out speed3, out Damage3, out KnockBack3, out var usedAmmoItemId, dontConsume: true);
            float numberProjectiles = 3;
            switch (usedAmmoItemId)
            {
                case ItemID.RocketI:
                    break;
                case ItemID.RocketII:
                    break;
                case ItemID.RocketIII:
                    waveSize = 1f;
                    break;
                case ItemID.RocketIV:
                    waveSize = 0.5f;
                     break;
                case ItemID.MiniNukeI:
                    waveSize = 1.3f;
                    break;
                case ItemID.MiniNukeII:
                    waveSize = 1.3f;
                    break;
                case ItemID.ClusterRocketI:
                    waveSize = 0.3f;
                    break;
                case ItemID.ClusterRocketII:
                    waveSize = 0.65f;
                    break;
                case ItemID.DryRocket:
                    break;
                case ItemID.WetRocket:
                    break;
                case ItemID.LavaRocket:
                    break;
                case ItemID.HoneyRocket:
                    break;
            }
            if (type == ItemType<LuminiteRocket>()) // what to do for luminite...
            {
             }
            type = ProjectileType<SonicRocket>(); // else it starts acting very weird
            if (waveSize == 0.3f) // if cluster rockets are being used
            {
                float rotation = MathHelper.ToRadians(6f) * player.GetModPlayer<RangedStats>().spreadModifier;

                position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 10f;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                    int projectile = Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, Main.myPlayer, .6f, ai1: waveSize);
                }
            }
            else
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, .6f, ai1: waveSize);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<EchoHeart>(), 6)
                 .AddIngredient(ItemID.SoulofSight, 8)
                .AddIngredient(ItemID.SoulofMight, 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
		public override Vector2? HoldoutOffset() {
			return new Vector2(0f, 0f);
		}
    }
 }