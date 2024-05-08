using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TRAEProject.NewContent.NPCs.BomberBones;
using TRAEProject.NewContent.NPCs.GraniteOvergrowth;
using TRAEProject.NewContent.NPCs.Underworld.Boomxie;
using TRAEProject.NewContent.NPCs.Underworld.Froggabomba;
using TRAEProject.NewContent.NPCs.Underworld.Lavamander;
using TRAEProject.NewContent.NPCs.Underworld.ObsidianBasilisk;
using TRAEProject.NewContent.NPCs.Underworld.OniRonin;
using TRAEProject.NewContent.NPCs.Underworld.Phoenix;
using TRAEProject.NewContent.NPCs.Underworld.Salalava;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.NPCs.Banners
{
    public abstract class BannerGeneric : ModTile
    {
        
        public override void SetStaticDefaults()
        {
            DustType = -1;

            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[3]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(Type);
        }
        
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
            if((tileFrameY == 0 && Main.tileSolidTop[Main.tile[i, j - 1].TileType]) || (tileFrameY == 18 && Main.tileSolidTop[Main.tile[i, j - 2].TileType]) || (tileFrameY == 36 && Main.tileSolidTop[Main.tile[i, j - 3].TileType]))
            { 
                offsetY -= 8;
            }
			base.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
		}
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
    public class BomberBonesBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)  
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<BomberBonesNPC>()] = true;
            }
        }
    }
    public class BoomxieBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<Boomxie>()] = true;
            }
        }

    }
    public class FroggabombaBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<FroggabombaClone>()] = true;

                Main.SceneMetrics.NPCBannerBuff[NPCType<Froggabomba>()] = true;
            }
        }
    }
    public class GraniteOvergrowthBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {

                Main.SceneMetrics.NPCBannerBuff[NPCType<GraniteOvergrowthNPC>()] = true;
            }
        }
    }
    public class LavamanderBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {

                Main.SceneMetrics.NPCBannerBuff[NPCType<LavamanderNPC>()] = true;
            }
        }
    }
    public class MagmanderBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<Lavalarva>()] = true;
            }
        }
    }
    public class SalalavaBannerPlaced : BannerGeneric
    {

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<SalalavaNPC>()] = true;
            }
        }
    }
    public class OniRoninBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<OniRoninNPC>()] = true;
            }
        }
    }
    public class PhoenixBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<PhoenixAsh>()] = true;

                Main.SceneMetrics.NPCBannerBuff[NPCType<PhoenixNPC>()] = true;
            }
        }
    }
    public class ObsidianBasiliskBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {

                Main.SceneMetrics.NPCBannerBuff[NPCType<ObsidianBasiliskHead>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<ObsidianBasiliskBody>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<ObsidianBasiliskTail>()] = true;
            }
        }
    }
}