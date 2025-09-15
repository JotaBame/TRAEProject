
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories;
using TRAEProject.NewContent.Items.Materials;
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
            Item.rare = 4;
            Item.buffType = BuffType<EchoSense>();
            Item.buffTime = 60 * 60 * 5;
            Item.value = 2000;
        }
 
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemID.BottledWater, 4)
                .AddIngredient(ItemID.Fireblossom, 1)
                .AddIngredient(ItemID.Shiverthorn, 1)
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
  
        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            if (Main.LocalPlayer.HasBuff(BuffType<EchoSense>()) && projectile.hostile && projectile.damage > 0 && projectile.alpha < 255)
            {
 
                     return Color.Pink;

              
            }
            return null;
        }   


}
}