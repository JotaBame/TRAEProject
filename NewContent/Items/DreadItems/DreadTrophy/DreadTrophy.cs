using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.DreadItems.DreadTrophy
{
    class DreadTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dreadnautilus Trophy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.rare = 2;
            Item.consumable = true;
            Item.value = 50000;
            Item.createTile = TileType<DreadTrophyTile>();
            Item.placeStyle = 0;
        }
    }
    class DreadTrophyTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (frameX == 0)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ItemType<DreadTrophy>());
            }
        }
    }
}
