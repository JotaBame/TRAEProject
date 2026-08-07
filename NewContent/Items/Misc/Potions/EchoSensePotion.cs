
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using static System.Net.Mime.MediaTypeNames;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Misc.Potions
{
    public class EchoSensePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;
            Item.useStyle = 9;
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 32;
            Item.rare = 2;
            Item.buffType = BuffType<EchoSense>();
            Item.buffTime = 60 * 60 * 5;
            Item.value = 2000;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ItemID.BottledWater, 3)
                .AddIngredient(ItemID.Fireblossom, 3)
                .AddIngredient(ItemID.Shiverthorn, 3)
                .AddIngredient(ItemType<EchoHeart>())
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
    public class EchoSense : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
        }

    }

    public class EchoSenseProjectile : GlobalProjectile
    {
        static Color ClampColor(Color colorToClamp, int minR, int minG, int minB)
        {
            if (colorToClamp.R < minR)
            {
                colorToClamp.R = (byte)minR;
            }
            if (colorToClamp.G < minG)
            {
                colorToClamp.G = (byte)minG;
            }
            if (colorToClamp.B < minB)
            {
                colorToClamp.B = (byte)minB;
            }
            return colorToClamp;
        }

        public override void Load()
        {
            On_Projectile.GetAlpha += ProjHunterPotionEffect;
        }

        private Color ProjHunterPotionEffect(On_Projectile.orig_GetAlpha orig, Projectile self, Color newColor)
        {

            if (self.damage <= 0 || self.alpha >= 255 || !Main.LocalPlayer.HasBuff<EchoSense>())
            {
                return orig(self, newColor);
            }
            if (self.hostile && !self.friendly)
            {
                newColor = ClampColor(newColor, 255, 50, 50);
            }
            else if (!self.hostile && self.friendly)
            {
                newColor = ClampColor(newColor, 50, 255, 50);
            }
            return orig(self, newColor);
        }
    }
}