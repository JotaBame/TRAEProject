using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject
{
    public class DefenseChanges : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.PharaohsMask:
                    item.defense = 2;
                    item.vanity = false;
                    return;
                case ItemID.PharaohsRobe:
                    item.defense = 2;
                    item.vanity = false;
                    return;
                case ItemID.RuneHat:
                    item.defense = 4;
                    item.vanity = false;
                    return;
                case ItemID.RuneRobe:
                    item.defense = 8;
                    item.vanity = false;
                    return;
                case ItemID.PirateHat:
                    item.defense = 7;
                    item.rare = ItemRarityID.Orange;
                    item.vanity = false;
                    return;
                case ItemID.PirateShirt:
                    item.defense = 12;
                    item.rare = ItemRarityID.Orange;
                    item.vanity = false;
                    return;
                case ItemID.PiratePants:
                    item.defense = 9;
                    item.rare = ItemRarityID.Orange;
                    item.vanity = false;
                    return;

                case ItemID.WoodGreaves:
                    item.defense = 1;
                    return;
                case ItemID.BorealWoodBreastplate:
                case ItemID.PalmWoodBreastplate:
                case ItemID.EbonwoodHelmet:
                case ItemID.EbonwoodGreaves:
                case ItemID.ShadewoodHelmet:
                case ItemID.ShadewoodGreaves:
                case ItemID.RichMahoganyHelmet:
                case ItemID.RichMahoganyBreastplate:
                case ItemID.RichMahoganyGreaves:
                    item.defense = 2;
                    return;
                case ItemID.CopperHelmet:
                case ItemID.CopperGreaves:
                case ItemID.TinGreaves:
                    item.defense = 3;
                    return;
                case ItemID.CopperChainmail:
                case ItemID.TinHelmet:
                case ItemID.TinChainmail:
                case ItemID.IronHelmet:
                case ItemID.AncientIronHelmet:
                case ItemID.IronGreaves:
                case ItemID.LeadGreaves:
                    item.defense = 4;
                    return;
                case ItemID.IronChainmail:
                case ItemID.LeadChainmail:
                case ItemID.LeadHelmet:
                case ItemID.SilverGreaves:
                case ItemID.TungstenGreaves:
                case ItemID.SilverHelmet:
                    item.defense = 5;
                    return;
                case ItemID.JungleHat:
                case ItemID.TungstenHelmet:
                case ItemID.TungstenChainmail:
                case ItemID.SilverChainmail:
                case ItemID.GoldHelmet:
                case ItemID.GoldGreaves:
                case ItemID.AncientGoldHelmet:
                case ItemID.PlatinumGreaves:
                case ItemID.GladiatorHelmet:
                case ItemID.GladiatorLeggings:
                    item.defense = 6;
                    return;
                case ItemID.MythrilHat:
                case ItemID.GoldChainmail:
                case ItemID.PlatinumChainmail:
                case ItemID.PlatinumHelmet:
                case ItemID.ShadowHelmet:
                case ItemID.AncientShadowHelmet:
                case ItemID.ShadowGreaves:
                case ItemID.CrimsonGreaves:
                case ItemID.AncientShadowGreaves:
                case ItemID.GladiatorBreastplate:
                    item.defense = 7;
                    return;
                case ItemID.PearlwoodHelmet:
                    item.defense = 6;
                    return;
                case ItemID.PearlwoodBreastplate:
                    item.defense = 8;
                    return;
                case ItemID.PearlwoodGreaves:
                    item.defense = 6;
                    return;
                case ItemID.FleshKnuckles:
                case ItemID.BerserkerGlove:
                    item.defense = 10;
                    return;
                case ItemID.ChlorophytePlateMail:
                    item.defense = 13;
                    return;   
                case ItemID.AncientArmorHat:
                    item.defense = 13;
                    item.vanity = false;
                    return;
                case ItemID.AncientArmorShirt:
                    item.defense = 8;
                    item.vanity = false;
                    return;
                case ItemID.AncientArmorPants:
                    item.defense = 7;
                    item.vanity = false;
                    return;
                case ItemID.DjinnsCurse:
                    item.defense = 12;
                    item.vanity = false;
                    return;
                case ItemID.SpectreMask:
                case ItemID.SpectreHood:
                    item.defense = 12;
                    return;

            }
      
            return;
        }      
    }
}
