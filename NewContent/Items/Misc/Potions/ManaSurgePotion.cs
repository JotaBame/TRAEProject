using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using TRAEProject.Changes.Prefixes;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Misc.Potions
{
    class ManaSurgePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;
            Item.healMana = 20;
            Item.useStyle = 9;
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.width = 14;
            Item.height = 24;
            Item.rare = 4;
            Item.value = 1500;
        }
        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Surge>(), 5 * 60);
        }
        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ItemID.GreaterManaPotion, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.CrystalShard, 3)
                .AddIngredient(ItemID.WormTooth, 3)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
    class Surge : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost *= 0;
        }
    }

}