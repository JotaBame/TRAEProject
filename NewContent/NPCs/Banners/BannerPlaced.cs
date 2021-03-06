using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.NPCs.Banners
{
    public class BomberBonesBannerPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            //disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Bomber Bones Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ItemType<BomberBonesBanner>());//this defines what to drop when this tile is destroyed
        }

        //public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        //{
        //    if (closer)    //so if a player is close to the banner
        //    {
        //        Player player = Main.LocalPlayer;
        //        player.NPCBannerBuff[mod.NPCType("BomberBones")] = true;  // give to player the npcbannerBuff. for a specific npc. change NpcName to your npc name
        //        player.hasBanner = true;
        //    }
        //}
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
    public class BoomxieBannerPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            //disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Boomxie Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ItemType<BoomxieBanner>());//this defines what to drop when this tile is destroyed
        }

        //public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        //{
        //    if (closer)    //so if a player is close to the banner
        //    {
        //        Player player = Main.LocalPlayer;
        //        player.NPCBannerBuff[mod.NPCType("BomberBones")] = true;  // give to player the npcbannerBuff. for a specific npc. change NpcName to your npc name
        //        player.hasBanner = true;
        //    }
        //}
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
    public class FroggabombaBannerPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            //disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Froggabomba Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ItemType<FroggabombaBanner>());//this defines what to drop when this tile is destroyed
        }

        //public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        //{
        //    if (closer)    //so if a player is close to the banner
        //    {
        //        Player player = Main.LocalPlayer;
        //        player.NPCBannerBuff[mod.NPCType("BomberBones")] = true;  // give to player the npcbannerBuff. for a specific npc. change NpcName to your npc name
        //        player.hasBanner = true;
        //    }
        //}
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
    public class GraniteOvergrowthBannerPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            //disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Granite Overgrowth Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ItemType<GraniteOvergrowthBanner>());//this defines what to drop when this tile is destroyed
        }

        //public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        //{
        //    if (closer)    //so if a player is close to the banner
        //    {
        //        Player player = Main.LocalPlayer;
        //        player.NPCBannerBuff[mod.NPCType("BomberBones")] = true;  // give to player the npcbannerBuff. for a specific npc. change NpcName to your npc name
        //        player.hasBanner = true;
        //    }
        //}
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}