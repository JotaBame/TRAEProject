using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TRAEProject.Changes.Prefixes
{
    /*
    public class PrefixBugDemonstration : GlobalItem
    {
		public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            //Trying to give a spear legendary fails
            if(item.type == ItemID.Spear)
            {
                Main.NewText("Should Get Legendary");
                return PrefixID.Legendary;
            }
            //Trying to give space gun unreal fails
            if(item.type == ItemID.SpaceGun)
            {
                Main.NewText("Should Get Unreal");
                return PrefixID.Unreal;
            }
            //Trying to give Tragic umbrella warding fails
            if(item.type == ItemID.TragicUmbrella)
            {
                Main.NewText("Should Get Warding");
                return PrefixID.Warding;
            }
            //Giving mace a modded prefix 'Devastating' works
            if(item.type == ItemID.Mace)
            {
                Main.NewText("Should Get Devastating");
                return ModContent.PrefixType<Devastating>();
            }
			return base.ChoosePrefix(item, rand);
        }
    }
    */
}