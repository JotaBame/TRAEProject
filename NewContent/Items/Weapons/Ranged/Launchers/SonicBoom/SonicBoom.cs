using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common.ModPlayers;
using TRAEProject.NewContent.Items.Materials;
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
            Item.damage = 44;
            Item.useAnimation = 30;
            Item.useTime = 10;
            Item.reuseDelay = 30;

            Item.knockBack = 6f;

            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 6);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileType<SonicRocket>();
             Item.shootSpeed = 6f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            // sound is not assigned here 
            Item.scale = 1f;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.itemAnimation < Item.useAnimation)
            {

                return false;
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 3;
 
            if (player.itemAnimation == Item.useAnimation)
            {
                //player.velocity.X += velocity.X * -1;
                //player.velocity.Y += velocity.Y * -1;
                SoundEngine.PlaySound(ShotSFX with { PitchVariance = 0, MaxInstances = 1, Volume = 0.75f }, position);
            }
 
            if (source.AmmoItemIdUsed == ItemID.HoneyRocket)
            {
                velocity *= 0.75f;
            }
 
            if (source.AmmoItemIdUsed == ItemID.ClusterRocketI) // if cluster rockets are being used
            {
                float rotation = MathHelper.ToRadians(6f) * player.GetModPlayer<RangedStats>().spreadModifier;
                position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 10f;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                    int projectile = Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileType<SonicRocket>(), damage, knockback, Main.myPlayer, .6f, ai2:source.AmmoItemIdUsed);
                }
            }
            else

                Projectile.NewProjectile(source, position, velocity, ProjectileType<SonicRocket>(), damage, knockback, Main.myPlayer, .6f, ai2: source.AmmoItemIdUsed);

          

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<EchoHeart>(), 4)
                 .AddIngredient(ItemID.SoulofFright, 8)
                .AddIngredient(ItemID.SoulofNight, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20f, 0f);
        }
    }
}