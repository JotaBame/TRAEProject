using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TRAEProject.NewContent.NPCs.BomberBones;
using TRAEProject.NewContent.NPCs.Echosphere.EchoLocator;
using TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan;
using TRAEProject.NewContent.NPCs.Echosphere.EchoStalker;
using TRAEProject.NewContent.NPCs.Echosphere.EchoSprite;
using TRAEProject.NewContent.NPCs.Sky.Griffin;
using TRAEProject.NewContent.NPCs.Sky.Gargoyle;
    using TRAEProject.NewContent.NPCs.Sky.Kinnara;
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
    public class EchoSpriteBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoSprite>()] = true;

             }
        }
    }
    public class EchoStalkerBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoStalkerHead>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoStalkerBody1>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoStalkerBody2>()] = true;
                 Main.SceneMetrics.NPCBannerBuff[NPCType<EchoStalkerTail>()] = true;

            }
        }
    }
    public class EchoLeviathanBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoLeviathanHead>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoLeviathanBody1>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoLeviathanBody2>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoLeviathanBody3>()] = true;
                 Main.SceneMetrics.NPCBannerBuff[NPCType<EchoLeviathanTail>()] = true;

            }
        }
    }
    public class EchoLocatorBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<EchoLocator>()] = true;
 

            }
        }
    }
    public class GargoyleBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<Gargoyle>()] = true;


            }
        }
    }
    public class GriffinBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<GriffinFlier>()] = true;
                Main.SceneMetrics.NPCBannerBuff[NPCType<GriffinWalker>()] = true;


            }
        }
    }
    public class KinnaraBannerPlaced : BannerGeneric
    {
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType<Kinnara>()] = true;
 

            }
        }
    }
}